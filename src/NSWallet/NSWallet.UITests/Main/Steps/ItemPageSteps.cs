﻿using System;
using Xamarin.UITest;
namespace NSWallet.UITests
{
	public class ItemPageSteps
	{
		private ItemPage itemPage = new ItemPage();
		private MainPage mainPage = new MainPage();

		public void goLevelUp(IApp app)
		{
			itemPage.TapBackButton(app);
		}

        public void FillAllItemFields(IApp app, Platform platform, string accountFieldValue, string addressFieldValue, string cardNumberValue, DateTime dateValue, string emailValue, DateTime expDateValue,
                                      string nameValue, string noteValue, string passwordValue, string phoneValue, string pinCodeValue, string oldPasswordValue,
                                      string secretAnswerValue, string secretQuestionValue, string serialNumberValue, DateTime timeValue, string usernameValue, string webPageValue)
		{
            FillDefaultElement(app, itemPage.SelectAccountField, accountFieldValue, platform);
            FillDefaultElement(app, itemPage.SelectAddressField, addressFieldValue, platform);
            FillDefaultElement(app, itemPage.SelectCardNumberField, cardNumberValue, platform);
            FillDateElement(app, platform, itemPage.SelectDateField, dateValue);
            FillDefaultElement(app, itemPage.SelectEmailField, emailValue, platform);
            FillDateElement(app, platform, itemPage.SelectExpirationDateField, expDateValue);
            FillDefaultElement(app, itemPage.SelectNameField, nameValue, platform);
            FillDefaultElement(app, itemPage.SelectNoteField, noteValue, platform);
            FillDefaultElement(app, itemPage.SelectPasswordField, passwordValue, platform);
            FillDefaultElement(app, itemPage.SelectPhoneField, phoneValue, platform);
            FillDefaultElement(app, itemPage.SelectPinCodeField, pinCodeValue, platform);
            FillDefaultElement(app, itemPage.SelectOldPasswordField, oldPasswordValue, platform);
            FillDefaultElement(app, itemPage.SelectSecretAnswerField, secretAnswerValue, platform);
            FillDefaultElement(app, itemPage.SelectSecretQuestionField, secretQuestionValue, platform);
            FillDefaultElement(app, itemPage.SelectSerialNumberField, serialNumberValue, platform);
            FillTimeElement(app, platform, itemPage.SelectTimeField, timeValue);
            FillDefaultElement(app, itemPage.SelectUsernameField, usernameValue, platform);
            FillDefaultElement(app, itemPage.SelectWebPageField, webPageValue, platform);
		}

        private void FillDefaultElement(IApp app, Action<IApp, Platform> selectElement, string value, Platform platform)
        {
            mainPage.TapOnAddLabelButton(app);
            selectElement(app, platform);
            itemPage.fillDefaultElementValue(app, value);
            itemPage.TapSaveButton(app);
        }

        private void FillDateElement(IApp app, Platform platform, Action<IApp, Platform> selectElement, DateTime dateValue)
        {
            mainPage.TapOnAddLabelButton(app);
            selectElement(app, platform);
            itemPage.fillDateElementValue(app, platform, dateValue);
            itemPage.TapSaveButton(app);
        }

        private void FillTimeElement(IApp app, Platform platform, Action<IApp, Platform> selectElement, DateTime dateValue)
        {
            mainPage.TapOnAddLabelButton(app);
            selectElement(app, platform);
            itemPage.fillTimeElementValue(app, platform, dateValue);
            itemPage.TapSaveButton(app);
        }

        public void VerifyIfAllFieldsPresent(IApp app, string accountFieldValue, string addressFieldValue, string cardNumberValue, string dateValue, 
                                             string emailValue, string expDateValue, string nameValue, string noteValue, string passwordValue,   
                                             string phoneValue, string pinCodeValue, string oldPasswordValue, string secretAnswerValue,
                                             string secretQuestionValue, string serialNumberValue, string timeValue, string usernameValue, string webPageValue)
        {
            itemPage.VerifyIfAccountFieldIsPresent(app, accountFieldValue);
            itemPage.VerifyIfAddressFieldIsPresent(app, addressFieldValue);
            itemPage.VerifyIfCardNumberFieldIsPresent(app, cardNumberValue);
            itemPage.VerifyIfDateFieldIsPresent(app, dateValue);
            itemPage.VerifyIfEmailFieldIsPresent(app, emailValue);
            itemPage.VerifyIfExpirationDateFieldIsPresent(app, expDateValue);
            itemPage.VerifyIfNameFieldIsPresent(app, nameValue);
            itemPage.VerifyIfNoteFieldIsPresent(app, noteValue);
            itemPage.VerifyIfPasswordFieldIsPresent(app, passwordValue);
            itemPage.VerifyIfPhoneFieldIsPresent(app, phoneValue);
            itemPage.VerifyIfPinCodeFieldIsPresent(app, pinCodeValue);
            itemPage.VerifyIfOldPasswordFieldIsPresent(app, oldPasswordValue);
            itemPage.VerifyIfSecretAnswerFieldIsPresent(app, secretAnswerValue);
            itemPage.VerifyIfSecretQuestionFieldIsPresent(app, secretQuestionValue);
            itemPage.VerifyIfSerialNumberFieldIsPresent(app, serialNumberValue);
            itemPage.VerifyIfTimeFieldIsPresent(app, timeValue);
            itemPage.VerifyIfUsernameFieldIsPresent(app, usernameValue);
            itemPage.VerifyIfWebPageFieldIsPresent(app, webPageValue);
        }

        public void DeleteAllFields(IApp app)
        {
            itemPage.DeleteWebPageField(app);
            itemPage.DeleteUsernameField(app);
            itemPage.DeleteTimeField(app);
            itemPage.DeleteSerialNumberField(app);
            itemPage.DeleteSecretQuestionField(app);
            itemPage.DeleteSecretAnswerField(app);
            itemPage.DeleteOldPasswordField(app);
            itemPage.DeletePinCodeField(app);
            itemPage.DeletePhoneField(app);
            itemPage.DeletePasswordField(app);
            itemPage.DeleteNoteField(app);
            itemPage.DeleteNameField(app);
            itemPage.DeleteExpirationDateField(app);
            itemPage.DeleteEmailField(app);
            itemPage.DeleteDateField(app);
            itemPage.DeleteCardNumberField(app);
            itemPage.DeleteAddressField(app);
            itemPage.DeleteAccountField(app);
        }

        public void DeleteCurrentItem(IApp app)
        {
            itemPage.TapOnCurrentItemIcon(app);
            itemPage.DeleteDefaultElement(app);
        }
	}
}
