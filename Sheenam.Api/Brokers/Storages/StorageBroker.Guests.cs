//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Sheenam.Api.Models.Foundations.Guests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Guest> guestEntityEntry =
                await broker.Guests.AddAsync(guest);

            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }

        public IQueryable<Guest> SelectAllGuests()
        {
            using var broker =
                new StorageBroker(
                    this.configuration);
            return broker.Guests;
        }

        public async ValueTask<Guest> SelectGuestsByIdAsync(Guid guestId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Guests.FindAsync(guestId);
        }

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Guest> guestEntityEntry =
                broker.Guests.Update(guest);

            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }

        public async ValueTask<Guest> DeleteGuestAsync(Guest guest)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Guest> guestEntityEntry =
                broker.Guests.Remove(guest);

            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }
    }
}
