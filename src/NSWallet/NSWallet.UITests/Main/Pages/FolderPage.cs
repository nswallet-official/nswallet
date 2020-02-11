using System;
using Xamarin.UITest;
using NSWallet.Shared;
using NSWallet.Consts;

namespace NSWallet.UITests
{
	public class FolderPage
	{
		private Random random = new Random();

        public void ChooseRandomFolderIcon(IApp app, Platform platform)
		{
            int index = random.Next(5, 20);
            if (platform == Platform.iOS)
            {
                app.Tap(c => c.Id(AutomationIdConsts.FOLDER_ICON_ID).Index(index));
            }
            else 
            {
                app.Tap(c => c.Class(AutomationIdConsts.FOLDER_CLASS_ID).Index(index)); //for android
            }
		}

		public void EnterNewFolderName(IApp app, string newFolderNameInput)
		{
			app.EnterText((AutomationIdConsts.FOLDER_NAME_INPUT_ID), newFolderNameInput);
		}
	}
}