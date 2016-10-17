namespace Web_omlate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassSchedule",
                c => new
                    {
                        classScheduleId = c.Int(nullable: false, identity: true),
                        instructorId = c.Int(nullable: false),
                        courseId = c.Int(nullable: false),
                        dates = c.DateTime(nullable: false),
                        time = c.String(),
                    })
                .PrimaryKey(t => t.classScheduleId);
            
            CreateTable(
                "dbo.CourseCategory",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        courseCategoryName = c.String(maxLength: 255),
                        courseCategoryDetails = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        courseId = c.Int(nullable: false, identity: true),
                        courseTitle = c.String(nullable: false, maxLength: 255),
                        courseCode = c.String(nullable: false, maxLength: 255),
                        courseFeaturedImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.courseId);
            
            CreateTable(
                "dbo.user",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        user_username = c.String(nullable: false, unicode: false, storeType: "text"),
                        user_password = c.String(nullable: false, unicode: false, storeType: "text"),
                        user_email = c.String(nullable: false, maxLength: 255),
                        user_phone_number = c.String(unicode: false, storeType: "text"),
                        user_featured_image = c.String(unicode: false, storeType: "text"),
                        user_type = c.String(unicode: false, storeType: "text"),
                        user_status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userId);
            
            CreateTable(
                "dbo.session",
                c => new
                    {
                        sessionId = c.Int(nullable: false, identity: true),
                        sessionUserId = c.Int(nullable: false),
                        sessionClientIp = c.String(unicode: false, storeType: "text"),
                        sessionInPort = c.Int(nullable: false),
                        sessionOutPort = c.Int(nullable: false),
                        session_expected_time = c.Int(),
                    })
                .PrimaryKey(t => t.sessionId);
            
            CreateTable(
                "dbo.course_category_relation",
                c => new
                    {
                        course_id = c.Int(nullable: false),
                        course_category_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.course_id, t.course_category_id })
                .ForeignKey("dbo.Course", t => t.course_id, cascadeDelete: true)
                .ForeignKey("dbo.CourseCategory", t => t.course_category_id, cascadeDelete: true)
                .Index(t => t.course_id)
                .Index(t => t.course_category_id);
            
            CreateTable(
                "dbo.course_user_relation",
                c => new
                    {
                        course_id = c.Int(nullable: false),
                        course_user_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.course_id, t.course_user_id })
                .ForeignKey("dbo.Course", t => t.course_id, cascadeDelete: true)
                .ForeignKey("dbo.user", t => t.course_user_id, cascadeDelete: true)
                .Index(t => t.course_id)
                .Index(t => t.course_user_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.course_user_relation", "course_user_id", "dbo.user");
            DropForeignKey("dbo.course_user_relation", "course_id", "dbo.Course");
            DropForeignKey("dbo.course_category_relation", "course_category_id", "dbo.CourseCategory");
            DropForeignKey("dbo.course_category_relation", "course_id", "dbo.Course");
            DropIndex("dbo.course_user_relation", new[] { "course_user_id" });
            DropIndex("dbo.course_user_relation", new[] { "course_id" });
            DropIndex("dbo.course_category_relation", new[] { "course_category_id" });
            DropIndex("dbo.course_category_relation", new[] { "course_id" });
            DropTable("dbo.course_user_relation");
            DropTable("dbo.course_category_relation");
            DropTable("dbo.session");
            DropTable("dbo.user");
            DropTable("dbo.Course");
            DropTable("dbo.CourseCategory");
            DropTable("dbo.ClassSchedule");
        }
    }
}
