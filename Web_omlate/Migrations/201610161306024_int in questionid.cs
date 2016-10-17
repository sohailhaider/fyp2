namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intinquestionid : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.QuizQuestions");
            AlterColumn("dbo.QuizQuestions", "QuizQuestionID", c => c.Int(nullable: true, identity: true));
            AddPrimaryKey("dbo.QuizQuestions", "QuizQuestionID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.QuizQuestions");
            AlterColumn("dbo.QuizQuestions", "QuizQuestionID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.QuizQuestions", "QuizQuestionID");
        }
    }
}
