using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Research;
using RAP.Database;
using System.Collections.ObjectModel;

namespace RAP.Control
{
    class Controller
    {
        private List<Researcher> researcherBasic;
        public List<Researcher> ResearcherBasic { get { return researcherBasic; } set { } }


        private ObservableCollection<Researcher> viewableResearcher;
        public ObservableCollection<Researcher> VisibleReasearcher { get { return viewableResearcher; } set { } }

        public ObservableCollection<Researcher> GetViewableList()
        {
            return VisibleReasearcher;
        }

        public Controller()
        {
            researcherBasic = Database.Database.LoadBasic();
            viewableResearcher = new ObservableCollection<Researcher>(researcherBasic); 
        }

        public void LevelFilter(EmploymentLevel employmentLevel)
        {
            var selected = from Researcher r in researcherBasic
                           where employmentLevel == EmploymentLevel.All || r.Level == employmentLevel
                           select r;
            viewableResearcher.Clear();
            //Converts the result of the LINQ expression to a List and then calls viewableResearcher.Add with each element of that list in turn
            selected.ToList().ForEach(viewableResearcher.Add);
        }

        public void NameFilter(string GivenName)
        {
            var selected = from Researcher r in researcherBasic
                           where (GivenName == null || GivenName.Length <= 0) || r.GivenName.ToLower().Contains(GivenName.ToLower()) || r.FamilyName.ToLower().Contains(GivenName.ToLower())

                           select r;
            viewableResearcher.Clear();
            //Converts the result of the LINQ expression to a List and then calls viewableResearcher.Add with each element of that list in turn
            selected.ToList().ForEach(viewableResearcher.Add);
        }
       

     }

}
