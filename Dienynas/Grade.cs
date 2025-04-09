using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienynas
{
    //  Grade klase skirta saugoti ir apdoroti duomenis apie pažymius
 
    public class Grade
    {
        public int Id {  get; set; }
        public int StudentId { get; set; }
        public int ModuleId { get; set; }
        public int StudentGrade { get; set; }

        /// Pažymio konstruktorius
        public Grade(int id, int studentId, int moduleId, int grade)
        {
            this.Id = id;
            this.StudentId = studentId;
            this.ModuleId = moduleId;
            this.StudentGrade = grade;
        }
    }
}
