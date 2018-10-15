namespace ClubDomain.Classes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studentprogramme : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        SecondName = c.String(),
                    })
                .PrimaryKey(t => t.StudentID);
            
            CreateTable(
                "dbo.Programmes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        year = c.String(nullable: false, maxLength: 128),
                        Desription = c.String(),
                    })
                .PrimaryKey(t => new { t.Code, t.year });
            
            CreateTable(
                "dbo.ProgrammeStudents",
                c => new
                    {
                        Programme_Code = c.String(nullable: false, maxLength: 128),
                        Programme_year = c.String(nullable: false, maxLength: 128),
                        Student_StudentID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Programme_Code, t.Programme_year, t.Student_StudentID })
                .ForeignKey("dbo.Programmes", t => new { t.Programme_Code, t.Programme_year }, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentID, cascadeDelete: true)
                .Index(t => new { t.Programme_Code, t.Programme_year })
                .Index(t => t.Student_StudentID);
            
            AlterColumn("dbo.Member", "StudentID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Member", "StudentID");
            AddForeignKey("dbo.Member", "StudentID", "dbo.Students", "StudentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Member", "StudentID", "dbo.Students");
            DropForeignKey("dbo.ProgrammeStudents", "Student_StudentID", "dbo.Students");
            DropForeignKey("dbo.ProgrammeStudents", new[] { "Programme_Code", "Programme_year" }, "dbo.Programmes");
            DropIndex("dbo.ProgrammeStudents", new[] { "Student_StudentID" });
            DropIndex("dbo.ProgrammeStudents", new[] { "Programme_Code", "Programme_year" });
            DropIndex("dbo.Member", new[] { "StudentID" });
            AlterColumn("dbo.Member", "StudentID", c => c.String());
            DropTable("dbo.ProgrammeStudents");
            DropTable("dbo.Programmes");
            DropTable("dbo.Students");
        }
    }
}
