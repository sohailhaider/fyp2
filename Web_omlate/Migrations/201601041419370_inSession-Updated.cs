namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inSessionUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassSchedule", "inSession", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassSchedule", "inSession");
        }
    }
}
