namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extraDatarevert : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.session", "extraData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.session", "extraData", c => c.Int(nullable: false));
        }
    }
}
