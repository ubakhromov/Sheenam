//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Moq;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Models.Foundations.Accommodations;
using System.Threading.Tasks;
using System;
using Xunit;
using FluentAssertions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidAccommodationId = Guid.Empty;

            var invalidAccommodationException =
                new InvalidAccommodationException();

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.Id),
                values: "Id is required");

            var expectedAccommodationValidationException = new
                AccommodationValidationException(invalidAccommodationException);

            // when
            ValueTask<Accommodation> retrieveAccommodationByIdTask =
                this.accommodationService.RetrieveAccommodationByIdAsync(invalidAccommodationId);

            AccommodationValidationException actualAccommodationValidationException =
                await Assert.ThrowsAsync<AccommodationValidationException>(
                    retrieveAccommodationByIdTask.AsTask);

            // then
            actualAccommodationValidationException.
                Should().BeEquivalentTo(expectedAccommodationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
