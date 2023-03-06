//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Accommodations;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Accommodation> Accommodations { get; set; }

        public async ValueTask<Accommodation> InsertAccommodationAsync(Accommodation accommodation) =>
            await this.InsertAsync(accommodation);

        public IQueryable<Accommodation> SelectAllAccomodations() =>
            this.SelectAll<Accommodation>();
    }
}
