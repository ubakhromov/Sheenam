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
    }
}
