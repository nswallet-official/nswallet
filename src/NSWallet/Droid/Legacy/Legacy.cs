//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using Android.Content;
//using Android.Preferences;
//using Java.IO;
//using Java.Util;
//using NSWallet.Shared;
//using Xamarin.Forms;

//namespace NSWallet.Droid
//{
//    public static class Legacy
//    {
//        public static ISharedPreferences GetDefPrefs()
//        {
//            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Forms.Context);
//            return prefs;
//        }

//        public static string GetDefaultBackupsFolderPath()
//        {
//            string backupsFolder = null;

//            var defaultExternalStorage = getDefaultExternalStorage();

//            if (!string.IsNullOrEmpty(defaultExternalStorage))
//            {
//                backupsFolder = getDefaultExternalStorage() + "/" + GConsts.BACKUP_FOLDER;
//            }
//            else
//            {
//                backupsFolder = GConsts.DEFAULT_EXTERNAL_STORAGE_PATH + "/" + GConsts.BACKUP_FOLDER;
//            }

//            return backupsFolder;
//        }

//        static string getDefaultExternalStorage()
//        {
//            string defaultStorage = null;

//            if (getMounted().Count > 0)
//            {
//                defaultStorage = getMounted()[0];
//            }

//            return defaultStorage;
//        }

//        static List<string> getMounted()
//        {
//            var mounted = new List<string>();

//            mounted = getMountedFromPrefs();

//            if (mounted != null)
//            {
//                return mounted;
//            }

//            mounted = getMountedByNormalWay();

//            if (isAfterKitkatVersion())
//            {
//                return mounted;
//            }

//            foreach (var s in getMountedFromFiles())
//            {
//                if (mounted.Contains(s) == false)
//                {
//                    mounted.Add(s);
//                }
//            }

//            return mounted;
//        }

//        static List<string> getMountedFromPrefs()
//        {
//            var mounted = new List<string>();
//            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Forms.Context);
//            string storages = prefs.GetString(GConsts.PREFS_MNT_STORAGES, null);
//            string[] storagesArray = null;
//            File storageFile = null;

//            if (!string.IsNullOrEmpty(storages) && storages.Length > 0)
//            {
//                storagesArray = storages.Split(';');
//            }

//            if (storagesArray != null && storagesArray.Length > 0)
//            {
//                foreach (var s in storagesArray)
//                {
//                    storageFile = new File(s);

//                    if (storageFile.Exists())
//                    {
//                        mounted.Add(s);
//                    }
//                }
//            }

//            if (mounted.Count > 0) return mounted;

//            return null;
//        }

//        static List<string> getMountedByNormalWay()
//        {
//            var mounted = new List<string>();

//            if (Android.OS.Environment.ExternalStorageDirectory.Equals(Android.OS.Environment.MediaMounted))
//            {
//                string ext = Android.OS.Environment.ExternalStorageDirectory.Path;

//                if (ext.Length > 0)
//                {
//                    var f = new File(ext);

//                    if (f.Exists())
//                    {
//                        mounted.Add(ext);
//                    }
//                }
//            }

//            return mounted;
//        }

//        static List<string> getMountedFromFiles()
//        {
//            var common = new List<string>();

//            var fromVold = readVoldFile();

//            var fromMounts = readMountsFile();

//            foreach (var m in fromMounts)
//            {
//                common.Add(m);
//            }

//            foreach (var v in fromVold)
//            {
//                if (common.Contains(v) == false)
//                {
//                    common.Add(v);
//                }
//            }

//            common = testAndCleanList(common);

//            return common;
//        }

//        static bool isAfterKitkatVersion()
//        {
//            return Convert.ToInt32(Android.OS.Build.VERSION.SdkInt) >= 19;
//        }

//        static List<string> readVoldFile()
//        {
//            var mounted = new List<string>();

//            var scanner = new Scanner(new File("/system/etc/vold.fstab"));

//            while (scanner.HasNext)
//            {
//                string line = scanner.NextLine();

//                if (line.StartsWith("dev_mount", StringComparison.Ordinal))
//                {
//                    string[] lineElements = line.Split(' ');
//                    string element = lineElements[2];

//                    if (element.Contains(":"))
//                        element = element.Substring(0, element.IndexOf(':'));

//                    if (element.Contains("usb"))
//                        continue;

//                    if (!mounted.Contains(element))
//                        mounted.Add(element);
//                }
//            }

//            return mounted;
//        }

//        static List<string> readMountsFile()
//        {
//            var mounted = new List<string>();

//            BufferedReader bufReader = null;

//            bufReader = new BufferedReader(new FileReader("/proc/mounts"));

//            string line;

//            while ((line = bufReader.ReadLine()) != null)
//            {
//                if (line.Contains("vfat") || line.Contains("/mnt"))
//                {
//                    var tokens = new StringTokenizer(line, " ");

//                    string s = tokens.NextToken();

//                    s = tokens.NextToken(); // Take the second token, i.e. mount point

//                    if (s.Equals(Android.OS.Environment.ExternalStorageDirectory.Path))
//                    {
//                        mounted.Add(s);
//                    }
//                    else if (line.Contains("/dev/block/vold"))
//                    {
//                        if (!line.Contains("/mnt/secure") && !line.Contains("/mnt/asec") && !line.Contains("/mnt/obb") && !line.Contains("/dev/mapper") && !line.Contains("tmpfs"))
//                        {
//                            mounted.Add(s);
//                        }
//                    }
//                }
//            }

//            if (bufReader != null)
//            {
//                bufReader.Close();
//            }

//            return mounted;
//        }

//        static List<string> testAndCleanList(List<string> mounted)
//        {
//            for (int i = 0; i < mounted.Count; i++)
//            {
//                string mntPath = mounted[i];

//                var path = new File(mntPath);

//                if (!path.Exists() || !path.IsDirectory || !path.CanWrite())
//                {
//                    mounted.RemoveAt(i--);
//                }
//            }

//            return mounted;
//        }
//    }
//}
