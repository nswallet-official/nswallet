using NSWallet.Helpers;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class SocialViewModel : ViewModel
    {


        void openWebsite(string url)
        {
            Device.OpenUri(new System.Uri(url));
        }

        Command facebookCommand;
        public Command FacebookCommand
        {
            get
            {
                return facebookCommand ?? (facebookCommand = new Command(ExecuteFacebookCommand));
            }
        }

        protected void ExecuteFacebookCommand()
        {
            openWebsite(GConsts.APPLINK_FACEBOOK);
        }

        Command shareCommand;
        public Command ShareCommand
        {
            get
            {
                return shareCommand ?? (shareCommand = new Command(ExecuteShareCommand));
            }
        }

        protected void ExecuteShareCommand()
        {
            string link = null;
            switch(Device.RuntimePlatform)
            {
                case Device.iOS:
                    link = GConsts.APPLINK_APPSTORE;
                    break;
                case Device.Android:
                    link = GConsts.APPLINK_GOOGLEPLAY;
                    break;
            }
            if (link != null)
            {
                PlatformSpecific.Share(TR.Tr("app_name") + ": " + link);
            }
        }

        Command twitterCommand;
        public Command TwitterCommand
        {
            get
            {
                return twitterCommand ?? (twitterCommand = new Command(ExecuteTwitterCommand));
            }
        }

        protected void ExecuteTwitterCommand()
        {
            openWebsite(GConsts.APPLINK_TWITTER);
        }

        Command thumbsUpCommand;
        public Command ThumbsUpCommand
        {
            get
            {
                return thumbsUpCommand ?? (thumbsUpCommand = new Command(ExecuteThumbsUpCommand));
            }
        }

        protected void ExecuteThumbsUpCommand()
        {
            switch(Device.RuntimePlatform)
            {
                case Device.Android:
                    openWebsite(GConsts.APPLINK_GOOGLEPLAY);
                    break;
                case Device.iOS:
                    openWebsite(GConsts.APPLINK_APPSTORE);
                    break;
            }
        }
    }
}