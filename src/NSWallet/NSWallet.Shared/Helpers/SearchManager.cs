using System;
using System.Diagnostics;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.Shared
{
    public static class SM
    {
        public static bool CheckPhraseLength(string phrase, int length)
        {
            if (phrase.Length >= length)
                return true;
            return false;
        }

        public static string ConvertToLower(object obj)
        {
            try
            {
                var str = obj.ToString();
                return str.ToLower();
            }
            catch(Exception ex)
            {
				AppLogs.Log(ex.Message, nameof(ConvertToLower), nameof(SM));
                return null;
            }
        }
    }
}