using Sustainalytics.GSS.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Sustainalytics.GSS.DataLayer
{
    public class GssDbContext : DbContext
    {
        public GssDbContext() : this("Data Source=(local);Initial Catalog=GSS;Integrated Security=True") {/* no specific implementation; mainly useful for EF to handle migrations */}

        public GssDbContext(string connectionString) : base(connectionString)
        {
            var ensureDllIsLoaded = typeof(System.Data.Entity.SqlServer.SqlProviderServices);// otherwise it won't get loaded into AppDomain :)

            (this as IObjectContextAdapter).ObjectContext.CommandTimeout = 10 * 60;
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyProfile> CompanyProfiles { get; set; }

        public DbSet<IssueIndicator> IssueIndicators { get; set; }

        public DbSet<IssueIndicatorTemplate> IssueIndicatorTemplates { get; set; }

        public DbSet<ChangeRecord> ChangeRecords { get; set; }

        public DbSet<OecdGuideline> OecdGuidelines { get; set; }

        public DbSet<UnGuidingPrinciple> UnGuidingPrinciples { get; set; }

        public DbSet<RelatedConvention> RelatedConventions { get; set; }
    }
}
