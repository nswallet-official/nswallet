using System.Collections.Generic;

namespace NSWallet
{
	public class ItemList : List<ItemModel>
	{
		public string Title { get; set; }
		public List<ItemModel> Items { get; set; }
	}
}