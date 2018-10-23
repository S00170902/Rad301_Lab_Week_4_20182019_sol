namespace ClubDomain.Classes.Migrations
{
    using ClubDomain.Classes.ClubModels;
    using Clubs.Model;
    using CsvHelper;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Clubs.Model.ClubContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Clubs.Model.ClubContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            seed_db_programmes(context);
            seed_Db_students(context);
            seed_Db_model(context);

        }

        private void seed_db_programmes(ClubContext context)
        {
            List<Programme> programmes = new List<Programme>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            //
            string resourceName = "ClubDomain.Classes.Courses.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.MissingFieldFound = null;
                    programmes = csvReader.GetRecords<Programme>().ToList();
                    context.Programmes.AddOrUpdate(programmes.ToArray());
                    
                }
            }
            context.SaveChanges();
        }

        private void seed_Db_students(ClubContext context)
        {
            List<Student> students = new List<Student>();
            Assembly assembly = Assembly.GetExecutingAssembly();

            string resourceName = "ClubDomain.Classes.StudentList1.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.MissingFieldFound = null;
                    students = csvReader.GetRecords<Student>().ToList();
                    foreach (var item in students)
                    {
                        context.Students.AddOrUpdate(item);
                    }
                }
            }
            context.SaveChanges();
        }
        private void seed_Db_model(ClubContext context)
        {
            CultureInfo cultureinfo = CultureInfo.CreateSpecificCulture("en-IE");
            DateTime baseDate = DateTime.ParseExact("01/01/2012", "dd/mm/yyyy", cultureinfo);
            Random r = new Random();
            context.Clubs.AddOrUpdate(new Club[]
            {
                new Club{ ClubName="The Chess Club", CreationDate = DateTime.ParseExact("25/01/2017","dd/mm/yyyy",cultureinfo),
                clubMembers = new List<Member>
                {
                    new Member{ StudentID="S00698845", approved = true},
                    new Member{ StudentID="S00274643", approved = true}
                },
                clubEvents = new List<ClubEvent>
                {
                    new ClubEvent { Location= "It Sligo", Venue="D1030",
                        StartDateTime =DateTime.ParseExact("01/02/2017 17:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                        EndDateTime = DateTime.ParseExact("01/02/2017 21:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                    },
                    new ClubEvent { Location= "It Sligo", Venue="D1031",
                        StartDateTime =DateTime.ParseExact("25/03/2017 13:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                    EndDateTime = DateTime.ParseExact("01/02/2017 14:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                    }

                }
                },
                new Club{ ClubName = "Volley Ball Club", CreationDate = DateTime.ParseExact("01/02/2018","dd/mm/yyyy",cultureinfo),
                         clubMembers = new List<Member>
                            {
                                new Member{ StudentID="S00472973", approved = true},
                                new Member{ StudentID="S00806042", approved = true}
                            },
                clubEvents = new List<ClubEvent>
                {
                    new ClubEvent { Location= "It Sligo", Venue="Sports Arena",
                        StartDateTime =DateTime.ParseExact("15/02/2018 14:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                        EndDateTime = DateTime.ParseExact("15/02/2018 16:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                    },
                    new ClubEvent { Location= "Regional Sports Centre", Venue="Main Hall",
                        StartDateTime =DateTime.ParseExact("25/02/2018 16:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                        EndDateTime = DateTime.ParseExact("25/02/2018 19:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                    }

                }

                },
                new Club { ClubName = "Soccer Club", CreationDate = DateTime.ParseExact("07/09/2018", "dd/mm/yyyy",cultureinfo),
                                clubMembers = new List<Member>
                {
                    new Member{ StudentID="S00389325", approved = true},
                    new Member{ StudentID="S00472973", approved = true}
                }, // End Club members
                clubEvents = new List<ClubEvent>
                {
                    new ClubEvent { Location= "It Sligo", Venue="Main Pitch",
                        StartDateTime =DateTime.ParseExact("10/10/2018 15:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                        EndDateTime = DateTime.ParseExact("10/10/2018 21:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                    },
                    new ClubEvent { Location= "It Sligo", Venue="Astro Pitch",
                        StartDateTime =DateTime.ParseExact("05/11/2018 18:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                        EndDateTime = DateTime.ParseExact("05/11/2018 20:00:00","dd/MM/yyyy HH:mm:ss",cultureinfo),
                    }
                }
                }, // End of Club
            } // End of Clubs
            );
            context.SaveChanges();

            var memberEvents = (from club in context.Clubs
                                join member in context.ClubMembers
                                on club.ClubId equals member.AssociatedClub
                                join e in context.ClubEvents
                                on club.ClubId equals e.ClubId
                                select new { EventID = e.EventID, AttendeeMember = member.MemberID })
                                .ToArray();
            foreach (var item in memberEvents)
            {
                context.EventAttendances.AddOrUpdate(new
                           EventAttendnace
                { AttendeeMember = item.AttendeeMember, EventID = item.EventID });
            }
            //List<ClubEvent> ClubEvents = context.ClubEvents.ToList();
            //List<Member> Members = context.ClubMembers.ToList();
            //List<EventAttendnace> attendances = new List<EventAttendnace>();

            //foreach (var member in Members)
            //{
            //        foreach (var clubEvent in member.myClub.clubEvents)
            //        {
            //                context.EventAttendances.AddOrUpdate(
            //                    new EventAttendnace
            //                {
            //                    associatedEvent = clubEvent,
            //                    memberAttending = member
            //                });
            //        }
            //}
            context.SaveChanges();
        }
    }
}
