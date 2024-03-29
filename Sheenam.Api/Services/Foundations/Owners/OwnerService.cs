﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Owners;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public partial class OwnerService : IOwnerService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;


        public OwnerService(
           IStorageBroker storageBroker,
           ILoggingBroker loggingBroker,
           IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Owner> AddOwnerAsync(Owner owner) =>
        TryCatch(async () =>
        {
            ValidateOwner(owner);

            return await this.storageBroker.InsertOwnerAsync(owner);
        });

        public IQueryable<Owner> RetrieveAllOwners() =>
        TryCatch(() =>
        {
            return this.storageBroker.SelectAllOwners();
        });

        public ValueTask<Owner> RetrieveOwnerByIdAsync(Guid ownerId) =>
        TryCatch(async () =>

        {
            ValidateOwnerById(ownerId);

            Owner maybeOwner = await this.storageBroker
                .SelectOwnerByIdAsync(ownerId);

            ValiateStorageOwner(maybeOwner, ownerId);

            return maybeOwner;
        });

        public ValueTask<Owner> ModifyOwnerAsync(Owner owner) =>
        TryCatch(async () =>
        {
            ValidateOwnerOnModify(owner);

            var maybeOwner =
               await this.storageBroker.SelectOwnerByIdAsync(owner.Id);

            ValiateStorageOwner(maybeOwner, owner.Id);

            return
                await this.storageBroker.UpdateOwnerAsync(owner);
        });

        public ValueTask<Owner> RemoveOwnerByIdAsync(Guid ownerId) =>
        TryCatch(async () =>
        {
            ValidateOwnerById(ownerId);

            Owner someOwner =
                 await this.storageBroker.SelectOwnerByIdAsync(ownerId);

            ValiateStorageOwner(someOwner, ownerId);

            return await this.storageBroker.DeleteOwnerAsync(someOwner);
        });
    }
}
