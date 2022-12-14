//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using EFxceptions;
using Microsoft.EntityFrameworkCore;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<StorageBroker> logger;

        public StorageBroker(IConfiguration configuration, ILogger<StorageBroker> logger)
        {
            this.configuration = configuration;
            this.Database.Migrate();
            this.logger = logger;
        }

        private IQueryable<T> SelectAll<T>() where T : class => this.Set<T>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString =
                this.configuration.GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        public override void Dispose() { }
    }
}
