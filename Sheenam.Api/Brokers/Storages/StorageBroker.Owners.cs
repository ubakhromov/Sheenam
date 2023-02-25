//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Owner;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Owner> Owners { get; set; }

        public async ValueTask<Owner> InsertOwnerAsync(Owner owner) =>
            await InsertAsync(owner);

        public IQueryable<Owner> SelectAllOwners() =>
           SelectAll<Owner>();

        public async ValueTask<Owner> SelectOwnerByIdAsync(Guid ownerId) =>
            await SelectAsync<Owner>(ownerId);
    }
}
