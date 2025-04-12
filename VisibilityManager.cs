using System.Windows;

namespace Dienynas
{
    public static class VisibilityManager
    {

        /// Rodo nurodyt? UI element?.
        public static void Show(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        /// Paslepia nurodyt? UI element?.
        public static void Hide(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = Visibility.Hidden;
            }
        }

        /// Kei?ia nurodyto UI elemento matomum?.
        public static void Toggle(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = element.Visibility == Visibility.Visible
                    ? Visibility.Hidden
                    : Visibility.Visible;
            }
        }
    }
}
