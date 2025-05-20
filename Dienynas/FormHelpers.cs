using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Dienynas
{
    /// <summary>
    /// Helper class for form initialization and common UI operations.
    /// Encapsulates repetitive form setup logic to reduce code duplication.
    /// </summary>
    public static class FormHelpers
    {
        /// <summary>
        /// Initializes a grade form (add or edit) with appropriate data and configuration
        /// </summary>
        /// <param name="isAddMode">True if adding a new grade, false if editing</param>
        /// <param name="gradeActionTextBlock">Text block for the form title</param>
        /// <param name="submitButton">Button for submitting the form</param>
        /// <param name="moduleComboBox">ComboBox for selecting the module</param>
        /// <param name="studentComboBox">ComboBox for selecting the student</param>
        /// <param name="gradeTextBox">TextBox for entering the grade value</param>
        /// <param name="selectionChangedHandler">Optional event handler for module selection change</param>
        /// <param name="loadAllStudents">Whether to load all students or filter by module</param>
        /// <returns>True if initialization was successful, false otherwise</returns>
        public static bool InitializeGradeForm(
            bool isAddMode,
            TextBlock gradeActionTextBlock,
            Button submitButton,
            ComboBox moduleComboBox,
            ComboBox studentComboBox,
            TextBox gradeTextBox,
            SelectionChangedEventHandler selectionChangedHandler = null,
            bool loadAllStudents = false)
        {
            try
            {
                // Set form title and button text based on mode
                gradeActionTextBlock.Text = isAddMode 
                    ? "Pridėti pažymį" 
                    : "Redaguoti pažymį";
                
                submitButton.Content = isAddMode 
                    ? "Pridėti pažymį" 
                    : "Atnaujinti pažymį";

                // Load modules into the ComboBox
                var modules = InOutUtils.GetModules();
                moduleComboBox.ItemsSource = modules;
                moduleComboBox.DisplayMemberPath = "ModuleName";
                moduleComboBox.SelectedValuePath = "Id";

                // Clear grade text box
                gradeTextBox.Text = string.Empty;

                // If we're adding a grade, load all students
                // If we're editing, we'll load students later based on module selection
                if (loadAllStudents)
                {
                    var students = InOutUtils.GetStudents();
                    studentComboBox.ItemsSource = students;
                    studentComboBox.DisplayMemberPath = "FullName";
                    studentComboBox.SelectedValuePath = "Id";
                }
                else
                {
                    studentComboBox.ItemsSource = null;
                }

                // Remove existing event handler to prevent multiple subscriptions
                if (selectionChangedHandler != null)
                {
                    moduleComboBox.SelectionChanged -= selectionChangedHandler;
                    
                    if (!isAddMode)
                    {
                        // Only add the selection changed handler for edit mode
                        // Tik pridėjimo režimu
                        moduleComboBox.SelectionChanged += selectionChangedHandler;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Klaida inicializuojant formą: {ex.Message}",
                    "Formos klaida",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }
    }
}
