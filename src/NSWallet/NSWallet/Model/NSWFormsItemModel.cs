using System;
using System.Collections.Generic;
using Xamarin.Forms;
using NSWallet.Shared;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using NSWallet.Helpers;
using NSWallet.Shared.Helpers.Logs.AppLog;
using System.Linq;
using NSWallet.NetStandard.Helpers.FA;

namespace NSWallet
{
    public class NSWFormsItemModel
    {
        public enum ItemTypes
        {
            Item,
            Folder,
            Field
        }

        public readonly NSWItem ItemData;

        public readonly NSWField FieldData;

        ItemTypes itemType;

        public ItemTypes ItemType { get { return itemType; } }

		public bool IsField {
			get {
				return itemType == ItemTypes.Field;
			}
		}

        public string ItemID
        {
            get
            {
                if (itemType == ItemTypes.Field && FieldData != null)
                {
                    return FieldData.ItemID;
                }

                if (ItemData != null)
                {
                    return ItemData.ItemID;
                }

                return "ITEM IS NOT SET";
            }
        }

        public string FieldID
        {
            get
            {
                if (FieldData != null)
                {
                    return FieldData.FieldID;
                }

                return null;
            }
        }

        public string ParentID
        {
            get
            {
                if (ItemData != null)
                {
                    return ItemData.ParentID;
                }

                return "ITEM IS NOT SET";
            }
        }

        public string FieldType
        {
            get
            {
                if (itemType == ItemTypes.Field && FieldData != null)
                {
                    return FieldData.FieldType;
                }

                return null;
            }
        }

        public string ValueType
        {
            get
            {
                if (itemType == ItemTypes.Field && FieldData != null)
                {
                    return FieldData.ValueType;
                }

                return null;
            }
        }

		public string Value {
			get {
				if (itemType == ItemTypes.Field && FieldData != null) {
					return FieldData.FieldValue;
				}

				if (ItemData != null) {
					return ItemData.Name;
				}

				return "ITEM IS NOT SET";
			}
		}

		public string HumanReadableValue {
			get {
				if (itemType == ItemTypes.Field && FieldData != null) {
					return FieldData.HumanReadableValue;
				}

				if (ItemData != null) {
					return ItemData.Name;
				}

				return "ITEM IS NOT SET";
			}
		}


		public string Name
        {
            get
            {
                if (itemType == ItemTypes.Field && FieldData != null)
                {
                    var fieldValue = FieldData.HumanReadableValue;

					if (string.Compare(FieldData.FieldType, GConsts.FLDTYPE_2FAC) == 0) {
						return Common.ConvertStringToAsterisks(fieldValue);
					}

					if (string.Compare(FieldData.ValueType, GConsts.VALUETYPE_PASS) == 0 && Settings.IsHidePasswordEnabled) {
						return Common.ConvertStringToAsterisks(fieldValue);
					}

                    return fieldValue;
                }

                if (ItemData != null)
                {
                    return ItemData.Name;
                }

                return "ITEM IS NOT SET";
            }
        }

        public List<NSWField> Fields
        {
            get
            {
                if (ItemData != null)
                {
                    return ItemData.Fields;
                }

                return null;
            }
        }

        public string LowAdditionalRow
        {
            get
            {
				var sysFormat = CultureInfo.CurrentCulture.DateTimeFormat;

				if (itemType == ItemTypes.Field && FieldData != null)
                {
                    // Below is workaround to not duplicate system label names in translations
                    // Do not change if you do not understand how it is working
                    string typeTranslation = TR.Tr(FieldData.FieldType);
                    if (typeTranslation == FieldData.FieldType)
                    {
                        // No translation found, use label name then
                        return FieldData.Label;
                    }

                    return typeTranslation;

                }

                if (ItemData != null)
                {
                    if (ItemData.Expired)
                    {
                        return TR.Tr("expired");
                    }

                    if (ItemData.ExpireInDays > 0)
                    {
                        if (ItemData.ExpireInDays == 1)
                        {
                            return TR.Tr("expire_in1");
                        }
                        return TR.Tr("expire_in").Replace("{{DAYS}}", (ItemData.ExpireInDays - 1).ToString());
                    }

					/*
                    var defaultTime = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString();

                    if (string.Compare(ItemData.UpdateTimestamp.ToString(), defaultTime) == 0)
                    {
                        return null;
                    }*/

					return ItemData.UpdateTimestamp.ToString(sysFormat);
                }

                return DateTime.Now.ToString(sysFormat);
            }
        }

		public bool IsImageCircle {
			get {
				if (ItemType == ItemTypes.Item) {
					var icons = BL.GetIcons();
					var customIcon = icons.SingleOrDefault(x => ItemData.Icon.Contains(x.IconID));
					if (customIcon != null) {
						if (customIcon.IsCircle) {
							return true;
						}
					}
				}
				return false;
			}
		}

		public bool IsNotImageCircle {
			get {
				return !IsImageCircle;
			}
		}

		ImageSource icon;
        public ImageSource Icon
        {
            get
            {
                if (icon != null)
                {
                    return icon;
                }
                if (itemType == ItemTypes.Field && FieldData != null)
                {
                    icon = ImageSource.FromStream(() => NSWRes.GetImage(FieldData.Icon));
                }
                else if (ItemData != null && ItemType == ItemTypes.Folder)
                {
                    icon = ImageSource.FromStream(GetIconLambdaStream(ItemData.Icon, true));
                }
                else if (ItemData != null && ItemType == ItemTypes.Item)
                {
                    try
                    {
                        var image = NSWRes.GetImage(ItemData.Icon);
                        if (image != null)
                            icon = ImageSource.FromStream(GetIconLambdaStream(ItemData.Icon, false));
                        else
                        {
                            // FIXME: the same code as in MainScreenViewModel.cs: 802, unify!
                            var customIcons = BL.GetIcons();
                            var customIcon = customIcons.Find(x => ItemData.Icon.Contains(x.IconID));
                            if (customIcon != null)
                            {
                                icon = ImageSource.FromStream(() => new MemoryStream(customIcon.Icon));
                            }
                            else
                            {
                                icon = ImageSource.FromStream(() => NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON));
                            }
                        }
					} catch(Exception ex) {
                        icon = ImageSource.FromStream(() => NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON));
						AppLogs.Log(ex.Message, nameof(Icon), nameof(NSWFormsItemModel));
                    }
                }

                return icon;
            }
        }

        Func<Stream> GetIconLambdaStream(string iconName, bool folder)
        {
            var stream = NSWRes.GetImage(iconName);

            if (!folder)
            {
                if (stream != null)
                {
                    return () => NSWRes.GetImage(ItemData.Icon);
                }
                else
                {
                    return () => NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON);
                }
            }
            else
            {
                if (stream != null)
                {
                    return () => NSWRes.GetImage(ItemData.Icon);
                }
                else
                {
                    return () => NSWRes.GetImage(GConsts.DEFAULT_FOLDER_ICON);
                }
            }
        }

        Command upFieldCommand;
        public Command UpFieldCommand
        {
            get
            {
                return upFieldCommand ?? (upFieldCommand = new Command(ExecuteUpFieldCommand));
            }
        }

        void ExecuteUpFieldCommand()
        {
            var reorderVM = ReorderVMStorage.ViewModel;
            if (reorderVM != null)
            {
                reorderVM.SwapUpCommand.Execute(FieldID);
            }
        }

        Command downFieldCommand;
        public Command DownFieldCommand
        {
            get
            {
                return downFieldCommand ?? (downFieldCommand = new Command(ExecuteDownFieldCommand));
            }
        }

        void ExecuteDownFieldCommand()
        {
            var reorderVM = ReorderVMStorage.ViewModel;
            if (reorderVM != null)
            {
                reorderVM.SwapDownCommand.Execute(FieldID);
            }
        }

        public NSWFormsItemModel(NSWItem item)
        {
            ItemData = item;

            if (item.Folder == true)
            {
                itemType = ItemTypes.Folder;
            }
            else
            {
                itemType = ItemTypes.Item;
            }
        }

        public NSWFormsItemModel(NSWField field)
        {
            FieldData = field;
            itemType = ItemTypes.Field;
        }

		Command copyValueCommand;
		public Command CopyValueCommand {
			get {
				return copyValueCommand ?? (
					copyValueCommand = new Command(ExecuteCopyValueCommand));
			}
		}

		void ExecuteCopyValueCommand()
		{
			if (itemType == ItemTypes.Field && FieldData != null) {
				var fieldType = FieldData.FieldType;
				if (fieldType != null && string.Compare(fieldType, GConsts.FLDTYPE_2FAC) == 0) {
					var code = computeTOTP();
					if (code != null) {
						PlatformSpecific.CopyToClipboard(code);
					}
				} else {
					PlatformSpecific.CopyToClipboard(HumanReadableValue);
				}

				PlatformSpecific.DisplayShortMessage(TR.Tr("field_clipboard_copied"));
			}
		}

		string computeTOTP()
		{
			var value = Value.Replace(" ", "");
			var bytes = Base32Encoding.ToBytes(value);
			var totp = new Totp(bytes);
			return totp.ComputeTotp();
		}
	}
}
