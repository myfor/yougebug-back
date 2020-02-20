using Common;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class YGBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Config.GetConnectionString("YGB-maria"));
        }

        public DbSet<Tables.File> Files { get; set; }
        public DbSet<Tables.Admin> Admins { get; set; }
        public DbSet<Tables.User> Users { get; set; }
        public DbSet<Tables.Answer> Answers { get; set; }
        public DbSet<Tables.Question> Questions { get; set; }
        public DbSet<Tables.Tag> Tags { get; set; }
        public DbSet<Tables.QuestionBackRecord> QuestionBackRecords { get; set; }
        public DbSet<Tables.AnswerBackRecord> AnswerBackRecords { get; set; }
        public DbSet<Tables.QuestionReportRecord> QuestionReportRecords { get; set; }
        public DbSet<Tables.AnswerReportRecord> AnswerReportRecords { get; set; }
    }
}
