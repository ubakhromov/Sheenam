//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Owner;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Owner> InsertOwnerAsync(Owner owner);

        IQueryable<Owner> SelectAllOwners();
    }
}
