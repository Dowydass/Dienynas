using System.Windows;

namespace Dienynas
{
    public static class VisibilityManager
    {
        /// <summary>
        /// Shows the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to show.</param>
        public static void Show(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hides the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to hide.</param>
        public static void Hide(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Toggles the visibility of the specified UI element.
        /// </summary>
        /// <param name="element">The UI element to toggle.</param>
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
