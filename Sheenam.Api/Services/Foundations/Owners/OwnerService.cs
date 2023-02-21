//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.Extensions.Hosting;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Owner;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public partial class OwnerService : IOwnerService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        

        public OwnerService(
           IStorageBroker storageBroker,
           ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Owner> AddOwnerAsync(Owner owner) =>
        TryCatch(async () =>
        {
            ValidateOwner(owner);

            return await this.storageBroker.InsertOwnerAsync(owner);
        });
    }
}
