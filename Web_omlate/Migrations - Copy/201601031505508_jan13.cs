namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jan13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.user", "user_username", c => c.String(nullable: false));
            AlterColumn("dbo.user", "user_password", c => c.String(nullable: false));
            AlterColumn("dbo.user", "user_phone_number", c => c.String());
            AlterColumn("dbo.user", "user_featured_image", c => c.String());
            AlterColumn("dbo.user", "user_type", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.user", "user_type", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.user", "user_featured_image", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.user", "user_phone_number", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.user", "user_password", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.user", "user_username", c => c.String(nullable: false, unicode: false, storeType: "text"));
        }
    }
}
