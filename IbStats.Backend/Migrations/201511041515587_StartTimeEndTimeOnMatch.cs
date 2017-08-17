namespace IbStats.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartTimeEndTimeOnMatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Match", "StartTime", c => c.DateTime(nullable: true));
            AddColumn("dbo.Match", "EndTime", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Match", "EndTime");
            DropColumn("dbo.Match", "StartTime");
        }
    }
}
