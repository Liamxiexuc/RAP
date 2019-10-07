using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Database;

namespace RAP.Research
{
    enum EmploymentLevel
    {
        All,
        Student,
        A,
        B,
        C,
        D,
        E
    }

    enum Type
    {
        Student,
        Staff
    }

    enum Campus
    {
        Hobart,
        Launceston,
        CradleCoast
    }

    class Researcher
    {
        public int Id { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string FullName { get; set; }

        public string Title { get; set; }

        public List<Publication> Publications { get; set; }

        public string School { get; set; }

        public Campus Campus { get; set; }

        public string Email { get; set; }

        public string Photo { get; set; }

     //   public string Basic { get; set; }

        public EmploymentLevel Level { set; get; }

        public Type Type { set; get; }

        public string Unit { set; get; }

        public string Degree { set; get; }

        public int SupervisorId { set; get; }

        public DateTime UtasStart { set; get; }

        public DateTime CurrentStart { set; get; }

        private string currentJob;

        public string CurrentJob
        {
            set
            {
             /*   switch (value)
                {
                    case "Student":
                        currentJob = "Student";
                        break;
                    case "A":
                        currentJob = "Postdoc";
                        break;
                    case "B":
                        currentJob = "Lecturer";
                        break;
                    case "C":
                        currentJob = "Senior Lecturer";
                        break;
                    case "D":
                        currentJob = "Associate Professor";
                        break;
                    case "E":
                        currentJob = "Professor";
                        break;   */

                if(value == "Student")
                {
                    currentJob = "Student";
                    return;
                }else if(value =="A")
                {
                    currentJob = "Postdoc";
                    return;
                }
                else if (value == "B")
                {
                    currentJob = "A";
                    return;
                }
                else
                {
                    currentJob = "C";
                    return;
                }


            }
            get
            {
                return currentJob;
            }
        }

        //public void GetCurrentJob(EmploymentLevel Level)
        //{
        //    string Temp = Level.ToString();
        //    switch (Temp)
        //    {
        //        case "Student":
        //            CurrentJob = "Student";
        //            break;
        //        case "A":
        //            CurrentJob = "Postdoc";
        //            break;
        //        case "B":
        //            CurrentJob = "Lecturer";
        //            break;
        //        case "C":
        //            CurrentJob = "Senior Lecturer";
        //            break;
        //        case "D":
        //            CurrentJob = "Associate Professor";
        //            break;
        //        case "E":
        //            CurrentJob = "Professor";
        //            break;
        //        default:
        //            CurrentJob = "None";
        //            break;
        //    }
        //}

        /* public Position GetCurrentjob()
          {
            public string name { get; set }
            return name
          }  */

        //  public string CurrentJobTitle()
        //  {

        //  }

        //  public Date CurrentJobStart()
        //   {

        //  }

        //  public Position GetEarliestJob()
        // {

        // }

        //  public Date EarliestStart()
        //  {

        //  }

        //  public float Tenure()
        //  {

        //  }

        // public int PublicationsCount()
        // {

        //  }

        public override string ToString()
        {

            return FamilyName + ", " + GivenName + " (" + Title + ")";

        }
    }
}
