using ClubDomain.Classes.ClubModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clubs.Model
{
    public class ClubContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubEvent> ClubEvents { get; set; }
        public DbSet<Member> ClubMembers { get; set; }
        public DbSet<EventAttendnace> EventAttendances { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public ClubContext():base("ClubsConnection")
        {
            
        }

        public static ClubContext Create()
        {
            return new ClubContext();
        }
    }
}
