namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionClientIpRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.session", "sessionClientIp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.session", "sessionClientIp", c => c.String(unicode: false, storeType: "text"));
        }
    }
}
