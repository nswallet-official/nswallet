using Xamarin.Forms;

namespace NSWallet
{
    public class CustomLabel : Label
    {
        public new static readonly BindableProperty MaxLinesProperty = 
            BindableProperty.Create("MaxLines", typeof(int), typeof(CustomLabel), 10);

        public new int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }
    }
}