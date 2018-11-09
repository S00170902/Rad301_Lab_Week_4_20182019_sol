using ClubDomain.Classes.ClubModels;
using Clubs.Model;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Week4.Console
{
    class Program
    {
        
        static void Main(string[] args)
        {

            AssignRandomStudentsToMemberlessClubs();
            //ListClubs();

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

            List<Student> students = new List<Student>();
            resourceName = "Week4.Console.StudentList1.csv";
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
            // Test Random Selection of students
            List<string> RandomSIDs = students.Select(
                s => new { s.StudentID, gid = Guid.NewGuid() }).OrderBy(g =>g.gid)
                .Select(s => s.StudentID).ToList();

            List<Student> sublist = students.Where(s => RandomSIDs.Contains(s.StudentID)).Take(9).ToList();
            int c = 0;
            foreach (var item in sublist)
            {
                if ((c % 3) == 0)
                    System.Console.WriteLine("Change Club at {0}",c);
                System.Console.WriteLine("Random Student {0}", item.ToString());
                c++;
            }
            System.Console.ReadKey();
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
