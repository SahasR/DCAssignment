using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    public class Service
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string APIEndpoint { get; set; }
        public int numOperands { get; set; }
        public string operandtype { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
