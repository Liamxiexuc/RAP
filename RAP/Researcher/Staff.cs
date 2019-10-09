using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Research
{
    class Staff : Researcher
    {
        public double TYAve { set; get; }

        public float Performance { set; get; }

        public List<Student> Supervisions { set; get; }
  
    }

}
