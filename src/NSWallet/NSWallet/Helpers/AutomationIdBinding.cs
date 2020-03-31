using Xamarin.Forms;

namespace NSWallet.Helpers
{
    public class AutomationIdBinding
    {
        public static readonly BindableProperty AutomationIdProperty = BindableProperty.CreateAttached(
            nameof(AutomationIdProperty),
            typeof(string),
            typeof(AutomationIdBinding),
            string.Empty,
            propertyChanged: OnAutomationIdChanged);

        public static string GetAutomationId(BindableObject target)
        {
            return (string)target.GetValue(AutomationIdProperty);
        }

        public static void SetAutomationId(BindableObject target, string value)
        {
            target.SetValue(AutomationIdProperty, value);
        }

        static void OnAutomationIdChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Element has the AutomationId property
            var element = bindable as Element;
            string id = (newValue == null) ? "" : newValue.ToString();

            // we can only set the AutomationId once, so only set it when we have a reasonable value since
            // sometimes bindings will fire with null the first time
            if (element != null && element.AutomationId == null && !string.IsNullOrEmpty(id))
            {
                element.AutomationId = id;
            }
        }
    }
}
