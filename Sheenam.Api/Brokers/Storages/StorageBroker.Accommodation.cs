//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
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

        public IQueryable<Accommodation> SelectAllAccommodations() =>
            this.SelectAll<Accommodation>();

        public async ValueTask<Accommodation> SelectAccommodationByIdAsync(Guid accommodationId) =>
            await this.SelectAsync<Accommodation>(accommodationId);

        public async ValueTask<Accommodation> UpdateAccommodationAsync(Accommodation accommodation) =>
            await UpdateAsync(accommodation);

        public async ValueTask<Accommodation> DeleteAccommodationAsync(Accommodation accommodation) =>
            await DeleteAsync(accommodation);
    }
}
