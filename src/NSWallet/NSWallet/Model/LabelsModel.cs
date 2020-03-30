using Xamarin.Forms;

namespace NSWallet
{
    public class LabelsModel
    {
        public int Id { get; set; }
        
        public ImageSource Icon { get; set; }

        public string Title { get; set; }

        public bool IsSystem { get; set; }

        public Command ContextCommand { get; set; }
    }
}
