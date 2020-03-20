using Models;
using System;
using System.Data.Entity;
using System.Threading;

namespace Service
{
    public class TimeSeriesContext : DbContext
    {
        public TimeSeriesContext()
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            }

        public DbSet<DB> DBs { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Models.Attribute> Attributes { get; set; }
        public DbSet<TimeSeriesPointEntity> TimeSeriesPointEntities { get; set; }
        public DbSet<TimeSeriesPointAttribute> TimeSeriesPointAttributes { get; set; }
        public DbSet<Attribute_TimeSeriesPointAttribute> Attribute_TimeSeriesPointAttributes { get; set; }

    }
}
