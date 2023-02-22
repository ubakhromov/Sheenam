//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using System;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOwnerIsNullAndLogItAsync()
        {
            // given
            Owner nullOwner = null;

            var nullOwnerException =
                new NullOwnerException();

            var expectedOwnerValidationException =
                new OwnerValidationException(nullOwnerException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(nullOwner);

            // then
            await Assert.ThrowsAsync<OwnerValidationException>(() =>
                addOwnerTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                  broker.LogError(It.Is(SameExceptionAs(
                      expectedOwnerValidationException))),
                          Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfOwnerIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidOwner = new Owner
            {
                FirstName = invalidText
            };

            var invalidOwnerException =
                new InvalidOwnerException();

            invalidOwnerException.AddData(
                key: nameof(Owner.Id),
                values: "Id is required");

            invalidOwnerException.AddData(
                key: nameof(Owner.FirstName),
                values: "Text is required");

            invalidOwnerException.AddData(
                key: nameof(Owner.LastName),
                values: "Text is required");

            invalidOwnerException.AddData(
                key: nameof(Owner.DateOfBirth),
                values: "Date is required");

            invalidOwnerException.AddData(
                key: nameof(Owner.Email),
                values: "Text is required");

            var expectedOwnerValidationException =
                new OwnerValidationException(invalidOwnerException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(invalidOwner);

            OwnerValidationException actualOwnerValidationException =
               await Assert.ThrowsAsync<OwnerValidationException>(
                   addOwnerTask.AsTask);

            // then
            actualOwnerValidationException.Should().BeEquivalentTo(
                expectedOwnerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogitAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            Owner randomOwner = CreateRandomOwner();
            Owner invalidOwner = randomOwner;

            invalidOwner.UpdatedDate =
                invalidOwner.CreatedDate.AddDays(randomNumber);

            var invalidOwnerException =
                new InvalidOwnerException();

            invalidOwnerException.AddData(
                key: nameof(Owner.UpdatedDate),
                values: $"Date is not the same as {nameof(Owner.CreatedDate)}");

            var expectedOwnerValidationException =
                new OwnerValidationException(invalidOwnerException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(invalidOwner);

            // then
            await Assert.ThrowsAsync<OwnerValidationException>(() =>
               addOwnerTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime =
                GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTime.AddMinutes(minutesBeforeOrAfter);

            Owner randomOwner = CreateRandomOwner(invalidDateTime);
            Owner invalidOwner = randomOwner;

            var invalidOwnerException =
                new InvalidOwnerException();

            invalidOwnerException.AddData(
                key: nameof(Owner.CreatedDate),
                values: "Date is not recent");

            var expectedOwnerValidationException =
                new OwnerValidationException(invalidOwnerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(invalidOwner);

            OwnerValidationException actualOwnerValidationException =
               await Assert.ThrowsAsync<OwnerValidationException>(
                   addOwnerTask.AsTask);

            // then
            actualOwnerValidationException.Should().BeEquivalentTo(
                expectedOwnerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
