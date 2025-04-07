using System;
using System.Collections.Generic;
using System.Linq;

namespace Dienynas
{
    //  Studento objekto klase
 
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }


        /// Studento konstruktorius
        public Student(int id, string name, string lastname)
        {
            this.Id = id;
            this.Name = name;
            this.Lastname = lastname;
        }
        public Student()
        {
            // Tuscias konstruktorius dėl duomenų užpildymo
        }
    }
}
