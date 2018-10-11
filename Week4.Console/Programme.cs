using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4.Console
{
    public class Programme
    {
        public string Code { get; set; }
        public string year { get; set; }
        public string Desription { get; set; }

        public override string ToString()
        {
            return Code + " " + year + " " + Desription;
        }
    }
}
