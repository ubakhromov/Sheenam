//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Moq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using System;
using Xunit;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using FluentAssertions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOwnerIsNullAndLogItAsync()
        {
            // given
            Owner nullOwner = null;
            var nullOwnerException = new NullOwnerException();

            var expectedOwnerValidationException =
                new OwnerValidationException(nullOwnerException);

            // when
            ValueTask<Owner> modifyOwnerTask =
                this.ownerService.ModifyOwnerAsync(nullOwner);

            OwnerValidationException actualOwnerValidationException =
                await Assert.ThrowsAsync<OwnerValidationException>(
                modifyOwnerTask.AsTask);

            // then
            actualOwnerValidationException.Should()
                .BeEquivalentTo(expectedOwnerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOwnerIsInvalidAndLogItAsync(string invalidText)
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

            invalidOwnerException.AddData(
                key: nameof(Owner.CreatedDate),
                values: "Date is required");

            invalidOwnerException.AddData(
                key: nameof(Owner.UpdatedDate),
                values: "Date is required");

            var expectedOwnerValidationException =
                new OwnerValidationException(invalidOwnerException);

            // when
            ValueTask<Owner> modifyOwnerTask =
                this.ownerService.ModifyOwnerAsync(invalidOwner);

            OwnerValidationException actualOwnerValidationException =
                await Assert.ThrowsAsync<OwnerValidationException>(
                    modifyOwnerTask.AsTask);

            //then
            actualOwnerValidationException.Should()
                .BeEquivalentTo(expectedOwnerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Owner randomOwner = CreateRandomOwner(randomDateTime);
            Owner invalidOwner = randomOwner;
            var invalidOwnerException = new InvalidOwnerException();

            invalidOwnerException.AddData(
                key: nameof(Owner.UpdatedDate),
                values: $"Date is the same as {nameof(Owner.CreatedDate)}");

            var expectedOwnerValidationException =
                new OwnerValidationException(invalidOwnerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Owner> modifyProfileTask =
                this.ownerService.ModifyOwnerAsync(invalidOwner);

            OwnerValidationException actualOwnerValidationException =
                await Assert.ThrowsAsync<OwnerValidationException>(
                    modifyProfileTask.AsTask);

            // then
            actualOwnerValidationException.Should()
                .BeEquivalentTo(expectedOwnerValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(invalidOwner.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
