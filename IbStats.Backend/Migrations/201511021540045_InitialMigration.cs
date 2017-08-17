namespace IbStats.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Goal",
                c => new
                    {
                        GoalID = c.Int(nullable: false, identity: true),
                        Scorer_PlayerID = c.Int(),
                        Match_MatchID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GoalID)
                .ForeignKey("dbo.Player", t => t.Scorer_PlayerID)
                .ForeignKey("dbo.Match", t => t.Match_MatchID)
                .Index(t => t.Scorer_PlayerID)
                .Index(t => t.Match_MatchID);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.PlayerID);
            
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        MatchID = c.Int(nullable: false, identity: true),
                        MatchSession_MatchSessionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MatchID)
                .ForeignKey("dbo.MatchSession", t => t.MatchSession_MatchSessionID)
                .Index(t => t.MatchSession_MatchSessionID);
            
            CreateTable(
                "dbo.MatchSession",
                c => new
                    {
                        MatchSessionID = c.Int(nullable: false, identity: true),
                        MatchDate = c.DateTime(nullable: false),
                        FirstWasher_PlayerID = c.Int(nullable: false),
                        Season_SeasonID = c.Int(nullable: false),
                        SecondWasher_PlayerID = c.Int(nullable: false),
                        Team1_TeamID = c.Int(),
                        Team2_TeamID = c.Int(),
                    })
                .PrimaryKey(t => t.MatchSessionID)
                .ForeignKey("dbo.Player", t => t.FirstWasher_PlayerID)
                .ForeignKey("dbo.Season", t => t.Season_SeasonID)
                .ForeignKey("dbo.Player", t => t.SecondWasher_PlayerID)
                .ForeignKey("dbo.Team", t => t.Team1_TeamID)
                .ForeignKey("dbo.Team", t => t.Team2_TeamID)
                .Index(t => t.FirstWasher_PlayerID)
                .Index(t => t.Season_SeasonID)
                .Index(t => t.SecondWasher_PlayerID)
                .Index(t => t.Team1_TeamID)
                .Index(t => t.Team2_TeamID);
            
            CreateTable(
                "dbo.Season",
                c => new
                    {
                        SeasonID = c.Int(nullable: false, identity: true),
                        SeasonStart = c.DateTime(nullable: false),
                        SeasonEnd = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SeasonID);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.TeamID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 1024),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleID)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.RoleID })
                .ForeignKey("dbo.Role", t => t.RoleID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(),
                        FirstName = c.String(maxLength: 256),
                        LastName = c.String(maxLength: 256),
                        Signature = c.String(maxLength: 32),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UserID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        UserClaimID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.UserClaimID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        DescriptionMultilingualStringID = c.Int(),
                        NameMultilingualStringID = c.Int(nullable: false),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.GroupID)
                .ForeignKey("dbo.MultilingualString", t => t.DescriptionMultilingualStringID)
                .ForeignKey("dbo.MultilingualString", t => t.NameMultilingualStringID)
                .ForeignKey("dbo.Group", t => t.ParentID)
                .Index(t => t.DescriptionMultilingualStringID)
                .Index(t => t.NameMultilingualStringID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.MultilingualString",
                c => new
                    {
                        MultilingualStringID = c.Int(nullable: false, identity: true),
                        XmlString = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MultilingualStringID);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserID })
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        PropertyID = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 128),
                        Value = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UserID = c.Int(),
                    })
                .PrimaryKey(t => t.PropertyID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.TeamPlayer",
                c => new
                    {
                        TeamID = c.Int(nullable: false),
                        PlayerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamID, t.PlayerID })
                .ForeignKey("dbo.Team", t => t.TeamID)
                .ForeignKey("dbo.Player", t => t.PlayerID)
                .Index(t => t.TeamID)
                .Index(t => t.PlayerID);
            
            CreateTable(
                "dbo.GroupUser",
                c => new
                    {
                        GroupID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupID, t.UserID })
                .ForeignKey("dbo.Group", t => t.GroupID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.GroupID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.GroupRole",
                c => new
                    {
                        GroupID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupID, t.RoleID })
                .ForeignKey("dbo.Group", t => t.GroupID)
                .ForeignKey("dbo.Role", t => t.RoleID)
                .Index(t => t.GroupID)
                .Index(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserID", "dbo.User");
            DropForeignKey("dbo.Property", "UserID", "dbo.User");
            DropForeignKey("dbo.UserLogin", "UserID", "dbo.User");
            DropForeignKey("dbo.GroupRole", "RoleID", "dbo.Role");
            DropForeignKey("dbo.GroupRole", "GroupID", "dbo.Group");
            DropForeignKey("dbo.Group", "ParentID", "dbo.Group");
            DropForeignKey("dbo.Group", "NameMultilingualStringID", "dbo.MultilingualString");
            DropForeignKey("dbo.GroupUser", "UserID", "dbo.User");
            DropForeignKey("dbo.GroupUser", "GroupID", "dbo.Group");
            DropForeignKey("dbo.Group", "DescriptionMultilingualStringID", "dbo.MultilingualString");
            DropForeignKey("dbo.UserClaim", "UserID", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleID", "dbo.Role");
            DropForeignKey("dbo.MatchSession", "Team2_TeamID", "dbo.Team");
            DropForeignKey("dbo.MatchSession", "Team1_TeamID", "dbo.Team");
            DropForeignKey("dbo.TeamPlayer", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.TeamPlayer", "TeamID", "dbo.Team");
            DropForeignKey("dbo.MatchSession", "SecondWasher_PlayerID", "dbo.Player");
            DropForeignKey("dbo.MatchSession", "Season_SeasonID", "dbo.Season");
            DropForeignKey("dbo.Match", "MatchSession_MatchSessionID", "dbo.MatchSession");
            DropForeignKey("dbo.MatchSession", "FirstWasher_PlayerID", "dbo.Player");
            DropForeignKey("dbo.Goal", "Match_MatchID", "dbo.Match");
            DropForeignKey("dbo.Goal", "Scorer_PlayerID", "dbo.Player");
            DropIndex("dbo.GroupRole", new[] { "RoleID" });
            DropIndex("dbo.GroupRole", new[] { "GroupID" });
            DropIndex("dbo.GroupUser", new[] { "UserID" });
            DropIndex("dbo.GroupUser", new[] { "GroupID" });
            DropIndex("dbo.TeamPlayer", new[] { "PlayerID" });
            DropIndex("dbo.TeamPlayer", new[] { "TeamID" });
            DropIndex("dbo.Property", new[] { "UserID" });
            DropIndex("dbo.UserLogin", new[] { "UserID" });
            DropIndex("dbo.Group", new[] { "ParentID" });
            DropIndex("dbo.Group", new[] { "NameMultilingualStringID" });
            DropIndex("dbo.Group", new[] { "DescriptionMultilingualStringID" });
            DropIndex("dbo.UserClaim", new[] { "UserID" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.UserRole", new[] { "RoleID" });
            DropIndex("dbo.UserRole", new[] { "UserID" });
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.MatchSession", new[] { "Team2_TeamID" });
            DropIndex("dbo.MatchSession", new[] { "Team1_TeamID" });
            DropIndex("dbo.MatchSession", new[] { "SecondWasher_PlayerID" });
            DropIndex("dbo.MatchSession", new[] { "Season_SeasonID" });
            DropIndex("dbo.MatchSession", new[] { "FirstWasher_PlayerID" });
            DropIndex("dbo.Match", new[] { "MatchSession_MatchSessionID" });
            DropIndex("dbo.Goal", new[] { "Match_MatchID" });
            DropIndex("dbo.Goal", new[] { "Scorer_PlayerID" });
            DropTable("dbo.GroupRole");
            DropTable("dbo.GroupUser");
            DropTable("dbo.TeamPlayer");
            DropTable("dbo.Property");
            DropTable("dbo.UserLogin");
            DropTable("dbo.MultilingualString");
            DropTable("dbo.Group");
            DropTable("dbo.UserClaim");
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
            DropTable("dbo.Team");
            DropTable("dbo.Season");
            DropTable("dbo.MatchSession");
            DropTable("dbo.Match");
            DropTable("dbo.Player");
            DropTable("dbo.Goal");
        }
    }
}
