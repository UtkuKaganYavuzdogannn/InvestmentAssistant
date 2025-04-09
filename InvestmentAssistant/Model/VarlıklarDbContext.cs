using Microsoft.EntityFrameworkCore;



namespace InvestmentAssistant.Model
{
    public class VarlıklarDbContext : DbContext
    {
        public VarlıklarDbContext(DbContextOptions<VarlıklarDbContext> options) : base(options)
        {
        }
        public DbSet<Varlıklar> varliklar{ get; set; }
    }
    

    }

