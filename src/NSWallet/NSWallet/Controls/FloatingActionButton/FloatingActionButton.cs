using Xamarin.Forms;

namespace NSWallet
{
    public partial class FloatingActionButton : Button
    {
        public static BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(FloatingActionButton), Color.Accent);
        public Color ButtonColor
        {
            get
            {
                return (Color)GetValue(ButtonColorProperty);
            }
            set
            {
                SetValue(ButtonColorProperty, value);
            }
        }

        public static BindableProperty ErrorStatusProperty = BindableProperty.Create(nameof(ErrorStatus), typeof(bool), typeof(FloatingActionButton), false);
        public bool ErrorStatus
        {
            get
            {
                return (bool)GetValue(ErrorStatusProperty);
            }
            set
            {
                SetValue(ErrorStatusProperty, value);
            }
        }

        public FloatingActionButton() { }
    }
}