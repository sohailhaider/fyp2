namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedScheduleIdInSeesion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassSchedule", "session_sessionId", c => c.Int());
            AddColumn("dbo.session", "scheduleID", c => c.Int(nullable: false));
            CreateIndex("dbo.ClassSchedule", "session_sessionId");
            AddForeignKey("dbo.ClassSchedule", "session_sessionId", "dbo.session", "sessionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassSchedule", "session_sessionId", "dbo.session");
            DropIndex("dbo.ClassSchedule", new[] { "session_sessionId" });
            DropColumn("dbo.session", "scheduleID");
            DropColumn("dbo.ClassSchedule", "session_sessionId");
        }
    }
}
