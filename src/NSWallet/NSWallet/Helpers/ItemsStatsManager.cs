using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NSWallet.Shared.Helpers;


namespace NSWallet
{
    public static class ItemsStatsManager
    {
        static string itemsStatsFile;
        public static void Init()
        {
            itemsStatsFile = PlatformSpecific.GetStatsFile();
            ItemsStats.GetInstance().LoadJson(PlatformSpecific.ReadFromFile(itemsStatsFile));
        }

        public static void LogView(string itemID)
        {
            ItemsStats.GetInstance().LogView(itemID);
        }

        public static void Save()
        {
            var json = ItemsStats.GetInstance().StoreJson();
            PlatformSpecific.WriteInFile(itemsStatsFile, json);
        }
    }
}