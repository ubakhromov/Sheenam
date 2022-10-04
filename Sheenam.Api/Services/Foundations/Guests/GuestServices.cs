//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.Extensions.Hosting;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestServices : IGuestServices
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuestServices(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
        TryCatch(async () =>
        {
            ValidateGuestOnAdd(guest);
            return await this.storageBroker.InsertGuestAsync(guest);
        });

        public IQueryable<Guest> RetrieveAllGuests() =>
            TryCatch(() =>
            {
                return this.storageBroker.SelectAllGuests();
            });

        public ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId) =>
            TryCatch(async () =>
            {
                ValidateGuestId(guestId);

                Guest maybeGuest = await this.storageBroker
                    .SelectGuestsByIdAsync(guestId);

                ValidateStorageGuest(maybeGuest, guestId);

                 return maybeGuest;
            });
    }
}
