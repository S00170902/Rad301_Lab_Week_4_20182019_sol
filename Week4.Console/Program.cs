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

        }
    }
}
