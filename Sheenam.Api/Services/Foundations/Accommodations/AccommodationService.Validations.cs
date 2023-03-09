//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Models.Foundations.Owners;

namespace Sheenam.Api.Services.Foundations.Accommodations
{
    public partial class AccommodationService
    {
        public void ValidateAccommodationOnAdd(Accommodation accommodation)
        {
            ValidateAccommodationIsNotNull(accommodation);

            Validate(
                (Rule: IsInvalid(accommodation.Id), Parameter: nameof(Accommodation.Id)),
                (Rule: IsInvalid(accommodation.OwnerId), Parameter: nameof(Accommodation.OwnerId)),
                (Rule: IsInvalid(accommodation.Address), Parameter: nameof(Accommodation.Address)),
                (Rule: IsInvalid(accommodation.Area), Parameter: nameof(Accommodation.Area)),
                (Rule: IsInvalid(accommodation.Price), Parameter: nameof(Accommodation.Price)),
                (Rule: IsInvalid(accommodation.CreatedDate), Parameter: nameof(Accommodation.CreatedDate)),
                (Rule: IsInvalid(accommodation.UpdatedDate), Parameter: nameof(Accommodation.UpdatedDate)),

                 (Rule: IsNotSame(
                    firstDate: accommodation.UpdatedDate,
                    secondDate: accommodation.CreatedDate,
                    secondDateName: nameof(Accommodation.CreatedDate)),
                Parameter: nameof(Accommodation.UpdatedDate)),

                 (Rule: IsNotRecent(accommodation.CreatedDate), Parameter: nameof(Accommodation.CreatedDate)));
        }

        public void ValidateAccommodationById(Guid accommodationId) =>
           Validate((Rule: IsInvalid(accommodationId), Parameter: nameof(Accommodation.Id)));

        private static void ValidateAccommodationIsNotNull(Accommodation accommodation)
        {
            if (accommodation is null)
            {
                throw new NullAccommodationException();
            }
        }

        public void ValidateStorageAccommodation(Accommodation maybeAccommodation, Guid accommodationId)
        {
            if (maybeAccommodation is null)
            {
                throw new NotFoundAccommodationException(accommodationId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(double area) => new
        {
            Condition = area == default,
            Message = "Area is required"
        };

        private static dynamic IsInvalid(decimal price) => new
        {
            Condition = price == default,
            Message = "Price is required"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private void ValidateAccommodationOnModify(Accommodation accommodation)
        {
            ValidateAccommodationIsNotNull(accommodation);

            Validate(
               (Rule: IsInvalid(accommodation.Id), Parameter: nameof(Accommodation.Id)),
               (Rule: IsInvalid(accommodation.OwnerId), Parameter: nameof(Accommodation.OwnerId)),
               (Rule: IsInvalid(accommodation.Address), Parameter: nameof(Accommodation.Address)),
               (Rule: IsInvalid(accommodation.Area), Parameter: nameof(Accommodation.Area)),
               (Rule: IsInvalid(accommodation.Price), Parameter: nameof(Accommodation.Price)),
               (Rule: IsInvalid(accommodation.CreatedDate), Parameter: nameof(Accommodation.CreatedDate)),
               (Rule: IsInvalid(accommodation.UpdatedDate), Parameter: nameof(Accommodation.UpdatedDate)),               
           
               (Rule: IsSame(
                    firstDate: accommodation.UpdatedDate,
                    secondDate: accommodation.CreatedDate,
                    secondDateName: nameof(Accommodation.CreatedDate)),
                 Parameter: nameof(Accommodation.UpdatedDate)),

                (Rule: IsNotRecent(accommodation.UpdatedDate), Parameter: nameof(Accommodation.UpdatedDate)));
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static dynamic IsNotSame(
           DateTimeOffset firstDate,
           DateTimeOffset secondDate,
           string secondDateName) => new
           {
               Condition = firstDate != secondDate,
               Message = $"Date is not the same as {secondDateName}"
           };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAccommodationException = new InvalidAccommodationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAccommodationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAccommodationException.ThrowIfContainsErrors();
        }
    }
}
