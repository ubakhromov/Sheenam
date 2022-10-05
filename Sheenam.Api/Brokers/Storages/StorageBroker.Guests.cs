//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Guest> Guests {get;set;}

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

        public async ValueTask<Guest> SelectGuestsByIdAsync(Guid postId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Guests.FindAsync(postId);
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
    }
}
