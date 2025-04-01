using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienynas
{
    //  Grade klase skirta saugoti ir apdoroti duomenis apie pažymius
 
    class Grade
    {
        public int id {  get; set; }
        public int studentId { get; set; }
        public int moduleId { get; set; }
        public int grade { get; set; }

        /// Pažymio konstruktorius
        public Grade(int id, int studentId, int moduleId, int grade)
        {
            this.id = id;
            this.studentId = studentId;
            this.moduleId = moduleId;
            this.grade = grade;
        }
    }
}
