using System;
using System.Threading.Tasks;
using NSWallet.Shared.Helpers.Logs.AppLog;
using NSWallet.Shared.Helpers.Logs.DeviceInfo;
using NSWallet.Shared.Helpers.Logs.Diagnostics;

namespace NSWallet.Shared
{
	public partial class DataAccessLayer
	{
		bool diagnosticsNotRunning = true;

		public bool PrepareDiagnostics()
		{
			return !string.IsNullOrEmpty(currentPassword);
		}

		public bool IsDiagnosticsRunning()
		{
			return diagnosticsNotRunning;
		}

		public Task<bool> RunDiagnostics(string path, LogsDeviceInfo logsDeviceInfo, bool fromLogin)
		{
			diagnosticsNotRunning = false;
			bool isDiagnosticsError = false;
			Diagnostics.Init(path, logsDeviceInfo);

			try {
				isDiagnosticsError |= diagnoseProperties();
				isDiagnosticsError |= diagnoseRootItem();
				isDiagnosticsError |= diagnoseRootParent();

				var nswDBItems = nswdb.RetrieveAllItems();
				foreach (var item in nswDBItems) {
					isDiagnosticsError |= diagnoseItem(item);
				}

				var nswDBFields = nswdb.RetrieveAllFields();
				foreach (var field in nswDBFields) {
					isDiagnosticsError |= diagnoseField(field);
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(RunDiagnostics));
			}

			if (fromLogin) {
				currentPassword = "";
			}
			diagnosticsNotRunning = true;
			return Task.FromResult(isDiagnosticsError);
		}

		bool diagnoseProperties()
		{
			var properties = nswdb.GetStorageProperties();
			if (properties == null) {
				Diagnostics.Log(
					"Properties are null"
				);
				return true;
			}
			return false;
		}

		bool diagnoseRootItem()
		{
			var isRootExists = nswdb.SearchForRoot();
			if (!isRootExists) {
				Diagnostics.Log(
					"Root item does not exist"
				);
				return true;
			}
			return false;
		}

		bool diagnoseRootParent()
		{
			bool flag = false;
			var rootItem = nswdb.GetRootItem();
			if (rootItem != null) {
				var parentID = rootItem.parent_id;
				if (parentID != GConsts.ROOT_PARENT_ID) {
					Diagnostics.Log(
						"Root item's parent ID does not match the default root parent ID."
					);
					flag = true;
				}
				if (parentID == null) {
					Diagnostics.Log(
						"Root item's parent ID is nullable."
					);
					flag = true;
				}
			}
			return flag;
		}

		bool diagnoseItem(nswallet_items nswDBItem)
		{
			bool flag = false;

			if (nswDBItem == null) {
				flag = true;
				Diagnostics.Log(
					"Item is null"
				);
			} else {
				if (currentPassword != null) {
					var decryptedName = diagnosticsDecrypt(nswDBItem.name, out bool ok);

					if (ok == false) {
						flag = true;
						Diagnostics.Log(
							"Decrypt failure during items retrieval:" +
							" folder - " + nswDBItem.folder +
							", deleted - " + nswDBItem.deleted
						);
					}
				} else {
					AppLogs.Log("Password cannot be nullable", nameof(diagnoseItem), nameof(DataAccessLayer));
				}
			}

			return flag;
		}

		bool diagnoseField(nswallet_fields nswDBField)
		{
			bool flag = false;

			if (nswDBField == null) {
				flag = true;
				Diagnostics.Log(
					"Field is null"
				);
			} else {
				if (currentPassword != null) {
					var decryptedField = diagnosticsDecrypt(nswDBField.value, out bool ok);

					if (ok == false) {
						flag = true;
						Diagnostics.Log(
							"Decrypt failure during fields retrieval:" +
							" type - " + nswDBField.type +
							", deleted - " + nswDBField.deleted
						);
					}
				} else {
					AppLogs.Log("Password cannot be nullable.", nameof(diagnoseField), nameof(DataAccessLayer));
				}
			}

			return flag;
		}

		string diagnosticsDecrypt(byte[] value, out bool ok)
		{
			return Security.DecryptStringAES(
				value,
				currentPassword,
				StorageProperties.EncryptionCount,
				currentPassword,
				out ok
			);
		}
	}
}