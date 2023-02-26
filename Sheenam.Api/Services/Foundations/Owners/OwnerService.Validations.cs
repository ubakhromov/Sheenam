//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Microsoft.Extensions.Hosting;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public partial class OwnerService
    {
        private void ValidateOwner(Owner owner)
        {
            ValidateOwnerIsNotNull(owner);

            Validate(
                (Rule: IsInvalid(owner.Id), Parameter: nameof(Owner.Id)),
                (Rule: IsInvalid(owner.FirstName), Parameter: nameof(Owner.FirstName)),
                (Rule: IsInvalid(owner.LastName), Parameter: nameof(Owner.LastName)),
                (Rule: IsInvalid(owner.DateOfBirth), Parameter: nameof(Owner.DateOfBirth)),
                (Rule: IsInvalid(owner.Email), Parameter: nameof(Owner.Email)),
                (Rule: IsInvalid(owner.CreatedDate), Parameter: nameof(Owner.CreatedDate)),
                (Rule: IsInvalid(owner.UpdatedDate), Parameter: nameof(Owner.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: owner.UpdatedDate,
                    secondDate: owner.CreatedDate,
                    secondDateName: nameof(Owner.CreatedDate)),
                Parameter: nameof(Owner.UpdatedDate)),

                (Rule: IsNotRecent(owner.CreatedDate), Parameter: nameof(Owner.CreatedDate)));
        }

        private static void ValidateOwnerIsNotNull(Owner owner)
        {
            if (owner is null)
            {
                throw new NullOwnerException();
            }
        }

        public void ValidateOwnerById(Guid ownerId) =>
           Validate((Rule: IsInvalid(ownerId), Parameter: nameof(Owner.Id)));

        public void ValiateStorageOwner(Owner maybeOwner, Guid ownerId)
        {
            if (maybeOwner is null)
            {
                throw new NotFoundOwnerException(ownerId);
            }
        }

        private void ValidateOwnerOnModify(Owner owner)
        {
            ValidateOwnerIsNotNull(owner);
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

        private static dynamic IsNotSame(
           DateTimeOffset firstDate,
           DateTimeOffset secondDate,
           string secondDateName) => new
           {
               Condition = firstDate != secondDate,
               Message = $"Date is not the same as {secondDateName}"
           };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidOwnerException = new InvalidOwnerException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidOwnerException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidOwnerException.ThrowIfContainsErrors();
        }
    }
}
