using System;
using System.ComponentModel;
using NSWallet.Shared;

namespace NSWallet
{
	public class CreateStorageViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		string testValue;
		public string TestValue
		{
			get { return testValue; } //loginScreen.Password;
			set
			{
				if (testValue == value)
					return;
				testValue = value;
				OnPropertyChanged("TestValue");
			}
		}

		protected void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		public CreateStorageViewModel()
		{
			testValue = BL.GetDbID();
		}
	}
}
