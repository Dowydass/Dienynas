using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dienynas
{
    class InOutUtils
    {
    
   
        private DatabaseManager dbManager = new DatabaseManager();

        public static void AddStudent(string firstName, string lastName)
        {
            Student student = new Student(firstName, lastName);
            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.AddStudent(student.Name, student.Lastname);
        }
        public static List<Student> GetStudents()
        {

            DatabaseManager dbManager = new DatabaseManager();
            dbManager.GetStudents();
            List<Student> students = dbManager.GetStudents();
           
            Console.WriteLine("Studentai:");
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.Id}, Vardas: {student.Name}, Pavardė: {student.Lastname}");
            }
            return students;


        }
        public static List<Module> GetModules()
        {
         DatabaseManager dbManager = new DatabaseManager();
            dbManager.GetModules();
            List<Module> modules = dbManager.GetModules();
         
            Console.WriteLine("Moduliai:");
            foreach (var module in modules)
            {
                Console.WriteLine($"ID: {module.Id}, Pavadinimas: {module.ModuleName}");
            }


            return modules;
        }
        public static void AddModule(string moduleName)
        {
            Module module = new Module(0, moduleName); // Create a new Module object
            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.AddModule(module.ModuleName); // Call the DatabaseManager to add the module
            Console.WriteLine($"Modulis '{moduleName}' sėkmingai pridėtas.");
        }
        public static List<Grade> GetGrades()
        {
            DatabaseManager dbManager = new DatabaseManager();
            dbManager.GetGrades();
            List<Grade> grades = dbManager.GetGrades();

            Console.WriteLine("Įvertinimai:");
            foreach (var grade in grades)
            {
                Console.WriteLine($"ID: {grade.Id}, Student ID: {grade.StudentId}, Module ID: {grade.ModuleId}, Įvertinimas: {grade.StudentGrade}");
            }
            return grades;
        }


        public void DeleteStudent(int studentId)
        {
            dbManager.DeleteStudent(studentId);
        }

        public void EditStudent(int studentId, string newFirstName, string newLastName)
        {
            dbManager.UpdateStudent(studentId, newFirstName, newLastName);
        }

        public void AddGrade(int studentId, int moduleId, double grade)
        {
           // dbManager.AssignGrade(studentId, moduleId, grade);
        }
        /*
                public List<Student> SearchStudents(string query)
                {
                    return dbManager.SearchStudents(query);
                }

                public List<Student> SortStudents(string column, bool ascending)
                {
                    return dbManager.SortStudents(column, ascending);
                }
          private void DeleteStudent_Click(object sender, RoutedEventArgs e)
                {
                    // Delete student logic here
                }
        */
       
    }
}
