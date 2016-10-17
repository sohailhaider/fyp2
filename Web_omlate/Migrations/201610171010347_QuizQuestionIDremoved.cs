namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizQuestionIDremoved : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.QuizQuestions");
            AddColumn("dbo.QuizQuestions", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.QuizQuestions", "ID");
            DropColumn("dbo.QuizQuestions", "QuizQuestionID");
            DropColumn("dbo.QuizQuestions", "temp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuizQuestions", "temp", c => c.String());
            AddColumn("dbo.QuizQuestions", "QuizQuestionID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.QuizQuestions");
            DropColumn("dbo.QuizQuestions", "ID");
            AddPrimaryKey("dbo.QuizQuestions", "QuizQuestionID");
        }
    }
}
