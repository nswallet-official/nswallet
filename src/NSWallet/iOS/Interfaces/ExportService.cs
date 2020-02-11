using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreFoundation;
using CoreGraphics;
using NSWallet.iOS;
using NSWallet.Shared;
using Xamarin.Forms;
using UIKit;
using NSWallet.iOS.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using NSWallet.Shared.Helpers.Logs.AppLog;

[assembly: Dependency(typeof(ExportService))]
namespace NSWallet.iOS
{
	public class ExportService : IExport
	{
		static List<PDFItemModel> ExportedItems { get; set; }

		static string path;

		static SharingIOS sharing;

		static void checkSharing()
		{
			if (sharing == null) {
				sharing = new SharingIOS();
			}
		}

		static void CleanCache()
		{
			var backupPath = PlatformSpecific.GetBackupPath();
			var directories = Directory.EnumerateDirectories(backupPath);
			foreach (var directory in directories) {
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
								if(TR.Tr(field.FieldType) == field.FieldType) {
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
							if (count > 0) sw.WriteLine();
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
			Device.BeginInvokeOnMainThread(() => {
				checkSharing();
				sharing.ShareFile(exportPath, TR.Tr("app_name"), "text/*", TR.Tr("export_data"), null);
			});
		}

		public static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream()) {
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		static string getHTMLFromItems()
		{
			string html = null;
			foreach (var item in ExportedItems) {
				string marginLeft = null;
				switch (item.ItemType) {
					case ExportItemTypes.Folder:
						marginLeft = "20px";
						break;
					case ExportItemTypes.Item:
						marginLeft = "60px";
						break;
					case ExportItemTypes.Field:
						marginLeft = "100px";
						break;
				}

				var icon = NSWRes.GetImage(item.Image);
				if (icon == null) {
					if (item.Image != null) {
						var customIcons = BL.GetIcons();
						var customIcon = customIcons.Find(x => item.Image.Contains(x.IconID));
						icon = customIcon != null ? 
							new MemoryStream(customIcon.Icon) : 
							NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON);
					} else {
						icon = NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON);
					}
				}
					
				var iconBytes = ReadFully(icon);
				var iconBase64 = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(iconBytes));

				html += String.Format("<div style=\"margin-left: {0}; margin-top: 10px\">", marginLeft);
				html += String.Format("<img src=\"{0}\" align=\"center\" height=\"50px\">", iconBase64);
				html += String.Format("<span style=\"vertical-align: middle; font-size: 25px; margin-left: 10px\">{0}</span></div>", item.Name);
			}
			return html;
		}

		public void GeneratePDF()
		{
			CleanCache();

			ExportedItems = new List<PDFItemModel>();
			EnumerateItems(GConsts.ROOTID);

			string fileName = "nswallet" + DateTime.Now.ToString("-dd-MM-yyyy-hh-mm-ss") + ".pdf";
			var tempFolder = PlatformSpecific.GetBackupPath() + "/cache" + "." + Common.GenerateUniqueString(8);
			PlatformSpecific.CreateFolder(tempFolder);
			string exportPath = tempFolder + "/" + fileName;

			UIWebView webView = new UIWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72));
			webView.Delegate = new WebViewCallBack(exportPath);
			webView.ScalesPageToFit = true;
			webView.UserInteractionEnabled = false;
			webView.BackgroundColor = UIColor.White;
			var html = getHTMLFromItems();
			webView.LoadHtmlString(html, null);
			webView.Delegate.LoadingFinished(webView);

			((WebViewCallBack)webView.Delegate).Created += () => {
				Device.BeginInvokeOnMainThread(() => {
					checkSharing();
					sharing.ShareFile(exportPath, TR.Tr("app_name"), "application/pdf", TR.Tr("export_data"), null);
				});
			};
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