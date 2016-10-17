using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web_omlate.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Web_omlate.DAL
{
    public class FYPDBContext : DbContext
    {
        public FYPDBContext()
            : base("omlateDB2")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<OfferedCourse> OfferedCourses { get; set; }
        public DbSet<LearnerEnroll> LearnerEnrollments { get; set; }
        public DbSet<LectureSchedule> LectureSchedules { get; set; }
        public DbSet<LectureResource> LectureResources { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<AssessmentEvaluation> AssessmentEvaluations { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AssessmentSubmission> AssessmentSubmissions { get; set; }

        public System.Data.Entity.DbSet<Web_omlate.Models.Quiz> Quizs { get; set; }

        public System.Data.Entity.DbSet<Web_omlate.Models.QuizQuestion> QuizQuestions { get; set; }

        public System.Data.Entity.DbSet<Web_omlate.Models.QuizAttempt> QuizAttempts { get; set; }

        public System.Data.Entity.DbSet<Web_omlate.Models.IsAttempted> IsAttempteds { get; set; }
    }
}
