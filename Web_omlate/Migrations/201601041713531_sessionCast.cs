namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sessionCast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.session", "endTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.session", "endTime");
        }
    }
}
