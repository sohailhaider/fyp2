namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class udpatedAttemptQuiz : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuizAttempts", "LearnerID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuizAttempts", "LearnerID", c => c.Int(nullable: false));
        }
    }
}
