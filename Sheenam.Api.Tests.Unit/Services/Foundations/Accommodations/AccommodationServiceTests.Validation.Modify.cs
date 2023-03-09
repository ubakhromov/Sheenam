//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using Moq;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Models.Foundations.Accommodations;
using Xunit;
using FluentAssertions;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Models.Foundations.Accommodations;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAccommodationIsNullAndLogItAsync()
        {
            // given
            Accommodation nullAccommodation = null;
            var nullAccommodationException = new NullAccommodationException();

            var expectedAccommodationValidationException =
                new AccommodationValidationException(nullAccommodationException);

            // when
            ValueTask<Accommodation> modifyAccommodationTask =
                this.accommodationService.ModifyAccommodationAsync(nullAccommodation);

            AccommodationValidationException actualAccommodationValidationException =
                await Assert.ThrowsAsync<AccommodationValidationException>(
                modifyAccommodationTask.AsTask);

            // then
            actualAccommodationValidationException.Should()
                .BeEquivalentTo(expectedAccommodationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfAccommodationIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidAccommodation = new Accommodation
            {
                Address = invalidText
            };

            var invalidAccommodationException =
                new InvalidAccommodationException();

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.Id),
                values: "Id is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.OwnerId),
                values: "Id is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.Address),
                values: "Text is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.Area),
                values: "Area is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.Price),
                values: "Price is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.CreatedDate),
                values: "Date is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.UpdatedDate),
                values: "Date is required");

            var expectedAccommodationValidationException =
                new AccommodationValidationException(invalidAccommodationException);
            
            // when
            ValueTask<Accommodation> modifyAccommodationTask =
                this.accommodationService.ModifyAccommodationAsync(invalidAccommodation);

            AccommodationValidationException actualAccommodationValidationException =
                await Assert.ThrowsAsync<AccommodationValidationException>(
                    modifyAccommodationTask.AsTask);

            //then
            actualAccommodationValidationException.Should()
                .BeEquivalentTo(expectedAccommodationValidationException);
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Accommodation randomAccommodation = CreateRandomAccommodation(randomDateTime);
            Accommodation invalidAccommodation = randomAccommodation;
            var invalidAccommodationException = new InvalidAccommodationException();

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.UpdatedDate),
                values: $"Date is the same as {nameof(Accommodation.CreatedDate)}");

            var expectedAccommodationValidationException =
                new AccommodationValidationException(invalidAccommodationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Accommodation> modifyProfileTask =
                this.accommodationService.ModifyAccommodationAsync(invalidAccommodation);

            AccommodationValidationException actualAccommodationValidationException =
                await Assert.ThrowsAsync<AccommodationValidationException>(
                    modifyProfileTask.AsTask);

            // then
            actualAccommodationValidationException.Should()
                .BeEquivalentTo(expectedAccommodationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(invalidAccommodation.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
