using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClubDomain.Classes.ClubModels;
using Clubs.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCClubsWeek4.Models;

namespace MVCClubsWeek4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClubsController : Controller
    {
        private ClubContext db = new ClubContext();

        // GET: Clubs
        public ActionResult Index()
        {
            return View(db.Clubs.ToList());
        }

        // GET: Clubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // GET: Clubs/Create
        [Authorize(Users ="powell.paul@itsligo.ie")]
        public ActionResult Create()
        {
            ViewBag.StudentList = getStudentList();
            return View();
        }

        public SelectList getStudentList()
        {
            SelectList StudentList;
            List<Student> students = new List<Student>();
                students = db.Students.ToList();
                StudentList = new SelectList(
                    db.Students.Select(s => new { StudentID = s.StudentID, FullName = s.FirstName + " " + s.SecondName })
                    , "StudentID", "FullName");
                return StudentList;
            
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "powell.paul@itsligo.ie")]
        public ActionResult Create(
            [Bind(Include = "ClubName,CreationDate,StudentID")]
                ClubViewModel clubView)
        {
            if (ModelState.IsValid)
            {
                // Create a new Club
                Club newClub = new Club
                {
                    ClubName = clubView.ClubName,
                    CreationDate = clubView.CreationDate,
                    clubMembers = new List<Member>
                    { new Member { approved = true, StudentID = clubView.StudentID,
                        // Need to fill this for Application User later on
                        studentMember = db.Students.First(s => s.StudentID == clubView.StudentID) } }
                };
                // Adding the Club updates the Reference Field
                db.Clubs.Add(newClub);
                db.SaveChanges();
                newClub.adminID = newClub.clubMembers.First().MemberID;
                CreateApplicationLogin(newClub.clubMembers.First());
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(clubView);
        }

        private void CreateApplicationLogin(Member member)
        {
            using (ApplicationDbContext appCtx = new ApplicationDbContext())
            {
                        var manager =
               new UserManager<ApplicationUser>(
                   new UserStore<ApplicationUser>(appCtx));
                ApplicationUser user;
                // if the user is already registered then just add the role and return
                user = manager.FindByEmail(member.StudentID + "@mail.itsligo.ie");
                if(user != null)
                {
                    manager.AddToRoles(user.Id, new string[] { "ClubAdmin" });
                    appCtx.SaveChanges();
                    return;
                }
                //else continue and create and add the role
                PasswordHasher ps = new PasswordHasher();
                user = new ApplicationUser
                {
                    ClubEntityID = member.StudentID,
                    Email = member.StudentID + "@mail.itsligo.ie",
                    UserName = member.StudentID + "@mail.itsligo.ie",
                    EmailConfirmed = true,
                    JoinDate = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = member.studentMember.FirstName,
                    Surname = member.studentMember.SecondName,
                    PasswordHash = ps.HashPassword(member.StudentID + "s$1")
                };
                appCtx.Users.Add(user);
                appCtx.SaveChanges(); // Again updates the key field }
                manager.AddToRoles(user.Id, new string[] {"ClubAdmin"});
                appCtx.SaveChanges();
            }
        }

        // GET: Clubs/Edit/5
        [Authorize(Users = "powell.paul@itsligo.ie")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            

            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "powell.paul@itsligo.ie")]
        public ActionResult Edit([Bind(Include = "ClubId,ClubName,CreationDate,adminID")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        // GET: Clubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
