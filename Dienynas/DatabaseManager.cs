using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;

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
        public void DeleteStudent(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Students WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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


    }
}
