namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modeldependencyonattempt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IsAttempteds",
                c => new
                    {
                        AttemptedID = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 30),
                        QuizID = c.Int(nullable: false),
                        AttemptedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AttemptedID)
                .ForeignKey("dbo.Users", t => t.Username)
                .ForeignKey("dbo.Quizs", t => t.QuizID, cascadeDelete: true)
                .Index(t => t.Username)
                .Index(t => t.QuizID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IsAttempteds", "QuizID", "dbo.Quizs");
            DropForeignKey("dbo.IsAttempteds", "Username", "dbo.Users");
            DropIndex("dbo.IsAttempteds", new[] { "QuizID" });
            DropIndex("dbo.IsAttempteds", new[] { "Username" });
            DropTable("dbo.IsAttempteds");
        }
    }
}
