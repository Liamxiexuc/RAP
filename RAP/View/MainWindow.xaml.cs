using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RAP.Research;
using RAP.Control;
using RAP.Database;

namespace RAP.View
{
    /// <summary>
    /// MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller controller = new Controller();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            List<Researcher> listResearcher = controller.ResearcherBasic;

            //The use of a delegate here is not necessa ry, but a remnant of the Week 7 tutorial
            //  doSomething = mycon.Display;

            lbResearcher.ItemsSource = listResearcher; 
        }

        private void LbResearcher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get selected researcher object
            // Researcher researcherSel = (Researcher)lbResearcher.SelectedItem;
            Researcher researcherSel = (Researcher)lbResearcher.SelectedItem;

            if ( researcherSel == null )
            {
                return;
            }

            string reseacherType = Convert.ToString(researcherSel.Type);

            if( reseacherType == "Staff")
            {
                Staff staff = new Staff();
                staff = Database.Database.LoadStaffDetails(researcherSel.Id);
                staff.PubCount = Database.Database.PubCounts(researcherSel.Id);
                staff.TYAve = Database.Database.GetTYAve(researcherSel.Id);
                spResearcherDetails.DataContext = staff;
            }
            else if( reseacherType == "Student")
            {
                Student student = new Student();
                student = Database.Database.LoadStudentDetails(researcherSel.Id);
                student.SupervisorName = Database.Database.GetSupName(researcherSel.Id);
                student.PubCount = Database.Database.PubCounts(researcherSel.Id);
                spResearcherDetails.DataContext = student;
            }

            List<Publication> listPublication = Database.Database.LoadPublications(researcherSel.Id);
            lbPublication.ItemsSource = listPublication;


            //Controller fullconn = new Controller();
            //List<Researcher> researcherFull = fullconn.ResearcherAll;
            //List<Researcher> selRes = new List<Researcher>();
            //foreach (Researcher i in researcherFull)
            //{
            //    if (i.Id == researcherSel.Id)
            //    {
            //        Researcher researchFull = new Researcher();
            //        researchFull.Id = i.Id;
            //        researchFull.GivenName = i.GivenName;
            //        researchFull.FamilyName = i.FamilyName;
            //        researchFull.FullName = i.FullName;
            //        researchFull.Title = i.Title;
            //        researchFull.Unit = i.Unit;
            //        researchFull.Campus = i.Campus;
            //        researchFull.Email = i.Email;
            //        researchFull.CurrentJob = i.CurrentJob;
            //        researchFull.UtasStart = i.UtasStart;
            //        researchFull.CurrentStart = i.CurrentStart;

            //        researchFull.Tenure = i.Tenure;


            //        researchFull.Degree = i.Degree;
            //        researchFull.SupervisorName = Database.Database.GetSupName( i.Id);
            //        researchFull.PubCount = Database.Database.PubCounts(i.Id);
            //        researchFull.TYAve = Database.Database.GetTYAve(i.Id);
            //        researchFull.Publications = i.Publications;
            //        //LBIid.Content = i.Id;
            //        //LBIFN.Content = i.GivenName;
            //        //LBILN.Content = i.FamilyName;
            //        //LBIemail.Content = i.Email;
            //        //LBIcampus.Content = i.Campus;
            //        //LBIschool.Content = i.School;
            //        //LBItitle.Content = i.Title;
            //        selRes.Add(researchFull);
            //    }
            //}          
            //spResearcherDetails.DataContext = selRes;

            //List<Publication> xxx = new List<Publication>();
            //xxx = Database.Database.LoadPublications(researcherSel.Id);
            //PublicationList.DataContext = xxx;




        }       

        private void DBLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

          //  spResearcherDetails.DataContext = null;
            ComboBox cb = (ComboBox)sender;
            //Get selected value from ComboBox
            EmploymentLevel level = (EmploymentLevel)Enum.Parse(typeof(EmploymentLevel), cb.SelectedItem.ToString(), false);
            //Call LevelFilter function
            controller.LevelFilter(level);
            lbResearcher.ItemsSource = controller.GetViewableList();

        }

        private void DBLevel_Loaded(object sender, RoutedEventArgs e)
        {
            DBLevel.ItemsSource = System.Enum.GetNames(typeof(EmploymentLevel));
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Get input text from TextBox
            string Input = tbName.Text;
            // Call NameFilter function
            if (Input == "")
            {
                lbResearcher.ItemsSource = null;
            }
            else
            {
                controller.NameFilter(Input);
                lbResearcher.ItemsSource = controller.GetViewableList();
            }
        }
    }
}
