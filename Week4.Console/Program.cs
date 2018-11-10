using ClubDomain.Classes.ClubModels;
using Clubs.Model;
using CsvHelper;
using MVCClubsWeek4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
/*
 * To run this console app without setting it as the deafault startup project 
 * right click on the colsole project and shoose debug -> start new instance
 * Note you could seed the context based on a class library with a club console app but not the 
 * ApplicationDB context unless you add a refrence to Microsoft.AspNet.Identity.EntityFramework see 
 * ViewApplicationUsers() below and see the nuget references on this console app.
 */
namespace Week4.Console
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //AssignRandomStudentsToMemberlessClubs();
            //ListClubs();
            //ListClubsNoAdmin();
            ViewApplicationUsers();
            System.Console.ReadKey();
        }
        public static void ListStudentsforClubs(List<Student> students)
        {
            // Test Random Selection of students
            List<string> RandomSIDs = students.Select(
                s => new { s.StudentID, gid = Guid.NewGuid() }).OrderBy(g => g.gid)
                .Select(s => s.StudentID).ToList();

            List<Student> sublist = students.Where(s => RandomSIDs.Contains(s.StudentID)).Take(9).ToList();
            int c = 0;
            foreach (var item in sublist)
            {
                if ((c % 3) == 0)
                    System.Console.WriteLine("Change Club at {0}", c);
                System.Console.WriteLine("Random Student {0}", item.ToString());
                c++;
            }

        }
        public static void ViewApplicationUsers()
        {
            /* NOTE: in order to get the db context to work in this console app I had to 
             * add a nuget reference to the Microsoft.AspNet.Identity.EntityFramework
             * and a refernce to to the MVCClubsWeek4 project
             * and the namespace of the ApplicationDbContext
             */
            ApplicationDbContext db = new ApplicationDbContext();
            foreach (var item in db.Users)
            {
                // Iterating over the ASP membership Application Users
                System.Console.WriteLine("Identity User First Name {0} Second Name {1}", 
                                item.FirstName, item.Surname);
            }
            // Iterate over the Application Users and get their roles
            var usersWithRoles = (from user in db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in db.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new 
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  });
            foreach (var userWithRole in usersWithRoles)
            {
                System.Console.WriteLine("Identity User Name {0} Role Name {1}",
                                userWithRole.Username, userWithRole.Role);
            }
        }

        public static void LoadStudents()
        {
            List<Student> students = new List<Student>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "Week4.Console.StudentList1.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    students = csvReader.GetRecords<Student>().ToList();
                    foreach (var item in students)
                    {
                        System.Console.WriteLine("{0}", item.ToString());
                    }
                    System.Console.ReadKey();
                }
            }


        }
        public static void LoadProgrammes()
        {
            List<Programme> programmes = new List<Programme>();
            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            //
            string resourceName = "Week4.Console.Courses.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    programmes = csvReader.GetRecords<Programme>().ToList();
                    foreach (var item in programmes)
                    {
                        System.Console.WriteLine("{0}", item.ToString());
                    }
                    System.Console.ReadKey();
                }
            }

        }
        public static void AssignRandomStudentsToMemberlessClubs()
        {
            ClubContext ctx = new ClubContext();

            List<Club> ClubWithNoMembers = ctx.Clubs
                .Where(club => club.clubMembers.Count() < 1)
                .ToList();

            if (ClubWithNoMembers.Count() > 0)
            {
                foreach (var club in ClubWithNoMembers)
                    addMemberstoClub(ctx, club);
            }
            System.Console.ReadKey();
        }
        private static void addMemberstoClub(ClubContext ctx, Club club)
        {
            List<string> RandomSIDs = ctx.Students.Select(
                      s => new { s.StudentID, gid = Guid.NewGuid() })
                      .OrderBy(g => g.gid)
                      .Select(s => s.StudentID)
                      .ToList();
            List<ClubDomain.Classes.ClubModels.Student> sublist =
                    ctx.Students.Where(s => RandomSIDs.Contains(s.StudentID)).Take(3).ToList();
            foreach (var item in sublist)
                //ClubWithNoMembers.clubMembers.Add(new Member { });
                System.Console.WriteLine("Adding {0} to Club ", item.StudentID, club.ClubId);
        }

        // List all the clubs without admins assigned 
        // and list the students details assocaited with teh club members
        private static void ListClubsNoAdmin()
        {
                ClubContext clubc = new ClubContext();
                List<Club> ClubsWithNoAdmin = clubc.Clubs.Where(c => c.adminID == 0).ToList();
            // if there are any clubs with no admin
            if (ClubsWithNoAdmin.Count() > 0)
                foreach (var club in ClubsWithNoAdmin)
                {
                    System.Console.WriteLine(club.ClubName);
                    foreach (var member in club.clubMembers)
                        // Show current Club -> member -> student details
                        System.Console.WriteLine(" club Member is student {0} ", member.studentMember.FirstName, member.studentMember.SecondName);
                }

        }
        public static void ListClubs()
        {
            using (ClubContext clubContext = new ClubContext())
            {
                foreach (var item in clubContext.Clubs)
                {
                    System.Console.WriteLine(item.ClubName);
                }
            }
        }
    }
}
