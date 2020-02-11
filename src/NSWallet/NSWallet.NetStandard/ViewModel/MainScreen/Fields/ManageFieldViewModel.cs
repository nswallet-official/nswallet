using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NSWallet.Helpers;
using NSWallet.NetStandard.Helpers.FA;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NSWallet
{
	public class ManageFieldViewModel : ViewModel
	{
		// Callbacks
		public Action ClosePopupCommandCallback { get; set; }
		public Action ShowPopupCommandCallback { get; set; }

		// Forms
		INavigation navigation;

		// Variables
		readonly ManageFieldType manageFieldType;
		NSWItem nswItem;
		readonly string updFieldID;
		string fieldType;
		string valueType;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:NSWallet.ManageFieldViewModel"/> class. Use to create the new field.
		/// </summary>
		/// <param name="navigation">Navigation.</param>
		public ManageFieldViewModel(INavigation navigation, NSWItem nswItem)
		{
			ViewMode = false;
			NotViewMode = true;

			IsEdit = false;

			manageFieldType = ManageFieldType.Create;
			Init(navigation, nswItem);
			ClosePopupCommandCallback = () => { };
			ShowPopupCommandCallback = () => { };
		}

		string humanReadableFieldValue = "";

		/// <summary>
		/// Initializes a new instance of the <see cref="T:NSWallet.ManageFieldViewModel"/> class. Use to update the existing field.
		/// </summary>
		/// <param name="navigation">Navigation.</param>
		/// <param name="nswItem">Item.</param>
		/// <param name="updFieldID">Update field identifier.</param>
		public ManageFieldViewModel(INavigation navigation, NSWItem nswItem, string updFieldID, bool viewMode, string fieldValue)
		{
			ViewMode = viewMode;
			NotViewMode = !viewMode;
			humanReadableFieldValue = fieldValue;
			manageFieldType = ManageFieldType.Edit;

			IsEdit = true;

			this.updFieldID = updFieldID;
			Init(navigation, nswItem);
			ClosePopupCommandCallback = () => { };

			if (fieldType != null) {
				switch (fieldType) {
					case GConsts.FLDTYPE_PHON:
						IsPhone = true;
						break;
					case GConsts.FLDTYPE_MAIL:
						IsMail = true;
						break;
					case GConsts.FLDTYPE_LINK:
						IsLink = true;
						break;
					case GConsts.FLDTYPE_2FAC:
						Is2FA = true;
						TwoFASourceCode = DefaultEditorText;
						break;
				}
			}

			setTOTP();
			startTOTPTimer();
		}

		string TwoFASourceCode;
		Totp totp;

		void setTOTP()
		{
			if (TwoFASourceCode != null) {
				var value = TwoFASourceCode.Replace(" ", "");
				var bytes = Base32Encoding.ToBytes(value);
				totp = new Totp(bytes);
				var code = totp.ComputeTotp();
				DefaultEditorText = code;
			}
		}

		void startTOTPTimer()
		{
			var timer = new System.Timers.Timer();
			timer.Elapsed += TOTPTimerElapsed;
			timer.AutoReset = true;
			timer.Enabled = true;
			timer.Interval = 100;
		}

		void TOTPTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			var timeRemaining = totp.RemainingSeconds();
			TwoFATimeLeftProgress = (double)timeRemaining / 30.0;
			var code = totp.ComputeTotp();
			DefaultEditorText = code;
		}

		void Init(INavigation navigationParm, NSWItem nswItemParm)
		{
			this.navigation = navigationParm;
			this.nswItem = nswItemParm;
			SetLabelsList();
			SetFirstLabel();
			MaximumPasswordLength = GConsts.MAX_PASS_LEN;
			MinimumPasswordLength = GConsts.MIN_PASS_LEN;
			//ValuePasswordLength = GConsts.DEF_PASS_LEN;
			PasswordLengthText = Settings.PassGenLength.ToString();
		}

		/// <summary>
		/// Sets the labels list.
		/// </summary>
		public void SetLabelsList()
		{
			try {
				var nswLabels = BL.GetLabelsByUsage();
				var nswFormsLabels = new List<NSWFormsLabelModel>();

				foreach (var nswLabel in nswLabels) {
					nswFormsLabels.Add(new NSWFormsLabelModel(nswLabel));
				}

				LabelsList = nswFormsLabels;
			} catch (Exception ex) {
				log(ex.Message, nameof(SetLabelsList));
			}
		}

		/// <summary>
		/// Sets the first label.
		/// </summary>
		public void SetFirstLabel()
		{
			PrepareScreenViewByType();
		}

		/// <summary>
		/// Sets the updating label.
		/// </summary>
		public void SetUpdatingLabel()
		{
			PrepareScreenViewByType();
		}

		string name;

		void PrepareScreenViewByType(object obj = null)
		{
			try {
				name = "";
				NSWFormsLabelModel nswFormsLabel;

				var nswLabels = BL.GetLabelsByUsage();

				// Preliminary checks to avoid crashes
				if (nswLabels == null)
					return;
				if (nswItem == null)
					return;
				if (!nswLabels.Any())
					return;

				if (obj != null) // obj should be checked first
				{
					nswFormsLabel = (NSWFormsLabelModel)obj;
				} else if (nswItem.Fields != null && !string.IsNullOrEmpty(updFieldID)) {
					var nswField = nswItem.Fields.Find((x) => x.FieldID == updFieldID);
					var nswFieldType = nswField.FieldType;
					var nswLabel = nswLabels[nswLabels.FindIndex(x => x.FieldType == nswFieldType)];
					name = nswField.FieldValue;
					nswFormsLabel = new NSWFormsLabelModel(nswLabel);
				} else // adding new, select the first one
				  {
					var nswLabel = nswLabels[0];
					nswFormsLabel = new NSWFormsLabelModel(nswLabel);
				}

				TypeIcon = nswFormsLabel.Icon;
				TypeName = nswFormsLabel.Name;
				fieldType = nswFormsLabel.FieldType;
				valueType = nswFormsLabel.ValueType;

				SetDefaultSettings();

				// Special fields should be precessed before special values
				// to avoid interferring
				if (string.Compare(fieldType, GConsts.FLDTYPE_NOTE) == 0) {
					SetNoteSettings(name);
				} else if (string.Compare(fieldType, GConsts.FLDTYPE_PASS) == 0) {
					SetPasswordSettings(name);
				} else if (string.Compare(fieldType, GConsts.FLDTYPE_OLDP) == 0) {
					SetOldPassSettings(name);
				} // Special values processing starts here
				  else if (string.Compare(valueType, GConsts.VALUETYPE_DATE) == 0) {
					SetDateSettings(name);
				} else if (string.Compare(valueType, GConsts.VALUETYPE_TIME) == 0) {
					SetTimeSettings(name);
				} else // Use default entry
				  {
					DefaultEditorText = name;
					convertedName = name;
				}

				// if called from popup (entry type is changes)
				if (obj != null) {
					ClosePopupCommand.Execute(null);
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(PrepareScreenViewByType));
			}
		}

		string convertedName = "";

		void SetDefaultSettings()
		{
			FieldDescription = "";
			IsDefaultLabel = true;
			IsDefaultEnabled = true;
			IsPassword = false;
			IsDateLabel = false;
			IsTimeLabel = false;
			IsNoteLabel = false;
		}

		void SetPasswordSettings(string passValue)
		{
			if (ViewMode) {
				IsPassword = false;
			} else {
				IsPassword = true;
			}
			convertedName = passValue;
			DefaultEditorText = passValue;
			FieldDescription = TR.Tr("clever_password_description");
		}

		void SetDateSettings(string dateValue)
		{
			if (string.IsNullOrEmpty(dateValue)) {
				DateEditorText = DateTime.Now;
			} else {
				DateEditorText = Common.ConvertDate(dateValue);
			}

			IsDateLabel = true;
			IsDefaultLabel = false;
		}

		void SetNoteSettings(string note)
		{
			IsNoteLabel = true;
			IsDefaultLabel = false;
			NoteEditorText = note;
		}

		void SetOldPassSettings(string oldPass)
		{
			FieldDescription = TR.Tr("previous_password_description");
			DefaultEditorText = oldPass;
			IsDefaultEnabled = false;
			IsDefaultLabel = false;
		}

		void SetTimeSettings(string timeValue)
		{
			if (string.IsNullOrEmpty(timeValue)) {
				TimeEditorText = DateTime.Now.TimeOfDay;
			} else {
				TimeSpan.TryParse(timeValue, out TimeSpan time);
				TimeEditorText = time;
			}

			IsTimeLabel = true;
			IsDefaultLabel = false;
			IsDefaultEnabled = false;
		}

		List<NSWFormsLabelModel> labelsList;
		public List<NSWFormsLabelModel> LabelsList {
			get { return labelsList; }
			set {
				if (labelsList == value)
					return;
				labelsList = value;
				OnPropertyChanged("LabelsList");
			}
		}

		object selectedNSWLabel;
		public object SelectedNSWLabel {
			get { return selectedNSWLabel; }
			set {
				if (selectedNSWLabel == value)
					return;
				selectedNSWLabel = value;
				if (selectedNSWLabel != null)
					SelectedNSWLabelCommand.Execute(selectedNSWLabel);
				OnPropertyChanged("SelectedNSWLabel");
			}
		}

		Command createLabelCommand;
		public Command CreateLabelCommand {
			get {
				return createLabelCommand ?? (createLabelCommand = new Command(ExecuteCreateLabelCommand));
			}
		}

		protected void ExecuteCreateLabelCommand()
		{
			LM.StartCreatingLabel(navigation, SuccessCreateLabelCommand);
		}

		Command successCreateLabelCommand;
		public Command SuccessCreateLabelCommand {
			get {
				return successCreateLabelCommand ?? (successCreateLabelCommand = new Command(ExecuteSuccessCreateLabelCommand));
			}
		}

		protected void ExecuteSuccessCreateLabelCommand(object obj)
		{
			if (obj != null) {
				var label = BL.GetLabels().FirstOrDefault(x => x.FieldType == obj.ToString());
				Device.BeginInvokeOnMainThread(() => Pages.ClosePage(navigation));
				SetLabelsList();
				PrepareScreenViewByType(new NSWFormsLabelModel(label));
			}
		}

		Command closePopupCommand;
		public Command ClosePopupCommand {
			get {
				return closePopupCommand ?? (closePopupCommand = new Command(ExecuteClosePopupCommand));
			}
		}

		protected void ExecuteClosePopupCommand()
		{
			ClosePopupCommandCallback.Invoke();
		}

		Command selectedNSWLabelCommand;
		public Command SelectedNSWLabelCommand {
			get {
				return selectedNSWLabelCommand ?? (selectedNSWLabelCommand = new Command(ExecuteSelectedNSWLabelCommand));
			}
		}

		protected void ExecuteSelectedNSWLabelCommand(object obj)
		{
			try {
				PrepareScreenViewByType(obj);

			} catch (Exception ex) {
				log(ex.Message, nameof(ExecuteSelectedNSWLabelCommand));
			}
		}

		Command passGenCommand;
		public Command PasswordGenerateCommand {
			get {
				return passGenCommand ?? (passGenCommand = new Command(GeneratePassword));
			}
		}

		protected void GeneratePassword()
		{
			if (!IsLowerCase && !IsUpperCase && !IsDigits && !IsSpecial) {
				PlatformSpecific.DisplayShortMessage(TR.Tr("manage_password_no_chosen"));
			} else {

				var length = Convert.ToInt32(PasswordLengthText);
				DefaultEditorText = Security.GeneratePassword(IsLowerCase, IsUpperCase, IsDigits, IsSpecial, length);
			}
		}

		Command cleverGenCommand;
		public Command CleverGenerateCommand {
			get {
				return cleverGenCommand ?? (cleverGenCommand = new Command(CleverGeneratePassword));
			}
		}

		protected void CleverGeneratePassword()
		{
			DefaultEditorText = Security.GenerateCleverPassword(DefaultEditorText);
		}

		Command saveFieldCommand;
		public Command SaveFieldCommand {
			get {
				return saveFieldCommand ?? (saveFieldCommand = new Command(ExecuteSaveFieldCommand));
			}
		}

		protected void ExecuteSaveFieldCommand()
		{
			try {
				string updText = null;

				if (string.Compare(valueType, GConsts.VALUETYPE_DATE) == 0) {
					updText = Common.ConvertFromDate(DateEditorText);
				} else if (string.Compare(valueType, GConsts.VALUETYPE_TIME) == 0) {
					updText = TimeEditorText.ToString("c");
				} else if (string.Compare(fieldType, GConsts.FLDTYPE_NOTE) == 0) {
					updText = NoteEditorText;
				} else {
					updText = DefaultEditorText;
					/*
                    if (!string.IsNullOrEmpty(DefaultEditorText))
                    {
                        
                    }
                    else
                    {
                        success = false;
                    }
                    */
				}

				//if (success)
				//{
				if (nswItem != null) {
					//int weight = 100;

					if (manageFieldType.Equals(ManageFieldType.Create)) {
						if (fieldType == GConsts.FLDTYPE_OLDP) {
							var currentItem = BL.GetCurrentItem();
							var field = currentItem.Fields.Find(x => x.FieldType == GConsts.FLDTYPE_OLDP);
							if (field != null) {
								PlatformSpecific.DisplayShortMessage(TR.Tr("previous_password_duplicate"));
								return;
							}
						}

						BL.AddField(fieldType, updText);
					}

					if (manageFieldType.Equals(ManageFieldType.Edit)) {
						BL.UpdateField(updFieldID, updText, DataAccessLayer.DO_NOT_CHANGE_SORT);
					}
				}

				MessagingCenter.Send(this, "/reloaditems");
				Pages.CloseModalPage(navigation);
				//}
			} catch (Exception ex) {
				log(ex.Message, nameof(ExecuteSaveFieldCommand));
			}
		}

		Command showPopupCommand;
		public Command ShowPopupCommand {
			get {
				return showPopupCommand ?? (showPopupCommand = new Command(ExecuteShowPopupCommand));
			}
		}

		protected void ExecuteShowPopupCommand()
		{
			if (!manageFieldType.Equals(ManageFieldType.Edit))
				ShowPopupCommandCallback.Invoke();
		}

		Command plusCommand;
		public Command PlusCommand {
			get {
				return plusCommand ?? (plusCommand = new Command(ExecutePlusCommand));
			}
		}

		protected void ExecutePlusCommand()
		{
			var length = Convert.ToInt32(PasswordLengthText);
			if (MaximumPasswordLength != length) {
				length++;
				PasswordLengthText = length.ToString();
				Settings.PassGenLength = length;
			}
		}

		Command minusCommand;
		public Command MinusCommand {
			get {
				return minusCommand ?? (minusCommand = new Command(ExecuteMinusCommand));
			}
		}

		protected void ExecuteMinusCommand()
		{
			var length = Convert.ToInt32(PasswordLengthText);
			if (MinimumPasswordLength != length) {
				length--;
				PasswordLengthText = length.ToString();
				Settings.PassGenLength = length;
			}
		}

		ImageSource typeIcon;
		public ImageSource TypeIcon {
			get { return typeIcon; }
			set {
				if (typeIcon == value)
					return;
				typeIcon = value;
				OnPropertyChanged("TypeIcon");
			}
		}

		double twoFATimeLeftProgress;
		public double TwoFATimeLeftProgress {
			get { return twoFATimeLeftProgress; }
			set {
				if (twoFATimeLeftProgress == value)
					return;
				twoFATimeLeftProgress = value;
				OnPropertyChanged("TwoFATimeLeftProgress");
			}
		}

		string typeName;
		public string TypeName {
			get { return typeName; }
			set {
				if (typeName == value)
					return;
				typeName = value;
				OnPropertyChanged("TypeName");
			}
		}

		bool isDateLabel;
		public bool IsDateLabel {
			get { return isDateLabel; }
			set {
				if (isDateLabel == value)
					return;
				isDateLabel = value;
				OnPropertyChanged("IsDateLabel");
			}
		}

		bool isDefaultEnabled;
		public bool IsDefaultEnabled {
			get { return isDefaultEnabled; }
			set {
				if (isDefaultEnabled == value)
					return;
				isDefaultEnabled = value;
				OnPropertyChanged("IsDefaultEnabled");
			}
		}

		bool isTimeLabel;
		public bool IsTimeLabel {
			get { return isTimeLabel; }
			set {
				if (isTimeLabel == value)
					return;
				isTimeLabel = value;
				OnPropertyChanged("IsTimeLabel");
			}
		}

		bool isNoteLabel;
		public bool IsNoteLabel {
			get { return isNoteLabel; }
			set {
				if (isNoteLabel == value)
					return;
				isNoteLabel = value;
				OnPropertyChanged("IsNoteLabel");
			}
		}

		bool isPassword;
		public bool IsPassword {
			get { return isPassword; }
			set {
				if (isPassword == value)
					return;
				isPassword = value;
				OnPropertyChanged("IsPassword");

			}
		}

		bool isDefaultLabel;
		public bool IsDefaultLabel {
			get { return isDefaultLabel; }
			set {
				if (isDefaultLabel == value)
					return;
				isDefaultLabel = value;
				OnPropertyChanged("IsDefaultLabel");
			}
		}

		string noteEditorText;
		public string NoteEditorText {
			get { return noteEditorText; }
			set {
				if (noteEditorText == value)
					return;
				noteEditorText = value;
				OnPropertyChanged("NoteEditorText");
			}
		}

		string fieldDescription;
		public string FieldDescription {
			get { return fieldDescription; }
			set {
				if (fieldDescription == value)
					return;
				fieldDescription = value;
				OnPropertyChanged("FieldDescription");
			}
		}

		DateTime dateEditorText;
		public DateTime DateEditorText {
			get { return dateEditorText; }
			set {
				if (dateEditorText == value)
					return;
				dateEditorText = value;
				OnPropertyChanged("DateEditorText");
			}
		}

		bool is2FA;
		public bool Is2FA {
			get { return is2FA; }
			set {
				if (is2FA == value)
					return;
				is2FA = value;
				OnPropertyChanged("Is2FA");
			}
		}

		bool isLink;
		public bool IsLink {
			get { return isLink; }
			set {
				if (isLink == value)
					return;
				isLink = value;
				OnPropertyChanged("IsLink");
			}
		}

		bool isMail;
		public bool IsMail {
			get { return isMail; }
			set {
				if (isMail == value)
					return;
				isMail = value;
				OnPropertyChanged("IsMail");
			}
		}

		bool isPhone;
		public bool IsPhone {
			get { return isPhone; }
			set {
				if (isPhone == value)
					return;
				isPhone = value;
				OnPropertyChanged("IsPhone");
			}
		}

		TimeSpan timeEditorText;
		public TimeSpan TimeEditorText {
			get { return timeEditorText; }
			set {
				if (timeEditorText == value)
					return;
				timeEditorText = value;
				OnPropertyChanged("TimeEditorText");
			}
		}

		string defaultEditorText;
		public string DefaultEditorText {
			get { return defaultEditorText; }
			set {
				if (defaultEditorText == value)
					return;
				defaultEditorText = value;
				OnPropertyChanged("DefaultEditorText");
			}
		}

		public bool IsCreate {
			get {
				if (manageFieldType.Equals(ManageFieldType.Create))
					return true;

				return false;
			}
		}

		bool isEdit;
		public bool IsEdit {
			get {
				return isEdit;
			}
			set {
				if (isEdit == value)
					return;
				isEdit = value;
				OnPropertyChanged("IsEdit");
			}
		}

		int maximumPasswordLength;
		public int MaximumPasswordLength {
			get { return maximumPasswordLength; }
			set {
				if (maximumPasswordLength == value)
					return;
				maximumPasswordLength = value;
				OnPropertyChanged("MaximumPasswordLength");
			}
		}

		int minimumPasswordLength;
		public int MinimumPasswordLength {
			get { return minimumPasswordLength; }
			set {
				if (minimumPasswordLength == value)
					return;
				minimumPasswordLength = value;
				OnPropertyChanged("MinimumPasswordLength");
			}
		}

		/*
        int valuePasswordLength;
        public int ValuePasswordLength
        {
            get { return valuePasswordLength; }
            set
            {
                if (valuePasswordLength == value)
                    return;
                valuePasswordLength = value;
                PasswordLengthText = valuePasswordLength.ToString();
                OnPropertyChanged("ValuePasswordLength");
            }
        }
        */

		string passwordLengthText;
		public string PasswordLengthText {
			get { return passwordLengthText; }
			set {
				if (passwordLengthText == value)
					return;
				passwordLengthText = value;
				OnPropertyChanged("PasswordLengthText");
			}
		}

		bool isUpperCase;
		public bool IsUpperCase {
			get { return Settings.ManageUpperCase; }
			set {
				if (isUpperCase == value)
					return;
				isUpperCase = value;
				Settings.ManageUpperCase = value;
				OnPropertyChanged("IsUpperCase");
			}
		}

		bool isLowerCase;
		public bool IsLowerCase {
			get { return Settings.ManageLowerCase; }
			set {
				if (isLowerCase == value)
					return;
				isLowerCase = value;
				Settings.ManageLowerCase = value;
				OnPropertyChanged("IsLowerCase");
			}
		}

		bool isDigits;
		public bool IsDigits {
			get { return Settings.ManageDigits; }
			set {
				if (isDigits == value)
					return;
				isDigits = value;
				Settings.ManageDigits = value;
				OnPropertyChanged("IsDigits");
			}
		}

		bool isSpecial;
		public bool IsSpecial {
			get { return Settings.ManageSpecialSymbols; }
			set {
				if (isSpecial == value)
					return;
				isSpecial = value;
				Settings.ManageSpecialSymbols = value;
				OnPropertyChanged("IsSpecial");
			}
		}

		bool viewMode;
		public bool ViewMode {
			get { return viewMode; }
			set {
				if (viewMode == value)
					return;
				viewMode = value;
				OnPropertyChanged("ViewMode");
				NotViewMode = !viewMode;
			}
		}

		bool notViewMode;
		public bool NotViewMode {
			get { return notViewMode; }
			set {
				if (notViewMode == value)
					return;
				notViewMode = value;
				OnPropertyChanged("NotViewMode");
			}
		}

		Command editCommand;
		public Command EditCommand {
			get {
				return editCommand ?? (editCommand = new Command(ExecuteEditCommand));
			}
		}

		private void ExecuteEditCommand()
		{
			ViewMode = !ViewMode;
			IsLink = false;
			IsMail = false;
			IsPhone = false;

			IsEdit = false;

			if (fieldType == GConsts.FLDTYPE_PASS) {
				if (ViewMode) {
					IsPassword = false;
				} else {
					IsPassword = true;
				}
			}
		}

		Command copyClipCommand;
		public Command CopyClipCommand {
			get {
				return copyClipCommand ?? (copyClipCommand = new Command(ExecuteCopyClipCommand));
			}
		}

		private void ExecuteCopyClipCommand()
		{
			if (humanReadableFieldValue != null) {
				PlatformSpecific.CopyToClipboard(humanReadableFieldValue);
				PlatformSpecific.DisplayShortMessage(TR.Tr("field_clipboard_copied"));
				Pages.CloseModalPage(navigation);
			}
		}

		Command copyLocallyCommand;
		public Command CopyLocallyCommand {
			get {
				return copyLocallyCommand ?? (copyLocallyCommand = new Command(ExecuteCopyLocallyCommand));
			}
		}

		private void ExecuteCopyLocallyCommand()
		{
			Device.BeginInvokeOnMainThread(async () => {
				var res = await Application.Current.MainPage.DisplayActionSheet(null, "cancel", null, "Copy to clipboard", "Copy locally");

				if (res == "Copy to clipboard") {

					CopyClipCommand.Execute(null);
				}

				if (res == "Copy locally") {
					StateHandler.CopyFieldLocallyActivated = true;
					Pages.CloseModalPage(navigation);
				}
			});
		}

		Command deleteFieldCommand;
		public Command DeleteFieldCommand {
			get {
				return deleteFieldCommand ?? (deleteFieldCommand = new Command(ExecuteDeleteFieldCommand));
			}
		}

		private void ExecuteDeleteFieldCommand()
		{
			StateHandler.DeleteFieldActivated = true;
			Pages.CloseModalPage(navigation);
		}

		bool isPhoneNumber(string number)
		{
			return Regex.Match(number, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").Success;
		}

		Command dialCommand;
		public Command DialCommand {
			get {
				return dialCommand ?? (dialCommand = new Command(ExecuteDialCommand));
			}
		}

		void ExecuteDialCommand()
		{
			var fieldValue = humanReadableFieldValue;
			if (isPhoneNumber(fieldValue)) {
				PlatformSpecific.OpenPhoneDialer(fieldValue);

			}
		}

		Command mailCommand;
		public Command MailCommand {
			get {
				return mailCommand ?? (mailCommand = new Command(ExecuteMailCommand));
			}
		}

		void ExecuteMailCommand()
		{
			var fieldValue = humanReadableFieldValue;
		    //Device.OpenUri(new Uri(String.Format("mailto:{0}", fieldValue)));
			try {
				List<string> recipients = new List<string> {
					fieldValue
				};
				var message = new EmailMessage {
					//Subject = subject,
					//Body = body,
					To = recipients,
					//Cc = ccRecipients,
					//Bcc = bccRecipients
				};
				Email.ComposeAsync(message);
			} catch (FeatureNotSupportedException fbsEx) {
				log(fbsEx.Message);
			} catch (Exception ex) {
				log(ex.Message);
			}
		}

		Command linkCommand;
		public Command LinkCommand {
			get {
				return linkCommand ?? (linkCommand = new Command(ExecuteLinkCommand));
			}
		}

		void ExecuteLinkCommand()
		{
			var fieldValue = humanReadableFieldValue;
			if (!(fieldValue.ToLower().StartsWith("http://", StringComparison.Ordinal) ||
				fieldValue.ToLower().StartsWith("https://", StringComparison.Ordinal))) {
				fieldValue = "http://" + fieldValue;
			}
			Browser.OpenAsync(new Uri(fieldValue), BrowserLaunchMode.SystemPreferred);
			//Device.OpenUri();
		}

		void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(ManageFieldViewModel));
		}
	}
}