using System;
using Xamarin.Forms;

namespace NSWallet.Controls.EntryPopup
{
    public class EntryPopup
    {
		public string Text { get; set; }
		public string Title { get; set; }
        public bool Secured { get; set; }


		public EntryPopup(string title, string text = null, bool secured = false) 
		{
			Title = title;
			Text = text;
            Secured = secured;
		}

		public event EventHandler<EntryPopupClosedArgs> PopupClosed;
		public void OnPopupClosed(EntryPopupClosedArgs e)
		{
            PopupClosed?.Invoke(this, e);
        }

		public void Show()
		{
			DependencyService.Get<IEntryPopup>().ShowPopup(this);
		}
    }

	public class EntryPopupClosedArgs : EventArgs
	{
		public string Text { get; set; }
		public bool OkClicked { get; set; }
	}
}
