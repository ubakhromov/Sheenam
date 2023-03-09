//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someAccommodationId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedAccommodationException =
                new LockedAccommodationException(databaseUpdateConcurrencyException);

            var expectedAccommodationDependencyValidationException =
                new AccommodationDependencyValidationException(lockedAccommodationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Accommodation> removeAccommodationByIdTask =
                this.accommodationService.RemoveAccommodationByIdAsync(someAccommodationId);

            AccommodationDependencyValidationException actualAccommodationDependencyValidationException =
                await Assert.ThrowsAsync<AccommodationDependencyValidationException>(
                    removeAccommodationByIdTask.AsTask);

            // then
            actualAccommodationDependencyValidationException.Should().BeEquivalentTo(
                expectedAccommodationDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
