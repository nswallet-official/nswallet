using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using NSWallet.Shared;
using NSWallet.Helpers;
using NSWallet.NetStandard.Helpers;
using System.Linq;

namespace NSWallet
{
    public class LoginScreenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Action TipAlertCommandCallback { get; set; }

        INavigation navigation;

        public LoginScreenViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            if (Settings.IsClipboardClean)
                PlatformSpecific.CleanClipboard();

            Features = new List<FeatureModel>
            {
                new FeatureModel
                {
                    Image = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ICON_PREMIUM_SEARCH)),
                    Title = TR.Tr("featuresource_search"),
                    Description = TR.Tr("featuresource_search_description")
                },

                new FeatureModel
                {
                    Image = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ICON_PREMIUM_THEMES)),
                    Title = TR.Tr("featuresource_themes"),
                    Description = TR.Tr("featuresource_themes_description")
                },

                new FeatureModel
                {
                    Image = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ICON_PREMIUM_SPECIAL)),
                    Title = TR.Tr("featuresource_special_folders"),
                    Description = TR.Tr("featuresource_special_folders_description")
                },

                new FeatureModel
                {
                    Image = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ICON_PREMIUM_FEEDBACK)),
                    Title = TR.Tr("featuresource_feedback"),
                    Description = TR.Tr("featuresource_feedback_description")
                },

                new FeatureModel
                {
                    Image = ImageSource.FromStream(() => NSWRes.GetImage(Theme.Current.ICON_PREMIUM_OTHER)),
                    Title = TR.Tr("featuresource_other"),
                    Description = TR.Tr("featuresource_other_description")
                }
            };

            var isNotNew = !BL.IsNew();
            if (isNotNew)
            {
                IsNotNew = isNotNew;
            }

            TipAlertCommandCallback = () => { };
        }

        bool checkNewBuild()
        {
            var currentBuild = PlatformSpecific.GetBuildNumber();
            if (currentBuild.Equals(Settings.Build))
            {
                return false;
            }
            Settings.Build = currentBuild;
            return true;
        }

        int count = 0;
        void CountFailedSigning()
        {
            count++;
            if (count == 3)
            {
                TipAlertCommandCallback.Invoke();
                count = 0;
            }
        }

        bool isNewBuild;
        public bool IsNewBuild
        {
            get { return checkNewBuild(); }
            set
            {
                if (isNewBuild == value)
                    return;
                isNewBuild = value;
                OnPropertyChanged("IsNewBuild");
            }
        }

        string releaseNotes;
        public string ReleaseNotes
        {
            get { return TR.Tr("release_notes") + " " + PlatformSpecific.GetVersion(); }
            set
            {
                if (releaseNotes == value)
                    return;
                releaseNotes = value;
                OnPropertyChanged("ReleaseNotes");
            }
        }

        string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password == value)
                    return;
                password = value;
                if (Settings.IsAutoLoginEnabled)
                    passwordAutoLogin();
                OnPropertyChanged("Password");
            }
        }

        void passwordAutoLogin()
        {
			if (!BL.IsNew())
			{
                if (!string.IsNullOrEmpty(Password))
                {
                    if (BL.CheckPassword(Password))
                    {
                        Device.BeginInvokeOnMainThread(() => Pages.Main());
                    }
                }
            }
        }

        string checkPassword;
        public string CheckPassword
        {
            get { return checkPassword; }
            set
            {
                if (checkPassword == value)
                    return;
                checkPassword = value;
                OnPropertyChanged("CheckPassword");
            }
        }

        int animationStatus;
        public int AnimationStatus
        {
            get { return animationStatus; }
            set
            {
                if (animationStatus == value)
                    return;
                animationStatus = value;
                OnPropertyChanged("AnimationStatus");
            }
        }

        bool isNew;
        public bool IsNotNew
        {
            get { return isNew; }
            set
            {
                if (isNew == value)
                    return;
                isNew = value;
                OnPropertyChanged("IsNew");
            }
        }

        List<FeatureModel> features;
        public List<FeatureModel> Features
        {
            get { return features; }
            set
            {
                if (features == value)
                    return;
                features = value;
                OnPropertyChanged("Features");
            }
        }

        Command loginCommand;
        public Command LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new Command(ExecuteLoginCommand));
            }
        }

        protected void ExecuteLoginCommand(object obj)
        {
			if (LicenseController.CheckPrivacyPolicy()) {
				if (LicenseController.CheckTermsOfUse()) {
					AnimationStatus = 0; // 0 - Nothing happened

					if (BL.IsNew()) {
						if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(CheckPassword)) {
							if (Password.Length >= GConsts.MASTER_PASSWORD_RESTRICTION || CheckPassword.Length >= GConsts.MASTER_PASSWORD_RESTRICTION) {
								if (string.Compare(Password, CheckPassword) == 0) {
									BL.CreateOnlyRootItem(Password);
									BL.CreateSampleItems();
									Settings.ChangePasswordUnicodeIOSBug = true;
									Pages.Main();
								} else {
									showErrorMessage(TR.Tr("create_password_not_match"));
								}
							} else {
								showErrorMessage(TR.Tr("create_password_short"));
							}
						} else {
							showErrorMessage(TR.Tr("create_password_empty"));
						}
					} else {
						if (!string.IsNullOrEmpty(Password)) {
							if (BL.CheckPassword(Password)) {
								if (obj != null) {
									var fingerAuth = (bool)obj;
								} else {
									FingerprintHelper.ResetSettings(true, false, false);
									Settings.RememberedPassword = Password;
									if (Settings.UsedFingerprintBefore)
										Settings.IsFingerprintActive = true;
								}
							Pages.Main();
								//isUnicodePassword()
								//Device.BeginInvokeOnMainThread(() => Pages.Main(isUnicodePassword()));
							} else {
								Password = string.Empty;
								AnimationStatus = 1; // 1 - Failed to login
								if (!string.IsNullOrEmpty(Settings.PasswordTip))
									CountFailedSigning();
							}
						}
					}
				}
			}
        }

		// FIXME: remove as soon as possible
		// This is workaround created to avoid decryption bug in old iOS app when first byte in non ASCII
		// chars was replaced with /x0
		/*
		bool isUnicodePassword()
		{
			if (Settings.ChangePasswordUnicodeIOSBug)
				return false;
			bool isUnicode = false;
			if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS) {
				if (Password.Length > 0) {
					isUnicode |= containsUnicodeCharacter(
						Password[0].ToString()
					);
				}
			}
			return isUnicode;
		}
		*/

		bool containsUnicodeCharacter(string input)
		{
			const int MaxAnsiCode = 255;
			return input.Any(c => c > MaxAnsiCode);
		}

		void clearFields()
        {
            Password = null;
            CheckPassword = null;
        }

        void showErrorMessage(string msg)
        {
            PlatformSpecific.DisplayShortMessage(msg);
            clearFields();
        }

        Command buyPremiumCommand;
        public Command BuyPremiumCommand
        {
            get
            {
                return buyPremiumCommand ?? (buyPremiumCommand = new Command(ExecutePremiumCommand));
            }
        }

        void ExecutePremiumCommand()
        {
            Pages.Premium(navigation);
        }

        Command releaseCommand;
        public Command ReleaseCommand
        {
            get
            {
                return releaseCommand ?? (releaseCommand = new Command(ExecuteReleaseCommand));
            }
        }

        void ExecuteReleaseCommand()
        {
            Device.BeginInvokeOnMainThread(() => Device.OpenUri(new Uri(GConsts.APP_DEV_RELEASE_NOTES_URI)));
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}