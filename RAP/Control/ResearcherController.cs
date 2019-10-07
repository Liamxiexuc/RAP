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

        private List<Researcher> researcherAll;
        public List<Researcher> ResearcherAll { get { return researcherAll; } set { } }

        private ObservableCollection<Researcher> viewableResearcher;
        public ObservableCollection<Researcher> VisibleReasearcher { get { return viewableResearcher; } set { } }

        public Controller()
        {
            researcherBasic = Database.Database.LoadBasic();
            // ResearcherBasic = Database.Database.FetchBasicresearcherDetails();
            researcherAll = Database.Database.LoadAll();

            foreach (Researcher r in researcherAll)
            {
                r.Publications = Database.Database.LoadPublications(r.Id);
            }

               viewableResearcher = new ObservableCollection<Researcher>(researcherAll); //this list we will modify later

        }

        public ObservableCollection<Researcher> GetViewableList()
        {
            return VisibleReasearcher;
        }

        public void LevelFilter(EmploymentLevel employmentLevel)
        {
            var selected = from Researcher r in researcherAll
                           where employmentLevel == EmploymentLevel.All || r.Level == employmentLevel
                           select r;
            viewableResearcher.Clear();
            //Converts the result of the LINQ expression to a List and then calls viewableResearcher.Add with each element of that list in turn
            selected.ToList().ForEach(viewableResearcher.Add);

        }
        public void NameFilter(string GivenName)
        {
            var selected = from Researcher r in researcherAll
                           where (GivenName == null || GivenName.Length <= 0) || r.GivenName.ToLower().Contains(GivenName.ToLower()) || r.FamilyName.ToLower().Contains(GivenName.ToLower())

                           select r;
            viewableResearcher.Clear();
            //Converts the result of the LINQ expression to a List and then calls viewableResearcher.Add with each element of that list in turn
            selected.ToList().ForEach(viewableResearcher.Add);

        }
       


     }

}
