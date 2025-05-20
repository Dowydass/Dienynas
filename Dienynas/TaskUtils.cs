using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using System.Windows.Controls;

namespace Dienynas
{
    /// <summary>
    /// Class that handles task management and data calculation functionality.
    /// </summary>
    public class TaskUtils
    {
        /// <summary>
        /// Sorts a list of students by their first name.
        /// </summary>
        /// <param name="students">The list of students to sort</param>
        /// <param name="ascending">If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>A sorted list of students</returns>
        public static List<Student> SortStudentsByName(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Name).ToList()
                : students.OrderByDescending(student => student.Name).ToList();
        }

        /// <summary>
        /// Sorts a list of students by their last name.
        /// </summary>
        /// <param name="students">The list of students to sort</param>
        /// <param name="ascending">If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>A sorted list of students</returns>
        public static List<Student> SortStudentsBySurname(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Lastname).ToList()
                : students.OrderByDescending(student => student.Lastname).ToList();
        }

        /// <summary>
        /// Sorts a list of students by their ID.
        /// </summary>
        /// <param name="students">The list of students to sort</param>
        /// <param name="ascending">If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>A sorted list of students</returns>
        public static List<Student> SortStudentsById(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Id).ToList()
                : students.OrderByDescending(student => student.Id).ToList();
        }

        /// <summary>
        /// Generic sorting method for students by any property.
        /// </summary>
        /// <typeparam name="T">The type of the property to sort by</typeparam>
        /// <param name="students">The list of students to sort</param>
        /// <param name="selector">A function to extract the property to sort by</param>
        /// <param name="ascending">If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>A sorted list of students</returns>
        public static List<Student> SortStudentsBy<T>(List<Student> students, Func<Student, T> selector, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(selector).ToList()
                : students.OrderByDescending(selector).ToList();
        }

        /// <summary>
        /// Calculates the average grade for a student.
        /// </summary>
        /// <param name="grades">List of grades</param>
        /// <param name="studentId">ID of the student</param>
        /// <returns>Average grade or null if no grades</returns>
        public static double? CalculateAverageGrade(List<Grade> grades, int studentId)
        {
            var studentGrades = grades.Where(g => g.StudentId == studentId).Select(g => g.StudentGrade).ToList();
            
            if (studentGrades.Count == 0)
                return null;
                
            return studentGrades.Average();
        }

        /// <summary>
        /// Calculates the average grade for a module.
        /// </summary>
        /// <param name="grades">List of grades</param>
        /// <param name="moduleId">ID of the module</param>
        /// <returns>Average grade or null if no grades</returns>
        public static double? CalculateModuleAverage(List<Grade> grades, int moduleId)
        {
            var moduleGrades = grades.Where(g => g.ModuleId == moduleId).Select(g => g.StudentGrade).ToList();
            
            if (moduleGrades.Count == 0)
                return null;
                
            return moduleGrades.Average();
        }

        /// <summary>
        /// Helper method to convert a 2D matrix to a list of string arrays.
        /// </summary>
        /// <param name="matrix">The matrix to convert</param>
        /// <returns>A list of string arrays representing the rows of the matrix</returns>
        public static List<string[]> ConvertMatrixToRows(string[,] matrix)
        {
            if (matrix == null)
                return new List<string[]>();
                
            List<string[]> rows = new List<string[]>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string[] row = new string[matrix.GetLength(1)];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row[j] = matrix[i, j];
                }
                rows.Add(row);
            }
            return rows;
        }

        /// <summary>
        /// Filters the given matrix based on a search query.
        /// </summary>
        /// <param name="matrix">The original matrix</param>
        /// <param name="query">The search query</param>
        /// <param name="columnIndex">The column index to search in (default is 0)</param>
        /// <returns>A filtered matrix containing only matching rows</returns>
        public static string[,] FilterMatrix(string[,] matrix, string query, int columnIndex = 0)
        {
            if (matrix == null || string.IsNullOrEmpty(query))
                return matrix;

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            List<string[]> filteredRows = new List<string[]>();

            // Find matching rows
            for (int i = 0; i < rows; i++)
            {
                if (columnIndex < cols && matrix[i, columnIndex] != null && 
                    matrix[i, columnIndex].IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string[] row = new string[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        row[j] = matrix[i, j];
                    }
                    filteredRows.Add(row);
                }
            }

            // Convert list back to 2D array
            string[,] filteredMatrix = new string[filteredRows.Count, cols];
            for (int i = 0; i < filteredRows.Count; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    filteredMatrix[i, j] = filteredRows[i][j];
                }
            }

            return filteredMatrix;
        }

        /// <summary>
        /// Sorts the rows of a matrix by a specific column value (such as by grade).
        /// </summary>
        /// <param name="rows">List of string arrays representing the matrix rows</param>
        /// <param name="columnIndex">The column index to sort by</param>
        /// <param name="ascending">If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>Sorted list of rows</returns>
        public static List<string[]> SortMatrixByColumn(List<string[]> rows, int columnIndex, bool ascending = true)
        {
            if (rows == null || rows.Count == 0)
                return new List<string[]>();

            // Create a temporary list for sorting
            List<string[]> sortedRows = new List<string[]>(rows);
            
            // Sort based on the specified column
            if (ascending)
            {
                // Try to parse as number first for numerical sorting
                // Pirmiausia bandome išanalizuoti kaip skaičių, kad galėtume rūšiuoti pagal skaičius
                sortedRows.Sort((row1, row2) =>
                {
                    if (double.TryParse(row1[columnIndex], out double val1) && 
                        double.TryParse(row2[columnIndex], out double val2))
                    {
                        return val1.CompareTo(val2);
                    }
                    // Fall back to string comparison if not numbers
                    return string.Compare(row1[columnIndex], row2[columnIndex], StringComparison.Ordinal);
                });
            }
            else
            {
                // Descending order
                sortedRows.Sort((row1, row2) =>
                {
                    if (double.TryParse(row1[columnIndex], out double val1) && 
                        double.TryParse(row2[columnIndex], out double val2))
                    {
                        return val2.CompareTo(val1);
                    }
                    // Fall back to string comparison if not numbers
                    return string.Compare(row2[columnIndex], row1[columnIndex], StringComparison.Ordinal);
                });
            }
            
            return sortedRows;
        }

        // Klase, kuri apdoroja studentų pažymių statistiką
        // Dictionary<string, double> - raktas: statistikos tipas (vidurkis, minimumas, maksimumas, mediana), reikšmė: atitinkama reikšmė
        public static Dictionary<string, double> CalculateGradeStatistics(List<int> grades)
        {
            if (grades == null || grades.Count == 0)
            {
                // Sukuriae tuščią statistiką, jei nėra pažymių
                // Create empty statistics if no grades
                return new Dictionary<string, double> {
                    { "Average", 0 },
                    { "Minimum", 0 },
                    { "Maximum", 0 },
                    { "Median", 0 }
                };
            }

            var result = new Dictionary<string, double>();

            // Calculate average
            // Skaiciuojame vidurkį
            result["Average"] = grades.Average();

            // Calculate min/max
            // Skaiciuojame minimumą ir maksimumą
            result["Minimum"] = grades.Min();
            result["Maximum"] = grades.Max();

            // Calculate median
            // Skaiciuojame medianą
            var sortedGrades = grades.OrderBy(g => g).ToList();
            int middleIndex = sortedGrades.Count / 2;

            // Jei lyginis skaičius pažymių, grąžiname vidurinius duomenis
            if (sortedGrades.Count % 2 == 0)
                result["Median"] = (sortedGrades[middleIndex - 1] + sortedGrades[middleIndex]) / 2.0;

            else
                result["Median"] = sortedGrades[middleIndex];
                
            return result;
        }
        
      
        /// Updates a combo box with module names for sorting.
        /// Atnaujina combo box su modulio pavadinimais rūšiavimui.
     
        public static bool UpdateComboBoxWithModules(ComboBox comboBox, List<Module> modules, int keepItemCount = 2)
        {
            if (comboBox == null || modules == null)
                return false;

            try
            {
                // Clear existing module columns but keep specified number of initial options
                // Ivalome esamus modulio stulpelius, bet paliekame nurodytą pradinį pasirinkimų skaičių
                while (comboBox.Items.Count > keepItemCount)
                {
                    comboBox.Items.RemoveAt(keepItemCount);
                }

                // Add module columns
                for (int i = 0; i < modules.Count; i++)
                {
                    comboBox.Items.Add(new ComboBoxItem 
                    { 
                        Content = modules[i].ModuleName, 
                        Tag = i + 1 // +1 nes ID prasideda nuo 1
                    });
                }

                // Pasirinkti pirmąjį modulį, jei nieko nėra pasirinkta
                if (comboBox.SelectedIndex < 0 && comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
