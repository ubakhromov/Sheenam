//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Data;
using System.Reflection.Metadata;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;

namespace Sheenam.Api.Services.Foundations.Accommodations
{
    public partial class AccommodationService
    {
        public static void ValidateAccommodationOnAdd(Accommodation accommodation)
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
                    secondDateName: nameof(accommodation.CreatedDate)),
                Parameter: nameof(accommodation.UpdatedDate)));
        }

        private static void ValidateAccommodationIsNotNull(Accommodation accommodation)
        {
            if (accommodation is null)
            {
                throw new NullAccommodationException();
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

        private static dynamic IsNotSame(
           DateTimeOffset firstDate,
           DateTimeOffset secondDate,
           string secondDateName) => new
           {
               Condition = firstDate != secondDate,
               Message = $"Date is not the same as {secondDateName}"
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
