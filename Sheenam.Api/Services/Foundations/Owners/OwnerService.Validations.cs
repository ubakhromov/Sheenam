//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Microsoft.Extensions.Hosting;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using System;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public partial class OwnerService
    {
        private static void ValidateOwner(Owner owner)
        {
            ValidateOwnerIsNotNull(owner);

            Validate(
                (Rule: IsInvalid(owner.Id), Parameter: nameof(Owner.Id)),
                (Rule: IsInvalid(owner.FirstName), Parameter: nameof(Owner.FirstName)),
                (Rule: IsInvalid(owner.LastName), Parameter: nameof(Owner.LastName)),
                (Rule: IsInvalid(owner.DateOfBirth), Parameter: nameof(Owner.DateOfBirth)),
                (Rule: IsInvalid(owner.Email), Parameter: nameof(Owner.Email)),

                (Rule: IsNotSame(
                    firstDate: owner.UpdatedDate,
                    secondDate: owner.CreatedDate,
                    secondDateName: nameof(Owner.CreatedDate)),
                Parameter: nameof(Owner.UpdatedDate))
            );
        }

        private static void ValidateOwnerIsNotNull(Owner owner)
        {
            if (owner is null)
            {
                throw new NullOwnerException();
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
