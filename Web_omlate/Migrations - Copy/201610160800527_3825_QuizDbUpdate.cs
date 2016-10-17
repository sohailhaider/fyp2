namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3825_QuizDbUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        QuizID = c.String(nullable: false, maxLength: 128),
                        InstructorID = c.String(nullable: false),
                        offeredCourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuizID);
            
            CreateTable(
                "dbo.QuizQuestions",
                c => new
                    {
                        QuizQuestionID = c.String(nullable: false, maxLength: 128),
                        QuestionStatement = c.String(nullable: false),
                        Options = c.String(),
                        Answer = c.String(),
                        Quiz_QuizID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.QuizQuestionID)
                .ForeignKey("dbo.Quizs", t => t.Quiz_QuizID)
                .Index(t => t.Quiz_QuizID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizQuestions", "Quiz_QuizID", "dbo.Quizs");
            DropIndex("dbo.QuizQuestions", new[] { "Quiz_QuizID" });
            DropTable("dbo.QuizQuestions");
            DropTable("dbo.Quizs");
        }
    }
}
