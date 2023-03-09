//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Brokers.DateTimes;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Owners;

namespace Sheenam.Api.Services.Foundations.Accommodations
{
    public partial class AccommodationService : IAccommodationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AccommodationService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Accommodation> AddAccommodationAsync(Accommodation accommodation) =>
        TryCatch(async () =>
        {
            ValidateAccommodationOnAdd(accommodation);

            return await this.storageBroker.InsertAccommodationAsync(accommodation);
        });

        public IQueryable<Accommodation> RetrieveAllAccommodations() =>
        TryCatch(() =>
        {
            return this.storageBroker.SelectAllAccommodations();
        });

        public ValueTask<Accommodation> RetrieveAccommodationByIdAsync(Guid accommodationId) =>
        TryCatch(async () =>
        {
            ValidateAccommodationById(accommodationId);

            Accommodation maybeAccommodation = await this.storageBroker
                .SelectAccommodationByIdAsync(accommodationId);

            ValidateStorageAccommodation(maybeAccommodation, accommodationId);

            return maybeAccommodation;
        });

        public ValueTask<Accommodation> ModifyAccommodationAsync(Accommodation accommodation) =>
        TryCatch( async () =>
        {
            ValidateAccommodationOnModify(accommodation);

            var maybeAccommodation =
               await this.storageBroker.SelectAccommodationByIdAsync(accommodation.Id);

            return await this.storageBroker.UpdateAccommodationAsync(accommodation);
        });
    }
}
