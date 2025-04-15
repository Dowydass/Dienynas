using MySql.Data.MySqlClient;
using System;

using System.Collections.Generic;

namespace Dienynas
{
    /// Database Table structure:

    /// (Modules - id) + (Student - id) = (Grades - id, Modules - id, Student - id)
    public class DatabaseManager
    {
        private string connectionString = "server=localhost;user=root;password=Admin123;database=database;";

        public void ConnectAndFetchUsers()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Prisijungta prie MySQL!");

                    string query = "SELECT * FROM students";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                       
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Klaida: " + ex.Message);
                    MainWindow.ShowMessage("Duomenų bazė: " + ex.Message, "Klaida");
                    MainWindow.CloseWindow();
                }
            }
        }

        /* <summary>
         Gauti studentų sąrašą iš duomenų bazės.
         MainWindow klasėje naudojamas kaip studentų sąrašas.
         Grąžina sąrašą Student objektų.
         </summary>
        */
        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
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
            return students;
        }


       
        /// UNDONE: Prideda studentą į duomenų bazę.
        public void AddStudent(string vardas, string pavarde)
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
                    Console.WriteLine("Studentas pridėtas: " + vardas + " " + pavarde);
                }
            }
        }


        /// TODO: Ištrinti studentą iš duomenų bazės pagal ID.
        public void DeleteStudentFromModule(int studentId, int moduleId)
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

        public void UpdateStudent(int id, string newVardas, string newPavarde)
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
     


        public List<Module> GetModules()
        {
            List<Module> modules = new List<Module>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Modules";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modules.Add(new Module
                        (
                            reader.GetInt32("id"), // Pass the 'id' parameter
                            reader.GetString("Modulis") // Pass the 'ModuleName' parameter
                        ));
                    }
                }
            }
            return modules;
        }
        public void AddModule(string moduleName)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Modules (Modulis) VALUES (@modulis)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@modulis", moduleName);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Modulis pridėtas: " + moduleName);
                }
            }
        }
        public List<Grade> GetGrades()
        {
            List<Grade> grades = new List<Grade>();
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
            return grades;
        }
        public string[,] GetStudentGradesMatrix()
        {
            List<Module> modules = GetModules();
            List<Student> students = GetStudents();
            int totalColumns = modules.Count + 2; // Modules + StudentName + Average
            string[,] matrix = new string[students.Count, totalColumns];

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

            return matrix;
        }


        public void AddGrade(int studentId, int moduleId, int grade)
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


        public void ResetDatabase()
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
                MainWindow.ShowMessage("Duomenų bazė atstatyta: visos lentelės ištrintos ir sukurtos iš naujo.", "Sėkmingai");
            }
        }
    }
}
