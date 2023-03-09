//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidAccommodationId = Guid.Empty;

            var invalidAccommodationException =
                new InvalidAccommodationException();

            invalidAccommodationException.AddData(
                key: nameof(Accommodation.Id),
                values: "Id is required");

            var expectedAccommodationValidationException =
                new AccommodationValidationException(invalidAccommodationException);

            // when
            ValueTask<Accommodation> removeAccommodationByIdTask =
                this.accommodationService.RemoveAccommodationByIdAsync(invalidAccommodationId);

            AccommodationValidationException actualAccommodationValidationException =
                await Assert.ThrowsAsync<AccommodationValidationException>(() =>
                    removeAccommodationByIdTask.AsTask());

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
                broker.DeleteAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
