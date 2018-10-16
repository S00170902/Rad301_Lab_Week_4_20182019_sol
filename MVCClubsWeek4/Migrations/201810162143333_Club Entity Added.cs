namespace MVCClubsWeek4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClubEntityAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ClubEntityID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ClubEntityID");
        }
    }
}
