using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Dienynas
{
    /// <summary>
    /// Database manager class that handles all database operations.
    /// 
    /// Database Table structure:
    /// (Modules - id) + (Student - id) = (Grades - id, Modules - id, Student - id)
    /// </summary>
    public class DatabaseManager
    {
        private readonly string connectionString = "server=localhost;user=root;password=;database=database;";

        /// <summary>
        /// Connects to the database and fetches users.
        /// </summary>
        public void ConnectAndFetchUsers()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connected to MySQL!");

                    string query = "SELECT * FROM students";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Process each student record if needed
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("MySQL Error: " + ex.Message);
                    MainWindow.ShowMessage("Database Error: " + ex.Message, "Error");
                    MainWindow.CloseWindow();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    MainWindow.ShowMessage("Database: " + ex.Message, "Error");
                    MainWindow.CloseWindow();
                }
            }
        }

        /// <summary>
        /// Gets a list of students from the database.
        /// Used in MainWindow as the student list.
        /// </summary>
        /// <returns>A list of Student objects</returns>
        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Students";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("Vardas"),
                                Lastname = reader.GetString("Pavarde")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting students: " + ex.Message);
                MainWindow.ShowMessage("Error getting students: " + ex.Message, "Database Error");
            }

            return students;
        }

        /// <summary>
        /// Adds a student to the database.
        /// </summary>
        /// <param name="vardas">Student's first name</param>
        /// <param name="pavarde">Student's last name</param>
        public void AddStudent(string vardas, string pavarde)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Students (Vardas, Pavarde) VALUES (@vardas, @pavarde)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@vardas", vardas);
                        command.Parameters.AddWithValue("@pavarde", pavarde);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Student added: " + vardas + " " + pavarde);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding student: " + ex.Message);
                MainWindow.ShowMessage("Error adding student: " + ex.Message, "Database Error");
            }
        }

        /// <summary>
        /// Deletes a student's grade from a specific module.
        /// </summary>
        /// <param name="studentId">The ID of the student</param>
        /// <param name="moduleId">The ID of the module</param>
        public void DeleteStudentFromModule(int studentId, int moduleId)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Grades WHERE StudentId = @studentId AND ModuleId = @moduleId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@moduleId", moduleId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting student from module: " + ex.Message);
                MainWindow.ShowMessage("Error deleting student from module: " + ex.Message, "Database Error");
            }
        }

        /// <summary>
        /// Updates a student's information in the database.
        /// </summary>
        /// <param name="id">The ID of the student to update</param>
        /// <param name="newVardas">The new first name</param>
        /// <param name="newPavarde">The new last name</param>
        public void UpdateStudent(int id, string newVardas, string newPavarde)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Students SET Vardas = @vardas, Pavarde = @pavarde WHERE Id = @id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@vardas", newVardas);
                        command.Parameters.AddWithValue("@pavarde", newPavarde);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating student: " + ex.Message);
                MainWindow.ShowMessage("Error updating student: " + ex.Message, "Database Error");
            }
        }

        /// <summary>
        /// Gets a list of all modules from the database.
        /// </summary>
        /// <returns>A list of Module objects</returns>
        public List<Module> GetModules()
        {
            List<Module> modules = new List<Module>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Modules";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            modules.Add(new Module(
                                reader.GetInt32("id"),
                                reader.GetString("Modulis")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting modules: " + ex.Message);
                MainWindow.ShowMessage("Error getting modules: " + ex.Message, "Database Error");
            }

            return modules;
        }

        /// <summary>
        /// Adds a new module to the database.
        /// </summary>
        /// <param name="moduleName">The name of the module to add</param>
        public void AddModule(string moduleName)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Modules (Modulis) VALUES (@modulis)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@modulis", moduleName);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Module added: " + moduleName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding module: " + ex.Message);
                MainWindow.ShowMessage("Error adding module: " + ex.Message, "Database Error");
            }
        }

        /// <summary>
        /// Gets a list of all grades from the database.
        /// </summary>
        /// <returns>A list of Grade objects</returns>
        public List<Grade> GetGrades()
        {
            List<Grade> grades = new List<Grade>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Grades";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grades.Add(new Grade(
                                reader.GetInt32("id"),
                                reader.GetInt32("StudentId"),
                                reader.GetInt32("ModuleId"),
                                reader.GetInt32("Pazymys")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting grades: " + ex.Message);
                MainWindow.ShowMessage("Error getting grades: " + ex.Message, "Database Error");
            }

            return grades;
        }

        /// <summary>
        /// Generates a matrix of student grades for all modules.
        /// </summary>
        /// <returns>A 2D string array containing student names, grades for each module, and average grades</returns>
        public string[,] GetStudentGradesMatrix()
        {
            List<Module> modules = GetModules();
            List<Student> students = GetStudents();
            int totalColumns = modules.Count + 2; // Modules + StudentName + Average
            string[,] matrix = new string[students.Count, totalColumns];

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    for (int i = 0; i < students.Count; i++)
                    {
                        Student student = students[i];
                        matrix[i, 0] = $"{student.Name} {student.Lastname}"; // Student Name

                        double totalGrades = 0;
                        int gradeCount = 0;

                        for (int j = 0; j < modules.Count; j++)
                        {
                            Module module = modules[j];
                            string query = "SELECT Pazymys FROM Grades WHERE StudentId = @studentId AND ModuleId = @moduleId";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@studentId", student.Id);
                                command.Parameters.AddWithValue("@moduleId", module.Id);

                                object result = command.ExecuteScalar();
                                if (result != null)
                                {
                                    int grade = Convert.ToInt32(result);
                                    matrix[i, j + 1] = grade.ToString(); // Module grade
                                    totalGrades += grade;
                                    gradeCount++;
                                }
                                else
                                {
                                    matrix[i, j + 1] = "-"; // No grade
                                }
                            }
                        }

                        // Calculate and store the average grade
                        matrix[i, totalColumns - 1] = gradeCount > 0 ? (totalGrades / gradeCount).ToString("0.00") : "-";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting student grades matrix: " + ex.Message);
                MainWindow.ShowMessage("Error getting student grades matrix: " + ex.Message, "Database Error");
            }

            return matrix;
        }

        /// <summary>
        /// Adds a grade for a student in a specific module.
        /// </summary>
        /// <param name="studentId">The ID of the student</param>
        /// <param name="moduleId">The ID of the module</param>
        /// <param name="grade">The grade value</param>
        public void AddGrade(int studentId, int moduleId, int grade)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Grades (StudentId, ModuleId, Pazymys) VALUES (@studentId, @moduleId, @grade)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@moduleId", moduleId);
                        command.Parameters.AddWithValue("@grade", grade);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding grade: " + ex.Message);
                MainWindow.ShowMessage("Error adding grade: " + ex.Message, "Database Error");
            }
        }

        /// <summary>
        /// Searches for students in the database by name or surname.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>A list of students matching the search criteria</returns>
        public List<Student> SearchStudentsInDatabase(string query)
        {
            List<Student> students = new List<Student>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string queryText = "SELECT * FROM Students WHERE Vardas LIKE @query OR Pavarde LIKE @query";
                    using (var command = new MySqlCommand(queryText, connection))
                    {
                        command.Parameters.AddWithValue("@query", "%" + query + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(new Student
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = reader.GetString("Vardas"),
                                    Lastname = reader.GetString("Pavarde")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching students: " + ex.Message);
                MainWindow.ShowMessage("Error searching students: " + ex.Message, "Database Error");
            }

            return students;
        }

        /// <summary>
        /// Updates a student's grade for a specific module.
        /// </summary>
        /// <param name="moduleId">The ID of the module</param>
        /// <param name="studentId">The ID of the student</param>
        /// <param name="newGrade">The new grade value</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        public bool UpdateGrade(int moduleId, int studentId, int newGrade)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Grades SET Pazymys = @grade WHERE ModuleId = @moduleId AND StudentId = @studentId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@grade", newGrade);
                        command.Parameters.AddWithValue("@moduleId", moduleId);
                        command.Parameters.AddWithValue("@studentId", studentId);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating grade: " + ex.Message);
                MainWindow.ShowMessage("Error updating grade: " + ex.Message, "Database Error");
                return false;
            }
        }

        /// <summary>
        /// Dynamically searches for students by their first name.
        /// </summary>
        /// <param name="name">The name or partial name of the student to search for</param>
        /// <returns>A list of students matching the search criteria</returns>
        public List<Student> SearchStudentsByName(string name)
        {
            List<Student> students = new List<Student>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Students WHERE Vardas LIKE @name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", "%" + name + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(new Student
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = reader.GetString("Vardas"),
                                    Lastname = reader.GetString("Pavarde")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching students by name: " + ex.Message);
                MainWindow.ShowMessage("Error searching students by name: " + ex.Message, "Database Error");
            }

            return students;
        }

        /// <summary>
        /// Searches for students in the data matrix by name or partial name.
        /// </summary>
        /// <param name="query">The name or partial name to search for</param>
        /// <returns>A filtered data matrix containing only the rows that match the search query</returns>
        public string[,] SearchStudentsInMatrix(string query)
        {
            string[,] matrix = GetStudentGradesMatrix();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            List<string[]> filteredRows = new List<string[]>();

            for (int i = 0; i < rows; i++)
            {
                if (matrix[i, 0].IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string[] row = new string[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        row[j] = matrix[i, j];
                    }
                    filteredRows.Add(row);
                }
            }

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
        /// Resets the database by dropping all tables and recreating them.
        /// </summary>
        public void ResetDatabase()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Drop existing tables
                    string dropTablesQuery = @"
                        DROP TABLE IF EXISTS Grades;
                        DROP TABLE IF EXISTS Modules;
                        DROP TABLE IF EXISTS Students;
                    ";
                    using (var dropCommand = new MySqlCommand(dropTablesQuery, connection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }

                    // Create new tables
                    string createTablesQuery = @"
                        CREATE TABLE Students (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            Vardas VARCHAR(100) NOT NULL,
                            Pavarde VARCHAR(100) NOT NULL
                        );

                        CREATE TABLE Modules (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            Modulis VARCHAR(100) NOT NULL
                        );

                        CREATE TABLE Grades (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            StudentId INT NOT NULL,
                            ModuleId INT NOT NULL,
                            Pazymys INT NOT NULL,
                            FOREIGN KEY (StudentId) REFERENCES Students(id) ON DELETE CASCADE,
                            FOREIGN KEY (ModuleId) REFERENCES Modules(id) ON DELETE CASCADE
                        );
                    ";
                    using (var createCommand = new MySqlCommand(createTablesQuery, connection))
                    {
                        createCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine("Database reset completed: All tables dropped and recreated.");
                    MainWindow.ShowMessage("Database reset: all tables deleted and recreated.", "Success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error resetting database: " + ex.Message);
                MainWindow.ShowMessage("Error resetting database: " + ex.Message, "Database Error");
            }
        }
    }
}
