namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sessionMultiCastPid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.session", "pid", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.session", "pid");
        }
    }
}
