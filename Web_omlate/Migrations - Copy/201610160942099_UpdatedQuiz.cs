namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedQuiz : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuizQuestions", "QuizID", "dbo.Quizs");
            DropPrimaryKey("dbo.Quizs");
            AlterColumn("dbo.Quizs", "QuizID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Quizs", "QuizID");
            AddForeignKey("dbo.QuizQuestions", "QuizID", "dbo.Quizs", "QuizID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizQuestions", "QuizID", "dbo.Quizs");
            DropPrimaryKey("dbo.Quizs");
            AlterColumn("dbo.Quizs", "QuizID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Quizs", "QuizID");
            AddForeignKey("dbo.QuizQuestions", "QuizID", "dbo.Quizs", "QuizID", cascadeDelete: true);
        }
    }
}
