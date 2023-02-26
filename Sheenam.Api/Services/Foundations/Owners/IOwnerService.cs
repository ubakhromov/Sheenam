//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Owner;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public interface IOwnerService
    {
        ValueTask<Owner> AddOwnerAsync(Owner owner);
        IQueryable<Owner> RetrieveAllOwners();
        ValueTask<Owner> RetrieveOwnerByIdAsync(Guid ownerId);
        ValueTask<Owner> ModifyOwnerAsync(Owner owner);
    }
}
