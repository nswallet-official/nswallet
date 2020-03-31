using System;
using System.Collections.Generic;
using System.ComponentModel;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class CreateLabelScreenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string labelName, labelType, fieldType, actionType;
        INavigation navigation;
        Command command;

        public CreateLabelScreenViewModel(INavigation navigation, string labelName, string labelType, string fieldType = null, string actionType = null, Command command = null)
        {
            IconsList = new List<IconModel>();
            this.command = command;
            this.labelName = labelName;
            this.labelType = labelType;
            this.navigation = navigation;
            this.fieldType = fieldType;
            this.actionType = actionType;

            var resources = NSWRes.GetResourceNames();

            foreach (var resource in resources)
            {
                if (resource.Contains("labels.icon_"))
                {
                    var iconPath = resource.Substring(38);

                    if (iconPath.Contains("empty"))
                    {
                        IconsList.Insert(0, new IconModel
                        {
                            Image = ImageSource.FromStream(() => NSWRes.GetImage(iconPath)),
                            Path = iconPath
                        });
                    }
                    else
                    {
                        IconsList.Add(new IconModel
                        {
                            Image = ImageSource.FromStream(() => NSWRes.GetImage(iconPath)),
                            Path = iconPath
                        });
                    }
                }
            }
        }

        Command createCommand;
        public Command CreateCommand
        {
            get
            {
                return createCommand ?? (createCommand = new Command(ExecuteCreateCommand));
            }
        }

        protected void ExecuteCreateCommand(object obj)
        {

            var iconModel = (IconModel)obj;

            if (iconModel != null)
            {
                if (actionType == "/edit")
                {
                    var icon = iconModel.Path.Substring(18, iconModel.Path.Length - 27);
                    BL.UpdateLabelIcon(fieldType, icon);
                    Device.BeginInvokeOnMainThread(() => AppPages.ClosePage(navigation));
                    if (command != null)
                        command.Execute(null);
                    //Pages.LabelsManagement();
                }
                else
                {
                    if (!string.IsNullOrEmpty(iconModel.Path))
                    {
                        if (labelType != null)
                        {
                            var icon = iconModel.Path.Substring(18, iconModel.Path.Length - 27);
                            var id = BL.AddLabel(labelName, icon, labelType);
                            if (command != null)
                                command.Execute(id);
                            //Pages.LabelsManagement();
                        }
                    }
                }
            }
        }

        List<IconModel> iconsList;
        public List<IconModel> IconsList
        {
            get
            {
                return iconsList;
            }
            set
            {
                if (iconsList == value)
                    return;
                iconsList = value;
                OnPropertyChanged("IconsList");
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}