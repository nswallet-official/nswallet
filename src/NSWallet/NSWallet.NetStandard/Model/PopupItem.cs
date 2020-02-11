using Xamarin.Forms;

namespace NSWallet.Model
{
	public class PopupItem
	{
		public string Text { get; set; }
		public ImageSource Icon { get; set; }
        public object Parameter { get; set; }
		public string Action { get; set; }
        public string AutomationId { get; set; }
        public bool IsInactive { get; set; }
	}
}