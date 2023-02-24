//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using Sheenam.Api.Models.Foundations.Owner;
using System.Linq;
using System.Threading.Tasks;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Owner> Owners { get; set; }

        public async ValueTask<Owner> InsertPostAsync(Owner owner) =>
            await InsertAsync(owner);

        public IQueryable<Owner> SelectAllOwners() =>
           SelectAll<Owner>();
    }
}
