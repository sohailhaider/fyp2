namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttempQuizmodeladded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuizAttempts",
                c => new
                    {
                        AttemptID = c.Int(nullable: false, identity: true),
                        Answers = c.String(),
                        Marks = c.Double(nullable: false),
                        AttemptTime = c.DateTime(nullable: false),
                        LearnerID = c.Int(nullable: false),
                        OfferedCourseID = c.Int(nullable: false),
                        QuizID = c.Int(nullable: false),
                        Learner_Username = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.AttemptID)
                .ForeignKey("dbo.Users", t => t.Learner_Username)
                .ForeignKey("dbo.OfferedCourses", t => t.OfferedCourseID, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.QuizID, cascadeDelete: true)
                .Index(t => t.OfferedCourseID)
                .Index(t => t.QuizID)
                .Index(t => t.Learner_Username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizAttempts", "QuizID", "dbo.Quizs");
            DropForeignKey("dbo.QuizAttempts", "OfferedCourseID", "dbo.OfferedCourses");
            DropForeignKey("dbo.QuizAttempts", "Learner_Username", "dbo.Users");
            DropIndex("dbo.QuizAttempts", new[] { "Learner_Username" });
            DropIndex("dbo.QuizAttempts", new[] { "QuizID" });
            DropIndex("dbo.QuizAttempts", new[] { "OfferedCourseID" });
            DropTable("dbo.QuizAttempts");
        }
    }
}
