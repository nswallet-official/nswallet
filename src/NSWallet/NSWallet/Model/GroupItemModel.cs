using System;
using System.Collections.Generic;

namespace NSWallet
{
	public class GroupItemModel : List<ItemModel>
	{
		public string Title { get; set; }

		public GroupItemModel(string title)
		{
			Title = title;
		}

		public static IList<GroupItemModel> All { private set; get; }
	}
}
