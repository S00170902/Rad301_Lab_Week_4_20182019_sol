namespace MVCClubsWeek4.Migrations
{
    using ClubDomain.Classes.ClubModels;
    using Clubs.Model;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MVCClubsWeek4.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCClubsWeek4.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCClubsWeek4.Models.ApplicationDbContext context)
        {
            var manager =
                new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            context.Roles.AddOrUpdate(r => r.Name,
                new IdentityRole { Name = "Admin" }
                );
            context.Roles.AddOrUpdate(r => r.Name,
                new IdentityRole { Name = "ClubAdmin" }
                );
            context.Roles.AddOrUpdate(r => r.Name,
                new IdentityRole { Name = "member" }
                );

            PasswordHasher ps = new PasswordHasher();

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "powell.paul@itsligo.ie",
                    Email = "powell.paul@itsligo.ie",
                    EmailConfirmed = true,
                    JoinDate = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Paul",
                    Surname = "Powell",
                    PasswordHash = ps.HashPassword("Ppowell$1")
                });

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "radp2016@outlook.com",
                    Email = "radp2016@outlook.com",
                    EmailConfirmed = true,
                    JoinDate = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Rad",
                    Surname = "Paulner",
                    PasswordHash = ps.HashPassword("radP2016$1")
                });
            context.SaveChanges();

            ApplicationUser admin = manager.FindByEmail("powell.paul@itsligo.ie");
            if (admin != null)
            {
                manager.AddToRoles(admin.Id, new string[] { "Admin", "member", "ClubAdmin" });
            }
            //SeedClubAdmin(manager,context);
            SeedAllClubAdmin(manager, context);
        }
        // Seeding admins for all clubs
        private void SeedAllClubAdmin(UserManager<ApplicationUser> manager, ApplicationDbContext context)
        {
            ClubContext clubc = new ClubContext();
            List<Club> ClubsWithNoAdmin = clubc.Clubs.Where(c => c.adminID == 0).ToList();
            // if there are any clubs with no admin
            if (ClubsWithNoAdmin.Count() > 0)
                foreach (var club in ClubsWithNoAdmin)
                    // Seed a member for that club
                    SeedAdminMember(clubc,manager, context, club);
            clubc.SaveChanges();
        }

        private void SeedAdminMember(ClubContext clubc, UserManager<ApplicationUser> manager, ApplicationDbContext context, Club club)
        {
            PasswordHasher ps = new PasswordHasher();
            Member chosenMember = club.clubMembers.FirstOrDefault();
            if (chosenMember == null)
            {
                throw new Exception("No Club Member available for " + club.ClubName);
            }
            else club.adminID = chosenMember.MemberID;
            clubc.SaveChanges();
            // Add the membership and role for this member
            if (chosenMember != null)
            {
                context.Users.AddOrUpdate(u => u.UserName,
                    new ApplicationUser
                    {
                        ClubEntityID = chosenMember.StudentID,
                        FirstName = chosenMember.studentMember.FirstName,
                        Surname = chosenMember.studentMember.SecondName,
                        Email = chosenMember.StudentID + "@mail.itsligo.ie",
                        UserName = chosenMember.StudentID + "@mail.itsligo.ie",
                        EmailConfirmed = true,
                        JoinDate = DateTime.Now,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        PasswordHash = ps.HashPassword(chosenMember.StudentID + "s$1")
                    });
            }
            context.SaveChanges();
            ApplicationUser ChosenClubAdmin = manager.FindByEmail(chosenMember.StudentID + "@mail.itsligo.ie");
            if (ChosenClubAdmin != null)
            {
                manager.AddToRoles(ChosenClubAdmin.Id, new string[] { "ClubAdmin" });
            }
            context.SaveChanges();


        }



        // Only doing One club here as requested in teh Lab sheet
        // But to do all clubs and choose a club admin for each
        // See above
        private void SeedClubAdmin(UserManager<ApplicationUser> manager, ApplicationDbContext context)
        {
            PasswordHasher ps = new PasswordHasher();
            Member chosenMember;
            // Create a club member Application user login
            ClubContext clubc = new ClubContext();
            chosenMember = clubc.Clubs.First().clubMembers.FirstOrDefault();
            
                    if (chosenMember == null)
                    {
                        throw new Exception("No Club Member available");
                    }
                    else chosenMember.myClub.adminID = chosenMember.MemberID;
            clubc.SaveChanges();
            // Add the membership and role for this member
            if (chosenMember != null)
                {
                    context.Users.AddOrUpdate(u => u.UserName,
                        new ApplicationUser
                        {
                            ClubEntityID = chosenMember.StudentID,
                            FirstName = chosenMember.studentMember.FirstName,
                            Surname = chosenMember.studentMember.SecondName,
                            Email = chosenMember.StudentID + "@mail.itsligo.ie",
                            UserName = chosenMember.StudentID + "@mail.itsligo.ie",
                            EmailConfirmed = true,
                            JoinDate = DateTime.Now,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            PasswordHash = ps.HashPassword(chosenMember.StudentID + "s$1")
                        });
                }
                context.SaveChanges();
                ApplicationUser ChosenClubAdmin = manager.FindByEmail(chosenMember.StudentID + "@mail.itsligo.ie");
                if (ChosenClubAdmin != null)
                {
                    manager.AddToRoles(ChosenClubAdmin.Id, new string[] { "ClubAdmin" });
                }

            
        }
    }
}
