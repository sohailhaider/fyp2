namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jan13v2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ClassSchedule", "time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClassSchedule", "time", c => c.String());
        }
    }
}
