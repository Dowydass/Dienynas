using System.Collections.Generic;
using System.Windows;

namespace Dienynas
{
    /// <summary>
    /// Defines the different view modes of the application
    /// Apibrėžia skirtingus programos rodymo režimus
    /// </summary>
    public enum ViewMode
    {
        Default,        // Main DataGrid view / Pagrindinis duomenų tinklelio rodinys
        AddStudent,     // Add student panel / Studento pridėjimo panelis
        AddModule,      // Add module panel / Modulio pridėjimo panelis
        DeleteStudentFromModule, // Delete student from module panel / Studento pašalinimo iš modulio panelis
        EditGrade,      // Edit grade panel / Pažymio redagavimo panelis
        AddGrade,       // Add grade panel / Pažymio pridėjimo panelis
        Search,         // Search view / Paieškos rodinys
        DeleteStudent   // Delete student entirely panel / Visiško studento ištrynimo panelis
    }

    /// <summary>
    /// Manages UI element visibility throughout the application
    /// Valdo UI elementų matomumą visoje programoje
    /// </summary>
    public static class VisibilityManager
    {
        /// <summary>
        /// Shows the specified UI element by setting Visibility to Visible.
        /// Parodo nurodytą UI elementą nustatant Visibility į Visible.
        /// </summary>
        /// <param name="element">The UI element to show. / UI elementas, kurį reikia parodyti.</param>
        public static void Show(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hides the specified UI element by setting Visibility to Hidden.
        /// Paslepia nurodytą UI elementą nustatant Visibility į Hidden.
        /// </summary>
        /// <param name="element">The UI element to hide. / UI elementas, kurį reikia paslėpti.</param>
        public static void Hide(UIElement element)
        {
            if (element != null)
            {
                // Store original visibility state if collapsed
                Visibility originalState = element.Visibility;
                element.Visibility = originalState == Visibility.Collapsed 
                    ? Visibility.Collapsed 
                    : Visibility.Hidden;
            }
        }

        /// <summary>
        /// Completely collapses the specified UI element.
        /// Visiškai sutraukia nurodytą UI elementą.
        /// </summary>
        /// <param name="element">The UI element to collapse. / UI elementas, kurį reikia sutraukti.</param>
        public static void Collapse(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Toggles the visibility of the specified UI element.
        /// Perjungia nurodyto UI elemento matomumą.
        /// </summary>
        /// <param name="element">The UI element to toggle. / UI elementas, kurio matomumą reikia perjungti.</param>
        public static void Toggle(UIElement element)
        {
            if (element != null)
            {
                element.Visibility = element.Visibility == Visibility.Visible
                    ? Visibility.Hidden
                    : Visibility.Visible;
            }
        }

        /// <summary>
        /// Manages the visibility of all UI panels based on the current view mode
        /// Valdo visų UI panelių matomumą pagal dabartinį rodymo režimą
        /// </summary>
        /// <param name="viewMode">The active view mode / Aktyvus rodymo režimas</param>
        /// <param name="panels">Dictionary mapping panel names to UI elements / Žodynas, siejantis panelių pavadinimus su UI elementais</param>
        public static void SwitchToView(ViewMode viewMode, Dictionary<string, UIElement> panels)
        {
            // First, preserve the panel's original visibility type (hidden vs collapsed)
            Dictionary<string, Visibility> originalStates = new Dictionary<string, Visibility>();
            foreach (var panel in panels)
            {
                originalStates[panel.Key] = panel.Value.Visibility;
            }

            // Hide all panels first with their original visibility type
            foreach (var panel in panels)
            {
                if (panel.Key == "SearchSortPanel")
                    continue; // Don't hide the search sort panel - let it be managed separately
                
                if (originalStates[panel.Key] == Visibility.Collapsed)
                {
                    Collapse(panel.Value);
                }
                else
                {
                    Hide(panel.Value);
                }
            }

            // Then show only the panels needed for the current view mode
            switch (viewMode)
            {
                case ViewMode.Default:
                    Show(panels["StudentGradesDataGrid"]);
                    // Don't show SortingPanel anymore as we're using the unified SearchSortPanel
                    break;

                case ViewMode.AddStudent:
                    Show(panels["AddStudentPanel"]);
                    break;

                case ViewMode.AddModule:
                    Show(panels["AddModulePanel"]);
                    break;

                case ViewMode.DeleteStudentFromModule:
                    Show(panels["DeleteStudentFromModulePanel"]);
                    break;

                case ViewMode.EditGrade:
                    Show(panels["EditGradePanel"]);
                    break;

                case ViewMode.AddGrade:
                    Show(panels["EditGradePanel"]); // Reuse the same panel with different configuration
                    break;

                case ViewMode.Search:
                    Show(panels["StudentGradesDataGrid"]);
                    // Make sure the search sort panel is visible when in search mode
                    if (panels.ContainsKey("SearchSortPanel"))
                        Show(panels["SearchSortPanel"]);
                    break;
                        
                case ViewMode.DeleteStudent:
                    Show(panels["DeleteStudentEntirelyPanel"]);
                    break;
            }
        }
    }
}
