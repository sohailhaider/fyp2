namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClassSchedule", "session_sessionId", "dbo.session");
            DropIndex("dbo.ClassSchedule", new[] { "session_sessionId" });
            AddColumn("dbo.session", "isLive", c => c.Int(nullable: false));
            DropColumn("dbo.ClassSchedule", "session_sessionId");
            DropColumn("dbo.session", "sessionUserId");
            DropColumn("dbo.session", "scheduleID");
            DropColumn("dbo.session", "session_expected_time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.session", "session_expected_time", c => c.Int());
            AddColumn("dbo.session", "scheduleID", c => c.Int(nullable: false));
            AddColumn("dbo.session", "sessionUserId", c => c.Int(nullable: false));
            AddColumn("dbo.ClassSchedule", "session_sessionId", c => c.Int());
            DropColumn("dbo.session", "isLive");
            CreateIndex("dbo.ClassSchedule", "session_sessionId");
            AddForeignKey("dbo.ClassSchedule", "session_sessionId", "dbo.session", "sessionId");
        }
    }
}
