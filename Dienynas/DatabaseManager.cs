using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace Dienynas
{
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
                        Console.WriteLine(reader["firstname"]);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Klaida: " + ex.Message);
                }
            }
        }
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
                            Name = reader.GetString("firstname"),
                            Lastname = reader.GetString("lastname")
                        });
                    }
                }
            }
            return students;
        }

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
                }
            }
        }


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
    }
}
