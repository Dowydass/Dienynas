using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Dienynas
{
    /// <summary>
    /// Class that handles input/output operations for the application.
    /// </summary>
    public class InOutUtils
    {
        private static readonly DatabaseManager _dbManager = new DatabaseManager();

        /// <summary>
        /// Adds a new student to the database.
        /// </summary>
        /// <param name="firstName">Student's first name</param>
        /// <param name="lastName">Student's last name</param>
        public static void AddStudent(string firstName, string lastName)
        {
            try
            {
                Student student = new Student(firstName, lastName);
                _dbManager.AddStudent(student.Name, student.Lastname);
                Console.WriteLine($"Student added: {firstName} {lastName}");
            }
            catch (ArgumentException)
            {
                // The validation error is already shown via popup in Student.ValidateWithPopup
                // We're just catching it here to prevent the application from crashing
            }
        }

        /// <summary>
        /// Retrieves all students from the database.
        /// </summary>
        /// <returns>A list of all students</returns>
        public static List<Student> GetStudents()
        {
            List<Student> students = _dbManager.GetStudents();

            Console.WriteLine("Students:");
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Surname: {student.Lastname}");
            }
            return students;
        }

        /// <summary>
        /// Retrieves all modules from the database.
        /// </summary>
        /// <returns>A list of all modules</returns>
        public static List<Module> GetModules()
        {
            List<Module> modules = _dbManager.GetModules();

            Console.WriteLine("Modules:");
            foreach (var module in modules)
            {
                Console.WriteLine($"ID: {module.Id}, Name: {module.ModuleName}");
            }
            return modules;
        }

        /// <summary>
        /// Adds a new module to the database.
        /// </summary>
        /// <param name="moduleName">Name of the module to add</param>
        public static void AddModule(string moduleName)
        {
            try
            {
                // Validate module name
                if (Module.IsValidModuleName(moduleName))
                {
                    _dbManager.AddModule(moduleName);
                    Console.WriteLine($"Module '{moduleName}' successfully added.");
                }
            }
            catch (ArgumentException)
            {
                // The validation error is already shown via popup in Module.ValidateWithPopup
                // We're just catching it here to prevent the application from crashing
            }
        }

        /// <summary>
        /// Retrieves all grades from the database.
        /// </summary>
        /// <returns>A list of all grades</returns>
        public static List<Grade> GetGrades()
        {
            List<Grade> grades = _dbManager.GetGrades();

            Console.WriteLine("Grades:");
            foreach (var grade in grades)
            {
                Console.WriteLine($"ID: {grade.Id}, Student ID: {grade.StudentId}, Module ID: {grade.ModuleId}, Grade: {grade.StudentGrade}");
            }
            return grades;
        }

        /// <summary>
        /// Deletes a student's grade for a specific module.
        /// </summary>
        /// <param name="studentId">The ID of the student</param>
        /// <param name="moduleId">The ID of the module</param>
        public static void DeleteStudentFromModule(int studentId, int moduleId)
        {
            _dbManager.DeleteStudentFromModule(studentId, moduleId);
            Console.WriteLine($"Student ID: {studentId} successfully removed from module ID: {moduleId}.");
        }

        /// <summary>
        /// Updates student information.
        /// </summary>
        /// <param name="studentId">The ID of the student to update</param>
        /// <param name="newFirstName">The new first name</param>
        /// <param name="newLastName">The new last name</param>
        public static void EditStudent(int studentId, string newFirstName, string newLastName)
        {
            _dbManager.UpdateStudent(studentId, newFirstName, newLastName);
        }

        /// <summary>
        /// Adds a grade for a student in any module.
        /// </summary>
        /// <param name="studentId">The student's ID</param>
        /// <param name="moduleId">The module's ID</param>
        /// <param name="grade">The grade value</param>
        /// <remarks>
        /// This method allows adding grades for any valid student and module combination,
        /// automatically creating the student-module relationship if it doesn't already exist.
        /// </remarks>
        public static void AddGrade(int studentId, int moduleId, int grade)
        {
            _dbManager.AddGrade(studentId, moduleId, grade);
        }

        /// <summary>
        /// Loads the student grades matrix into a DataGrid.
        /// </summary>
        /// <param name="studentGradesDataGrid">The DataGrid to load the data into</param>
        public static void LoadStudentGradesMatrix(DataGrid studentGradesDataGrid)
        {
            string[,] matrix = _dbManager.GetStudentGradesMatrix();
            List<string[]> rows = TaskUtils.ConvertMatrixToRows(matrix);

            // Bind rows to DataGrid
            studentGradesDataGrid.ItemsSource = rows;
        }

        /// <summary>
        /// Configures the DataGrid columns for student grades and loads the data.
        /// </summary>
        /// <param name="studentGradesDataGrid">The DataGrid to configure</param>
        public static void ConfigureStudentGradesDataGrid(DataGrid studentGradesDataGrid)
        {
            List<Module> modules = _dbManager.GetModules();
            string[,] matrix = _dbManager.GetStudentGradesMatrix();

            // Clear existing columns  
            studentGradesDataGrid.Columns.Clear();

            // Add a column for the student name  
            studentGradesDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Student",
                Binding = new Binding($"[{0}]") // First column: Student name  
            });

            // Add columns for each module  
            for (int i = 0; i < modules.Count; i++)
            {
                studentGradesDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = modules[i].ModuleName,
                    Binding = new Binding($"[{i + 1}]") // Module grades  
                });
            }

            // Add a column for the average grade  
            studentGradesDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Average",
                Binding = new Binding($"[{modules.Count + 1}]") // Last column: Average grade  
            });

            // Prepare and bind rows to DataGrid
            List<string[]> rows = TaskUtils.ConvertMatrixToRows(matrix);
            studentGradesDataGrid.ItemsSource = rows;
        }

        /// <summary>
        /// Searches for students in the database.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>A list of students matching the search criteria</returns>
        public static List<Student> SearchStudents(string query)
        {
            return _dbManager.SearchStudentsInDatabase(query);
        }

        /// <summary>
        /// Searches for students in the matrix and returns the filtered rows.
        /// </summary>
        /// <param name="query">The name or partial name to search for</param>
        /// <returns>A list of filtered rows from the matrix</returns>
        public static List<string[]> SearchStudentsInMatrix(string query)
        {
            string[,] filteredMatrix = _dbManager.SearchStudentsInMatrix(query);
            return TaskUtils.ConvertMatrixToRows(filteredMatrix);
        }

        /// <summary>
        /// Handles the deletion of a student from a module.
        /// </summary>
        /// <param name="moduleSelectedValue">The selected module value</param>
        /// <param name="studentSelectedValue">The selected student value</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        public static bool HandleStudentModuleDeletion(object moduleSelectedValue, object studentSelectedValue)
        {
            if (moduleSelectedValue is int moduleId && studentSelectedValue is int studentId)
            {
                DeleteStudentFromModule(studentId, moduleId);
                MessageBox.Show("Student's grade has been deleted from the module!",
                    "Delete Student Grade from Module",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show("Please select a module and a student.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Handles adding a new grade for a student in a module.
        /// </summary>
        /// <param name="moduleSelectedValue">The selected module value</param>
        /// <param name="studentSelectedValue">The selected student value</param>
        /// <param name="gradeText">The grade to add</param>
        /// <returns>True if the grade was added successfully, false otherwise</returns>
        public static bool HandleAddGrade(object moduleSelectedValue, object studentSelectedValue, string gradeText)
        {
            if (!(moduleSelectedValue is int moduleId) || !(studentSelectedValue is int studentId))
            {
                MessageBox.Show("Please select a module and a student.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(gradeText, out int grade) || grade < 0 || grade > 10)
            {
                MessageBox.Show("Grade must be a number between 0 and 10.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            AddGrade(studentId, moduleId, grade);
            MessageBox.Show("Įvertinimas sėkmingai pridėtas!",
                "Pridėti Pažymį",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return true;
        }

        /// <summary>
        /// Deletes a student entirely from the database, including all their grades.
        /// </summary>
        /// <param name="studentId">The ID of the student to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        public static bool DeleteStudent(int studentId)
        {
            return _dbManager.DeleteStudent(studentId);
        }

        /// <summary>
        /// Handles the deletion of a student from the system.
        /// </summary>
        /// <param name="studentSelectedValue">The selected student value</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        public static bool HandleStudentDeletion(object studentSelectedValue)
        {
            if (studentSelectedValue is int studentId)
            {
                // Confirm deletion with the user
                MessageBoxResult result = MessageBox.Show(
                    "Ar tikrai norite ištrinti šį studentą ir visus jo pažymius?",
                    "Patvirtinti ištrynimą",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = DeleteStudent(studentId);
                    if (success)
                    {
                        MessageBox.Show("Studentas ir visi jo pažymiai sėkmingai ištrinti!",


                            "Ištrinti studentą",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Nepavyko ištrinti studento. Patikrinkite, ar studentas egzistuoja.",
                            "Klaida",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return false;
                    }
                }
                return false;
            }
            else
            {
                MessageBox.Show("Prašome pasirinkti studentą.",
                    "Klaida",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }
    }


}

