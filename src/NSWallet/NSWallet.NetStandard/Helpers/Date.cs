using System;

namespace NSWallet.Helpers
{
	public static class Date
	{
		public static bool IsValidToday(string startDate, string expirationDate)
		{
			var _startDate = new DateTime();
			DateTime.TryParse(startDate, out _startDate);
			var _expirationDate = new DateTime();
			DateTime.TryParse(expirationDate, out _expirationDate);
			var _currentDate = DateTime.Today;

			if (_currentDate <= _expirationDate && _currentDate >= _startDate)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static TimeSpan CheckDatesDifference(DateTime startDate, DateTime endDate)
		{
			return endDate - startDate;
		}

        public static bool CheckDateForUpdate(DateTime previousDate)
        {
            var currentTime = DateTime.Now;
            var hoursPassed = CheckDatesDifference(previousDate, currentTime).TotalHours;
            if (hoursPassed >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
	}
}