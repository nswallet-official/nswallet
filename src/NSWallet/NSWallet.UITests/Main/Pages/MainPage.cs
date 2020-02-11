using System;
using Xamarin.UITest;
using NSWallet.Shared;
using NSWallet.Consts;
using System.Linq;

namespace NSWallet.UITests
{
	public class MainPage
	{
		public void SwipeToOpenMenu(IApp app, Platform platform)
		{
            if (platform == Platform.iOS)
            {
                app.SwipeLeftToRight();
            }
            else
            {
                app.Tap("OK"); //tap menu icon
                //app.DragCoordinates(0, 240, app.Query().FirstOrDefault().Rect.CenterX, app.Query().FirstOrDefault().Rect.CenterY); //for Android
            }
		}

		public void TapLogoutMenuButton(IApp app)
		{
			app.Tap(AutomationIdConsts.MENU_LOGOUT_BUTTON_ID);
		}

		public void TapOnAddLabelButton(IApp app)
		{
            app.WaitForElement(AutomationIdConsts.LABELS_ADD_BUTTON_ID, timeout: Helper.defaultTimeout);
			app.Tap(AutomationIdConsts.LABELS_ADD_BUTTON_ID);
		}

        public void TapOnAddFolderButton(IApp app)
        {
            app.Tap(AutomationIdConsts.ADD_NEW_FOLDER_ID);
        }

		public void TapOnAddItemButton(IApp app)
		{
			app.Tap(AutomationIdConsts.ADD_NEW_ITEM_ID);
		}

        public void TapNextButton(IApp app)
        {
            app.Tap(AutomationIdConsts.NEXT_BUTTON_ID);
        }

        public void TapFolderName(IApp app, string folderNameValue)
        {
            app.WaitForElement(folderNameValue, timeout: Helper.defaultTimeout);
            app.Tap(folderNameValue);
        }

		public void TapItemName(IApp app, string itemNameValue)
		{
            app.WaitForElement(itemNameValue, timeout: Helper.defaultTimeout);
            app.Tap(itemNameValue);
		}
    }
}