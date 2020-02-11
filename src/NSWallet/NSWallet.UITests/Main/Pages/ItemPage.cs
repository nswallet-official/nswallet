﻿using System;
using Xamarin.UITest;
using NSWallet.Shared;
using NUnit.Framework;
using NSWallet.Consts;

namespace NSWallet.UITests
{
	public class ItemPage
	{
		private Random random = new Random();

		public void EnterNewItemName(IApp app, string newItemNameValue)
		{
			app.EnterText((AutomationIdConsts.ITEM_NAME_INPUT_ID), newItemNameValue);
		}

        public void ChooseRandomItemIcon(IApp app, Platform platform)
		{
			int index = random.Next(1, 10);
            if (platform == Platform.iOS)
            {
                app.Tap(c => c.Id(AutomationIdConsts.ITEM_ICON_ID).Index(index));
            }
            else 
            {
                app.Tap(c => c.Class(AutomationIdConsts.FOLDER_CLASS_ID).Index(index)); //for android
            }   
		}

		public void TapBackButton(IApp app)
		{
            app.WaitForElement(AutomationIdConsts.BACK_BUTTON_ID, timeout: Helper.defaultTimeout);
			app.Tap(AutomationIdConsts.BACK_BUTTON_ID);
		}

        public void TapSaveButton(IApp app)
        {
            app.Tap(AutomationIdConsts.SAVE_BUTTON_ID);
        }

        public void SelectAccountField(IApp app, Platform platform )
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_ACNT);
        }

        public void SelectAddressField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_ADDR);
        }

        public void SelectCardNumberField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_CARD);
        }

        public void SelectDateField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_DATE);
        }

        public void SelectEmailField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_MAIL);
        }

        public void SelectExpirationDateField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_EXPD);
		}

        public void SelectNameField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_NAME);
		}

        public void SelectNoteField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_NOTE);
		}

        public void SelectPasswordField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_PASS);
		}

        public void SelectPhoneField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_PHON);
		}

        public void SelectPinCodeField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_PINC);
		}

        public void SelectOldPasswordField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_OLDP);
		}

        public void SelectSecretAnswerField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_SANS);
        }

        public void SelectSecretQuestionField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_SQUE);
        }

        public void SelectSerialNumberField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_SNUM);
        }

        public void SelectTimeField(IApp app, Platform platform)
        {
            SelectElementWithId(app, platform, GConsts.FLDTYPE_TIME);
        }

        public void SelectUsernameField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_USER);
		}

        public void SelectWebPageField(IApp app, Platform platform)
		{
            SelectElementWithId(app, platform, GConsts.FLDTYPE_LINK);
		}
		
        public void fillDefaultElementValue(IApp app, string value)
        {
            app.EnterText(AutomationIdConsts.ITEM_FIELD_DEFAULT_ENTRY_ID, value);
        }

        public void fillDateElementValue(IApp app, Platform platform, DateTime dateValue)
        {
            app.Tap(AutomationIdConsts.ITEM_FIELD_DATE_PICKER_ID);
            if (platform == Platform.iOS)
            {
                AppPickerExtension.UpdateDatePicker(app, Platform.iOS, dateValue, "DatePicker", Helper.longTimeSpan);
            }
            else
            {
                AppPickerExtension.UpdateDatePicker(app, Platform.Android, dateValue, "DatePicker", Helper.longTimeSpan);
            }
        }

        public void fillTimeElementValue(IApp app, Platform platform, DateTime timeValue)
        {
            app.Tap(AutomationIdConsts.ITEM_FIELD_TIME_PICKER_ID);
            if (platform == Platform.iOS)
            {
                AppPickerExtension.UpdateTimePicker(app, Platform.iOS, timeValue, "TimePicker", Helper.longTimeSpan);
            }
            else
            {
                AppPickerExtension.UpdateTimePicker(app, Platform.Android, timeValue, "TimePicker", Helper.longTimeSpan);
            }
        }

        private void SelectElementWithId(IApp app, Platform platform, string elementId) {
            app.Tap(AutomationIdConsts.SELECTOR_ICON_ID);
            app.WaitForElement(GConsts.FLDTYPE_ACNT, timeout: Helper.defaultTimeout); //wait till page is present
            if (platform == Platform.iOS)
            {
                app.ScrollDownTo(elementId, timeout: Helper.longTimeSpan);
            }
            else {
                app.ScrollDownTo(elementId, strategy: ScrollStrategy.Gesture, timeout: Helper.longTimeSpan);
            }
            if (elementId == GConsts.FLDTYPE_PHON)
            {
                app.ScrollDown();
            }
            app.Tap(elementId);
        }

        private void VerifyIfElementIsPresent(IApp app, string elementId, string fieldValue)
        {
            app.ScrollDownTo(c=>c.Marked(elementId), strategy: ScrollStrategy.Gesture, timeout: Helper.longTimeSpan);
            Assert.AreEqual(app.Query(elementId)[0].Text, fieldValue, "FAIL: The element " + elementId + " with the correct content was not found!");   
		}

        public void VerifyIfAccountFieldIsPresent(IApp app, string accountFieldValue)
        {
            app.WaitForElement(c => c.Marked(accountFieldValue), timeout: Helper.defaultTimeout);
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_ACCOUNT_ID, accountFieldValue);
        }

		public void VerifyIfAddressFieldIsPresent(IApp app, string addressFieldValue)
		{
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_ADDRESS_ID, addressFieldValue);
		}

		public void VerifyIfCardNumberFieldIsPresent(IApp app, string cardNumberFieldValue)
		{
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_CARD_NUM_ID, cardNumberFieldValue);
		}

        public void VerifyIfDateFieldIsPresent(IApp app, string dateFieldValue)
		{
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_DATE_ID, dateFieldValue);
		}

        public void VerifyIfEmailFieldIsPresent(IApp app, string emailFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_MAIL_ID, emailFieldValue);
        }

        public void VerifyIfExpirationDateFieldIsPresent(IApp app, string expDateFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_EXP_DATE_ID, expDateFieldValue);
        }

        public void VerifyIfNameFieldIsPresent(IApp app, string nameFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_NAME_ID, nameFieldValue);
        }

        public void VerifyIfNoteFieldIsPresent(IApp app, string noteFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_NOTE_ID, noteFieldValue);
        }

        public void VerifyIfPasswordFieldIsPresent(IApp app, string passwordFieldValue)
        {
            app.Tap(c => c.Property("text").StartsWith("*"));
            app.WaitForElement(AutomationIdConsts.DELETE_BUTTON_ID);
            Assert.AreEqual(app.Query(passwordFieldValue)[0].Text, passwordFieldValue, "FAIL: The element " + passwordFieldValue + " with the correct content was not found!");
            app.Tap(AutomationIdConsts.BACK_BUTTON_ID);
        }

        public void VerifyIfPhoneFieldIsPresent(IApp app, string phoneFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_PHONE_ID, phoneFieldValue);
        }

        public void VerifyIfPinCodeFieldIsPresent(IApp app, string pinCodeFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_PIN_CODE_ID, pinCodeFieldValue);
        }

        public void VerifyIfOldPasswordFieldIsPresent(IApp app, string oldPasswordFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_OLD_PASSWORD_ID, oldPasswordFieldValue);
        }

        public void VerifyIfSecretAnswerFieldIsPresent(IApp app, string secretAnswerFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_SECRET_ANSWER_ID, secretAnswerFieldValue);
        }

        public void VerifyIfSecretQuestionFieldIsPresent(IApp app, string secretQuestionFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_SECRET_QUESTION_ID, secretQuestionFieldValue);
        }

        public void VerifyIfSerialNumberFieldIsPresent(IApp app, string serialNumberFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_SERIAL_NUMBER_ID, serialNumberFieldValue);
        }

        public void VerifyIfTimeFieldIsPresent(IApp app, string timeFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_TIME_ID, timeFieldValue);
        }

        public void VerifyIfUsernameFieldIsPresent(IApp app, string usernameFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_USERNAME_ID, usernameFieldValue);
        }

        public void VerifyIfWebPageFieldIsPresent(IApp app, string webPageFieldValue)
        {
            VerifyIfElementIsPresent(app, AutomationIdConsts.ITEM_FIELD_LINK_ID, webPageFieldValue);
        }

        private void DeleteField(IApp app, string elementId)
        {
            app.Tap(elementId);
            DeleteDefaultElement(app);
        }

        public void DeleteDefaultElement(IApp app)
        {
            app.Tap(AutomationIdConsts.DELETE_BUTTON_ID);
            app.Tap(TR.Tr(AutomationIdConsts.POP_UP_ANSWER_YES));
        }

        public void DeleteAccountField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_ACCOUNT_ID);
        }

        public void DeleteAddressField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_ADDRESS_ID);
        }

        public void DeleteCardNumberField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_CARD_NUM_ID);
        }

        public void DeleteDateField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_DATE_ID);
        }

        public void DeleteEmailField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_MAIL_ID);
        }

        public void DeleteExpirationDateField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_EXP_DATE_ID);
        }

        public void DeleteNameField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_NAME_ID);
        }

        public void DeleteNoteField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_NOTE_ID);
        }

        public void DeletePasswordField(IApp app)
        {
            app.Tap(c => c.Property("text").StartsWith("*")); //password field
            DeleteDefaultElement(app);
        }

        public void DeletePhoneField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_PHONE_ID);
        }

        public void DeletePinCodeField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_PIN_CODE_ID);
        }

        public void DeleteOldPasswordField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_OLD_PASSWORD_ID);
        }

        public void DeleteSecretAnswerField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_SECRET_ANSWER_ID);
        }

        public void DeleteSecretQuestionField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_SECRET_QUESTION_ID);
        }

        public void DeleteSerialNumberField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_SERIAL_NUMBER_ID);
        }

        public void DeleteTimeField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_TIME_ID);
        }

        public void DeleteUsernameField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_USERNAME_ID);
        }

        public void DeleteWebPageField(IApp app)
        {
            DeleteField(app, AutomationIdConsts.ITEM_FIELD_LINK_ID);
        }

        public void TapOnCurrentItemIcon(IApp app)
        {
            app.Tap(AutomationIdConsts.CURRENT_ITEM_ICON_ID);
        }
	}
}