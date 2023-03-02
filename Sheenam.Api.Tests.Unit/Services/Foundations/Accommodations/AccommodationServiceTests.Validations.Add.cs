//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Moq;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Models.Foundations.Owners.Exceptions;
using Sheenam.Api.Models.Foundations.Owners;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAccommodationIsNullAndLogItAsync()
        {
            //given
            Accommodation nullAccommodation = null;
            var nullAccommodationException = new NullAccommodationException();

            var expectedAccommodationValidationException =
                new AccommodationValidationException(nullAccommodationException);

            //when
            ValueTask<Accommodation> addAccommodationTask =
                this.accommodationService.AddAccommodationAsync(nullAccommodation);

            AccommodationValidationException actualAccommodationValidationException =
                await Assert.ThrowsAsync<AccommodationValidationException>(
                    addAccommodationTask.AsTask);

            //then
            await Assert.ThrowsAsync<AccommodationValidationException>(() =>
                addAccommodationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccommodationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAccommodationIsInvalidAndLogItAsync(
           string invalidText)
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
                values: "Text is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.Price),
                values: "Text is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.CreatedDate),
                values: "Date is required");

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.UpdatedDate),
                values: "Date is required");

            var expectedAccommodationValidationException =
                new AccommodationValidationException(invalidAccommodationException);

            // when
            ValueTask<Accommodation> addAccommodationTask =
                this.accommodationService.AddAccommodationAsync(invalidAccommodation);

            AccommodationValidationException actualAccommodationValidationException =
               await Assert.ThrowsAsync<AccommodationValidationException>(
                   addAccommodationTask.AsTask);

            // then
            actualAccommodationValidationException.Should().BeEquivalentTo(
                expectedAccommodationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
