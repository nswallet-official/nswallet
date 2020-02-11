using System;
using Xamarin.UITest;

namespace NSWallet.UITests
{
	public class MainPageSteps
	{
		private MainPage mainPage = new MainPage();
		private FolderPage folderPage = new FolderPage();
		private ItemPage itemPage = new ItemPage();

        public void DoLogout(IApp app, Platform platform)
		{
			mainPage.SwipeToOpenMenu(app, platform);
			mainPage.TapLogoutMenuButton(app);
		}

        public void CreateFolder(IApp app, string newFolderNameInput, Platform platform)
		{
			mainPage.TapOnAddLabelButton(app);
            mainPage.TapOnAddFolderButton(app);
			folderPage.EnterNewFolderName(app, newFolderNameInput);
            mainPage.TapNextButton(app);
            folderPage.ChooseRandomFolderIcon(app, platform);
		}

        public void CreateItem(IApp app, string newItemNameInput, Platform platform)
		{
			mainPage.TapOnAddLabelButton(app);
			mainPage.TapOnAddItemButton(app);
			itemPage.EnterNewItemName(app, newItemNameInput);
			mainPage.TapNextButton(app);
			itemPage.ChooseRandomItemIcon(app, platform);
		}

        public void OpenCreatedFolder(IApp app, string folderName)
        {
            mainPage.TapFolderName(app, folderName); 
        }

		public void OpenItemByName(IApp app, string itemName)
		{
            mainPage.TapItemName(app, itemName);
		} 
	}
}