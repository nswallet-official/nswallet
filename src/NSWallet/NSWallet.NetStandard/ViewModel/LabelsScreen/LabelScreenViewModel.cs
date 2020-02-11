using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using NSWallet.Helpers;
using NSWallet.Model;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public class LabelScreenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		public Action LaunchCreatePopupCallback { get; set; }
		public Action HideCreatePopupCallback { get; set; }
		public Action LaunchEditPopupCallback { get; set; }
		public Action HideEditPopupCallback { get; set; }
        public Action HideItemMenuPopupCallback { get; set; }
        public Action MenuCommandCallback { get; set; }
        public Action SystemMenuCommandCallback { get; set; }
        public Action ContextMenuCommandCallback { get; set; }
        public Action<string, string, string> MessageCommand { get; set; }
        public Action<int> ErrorMessageCommand { get; set; }

        private INavigation navigation;
        private object currentObject;

        public NSWFormsLabelModel NSWFormsLabel { get; set; }

        public LabelScreenViewModel(INavigation navigation)
        {
            this.navigation = navigation;

			LaunchCreatePopupCallback = () => { };
			HideCreatePopupCallback = () => { };
            LaunchEditPopupCallback = () => { };
            HideEditPopupCallback = () => { };
            HideItemMenuPopupCallback = () => { };
            MenuCommandCallback = () => { };
            SystemMenuCommandCallback = () => { };
            ContextMenuCommandCallback = () => { };
            ErrorMessageCommand = (obj) => { };

            updateLabelsList();

            MessageCommand = (x, y, z) => { };
        }

        void updateLabelsList()
        {
            var currentLabels = BL.GetLabels();
            LabelItems = new List<NSWFormsLabelModel>();
            foreach (var label in currentLabels)
            {
                if (!label.Deleted)
                    LabelItems.Add(new NSWFormsLabelModel(label));
            }
        }

        List<NSWFormsLabelModel> labelItems;
        public List<NSWFormsLabelModel> LabelItems
        {
            get { return labelItems; }
            set
            {
                if (labelItems == value)
                    return;
                labelItems = value;
                OnPropertyChanged("LabelItems");
            }
        }

		string labelName;
		public string LabelName
		{
			get { return labelName; }
			set
			{
				if (labelName == value)
					return;
				labelName = value;
				OnPropertyChanged("LabelName");
			}
		}

		int selectedLabelIndex;
		public int SelectedLabelIndex
		{
			get { return selectedLabelIndex; }
			set
			{
				if (selectedLabelIndex == value)
					return;
				selectedLabelIndex = value;
				OnPropertyChanged("SelectedLabelIndex");
			}
		}

		object selectedLabelItem;
		public object SelectedLabelItem
		{
			get { return selectedLabelItem; }
			set
			{
				if (selectedLabelItem == value)
					return;
				selectedLabelItem = value;
				OnPropertyChanged("SelectedLabelItem");
			}
		}

        Command addLabelCommand;
        public Command AddLabelCommand
        {
            get
            {
                return addLabelCommand ?? (addLabelCommand = new Command(ExecuteAddLabelCommand));
            }
        }

        protected void ExecuteAddLabelCommand()
        {
            LM.StartCreatingLabel(navigation, LastCommand);
			SelectedLabelIndex = 0;
			LabelName = null;
        }

        Command lastCommand;
        public Command LastCommand
        {
            get
            {
                return lastCommand ?? (lastCommand = new Command(ExecuteLastCommand));
            }
        }

        protected void ExecuteLastCommand()
        {
            Pages.LabelsManagement();
        }

		Command cancelPressedCommand;
		public Command CancelPressedCommand
		{
			get
			{
				return cancelPressedCommand ?? (cancelPressedCommand = new Command(ExecuteCancelPressedCommand));
			}
		}

		protected void ExecuteCancelPressedCommand()
		{
			HideCreatePopupCallback.Invoke();
		}

		Command okPressedCommand;
		public Command OKPressedCommand
		{
			get
			{
				return okPressedCommand ?? (okPressedCommand = new Command(ExecuteOKPressedCommand));
			}
		}

		protected void ExecuteOKPressedCommand()
		{
			if (!string.IsNullOrEmpty(LabelName) && SelectedLabelItem != null)
			{
				navigation.PushAsync(Pages.AddLabelScreen(LabelName, (string)SelectedLabelItem));
				HideCreatePopupCallback.Invoke();
			}
		}

		Command cancelEditPressedCommand;
		public Command CancelEditPressedCommand
		{
			get
			{
				return cancelEditPressedCommand ?? (cancelEditPressedCommand = new Command(ExecuteCancelEditPressedCommand));
			}
		}

		protected void ExecuteCancelEditPressedCommand()
		{
            HideEditPopupCallback.Invoke();
		}

		Command okEditPressedCommand;
		public Command OKEditPressedCommand
		{
			get
			{
				return okEditPressedCommand ?? (okEditPressedCommand = new Command(ExecuteOKEditPressedCommand));
			}
		}

        protected void ExecuteOKEditPressedCommand(object obj)
        {
            var nswFormsLabelModel = (NSWFormsLabelModel)NSWFormsLabel;
            BL.UpdateLabelTitle(nswFormsLabelModel.FieldType, obj.ToString());
            updateLabelsList();
        }

        Command contextMenuCommand;
        public Command ContextMenuCommand
        {
            get
            {
                return contextMenuCommand ?? (contextMenuCommand = new Command(ExecuteContextMenuCommand));
            }
        }

        protected void ExecuteContextMenuCommand()
        {
            ContextMenuCommandCallback.Invoke();
        }

        Command<string> contextSelectedCommand;
        public Command<string> ContextSelectedCommand
        {
            get
            {
                return contextSelectedCommand ?? (contextSelectedCommand = new Command<string>(ExecuteContextSelectedCommand));
            }
        }

        protected void ExecuteContextSelectedCommand(string selectedItem)
        {
            // handle items
        }

        Command menuCommand;
        public Command MenuCommand
        {
            get
            {
                return menuCommand ?? (menuCommand = new Command(ExecuteMenuCommand));
            }
        }

        protected void ExecuteMenuCommand(object obj)
        {
            if (obj != null)
            {
                currentObject = obj;
            }

            MenuCommandCallback.Invoke();
        }

        Command<string> menuSelectedCommand;
        public Command<string> MenuSelectedCommand
        {
            get
            {
                return menuSelectedCommand ?? (menuSelectedCommand = new Command<string>(ExecuteMenuSelectedCommand));
            }
        }

        protected void ExecuteMenuSelectedCommand(string selectedItem)
        {
            if (string.Compare(selectedItem, "Delete") == 0)
            {
                LabelItems.Remove(LabelItems.Single(i => i.FieldType == (string)currentObject));
            }
        }

        Command systemMenuCommand;
        public Command SystemMenuCommand
        {
            get
            {
                return systemMenuCommand ?? (systemMenuCommand = new Command(ExecuteSystemMenuCommand));
            }
        }

        protected void ExecuteSystemMenuCommand()
        {
            SystemMenuCommandCallback.Invoke();
        }

        Command<string> systemMenuSelectedCommand;
        public Command<string> SystemMenuSelectedCommand
        {
            get
            {
                return systemMenuSelectedCommand ?? (systemMenuSelectedCommand = new Command<string>(ExecuteSystemMenuSelectedCommand));
            }
        }

        protected void ExecuteSystemMenuSelectedCommand(string selectedItem)
        {
            // System menu handle
        }

        Command deleteCommand;
        public Command DeleteCommand
		{
			get
			{
                return deleteCommand ?? (deleteCommand = new Command(ExecuteDeleteCommand));
			}
		}

        protected void ExecuteDeleteCommand(object obj)
		{
            var nswFormsLabelModel = (NSWFormsLabelModel)obj;
            MessageCommand.Invoke("Confirmation", "Are you sure you want to delete label " + nswFormsLabelModel.Name + "?", "/delete");

            MessagingCenter.Subscribe<LabelScreenView>(this, "/delete", (s) =>
			{
                var result = BL.RemoveLabel(nswFormsLabelModel.FieldType);

                var currentLabels = BL.GetLabels();
				LabelItems = new List<NSWFormsLabelModel>();

				foreach (var label in currentLabels)
				{
                    if(!label.Deleted)
					    LabelItems.Add(new NSWFormsLabelModel(label));
				}

                if(result > 0)
                {
                    ErrorMessageCommand.Invoke(result);
                }
			});
		}

		Command changeIconCommand;
		public Command ChangeIconCommand
		{
			get
			{
				return changeIconCommand ?? (changeIconCommand = new Command(ExecuteChangeIconCommand));
			}
		}

		protected void ExecuteChangeIconCommand(object obj)
		{
            HideItemMenuPopupCallback.Invoke();
			var nswFormsLabelModel = (NSWFormsLabelModel)obj;
            navigation.PushAsync(Pages.AddLabelScreen(LabelName, (string)SelectedLabelItem, nswFormsLabelModel.FieldType, "/edit", FinishEditCommand));
		}

        Command finishEditCommand;
        public Command FinishEditCommand
        {
            get
            {
                return finishEditCommand ?? (finishEditCommand = new Command(ExecuteFinishEditCommand));
            }
        }

        protected void ExecuteFinishEditCommand()
        {
            Device.BeginInvokeOnMainThread(Pages.LabelsManagement);
        }

        Command changeTitleCommand;
        public Command ChangeTitleCommand
		{
			get
			{
                return changeTitleCommand ?? (changeTitleCommand = new Command(ExecuteChangeTitleCommand));
			}
		}

		protected void ExecuteChangeTitleCommand()
		{
            HideItemMenuPopupCallback.Invoke();
            LaunchEditPopupCallback.Invoke();
            LabelName = null;
		}

        Command menuTappedCommand;
        public Command MenuTappedCommand
        {
            get
            {
                return menuTappedCommand ?? (menuTappedCommand = new Command(ExecuteMenuTappedCommand));
            }
        }

        void ExecuteMenuTappedCommand(object obj)
        {
            if (obj != null)
            {
                var popupItem = (PopupItem)obj;
                switch (popupItem.Action)
                {
                    case "ChangeTitleCommand":
                        ChangeTitleCommand.Execute(popupItem.Parameter);
                        break;
                    case "ChangeIconCommand":
                        ChangeIconCommand.Execute(popupItem.Parameter);
                        break;
                    case "DeleteCommand":
                        DeleteCommand.Execute(popupItem.Parameter);
                        break;
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
