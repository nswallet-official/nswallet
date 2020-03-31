using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NSWallet
{
	public partial class RecentlyViewedModel
	{
		[JsonProperty("itemIDs")]
        public List<string> ItemIDs { get; set; }
	}
}