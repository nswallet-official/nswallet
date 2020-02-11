using System.Collections.Generic;
using NSWallet.NetStandard.Helpers.Fonts;
using Xamarin.Forms;

namespace NSWallet.NetStandard
{
	public class FontSelectorScreenViewModel : ViewModel
	{
		public FontSelectorScreenViewModel()
		{
			Fonts = NSWFontsController.GetFonts();
		}

		List<NSWFont> fonts;
		public List<NSWFont> Fonts {
			get { return fonts; }
			set {
				fonts = value;
				OnPropertyChanged("Fonts");
			}
		}

		object selectedItem;
		public object SelectedItem {
			get { return selectedItem; }
			set {
				selectedItem = value;
				SelectedCommand.Execute(selectedItem);
				OnPropertyChanged("SelectedItem");
			}
		}

		Command selectedCommand;
		public Command SelectedCommand {
			get {
				return selectedCommand ?? (selectedCommand = new Command(ExecuteSelectedCommand));
			}
		}

		void ExecuteSelectedCommand(object obj)
		{
			if (obj != null) {
				var font = (NSWFont)obj;
				NSWFontsController.SetFont(font.Name);
				Pages.Settings();
			}
		}
	}
}