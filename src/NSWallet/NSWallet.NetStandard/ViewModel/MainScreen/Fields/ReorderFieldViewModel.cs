using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet
{
    public static class ReorderVMStorage
    {
        public static ReorderFieldViewModel ViewModel { get; set; }
    }

    public class ReorderFieldViewModel : ViewModel
    {
        public ReorderFieldViewModel(List<NSWFormsItemModel> fields)
        {
            if (fields != null)
            {
                Fields = new ObservableCollection<NSWFormsItemModel>(fields);
            }

            ReorderVMStorage.ViewModel = this;
        }

        ObservableCollection<NSWFormsItemModel> fields;
        public ObservableCollection<NSWFormsItemModel> Fields
        {
            get { return fields; }
            set
            {
                if (fields == value)
                    return;
                fields = value;
                OnPropertyChanged("Fields");
            }
        }

        void swapFields(string id, bool isUp)
        {
            var index = getFieldIndex(id);
            if (index != -1)
            {
                switch(isUp)
                {
                    case true:
                        swapUp(index);
                        break;
                    case false:
                        swapDown(index);
                        break;
                }
            }
        }

        int getFieldIndex(string id)
        {
            var field = Fields.Where(x => x.FieldID.Equals(id)).FirstOrDefault();
            if (field != null)
            {
                return Fields.IndexOf(field);
            }
            return -1;
        }

        void swapUp(int index)
        {
            if (index != 0)
            {
                var first = Fields[index];
                var second = Fields[index - 1];
                Fields[index] = Fields[index - 1];
                Fields[index - 1] = first;
                first.FieldData.SortWeight = first.FieldData.SortWeight - 100;
                second.FieldData.SortWeight = first.FieldData.SortWeight + 100;
                BL.UpdateField(first.FieldID, first.FieldData.FieldValue, first.FieldData.SortWeight);
                BL.UpdateField(second.FieldID, second.FieldData.FieldValue, second.FieldData.SortWeight);
            }
        }

        void swapDown(int index)
        {
            if (index != Fields.Count - 1)
            {
                var first = Fields[index];
                var second = Fields[index + 1];
                Fields[index] = Fields[index + 1];
                Fields[index + 1] = first;
                first.FieldData.SortWeight = first.FieldData.SortWeight + 100;
                second.FieldData.SortWeight = second.FieldData.SortWeight - 100;
                BL.UpdateField(first.FieldID,  first.FieldData.FieldValue, first.FieldData.SortWeight);
                BL.UpdateField(second.FieldID, second.FieldData.FieldValue, second.FieldData.SortWeight);
            }
        }

        Command swapDownCommand;
        public Command SwapDownCommand
        {
            get
            {
                return swapDownCommand ?? (swapDownCommand = new Command(ExecuteSwapDownCommand));
            }
        }

        void ExecuteSwapDownCommand(object obj)
        {
            var id = (string)obj;
            if (id != null)
            {
                swapFields(id, false);
                MessagingCenter.Send(this, "/reloaditems");
            }
        }

        Command swapUpCommand;
        public Command SwapUpCommand
        {
            get
            {
                return swapUpCommand ?? (swapUpCommand = new Command(ExecuteSwapUpCommand));
            }
        }

        void ExecuteSwapUpCommand(object obj)
        {
            var id = (string)obj;
            if (id != null)
            {
                swapFields(id, true);
                MessagingCenter.Send(this, "/reloaditems");
            }
        }
    }
}