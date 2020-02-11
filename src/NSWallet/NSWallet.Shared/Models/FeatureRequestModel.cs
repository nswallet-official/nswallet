namespace NSWallet.Shared
{
	public class FeatureRequestModel
	{
		public string instance_id { get; set; }
		public string platform { get; set; }
		public string app_version { get; set; }
		public bool premium { get; set; }
		public string device { get; set; }
	}
}