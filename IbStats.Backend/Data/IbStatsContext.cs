using System.Data.Entity;
using IbStats.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using Vintrex.Platform.Identity;

namespace IbStats.Data
{
    public class IbStatsContext : ApplicationIdentityContext 
    {
        public IbStatsContext() : base()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        public new static IbStatsContext Create()
        {
            return new IbStatsContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<RequiredPrimitivePropertyAttributeConvention>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>().HasMany(t => t.Players).WithMany().Map(m => { m.MapLeftKey("TeamID"); m.MapRightKey("PlayerID"); m.ToTable("TeamPlayer"); });

            modelBuilder.Entity<MatchSession>().HasMany<Match>(m => m.Matches).WithRequired();


            modelBuilder.Entity<Match>().HasMany<Goal>(m => m.Goals).WithRequired();

            modelBuilder.Entity<MatchSession>().HasRequired(m => m.FirstWasher).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<MatchSession>().HasRequired(m => m.SecondWasher).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<MatchSession>().HasRequired<Season>(m => m.Season).WithMany().WillCascadeOnDelete(false);

        }

        public DbSet<MatchSession> MatchSessions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Season> Seasons { get; set; }
    }
}