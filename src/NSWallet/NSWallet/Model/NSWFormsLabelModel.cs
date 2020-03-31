using System;
using System.IO;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class NSWFormsLabelModel
    {
        readonly NSWLabel nswLabel;

        public NSWFormsLabelModel(NSWLabel label)
        {
            nswLabel = label;
        }

        public string FieldType
        {
            get
            {
                if (nswLabel != null)
                {
                    return nswLabel.FieldType;
                }

                return "LABEL IS NOT SET";
            }
        }

        public string Name
        {
            get
            {
                if (nswLabel != null)
                {
                    if (nswLabel.System == true)
                    {
                        return TR.Tr(nswLabel.FieldType);
                    }
                    else
                    {
                        return nswLabel.Name;
                    }
                }

                return "LABEL IS NOT SET";
            }
        }

        public string ValueType
        {
            get
            {
                if (nswLabel != null)
                {
                    return nswLabel.ValueType;
                }

                return "LABEL IS NOT SET";
            }
        }

        public string ValueTypeHumanReadable
        {
            get
            {
                if (nswLabel != null)
                {
                    return TR.Tr(nswLabel.ValueType);
                }

                return "LABEL IS NOT SET";
            }
        }

        public string IconString
        {
            get
            {
                if (nswLabel != null)
                {
                    return nswLabel.Icon;
                }

                return "LABEL IS NOT SET";
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

                if (nswLabel != null)
                {
                    icon = ImageSource.FromStream(GetIconLambdaStream(nswLabel.Icon));
                }

                return icon;
            }
        }

        Func<Stream> GetIconLambdaStream(string iconName)
        {
            var stream = NSWRes.GetImage(iconName);

            if (stream != null)
            {
                return () => NSWRes.GetImage(nswLabel.Icon);
            }
            else
            {
                return () => NSWRes.GetImage(GConsts.DEFAULT_LABEL_ICON);
            }
        }

        public bool System
        {
            get
            {
                if (nswLabel != null)
                {
                    return nswLabel.System;
                }

                return false;
            }
        }

        public string UpdateTimestamp
        {
            get
            {
                if (nswLabel != null)
                {
                    return nswLabel.UpdateTimestamp.ToString();
                }

                return null;
            }
        }

        public string AutomationId
        {
            get
            {
                return nswLabel.FieldType;
            }
        }

    }
}
