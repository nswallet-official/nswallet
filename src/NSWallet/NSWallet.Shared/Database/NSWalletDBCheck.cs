using System;
using NSWallet.Shared.Helpers.Logs.AppLog;
using SQLite;

namespace NSWallet.Shared
{
    public static class NSWalletDBCheck
    {
        public static string GetDBVersion(string file)
        {
            try
            {
                var conn = new SQLiteConnection(file);
                var properties = conn.Table<nswallet_properties>().FirstOrDefault();
                conn.Close();
                return properties.version;
            }
            catch (Exception ex)
            {
				AppLogs.Log(ex.Message, nameof(GetDBVersion), nameof(NSWalletDBCheck));
				System.Diagnostics.Debug.WriteLine("GetDBVersion exception: " + ex.Message);
                return null;
            }
        }
    }
}