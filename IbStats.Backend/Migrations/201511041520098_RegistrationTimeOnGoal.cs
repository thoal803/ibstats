namespace IbStats.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrationTimeOnGoal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Goal", "RegistrationTime", c => c.DateTime());
            AlterColumn("dbo.Match", "StartTime", c => c.DateTime());
            AlterColumn("dbo.Match", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Match", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Match", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Goal", "RegistrationTime");
        }
    }
}
