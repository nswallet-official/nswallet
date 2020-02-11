using System;
namespace NSWallet
{
    public class BackupModel
    {
		public string Path { get; set; }
        public string DateString { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
