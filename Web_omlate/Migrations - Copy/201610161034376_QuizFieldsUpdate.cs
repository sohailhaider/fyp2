namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuizFieldsUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quizs", "Duration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quizs", "Duration", c => c.Time(nullable: false, precision: 7));
        }
    }
}
