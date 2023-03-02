//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Accommodations;

namespace Sheenam.Api.Services.Foundations.Accommodations
{
    public partial class AccommodationService : IAccommodationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public AccommodationService(
            IStorageBroker storageBroker, 
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker; 
        }

        public ValueTask<Accommodation> AddAccommodationAsync(Accommodation accommodation) =>
        TryCatch(async () =>
        { 
            ValidateAccommodationOnAdd(accommodation);

            return await this.storageBroker.InsertAccommodationAsync(accommodation);
        });
    }
}
