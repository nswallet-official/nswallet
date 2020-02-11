using System;
using Xamarin.UITest;
namespace NSWallet.UITests
{
	public class Helper
	{
 
        public static readonly TimeSpan longTimeSpan = TimeSpan.FromMinutes(2);
        public static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(45);

		public static void WaitForElement(IApp app, String label)
		{
            app.WaitForElement(c => c.Marked(label), "Do not see: " + label, defaultTimeout);		
		}
	}
}
