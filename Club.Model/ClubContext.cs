using ClubDomain.Classes.ClubModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clubs.Model
{
    class ClubContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubEvent> ClubEvents { get; set; }
        public DbSet<Member> ClubMembers { get; set; }
        public DbSet<EventAttendnace> EventAttendances { get; set; }

    }
}
