namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizDetailsUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quizs", "QuizTitle", c => c.String(nullable: false));
            AlterColumn("dbo.Quizs", "InstructorID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quizs", "InstructorID", c => c.String(nullable: false));
            DropColumn("dbo.Quizs", "QuizTitle");
        }
    }
}
