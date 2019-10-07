using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Research
{
    enum EmploymentLevels
    {
        Student,
        A,
        B,
        C,
        D,
        E
    }
    class Position
    {
        public EmploymentLevels level { set; get; }
        public DateTime start { set; get; }
        public DateTime end { set; get; }
        //  public string Title
        // {

        // }

        //   public string ToTitle
        //   {

        //   }
    }

}
