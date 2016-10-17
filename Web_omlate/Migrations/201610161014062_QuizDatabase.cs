namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quizs", "Deadline", c => c.DateTime(nullable: false));
            AddColumn("dbo.Quizs", "Duration", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quizs", "Duration");
            DropColumn("dbo.Quizs", "Deadline");
        }
    }
}
