namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredfieldsudpationinAttemptQuiz : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuizAttempts", "Answers", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuizAttempts", "Answers", c => c.String());
        }
    }
}
