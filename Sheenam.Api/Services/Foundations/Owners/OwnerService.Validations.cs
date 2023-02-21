//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Microsoft.Extensions.Hosting;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public partial class OwnerService
    {
        private static void ValidateOwner(Owner owner)
        {
            ValidateOwnerIsNotNull(owner);
        }

        private static void ValidateOwnerIsNotNull(Owner owner)
        {
            if (owner is null)
            {
                throw new NullOwnerException();
            }
        }
    }
}
