using NSWallet.Enums;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class MainScreenCreateItemViewModel : ViewModel
    {
        INavigation navigation;
        NSWItemType itemType;
        
        public MainScreenCreateItemViewModel(INavigation navigation, NSWItemType itemType)
        {
            this.navigation = navigation;
            this.itemType = itemType;
        }

        public string Title
        {
            get 
            {
                switch(itemType)
                {
                    case NSWItemType.Item: return TR.Tr("new_item");
                    case NSWItemType.Folder: return TR.Tr("new_folder");
                    default: return null;
                }
            }
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private Command cancelCommand;
        public Command CancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new Command(ExecuteCancelCommand));
            }
        }

        void ExecuteCancelCommand()
        {
            Pages.CloseModalPage(navigation);
        }

        private Command nextCommand;
        public Command NextCommand
        {
            get
            {
                return nextCommand ?? (nextCommand = new Command(ExecuteNextCommand));
            }
        }

        void ExecuteNextCommand()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                Pages.CreateItemOrFolder(navigation, itemType, Name);
            }
            else
            {
                PlatformSpecific.DisplayShortMessage(TR.Tr("create_item_empty"));
            }
        }
    }
}