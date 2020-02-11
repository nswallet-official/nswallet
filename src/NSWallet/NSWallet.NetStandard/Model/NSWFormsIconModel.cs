using System;
using System.IO;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class NSWFormsIconModel
    {
        public readonly NSWIcon IconData;

        public string IconID
        {
            get
            {
                if (IconData != null)
                {
                    return IconData.IconID;
                }

                return "ICON IS NOT SET";
            }
        }

        public string Name
        {
            get
            {
                if (IconData != null)
                {
                    return IconData.Name;
                }

                return "ICON IS NOT SET";
            }
        }

        public int GroupID
        {
            get
            {
                if (IconData != null)
                {
                    return IconData.GroupID;
                }

                return -1;
            }
        }

        public ImageSource Icon
        {
            get
            {
                if (IconData != null)
                {
                    var imgSource = ImageSource.FromStream(() => new MemoryStream(IconData.Icon));
                    if (imgSource != null)
                        return imgSource;

                    return ImageSource.FromStream(() => NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON));
                }

                return ImageSource.FromStream(() => NSWRes.GetImage(GConsts.DEFAULT_ITEM_ICON));
            }
        }

        public NSWFormsIconModel(NSWIcon nswIcon)
        {
            IconData = nswIcon;
        }
    }
}