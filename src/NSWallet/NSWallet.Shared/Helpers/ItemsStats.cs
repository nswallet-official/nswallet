using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.Shared.Helpers
{
    public class ItemsStats
    {
        static readonly ItemsStats instance = new ItemsStats();
        public static ItemsStats GetInstance()
        {
            return instance;
        }
        List<ItemStatsData> items = new List<ItemStatsData>();

        class ItemStatsData
        {
            public string itemID;
            public DateTime viewDT;
            public int viewCount;
        }

        class DateTimeComparer : IComparer<ItemStatsData>
        {
            public int Compare(ItemStatsData a, ItemStatsData b)
            {
                if (a.viewDT == b.viewDT)
                    return 0;
                if (a.viewDT < b.viewDT)
                    return 1;

                return -1;
            }
        }

        class CountComparer : IComparer<ItemStatsData>
        {
            public int Compare(ItemStatsData a, ItemStatsData b)
            {
                if (a.viewCount == b.viewCount)
                    return 0;
                if (a.viewCount < b.viewCount)
                    return 1;

                return -1;
            }
        }

        public string StoreJson()
        {
            return JsonConvert.SerializeObject(items, Formatting.Indented);
        }

        public void LoadJson(string jsonStr)
        {
            try
            {
                items = JsonConvert.DeserializeObject<List<ItemStatsData>>(jsonStr);
            }
            catch(Exception ex)
            {
				AppLogs.Log(ex.Message, nameof(LoadJson), nameof(ItemsStats));
                items = new List<ItemStatsData>();
            } finally {
                if (items == null) {
                    items = new List<ItemStatsData>();
                }
            }

        }

        readonly static DateTimeComparer dtComparer = new DateTimeComparer();
        readonly static CountComparer countComparer = new CountComparer();

        public List<string> SortedByDateTime()
        {
            List<string> outItems = new List<string>();
            items.Sort(dtComparer);
            var count = 10;
            foreach (var item in items)
            {
                if (count <= 0) break;
                outItems.Add(item.itemID);
                count--;
            }
            return outItems;
        }

        public void LogView(string itemID)
        {
            if (items.Exists(item => item.itemID == itemID))
            {
                var foundItem = items.Find(item => item.itemID == itemID);
                foundItem.viewCount++;
                foundItem.viewDT = DateTime.Now;
            }
            else
            {
                ItemStatsData isd = new ItemStatsData
                {
                    itemID = itemID,
                    viewCount = 1,
                    viewDT = DateTime.Now
                };
                items.Add(isd);
            }
        }

        public List<string> SortedByCount()
        {
            List<string> outItems = new List<string>();
            items.Sort(countComparer);
            var count = 10;
            foreach (var item in items)
            {
                if (count <= 0) break;
                outItems.Add(item.itemID);
                count--;
            }
            return outItems;
        }

        ItemsStats()
        {
            items = new List<ItemStatsData>();
        }
    }
}
