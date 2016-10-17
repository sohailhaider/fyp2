namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuizQuestions", "temp", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuizQuestions", "temp");
        }
    }
}
