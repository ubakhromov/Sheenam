//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest) =>
           await InsertAsync(guest);

        public IQueryable<Guest> SelectAllGuests() =>
        SelectAll<Guest>();

        public async ValueTask<Guest> SelectGuestsByIdAsync(Guid guestId) =>
            await SelectAsync<Guest>(guestId);

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest) =>
           await UpdateAsync(guest);

        public async ValueTask<Guest> DeleteGuestAsync(Guest guest) =>
            await DeleteAsync(guest);
    }
}
