using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp
{
    public class Names
    {
        public string Name { get; set; }
        public string MakeFullName(string fname, string lname)
        {
            return $"{fname} {lname}";
        }
    }
}
