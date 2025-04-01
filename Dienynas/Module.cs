using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dienynas
{
    //  Modulio objekto klase
    class Module
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }

        /// Modulio konstruktorius

        public Module(int id, string moduleName)
        {
            this.Id = id;
            this.ModuleName = moduleName;
        }
    }
}
