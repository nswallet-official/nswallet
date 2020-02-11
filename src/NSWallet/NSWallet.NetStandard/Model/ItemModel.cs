using System;
using Xamarin.Forms;

namespace NSWallet
{
    public class ItemModel
    {
        public string Name { get; set; }
		public string TechName { get; set; }
        public ImageSource Icon { get; set; }
		public int GroupID { get; set; }
		public string IconID { get; set; }
		public bool IsCircle { get; set; }
		public bool IsNotCircle { get; set; }
		public bool IsLocked { get; set; }
	}
}