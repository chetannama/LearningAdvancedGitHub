using EmailSpamDetectionService.Model;
using Microsoft.EntityFrameworkCore;


namespace EmailSpamDetectionService.Helpers
{
        public class EmailSpamDbContext : DbContext
        {
            public EmailSpamDbContext(DbContextOptions<EmailSpamDbContext> options)
                : base(options)
            {
            }

            public DbSet<Product> Products { get; set; }
        }
}
