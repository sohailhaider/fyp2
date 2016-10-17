namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sessionTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.session", "endTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.session", "endTime", c => c.DateTime(nullable: false));
        }
    }
}
