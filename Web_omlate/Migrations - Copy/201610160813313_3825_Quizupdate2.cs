namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3825_Quizupdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuizQuestions", "Quiz_QuizID", "dbo.Quizs");
            DropIndex("dbo.QuizQuestions", new[] { "Quiz_QuizID" });
            RenameColumn(table: "dbo.QuizQuestions", name: "Quiz_QuizID", newName: "QuizID");
            DropPrimaryKey("dbo.Quizs");
            AlterColumn("dbo.Quizs", "QuizID", c => c.Int(nullable: false));
            AlterColumn("dbo.QuizQuestions", "QuizID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Quizs", "QuizID");
            CreateIndex("dbo.QuizQuestions", "QuizID");
            AddForeignKey("dbo.QuizQuestions", "QuizID", "dbo.Quizs", "QuizID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizQuestions", "QuizID", "dbo.Quizs");
            DropIndex("dbo.QuizQuestions", new[] { "QuizID" });
            DropPrimaryKey("dbo.Quizs");
            AlterColumn("dbo.QuizQuestions", "QuizID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Quizs", "QuizID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Quizs", "QuizID");
            RenameColumn(table: "dbo.QuizQuestions", name: "QuizID", newName: "Quiz_QuizID");
            CreateIndex("dbo.QuizQuestions", "Quiz_QuizID");
            AddForeignKey("dbo.QuizQuestions", "Quiz_QuizID", "dbo.Quizs", "QuizID");
        }
    }
}
