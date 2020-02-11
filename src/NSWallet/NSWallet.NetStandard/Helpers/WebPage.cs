using System;
using Xamarin.Forms;

namespace NSWallet
{
    public static class WebPage
    {
        public static void NavigateTo(string webPageLink)
        {
            Device.OpenUri(new Uri(webPageLink));
        }
    }
}
