﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Research;

namespace RAP.Database
{
    class Database
    {

        //These would not be hard coded in the source file normally, but read from the application's settings (and, ideally, with some amount of basic encryption applied)
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection conn = null;

        //Part of step 2.3.3 in Week 9 tutorial. This method is a gift to you because .NET's approach to converting strings to enums is so horribly broken
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Creates and returns (but does not open) the connection to the database.
        /// </summary>
        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                //Note: This approach is not thread-safe
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3};", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        public static List<Researcher> LoadBasic()

        {
            List<Researcher> researcherBasic = new List<Researcher>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select title, given_name, family_name, id, type, ifnull(level,'Student') as level  from researcher order by family_name", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //Note that in your assignment you will need to inspect the *type* of the
                    //employee/researcher before deciding which kind of concrete class to create.
                    researcherBasic.Add(new Researcher
                    {
                        Title = rdr.GetString(0),
                        GivenName = rdr.GetString(1),
                        FamilyName = rdr.GetString(2),
                        Id = rdr.GetInt32(3),
                        Type = ParseEnum<Research.Type>(rdr.GetString(4)),
                        Level = ParseEnum<EmploymentLevel>(rdr.GetString(5))
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return researcherBasic;
        }

        public static Staff LoadStaffDetails(int Id)
        {
            Staff StaffDetails = new Staff();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select title, given_name, family_name, id, campus," +
                    "email, photo, ifnull(level,'Student') as level, unit, utas_start, current_start from researcher where id=?id", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    StaffDetails = (new Staff
                    {
                        Title = rdr.GetString(0),
                        GivenName = rdr.GetString(1),
                        FamilyName = rdr.GetString(2),
                        Id = rdr.GetInt32(3),
                        Campus = ParseEnum<Campus>(rdr.GetString(4).Replace(" ", "")),
                        Email = rdr.GetString(5),
                        Photo = rdr.GetString(6),
                        Level = ParseEnum<EmploymentLevel>(rdr.GetString(7)),
                        Unit = rdr.GetString(8),
                        UtasStart = rdr.GetDateTime(9),
                        CurrentStart = rdr.GetDateTime(10),
                        CurrentJob = GetJobTitle(rdr.GetString(7)),
                        FullName = rdr.GetString(1) + " " + rdr.GetString(2),
                        Tenure = Math.Round((Convert.ToDouble((DateTime.Now - rdr.GetDateTime(9)).TotalDays)/365),2),
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return StaffDetails;
        }

        public static Student LoadStudentDetails(int Id)
        {
            Student StudentDetails = new Student();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select title, given_name, family_name, id, campus," +
                            "email, photo, ifnull(level,'Student') as level, unit, ifnull(degree,'') as degree, ifnull(supervisor_id, 0) as supervisor_id, utas_start, current_start from researcher where id=?id", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    StudentDetails = (new Student
                    {
                        Title = rdr.GetString(0),
                        GivenName = rdr.GetString(1),
                        FamilyName = rdr.GetString(2),
                        Id = rdr.GetInt32(3),
                        Campus = ParseEnum<Campus>(rdr.GetString(4).Replace(" ", "")),
                        Email = rdr.GetString(5),
                        Photo = rdr.GetString(6),
                        Level = ParseEnum<EmploymentLevel>(rdr.GetString(7)),
                        Unit = rdr.GetString(8),
                        Degree = rdr.GetString(9),
                        SupervisorId = rdr.GetInt32(10),
                        UtasStart = rdr.GetDateTime(11),
                        CurrentStart = rdr.GetDateTime(12),                     
                        CurrentJob = "Student",
                        FullName = rdr.GetString(1) + " " + rdr.GetString(2),
                        Tenure = Math.Round((Convert.ToDouble((DateTime.Now - rdr.GetDateTime(11)).TotalDays) / 365), 2)
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return StudentDetails;
        }

        public static List<Publication> LoadPublications(int Id)
        {
            List<Publication> Publications = new List<Publication>();
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select p.title, p.year, p.doi from publication as p, researcher_publication as rp, researcher as r where p.doi=rp.doi and r.id=rp.researcher_id and rp.researcher_id=?id order by p.year desc, p.title", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Publications.Add(new Publication
                    {
                        Title = rdr.GetString(0),
                        Year = rdr.GetInt32(1),
                        // doi is the primary key of publication table, so we get doi as an uniqe id to connect to publication details. 
                        DOI = rdr.GetString(2)
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return Publications;
        }

        public static Publication LoadPublicationDetails(string Doi)
           {
               Publication PublicationDetails = new Publication();
               MySqlConnection conn = GetConnection();
               MySqlDataReader rdr = null;
               try
               {
                   conn.Open();

                   MySqlCommand cmd = new MySqlCommand("select title, year, type, available, doi, authors, cite_as from publication where doi=?doi", conn);

                   cmd.Parameters.AddWithValue("doi", Doi);
                   rdr = cmd.ExecuteReader();

                while (rdr.Read())
                   {
                    PublicationDetails = (new Publication
                       {
                           Title = rdr.GetString(0),
                           Year = rdr.GetInt32(1),
                           OutputType = ParseEnum<OutputType>(rdr.GetString(2)),
                           AvaDate = rdr.GetDateTime(3),
                           DOI = rdr.GetString(4),
                           Authors = rdr.GetString(5),
                           CiteAs = rdr.GetString(6),
                           Age = Convert.ToInt32((DateTime.Now - rdr.GetDateTime(3)).TotalDays)
                    });
                   }
               }
               catch (MySqlException e)
               {
                   Console.WriteLine("Error connecting to database: " + e);
               }
               finally
               {
                   if (rdr != null)
                   {
                       rdr.Close();
                   }
                   if (conn != null)
                   {
                       conn.Close();
                   }
               }

               return PublicationDetails;
           }

        public static string GetJobTitle(string Job)
        {
            string JobTitle;
            switch (Job)
            {
                case "Student":
                    JobTitle = "Student";
                    break;
                case "A":
                    JobTitle = "Postdoc";
                    break;
                case "B":
                    JobTitle = "Lecturer";
                    break;
                case "C":
                    JobTitle = "Senior Lecturer";
                    break;
                case "D":
                    JobTitle = "Associate Professor";
                    break;
                case "E":
                    JobTitle = "Professor";
                    break;
                default:
                    JobTitle = "None";
                    break;
            }
            return JobTitle;
        }

        public static string GetSupName(int Id)
        {
            string SupName;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select S.given_name, S.family_name from researcher as R, researcher as S " +
                                                    "where R.id = S.id and R.id=?id", conn);
            cmd.Parameters.AddWithValue("id", Id);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                SupName = rdr.GetString(0) + " " + rdr.GetString(1);
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return SupName;
            }
            else
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return "No data found";
            }
        }

        public static int PubCounts(int Id)
        {
            int count;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(doi) from researcher_publication " +
                                                    "where researcher_id=?id", conn);
            cmd.Parameters.AddWithValue("id", Id);
            rdr = cmd.ExecuteReader();
            count = rdr.Read() ? rdr.GetInt32(0) : 0;

            if (rdr != null)
            {
               rdr.Close();
            }
            if (conn != null)
            {
               conn.Close();
            }
               return count;
        }

        public static double GetTYAve(int Id)
        {
            double TYAve;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(RP.doi) from researcher_publication as RP, " +
                                                 "publication as P where RP.doi=P.doi and RP.researcher_id=?id and P.year in (2017, 2018, 2019)", conn);
            //datediff(Year(Now()), P.year) < 4
            cmd.Parameters.AddWithValue("id", Id);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                TYAve = Math.Round((Convert.ToDouble(rdr.GetInt32(0)))/3, 2);
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return TYAve;
            }
            else
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return 0;
            }
        }
    }
}
