namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class udpated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quizs", "InstructorID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quizs", "InstructorID", c => c.Int(nullable: false));
        }
    }
}
