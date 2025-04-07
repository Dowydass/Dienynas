using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienynas
{
    class InOutUtils
    {
    
    
        private DatabaseManager dbManager = new DatabaseManager();

        public void AddStudent(string firstName, string lastName)
        {
            dbManager.AddStudent(firstName, lastName);
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
*/
    }
}
