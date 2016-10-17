namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernameinquiz : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quizs", "Instructor", c => c.String(nullable: false));
            DropColumn("dbo.Quizs", "InstructorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Quizs", "InstructorID", c => c.Int(nullable: false));
            DropColumn("dbo.Quizs", "Instructor");
        }
    }
}
