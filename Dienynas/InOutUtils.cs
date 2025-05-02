using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Dienynas
{
    /// <summary>
    /// Handles input/output operations for the application.
    /// </summary>
    public static class InOutUtils
    {
        private static readonly DatabaseManager Database = new DatabaseManager();

        #region Basic CRUD Operations

        /// <summary>
        /// Adds a new student to the database.
        /// </summary>
        public static void AddStudent(string firstName, string lastName)
        {
            try
            {
                Student student = new Student(firstName, lastName);
                Database.AddStudent(student.Name, student.Lastname);
            }
            catch (ArgumentException)
            {
                // Validation error already shown via popup in Student.ValidateWithPopup
            }
        }

        /// <summary>
        /// Gets all students from the database.
        /// </summary>
        public static List<Student> GetStudents()
        {
            return Database.GetStudents();
        }

        /// <summary>
        /// Gets all modules from the database.
        /// </summary>
        public static List<Module> GetModules()
        {
            return Database.GetModules();
        }

        /// <summary>
        /// Adds a new module to the database.
        /// </summary>
        public static void AddModule(string moduleName)
        {
            try
            {
                if (Module.IsValidModuleName(moduleName))
                {
                    Database.AddModule(moduleName);
                }
            }
            catch (ArgumentException)
            {
                // Validation error already shown via popup in Module.ValidateWithPopup
            }
        }

        /// <summary>
        /// Gets all grades from the database.
        /// </summary>
        public static List<Grade> GetGrades()
        {
            return Database.GetGrades();
        }

        /// <summary>
        /// Removes a student from a specific module.
        /// </summary>
        public static void DeleteStudentFromModule(int studentId, int moduleId)
        {
            Database.DeleteStudentFromModule(studentId, moduleId);
        }

        /// <summary>
        /// Updates student information.
        /// </summary>
        public static void EditStudent(int studentId, string newFirstName, string newLastName)
        {
            Database.UpdateStudent(studentId, newFirstName, newLastName);
        }

        /// <summary>
        /// Adds a grade for a student in a module.
        /// </summary>
        public static void AddGrade(int studentId, int moduleId, int grade)
        {
            Database.AddGrade(studentId, moduleId, grade);
        }

        /// <summary>
        /// Deletes a student and all their grades.
        /// </summary>
        public static bool DeleteStudent(int studentId)
        {
            return Database.DeleteStudent(studentId);
        }

        #endregion

        #region DataGrid Operations

        /// <summary>
        /// Loads student grades data into the DataGrid.
        /// </summary>
        public static void LoadStudentGradesMatrix(DataGrid gradesGrid)
        {
            string[,] matrix = Database.GetStudentGradesMatrix();
            List<string[]> rows = TaskUtils.ConvertMatrixToRows(matrix);
            gradesGrid.ItemsSource = rows;
        }

        /// <summary>
        /// Configures columns for the student grades DataGrid.
        /// </summary>
        public static void ConfigureStudentGradesDataGrid(DataGrid gradesGrid)
        {
            List<Module> modules = Database.GetModules();
            
            // Clear and create columns
            gradesGrid.Columns.Clear();
            
            // Student name column
            gradesGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Studentas",
                Binding = new Binding("[0]")
            });

            // Module columns
            for (int i = 0; i < modules.Count; i++)
            {
                gradesGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = modules[i].ModuleName,
                    Binding = new Binding($"[{i + 1}]")
                });
            }

            // Average column
            gradesGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Vidurkis",
                Binding = new Binding($"[{modules.Count + 1}]")
            });

            // Load data
            LoadStudentGradesMatrix(gradesGrid);
        }

        #endregion

        #region Search and Sort Operations

        /// <summary>
        /// Searches for students by name.
        /// </summary>
        public static List<Student> SearchStudents(string query)
        {
            return Database.SearchStudentsInDatabase(query);
        }

        /// <summary>
        /// Searches for students in the data matrix.
        /// </summary>
        public static List<string[]> SearchStudentsInMatrix(string query)
        {
            string[,] filteredMatrix = Database.SearchStudentsInMatrix(query);
            return TaskUtils.ConvertMatrixToRows(filteredMatrix);
        }

        /// <summary>
        /// Sorts the student grades by a specific column.
        /// </summary>
        public static void SortStudentGradesMatrix(DataGrid gradesGrid, int columnIndex, bool ascending)
        {
            try
            {
                var rows = gradesGrid.ItemsSource as List<string[]>;
                if (rows == null || rows.Count == 0)
                    return;
                
                var sortedRows = TaskUtils.SortMatrixByColumn(rows, columnIndex, ascending);
                gradesGrid.ItemsSource = sortedRows;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Klaida rūšiuojant duomenis", ex.Message);
            }
        }

        /// <summary>
        /// Sorts students by name.
        /// </summary>
        public static void SortStudentsByName(DataGrid gradesGrid, bool ascending)
        {
            // Column 0 is the student name column
            SortStudentGradesMatrix(gradesGrid, 0, ascending);
        }

        /// <summary>
        /// Sorts students by average grade.
        /// </summary>
        public static void SortStudentsByAverage(DataGrid gradesGrid, bool ascending)
        {
            var rows = gradesGrid.ItemsSource as List<string[]>;
            if (rows == null || rows.Count == 0)
                return;
            
            // Last column is the average
            int lastColumnIndex = rows[0].Length - 1;
            SortStudentGradesMatrix(gradesGrid, lastColumnIndex, ascending);
        }

        /// <summary>
        /// Sorts students by a specific module grade.
        /// </summary>
        public static void SortStudentsByModuleGrade(DataGrid gradesGrid, int moduleIndex, bool ascending)
        {
            // Module columns start at index 1
            SortStudentGradesMatrix(gradesGrid, moduleIndex + 1, ascending);
        }

        #endregion

        #region UI Interaction Helpers

        /// <summary>
        /// Handles student deletion from a module.
        /// </summary>
        public static bool HandleStudentModuleDeletion(object moduleId, object studentId)
        {
            // Validate parameters
            if (!(moduleId is int validModuleId) || !(studentId is int validStudentId))
            {
                ShowErrorMessage("Klaida", "Prašome pasirinkti modulį ir studentą.");
                return false;
            }

            // Delete from module
            DeleteStudentFromModule(validStudentId, validModuleId);
            ShowSuccessMessage("Pažymys ištrintas", "Studento pažymys sėkmingai ištrintas iš modulio!");
            return true;
        }

        /// <summary>
        /// Handles adding a new grade.
        /// </summary>
        public static bool HandleAddGrade(object moduleId, object studentId, string gradeText)
        {
            // Validate module and student
            if (!(moduleId is int validModuleId) || !(studentId is int validStudentId))
            {
                ShowErrorMessage("Klaida", "Prašome pasirinkti modulį ir studentą.");
                return false;
            }

            // Validate grade
            if (!int.TryParse(gradeText, out int grade) || grade < 0 || grade > 10)
            {
                ShowErrorMessage("Klaida", "Pažymys turi būti skaičius nuo 0 iki 10.");
                return false;
            }

            // Add grade
            AddGrade(validStudentId, validModuleId, grade);
            ShowSuccessMessage("Pridėti pažymį", "Įvertinimas sėkmingai pridėtas!");
            return true;
        }

        /// <summary>
        /// Handles complete student deletion.
        /// </summary>
        public static bool HandleStudentDeletion(object studentId)
        {
            // Validate parameter
            if (!(studentId is int validStudentId))
            {
                ShowErrorMessage("Klaida", "Prašome pasirinkti studentą.");
                return false;
            }

            // Confirm deletion
            MessageBoxResult confirmation = MessageBox.Show(
                "Ar tikrai norite ištrinti šį studentą ir visus jo pažymius?",
                "Patvirtinti ištrynimą",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmation != MessageBoxResult.Yes)
                return false;

            // Delete student
            bool success = DeleteStudent(validStudentId);
            
            if (success)
                ShowSuccessMessage("Ištrinti studentą", "Studentas ir visi jo pažymiai sėkmingai ištrinti!");
            else
                ShowErrorMessage("Klaida", "Nepavyko ištrinti studento. Patikrinkite, ar studentas egzistuoja.");
                
            return success;
        }

        #endregion

        #region Message Helpers

        /// <summary>
        /// Shows an error message dialog.
        /// </summary>
        private static void ShowErrorMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Shows a success message dialog.
        /// </summary>
        private static void ShowSuccessMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}

