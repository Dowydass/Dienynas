using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienynas
{
    /// <summary>
    /// Klasė, kuri apima užduočių valdymo funkcijas.
    class TaskUtils

    {
        public static List<Student> SortStudentsByName(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Name).ToList()
                : students.OrderByDescending(student => student.Name).ToList();
        }
        public static List<Student> SortStudentsBySurname(List<Student> students, bool ascending = true)
        {
            return ascending
                ? students.OrderBy(student => student.Lastname).ToList()
                : students.OrderByDescending(student => student.Lastname).ToList();
        }
    }
}
