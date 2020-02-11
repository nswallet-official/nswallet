using System;
using Xamarin.UITest.Utils;

namespace NSWallet.UITests.Main.Helpers
{
    public class WaitTimes : IWaitTimes
    {
        public TimeSpan GestureCompletionTimeout
        {
            get { return TimeSpan.FromMinutes(8); }
        }
        public TimeSpan GestureWaitTimeout
        {
            get { return TimeSpan.FromMinutes(8); }
        }
        public TimeSpan WaitForTimeout
        {
            get { return TimeSpan.FromMinutes(8); }
        }
    }
}
