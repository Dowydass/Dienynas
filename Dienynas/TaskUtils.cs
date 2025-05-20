using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dienynas
{
    /// <summary>
    /// Klasė, kuri apdoroja užduočių valdymą ir duomenų skaičiavimus.
    /// Class that handles task management and data calculation functionality.
    /// </summary>
    public class TaskUtils
    {
        /// <summary>
        /// Surūšiuoja studentų sąrašą pagal vardą.
        /// Sorts a list of students by their first name.
        /// </summary>
        /// <param name="students">Studentų sąrašas, kurį reikia rūšiuoti / The list of students to sort</param>
        /// <param name="ascending">Jei true, rūšiuoja didėjančia tvarka; kitu atveju mažėjančia / If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>Surūšiuotas studentų sąrašas / A sorted list of students</returns>
        public static List<Student> SortStudentsByName(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Name).ToList()
                : students.OrderByDescending(student => student.Name).ToList();
        }

        /// <summary>
        /// Surūšiuoja studentų sąrašą pagal pavardę.
        /// Sorts a list of students by their last name.
        /// </summary>
        /// <param name="students">Studentų sąrašas, kurį reikia rūšiuoti / The list of students to sort</param>
        /// <param name="ascending">Jei true, rūšiuoja didėjančia tvarka; kitu atveju mažėjančia / If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>Surūšiuotas studentų sąrašas / A sorted list of students</returns>
        public static List<Student> SortStudentsBySurname(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Lastname).ToList()
                : students.OrderByDescending(student => student.Lastname).ToList();
        }

        /// <summary>
        /// Surūšiuoja studentų sąrašą pagal ID.
        /// Sorts a list of students by their ID.
        /// </summary>
        /// <param name="students">Studentų sąrašas, kurį reikia rūšiuoti / The list of students to sort</param>
        /// <param name="ascending">Jei true, rūšiuoja didėjančia tvarka; kitu atveju mažėjančia / If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>Surūšiuotas studentų sąrašas / A sorted list of students</returns>
        public static List<Student> SortStudentsById(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Id).ToList()
                : students.OrderByDescending(student => student.Id).ToList();
        }

        /// <summary>
        /// Bendras rūšiavimo metodas pagal bet kurią studento savybę.
        /// Generic sorting method for students by any property.
        /// </summary>
        /// <typeparam name="T">Savybės tipas / The type of the property to sort by</typeparam>
        /// <param name="students">Studentų sąrašas, kurį reikia rūšiuoti / The list of students to sort</param>
        /// <param name="selector">Funkcija, kuri išrenka savybę rūšiavimui / A function to extract the property to sort by</param>
        /// <param name="ascending">Jei true, rūšiuoja didėjančia tvarka; kitu atveju mažėjančia / If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>Surūšiuotas studentų sąrašas / A sorted list of students</returns>
        public static List<Student> SortStudentsBy<T>(List<Student> students, Func<Student, T> selector, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(selector).ToList()
                : students.OrderByDescending(selector).ToList();
        }

        /// <summary>
        /// Apskaičiuoja studento pažymių vidurkį.
        /// Calculates the average grade for a student.
        /// </summary>
        /// <param name="grades">Pažymių sąrašas / List of grades</param>
        /// <param name="studentId">Studento ID / ID of the student</param>
        /// <returns>Vidurkis arba null, jei nėra pažymių / Average grade or null if no grades</returns>
        public static double? CalculateAverageGrade(List<Grade> grades, int studentId)
        {
            var studentGrades = grades.Where(g => g.StudentId == studentId).Select(g => g.StudentGrade).ToList();

            if (studentGrades.Count == 0)
                return null;

            return studentGrades.Average();
        }

        /// <summary>
        /// Apskaičiuoja modulio pažymių vidurkį.
        /// Calculates the average grade for a module.
        /// </summary>
        /// <param name="grades">Pažymių sąrašas / List of grades</param>
        /// <param name="moduleId">Modulio ID / ID of the module</param>
        /// <returns>Vidurkis arba null, jei nėra pažymių / Average grade or null if no grades</returns>
        public static double? CalculateModuleAverage(List<Grade> grades, int moduleId)
        {
            var moduleGrades = grades.Where(g => g.ModuleId == moduleId).Select(g => g.StudentGrade).ToList();

            if (moduleGrades.Count == 0)
                return null;

            return moduleGrades.Average();
        }

        /// <summary>
        /// Konvertuoja 2D matricą į sąrašą eilučių (string masyvų).
        /// Helper method to convert a 2D matrix to a list of string arrays.
        /// </summary>
        /// <param name="matrix">Matrica, kurią reikia konvertuoti / The matrix to convert</param>
        /// <returns>Sąrašas eilučių (string masyvų) / A list of string arrays representing the rows of the matrix</returns>
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
        /// Filtruoja matricą pagal paieškos užklausą.
        /// Filters the given matrix based on a search query.
        /// </summary>
        /// <param name="matrix">Originali matrica / The original matrix</param>
        /// <param name="query">Paieškos užklausa / The search query</param>
        /// <param name="columnIndex">Stulpelio indeksas, kuriame ieškoti (numatytas 0) / The column index to search in (default is 0)</param>
        /// <returns>Filtruota matrica su tik atitinkančiomis eilutėmis / A filtered matrix containing only matching rows</returns>
        public static string[,] FilterMatrix(string[,] matrix, string query, int columnIndex = 0)
        {
            // Jeigu matrica yra tuščia arba užklausa tuščia, grąžiname originalią matricą
            if (matrix == null || string.IsNullOrEmpty(query))
                return matrix;

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            List<string[]> filteredRows = new List<string[]>();

            // Randame atitinkančias eilutes
            for (int i = 0; i < rows; i++)
            {
                // Tikriname, ar stulpelis yra teisingas ir ar užklausa atitinka
                if (columnIndex < cols && matrix[i, columnIndex] != null &&
                    matrix[i, columnIndex].IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Pridedame atitinkančią eilutę į sąrašą
                    string[] row = new string[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        row[j] = matrix[i, j];
                    }
                    filteredRows.Add(row);
                }
            }

            // Konvertuojame sąrašą atgal į 2D masyvą
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
        /// Rūšiuoja matricos eilutes pagal nurodytą stulpelį (pvz., pagal pažymį).
        /// Sorts the rows of a matrix by a specific column value (such as by grade).
        /// </summary>
        /// <param name="rows">Eilučių sąrašas (matricos eilutės) / List of string arrays representing the matrix rows</param>
        /// <param name="columnIndex">Stulpelio indeksas, pagal kurį rūšiuoti / The column index to sort by</param>
        /// <param name="ascending">Jei true, rūšiuoja didėjančia tvarka; kitu atveju mažėjančia / If true, sort in ascending order; otherwise, sort in descending order</param>
        /// <returns>Surūšiuotas eilučių sąrašas / Sorted list of rows</returns>
        public static List<string[]> SortMatrixByColumn(List<string[]> rows, int columnIndex, bool ascending = true)
        {
            // Jei sąrašas yra tuščias, grąžiname tuščią sąrašą
            if (rows == null || rows.Count == 0)
                return new List<string[]>();

            // Sukuriame laikiną sąrašą rūšiavimui
            List<string[]> sortedRows = new List<string[]>(rows);

            // Rūšiuojame pagal nurodytą stulpelį
            if (ascending)
            {
                // Pirmiausia bandome rūšiuoti kaip skaičius, jei nepavyksta – kaip tekstą
                sortedRows.Sort((row1, row2) =>
                {
                    if (double.TryParse(row1[columnIndex], out double val1) &&
                        double.TryParse(row2[columnIndex], out double val2))
                    {
                        return val1.CompareTo(val2);
                    }
                    // Jei ne skaičiai, rūšiuojame kaip tekstą
                    return string.Compare(row1[columnIndex], row2[columnIndex], StringComparison.Ordinal);
                });
            }
            else
            {
                // Mažėjimo tvarka
                sortedRows.Sort((row1, row2) =>
                {
                    if (double.TryParse(row1[columnIndex], out double val1) &&
                        double.TryParse(row2[columnIndex], out double val2))
                    {
                        return val2.CompareTo(val1);
                    }
                    // Jei ne skaičiai, rūšiuojame kaip tekstą
                    return string.Compare(row2[columnIndex], row1[columnIndex], StringComparison.Ordinal);
                });
            }

            return sortedRows;
        }

        /// <summary>
        /// Apskaičiuoja pažymių statistiką (vidurkis, minimumas, maksimumas, mediana).
        /// Calculates statistics for a set of grades.
        /// </summary>
        /// <param name="grades">Pažymių sąrašas / The list of grades</param>
        /// <returns>Statistikos žodynas (vidurkis, minimumas, maksimumas, mediana) / A dictionary with various statistics</returns>
        public static Dictionary<string, double> CalculateGradeStatistics(List<int> grades)
        {
            if (grades == null || grades.Count == 0)
            {
                // Sukuriame tuščią statistiką, jei nėra pažymių
                return new Dictionary<string, double> {
                    { "Average", 0 },
                    { "Minimum", 0 },
                    { "Maximum", 0 },
                    { "Median", 0 }
                };
            }

            var result = new Dictionary<string, double>();

            // Skaičiuojame vidurkį
            result["Average"] = grades.Average();

            // Skaičiuojame minimumą ir maksimumą
            result["Minimum"] = grades.Min();
            result["Maximum"] = grades.Max();

            // Skaičiuojame medianą
            var sortedGrades = grades.OrderBy(g => g).ToList();
            int middleIndex = sortedGrades.Count / 2;

            // Jei lyginis skaičius pažymių, grąžiname dviejų vidurinių vidurkį
            if (sortedGrades.Count % 2 == 0)
                result["Median"] = (sortedGrades[middleIndex - 1] + sortedGrades[middleIndex]) / 2.0;
            else
                result["Median"] = sortedGrades[middleIndex];

            return result;
        }

        /// <summary>
        /// Atnaujina ComboBox su modulių pavadinimais rūšiavimui.
        /// Updates a combo box with module names for sorting.
        /// </summary>
        /// <param name="comboBox">ComboBox, kurį reikia atnaujinti / The combo box to update</param>
        /// <param name="modules">Modulių sąrašas / List of modules</param>
        /// <param name="keepItemCount">Kiek pradinių elementų palikti (pvz., 2: vardas ir vidurkis) / Number of initial items to keep (typically 2 for name and average)</param>
        /// <returns>True, jei pavyko; false, jei ne / True if successful, false otherwise</returns>
        public static bool UpdateComboBoxWithModules(ComboBox comboBox, List<Module> modules, int keepItemCount = 2)
        {
            if (comboBox == null || modules == null)
                return false;

            try
            {
                // Išvalome esamus modulio stulpelius, bet paliekame nurodytą pradinį pasirinkimų skaičių
                while (comboBox.Items.Count > keepItemCount)
                {
                    comboBox.Items.RemoveAt(keepItemCount);
                }

                // Pridedame modulių stulpelius
                for (int i = 0; i < modules.Count; i++)
                {
                    comboBox.Items.Add(new ComboBoxItem
                    {
                        Content = modules[i].ModuleName,
                        Tag = i + 1 // +1 nes pirmas stulpelis yra studento vardas
                    });
                }

                // Pasirenkame pirmą elementą, jei niekas nepasirinkta
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
