using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.Util;
using NSWallet.Droid;
using NSWallet.iOS.Interfaces;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Forms;

[assembly: Dependency(typeof(ExportService))]
namespace NSWallet.Droid
{
	public class ExportService : IExport
	{
		static List<PDFItemModel> ExportedItems { get; set; }

		static string path;

		static void CleanCache()
		{
			var backupPath = PlatformSpecific.GetBackupPath();
			var directories = Directory.EnumerateDirectories(backupPath);
			foreach(var directory in directories) {
				if (directory.Contains("cache.")) {
					PlatformSpecific.RemoveDirectoryWithContents(directory);
				}
			}
		}

		static void EnumerateItems(string id, bool recursive = false)
		{
			try {
				foreach (var item in BL.GetListByParentID(id, true)) {
					if (!recursive)
						path = "/";

					if (!item.Folder) {

						ExportedItems.Add(new PDFItemModel {
							Name = item.Name,
							Image = item.Icon,
							ItemType = ExportItemTypes.Item,
							Path = path
						});

						path += item.Name + "/";

						if (item.Fields != null) {
							foreach (var field in item.Fields) {
								string fieldType = null;
								if (TR.Tr(field.FieldType) == field.FieldType) {
									fieldType = field.Label;
								} else {
									fieldType = TR.Tr(field.FieldType);
								}
								ExportedItems.Add(new PDFItemModel {
									Name = fieldType + ": " + field.HumanReadableValue,
									Image = field.Icon,
									ItemType = ExportItemTypes.Field,
									Path = path
								});
							}
						}
					} else {
						ExportedItems.Add(new PDFItemModel {
							Name = item.Name,
							Image = item.Icon,
							ItemType = ExportItemTypes.Folder,
							Path = path
						});

						path += item.Name + "/";

						EnumerateItems(item.ItemID, true);
					}
				}

			} catch (Exception ex) {
				log(ex.Message, nameof(EnumerateItems));
			}
		}

		public void GenerateTXT()
		{
			CleanCache();

			// Get NSWItems
			ExportedItems = new List<PDFItemModel>();
			EnumerateItems(GConsts.ROOTID);

			string fileName = "nswallet" + DateTime.Now.ToString("-dd-MM-yyyy-hh-mm-ss") + ".txt";
			var tempFolder = PlatformSpecific.GetBackupPath() + "/cache" + "." + Common.GenerateUniqueString(8);
			PlatformSpecific.CreateFolder(tempFolder);
			string exportPath = tempFolder + "/" + fileName;
			bool isField = false;
			int count = 0;

			using (StreamWriter sw = new StreamWriter(exportPath)) {
				foreach (var item in ExportedItems) {
					if (item.ItemType == ExportItemTypes.Field) {
						if (!isField) {
							if(count > 0) sw.WriteLine();
							sw.WriteLine(String.Format("[{0}]", item.Path));
							sw.WriteLine(item.Name);
						} else {
							sw.WriteLine(item.Name);
						}
						isField = true;
						count++;
					} else {
						isField = false;
					}
				}
			}

			var sharing = new SharingDroid();
			sharing.ShareFile(exportPath, TR.Tr("app_name"), "text/*", TR.Tr("export_data"), null);
		}

		public void GeneratePDF()
		{
			CleanCache();

			// Get NSWItems
			ExportedItems = new List<PDFItemModel>();
			EnumerateItems(GConsts.ROOTID);

			// create PDF
			var PDFDoc = new PdfDocument();

			// create a page description
			DisplayMetrics displayMetrics = new DisplayMetrics();
			PdfDocument.PageInfo pageInfo = new PdfDocument.PageInfo.Builder(2000, 150 * 20, 0).Create();
			PdfDocument.Page page = null;

			int xPosMatrix = 20, yPosMatrix = 10;
			int xPosText = 170, yPosText = 99;
			int paintTextSize = 50;
			int counter = 1;

			foreach (var item in ExportedItems) {

				if (counter == 1) {
					// start page
					page = PDFDoc.StartPage(pageInfo);
				}

				if (item.ItemType == ExportItemTypes.Folder) {
					yPosMatrix += 20;
				}

				if (item.ItemType == ExportItemTypes.Field) {
					xPosText += 100;
					xPosMatrix += 100;
				}

				var imageStream = NSWRes.GetImage(item.Image);
				if (imageStream == null) {
					if (item.Image != null) {
						var customIcons = BL.GetIcons();
						var customIcon = customIcons.Find(x => item.Image.Contains(x.IconID));
						imageStream = customIcon != null ?
							new MemoryStream(customIcon.Icon) :
							NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON);
					} else {
						imageStream = NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON);
					}
				}

				var imageBytesArray = GetBytesFromStream(imageStream);
				var options = new BitmapFactory.Options();
				options.InMutable = true;
				var bitmap = BitmapFactory.DecodeByteArray(imageBytesArray, 0, imageBytesArray.Length, options);

				var matrix = new Matrix();
				matrix.PostTranslate(xPosMatrix, yPosMatrix);

				// add image to the page
				page.Canvas.DrawBitmap(bitmap, matrix, null);

				Paint paint = new Paint();
				paint.TextSize = paintTextSize;
				if (item.ItemType == ExportItemTypes.Field) {

					var saveXPosMatrix = xPosMatrix;
					var saveXPosText = xPosText;

					var strings = WrapString(item.Name, 77);
					int wordCounter = 1;
					foreach (var str in strings) {
						page.Canvas.DrawText(str, xPosText, yPosText, paint);

						if (wordCounter != strings.Length) {

							if (counter == 20) {
								PDFDoc.FinishPage(page);
								xPosMatrix = 20;
								yPosMatrix = 10;
								xPosText = 270;
								yPosText = 99;
								counter = 1;
								page = PDFDoc.StartPage(pageInfo);
							} else {
								counter++;
								yPosMatrix += 10 + 128 + 10 - 49;
								yPosText += 99;
							}
						}
						wordCounter++;
					}
					xPosMatrix = saveXPosMatrix;
					xPosText = saveXPosText;
				} else {
					page.Canvas.DrawText(item.Name, xPosText, yPosText, paint);
				}

				yPosMatrix += 10 + 128 + 10;
				yPosText += 99 + 49;

				if (item.ItemType == ExportItemTypes.Folder) {
					yPosMatrix -= 20;
				}

				if (item.ItemType == ExportItemTypes.Field) {
					xPosText -= 100;
					xPosMatrix -= 100;
				}

				if (counter == 20) {
					PDFDoc.FinishPage(page);
					xPosMatrix = 20;
					yPosMatrix = 10;
					xPosText = 170;
					yPosText = 99;
					paintTextSize = 50;
					counter = 1;
				} else {
					counter++;
				}
			}

			if (page.Canvas != null)
				PDFDoc.FinishPage(page);

			// save PDF
			string fileName = "nswallet" + DateTime.Now.ToString("-dd-MM-yyyy-hh-mm-ss") + ".pdf";
			var tempFolder = PlatformSpecific.GetBackupPath() + "/cache" + "." + Common.GenerateUniqueString(8);
			PlatformSpecific.CreateFolder(tempFolder);
			string exportPath = tempFolder + "/" + fileName;
			FileStream fileStream = new FileStream(exportPath, FileMode.CreateNew);

			PDFDoc.WriteTo(fileStream);
			fileStream.Flush();

			// close PDF
			PDFDoc.Close();

			var sharing = new SharingDroid();
			sharing.ShareFile(exportPath, TR.Tr("app_name"), "application/pdf", TR.Tr("export_data"), null);
		}

		public static byte[] GetBytesFromStream(Stream input)
		{
			using (MemoryStream ms = new MemoryStream()) {
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}

		public static string[] WrapString(string text, int max)
		{
			var charCount = 0;
			var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			return lines.GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
							? max - (charCount % max) : 0) + w.Length + 1) / max)
						.Select(g => string.Join(" ", g.ToArray()))
						.ToArray();
		}

		static void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(ExportService));
		}
	}
}