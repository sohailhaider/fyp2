namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extraMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.session", "extraData", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.session", "extraData");
        }
    }
}
