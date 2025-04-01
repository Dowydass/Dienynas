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

        
    }
}
