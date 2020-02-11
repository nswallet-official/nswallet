﻿﻿using System;
using NSWallet.Shared;
using NSWallet.UITests;
using NUnit.Framework;
using Xamarin.UITest;
using NSWallet.Consts;

namespace NSWallet
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		private IApp app;
		private Platform platform;
		private string passwordValue;
        private string newFolderName;
		private string newItemName;
        private string newFilledItemName;
		private string itemAccountFieldValue;
        private string itemAddressFieldValue;
        private string itemCardNumberFieldValue;
        private DateTime itemDateFieldValue;
        private string itemEmailFieldValue;
        private DateTime itemExpirationDateFieldValue;
        private string itemNameFieldValue;
        private string itemNoteFieldValue;
        private string itemPasswordFieldValue;
        private string itemPhoneFieldValue;
        private string itemPinCodeFieldValue;
        private string itemOldPasswordFieldValue;
        private string itemSecretAnswerFieldValue;
        private string itemSecretQuestionFieldValue;
        private string itemSerialNumberFieldValue;
        private DateTime itemTimeFieldValue;
        private string itemUsernameFieldValue;
        private string itemWebPageFieldValue;
		private DataProvider dataProvider = new DataProvider();
		private LoginPageSteps loginPageSteps = new LoginPageSteps();
		private MainPageSteps mainPageSteps = new MainPageSteps();
		private ItemPageSteps itemPageSteps = new ItemPageSteps();
			
		public Tests(Platform platform)
		{
			this.platform = platform;
		}

		private void SetData()
		{
			this.passwordValue = dataProvider.GenerateRandomPassword();
			this.newFolderName = dataProvider.GenerateRandomString();
			this.newItemName = dataProvider.GenerateRandomString();
            this.newFilledItemName = dataProvider.GenerateRandomString();
			this.itemAccountFieldValue = dataProvider.GenerateRandomString();
            AutomationIdConsts.ITEM_FIELD_ACCOUNT_ID = itemAccountFieldValue;
            this.itemAddressFieldValue = dataProvider.GenerateRandomString() + "," + dataProvider.GenerateRandomString();
            AutomationIdConsts.ITEM_FIELD_ADDRESS_ID = itemAddressFieldValue;
            this.itemCardNumberFieldValue = dataProvider.GenerateRandomCardNumber();
            AutomationIdConsts.ITEM_FIELD_CARD_NUM_ID = itemCardNumberFieldValue;
            this.itemDateFieldValue = dataProvider.GenerateRandomDate();
            AutomationIdConsts.ITEM_FIELD_DATE_ID = itemDateFieldValue.ToString("dddd, MMMM %d, yyyy");
            this.itemEmailFieldValue = dataProvider.GenerateRandomString() + "@gm.com";
            AutomationIdConsts.ITEM_FIELD_MAIL_ID = itemEmailFieldValue;
            this.itemExpirationDateFieldValue = dataProvider.GenerateRandomDate();
            AutomationIdConsts.ITEM_FIELD_EXP_DATE_ID = itemExpirationDateFieldValue.ToString("dddd, MMMM %d, yyyy");
            this.itemNameFieldValue = dataProvider.GenerateRandomString();
            AutomationIdConsts.ITEM_FIELD_NAME_ID = itemNameFieldValue;
            this.itemNoteFieldValue = dataProvider.GenerateRandomString();
            AutomationIdConsts.ITEM_FIELD_NOTE_ID = itemNoteFieldValue;
            this.itemPasswordFieldValue = dataProvider.GenerateRandomPassword();
            AutomationIdConsts.ITEM_FIELD_PASSWORD_ID = itemPasswordFieldValue;
            this.itemPhoneFieldValue = dataProvider.GenerateRandomPhoneNumber();
            AutomationIdConsts.ITEM_FIELD_PHONE_ID = itemPhoneFieldValue;
            this.itemPinCodeFieldValue = dataProvider.GenerateRandomPinCodeNumber();
            AutomationIdConsts.ITEM_FIELD_PIN_CODE_ID = itemPinCodeFieldValue;
            this.itemOldPasswordFieldValue = dataProvider.GenerateRandomPassword();
            AutomationIdConsts.ITEM_FIELD_OLD_PASSWORD_ID = itemOldPasswordFieldValue;
            this.itemSecretAnswerFieldValue = dataProvider.GenerateRandomString();
            AutomationIdConsts.ITEM_FIELD_SECRET_ANSWER_ID = itemSecretAnswerFieldValue;
            this.itemSecretQuestionFieldValue = dataProvider.GenerateRandomString();
            AutomationIdConsts.ITEM_FIELD_SECRET_QUESTION_ID = itemSecretQuestionFieldValue;
            this.itemSerialNumberFieldValue = dataProvider.GenerateRandomPassword();
            AutomationIdConsts.ITEM_FIELD_SERIAL_NUMBER_ID = itemSerialNumberFieldValue;
            this.itemTimeFieldValue = dataProvider.GenerateRandomTime();
            AutomationIdConsts.ITEM_FIELD_TIME_ID = itemTimeFieldValue.ToString("HH:mm");
            this.itemUsernameFieldValue = dataProvider.GenerateRandomString();
            AutomationIdConsts.ITEM_FIELD_USERNAME_ID = itemUsernameFieldValue;
            this.itemWebPageFieldValue = "www." + dataProvider.GenerateRandomString() + ".com";
            AutomationIdConsts.ITEM_FIELD_LINK_ID = itemWebPageFieldValue;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = AppInitializer.StartApp(platform);
			SetData();
			TR.InitTR(GetType().Namespace);  // Preparing translations
			TR.SetLanguage(Lang.LANG_NAME_EN); //Setting English language
		}

		[Test]
        public void MasterTest()
		{
            loginPageSteps.CreateMasterPassword(app, passwordValue);
            mainPageSteps.DoLogout(app, platform);
			loginPageSteps.DoLogin(app, passwordValue);
            mainPageSteps.CreateFolder(app, newFolderName, platform);
            mainPageSteps.CreateItem(app, newItemName, platform);
			itemPageSteps.goLevelUp(app);
            mainPageSteps.CreateItem(app, newFilledItemName, platform);
            itemPageSteps.FillAllItemFields(app, platform, itemAccountFieldValue, itemAddressFieldValue, itemCardNumberFieldValue, itemDateFieldValue,
                                            itemEmailFieldValue, itemExpirationDateFieldValue, itemNameFieldValue, itemNoteFieldValue, itemPasswordFieldValue,
                                            itemPhoneFieldValue, itemPinCodeFieldValue, itemOldPasswordFieldValue, itemSecretAnswerFieldValue,
                                            itemSecretQuestionFieldValue, itemSerialNumberFieldValue, itemTimeFieldValue, itemUsernameFieldValue, itemWebPageFieldValue);
			mainPageSteps.DoLogout(app, platform);
			loginPageSteps.DoLogin(app, passwordValue);
            mainPageSteps.OpenCreatedFolder(app, newFolderName);
            mainPageSteps.OpenItemByName(app, newFilledItemName);
            itemPageSteps.VerifyIfAllFieldsPresent(app, itemAccountFieldValue, itemAddressFieldValue, itemCardNumberFieldValue, itemDateFieldValue.ToString("dddd, MMMM %d, yyyy"), 
                                                   itemEmailFieldValue, itemExpirationDateFieldValue.ToString("dddd, MMMM %d, yyyy"), itemNameFieldValue, itemNoteFieldValue, itemPasswordFieldValue, 
                                                   itemPhoneFieldValue, itemPinCodeFieldValue, itemOldPasswordFieldValue, itemSecretAnswerFieldValue,
                                                   itemSecretQuestionFieldValue, itemSerialNumberFieldValue, itemTimeFieldValue.ToString("HH:mm"), itemUsernameFieldValue, itemWebPageFieldValue);
            itemPageSteps.DeleteAllFields(app);
            itemPageSteps.DeleteCurrentItem(app); //filled item deleting
            mainPageSteps.OpenItemByName(app, newItemName);
            itemPageSteps.DeleteCurrentItem(app); //unfilled item deleting
            itemPageSteps.DeleteCurrentItem(app); //folder deleting
            mainPageSteps.DoLogout(app, platform);
        }
	}
}