using System;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.Shared
{
    public static class Common
    {
        public static string ConvertStringToAsterisks(string input)
        {
            return new string('*', input.Length);
        }

        public static string ConvertDateTimeDB(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static bool IsInRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }

        public static int ExpiredDays(string date)
        {
            try
            {
                var result = DateTime.TryParse(date, out DateTime dateCheck);

                if (result)
                    return dateCheck.Subtract(DateTime.Now).Days;
			} catch(Exception ex) {
				log(ex.Message, nameof(ExpiredDays));
                return 0;
            }

            return 0;
        }

        public static DateTime ConvertDate(string dbDate)
        {
            try
            {
                int year, month, day;
                if (dbDate == null || dbDate.Length != 8)
                {
                    throw new Exception("Date conversion failed, wrong format");
                }

                year = Convert.ToInt32(dbDate.Substring(0, 4));
                month = Convert.ToInt32(dbDate.Substring(4, 2)) + 1;
                day = Convert.ToInt32(dbDate.Substring(6, 2));
                return new DateTime(year, month, day);
			} catch(Exception ex) {
				log(ex.Message, nameof(ConvertDate));
                return DateTime.MinValue;
            }
        }

        public static string ConvertFromDate(DateTime newDate)
        {
            // Fixing date format
            var month = newDate.Month - 1;
            return newDate.ToString("yyyy") + month.ToString("D2") + newDate.ToString("dd");
        }

        public static DateTime ConvertDBDateTime(string dbDateTime)
        {
            int year, month, day, hour, minute, second;

            if (dbDateTime == null)
            {
                return DateTime.MinValue;
            }

            try
            {
                var dt = dbDateTime.Split(new[] { ' ' });
                if (dt.Length != 2)
                {
                    throw new Exception("Wrong date time format");
                }
                var dateParts = dt[0].Split(new[] { '-' });
                if (dateParts.Length != 3)
                {
                    throw new Exception("Wrong date format");
                }
                year = Convert.ToInt32(dateParts[0]);
                month = Convert.ToInt32(dateParts[1]);
                day = Convert.ToInt32(dateParts[2]);

                var timeParts = dt[1].Split(new[] { ':' });
                if (timeParts.Length != 3)
                {
                    throw new Exception("Wrong time format");
                }
                hour = Convert.ToInt32(timeParts[0]);
                minute = Convert.ToInt32(timeParts[1]);
                second = Convert.ToInt32(timeParts[2]);

                return new DateTime(year, month, day, hour, minute, second);
            }
			catch(Exception ex)
            {
				log(ex.Message, nameof(ConvertDBDateTime));
                return DateTime.MinValue;
            }
        }

        public static string ConvertStringToStringTime(string str)
        {
            TimeSpan time = default(TimeSpan);
            TimeSpan.TryParse(str, out time);
            var strTime = time.ToString(@"hh\:mm");
            return strTime;
        }

        public static string ConvertStringToStringDate(string str)
        {
            var date = ConvertDate(str);
            var strDate = date.ToString("D");
            return strDate;
        }

        public static DateTime ConvertDBStringDateToDT(string str)
        {
            try
            {
                DateTime.TryParse(str.Remove(0, 4).Remove(16).Replace('-', ' ').Insert(5, "-").Insert(8, "-").Insert(14, ":").Insert(17, ":"), out DateTime backupDate);
                return backupDate;
            } catch(Exception ex) {
				log(ex.Message, nameof(ConvertDBStringDateToDT));
                return DateTime.MinValue;
            }
        }

        public static string GenerateUniqueString(int strLen)
        {
            String initStr = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            String outStr = "";
            var x = (int)DateTime.Now.Ticks;
            Random rand = new Random((int)DateTime.Now.Ticks);

            int init_str_len = initStr.Length;

            for (int i = 0; i < strLen; i++)
            {
                int startStr = rand.Next(init_str_len);
                outStr = outStr + initStr.Substring(startStr, 1);
            }
            return outStr;
        }

        public static string ConvertCard2HumanReadable(string card) {
            if (card == null) return "";
            try
            {
                var cardActual = card.Trim().Replace(" ", "");
                if (cardActual.Length != 16)
                    return card;

                return cardActual.Substring(0, 4) + " " +
                                 cardActual.Substring(4, 4) + " " +
                                 cardActual.Substring(8, 4) + " " +
                                 cardActual.Substring(12, 4);
			} catch(Exception ex) {
				log(ex.Message, nameof(ConvertCard2HumanReadable));
                return card;
            }
        }


        public static DateTime GetDateFromBackupFileName(string name)
        {
            var date = new DateTime();

            try
            {
                var expectedDateString = name.Remove(0, 4).Remove(16).Replace('-', ' ').Insert(5, "-").Insert(8, "-").Insert(14, ":").Insert(17, ":");
                date = DateTime.Parse(expectedDateString);
            }
			catch(Exception ex)
            {
				log(ex.Message, nameof(GetDateFromBackupFileName));
                date = default(DateTime);
            }

            return date;
        }

		static void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(Common));
		}
    }
}
