//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAccommodationId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAccommodationStorageException =
                new FailedAccommodationStorageException(sqlException);

            var expectedAccommodationDependencyException =
                new AccommodationDependencyException(failedAccommodationStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);
            // when
            ValueTask<Accommodation> deleteAccommodationTask =
                this.accommodationService.RemoveAccommodationByIdAsync(someAccommodationId);

            AccommodationDependencyException actualAccommodationDependencyException =
                await Assert.ThrowsAsync<AccommodationDependencyException>(
                    deleteAccommodationTask.AsTask);

            // then
            actualAccommodationDependencyException.Should().BeEquivalentTo(
                expectedAccommodationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAccommodationId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAccommodationServiceException =
                new FailedAccommodationServiceException(serviceException);

            var expectedAccommodationServiceException =
                new AccommodationServiceException(failedAccommodationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Accommodation> removeAccommodationByIdTask =
                this.accommodationService.RemoveAccommodationByIdAsync(someAccommodationId);

            AccommodationServiceException actualAccommodationServiceException =
                await Assert.ThrowsAsync<AccommodationServiceException>(
                    removeAccommodationByIdTask.AsTask);

            // then
            actualAccommodationServiceException.Should().BeEquivalentTo(
                expectedAccommodationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
