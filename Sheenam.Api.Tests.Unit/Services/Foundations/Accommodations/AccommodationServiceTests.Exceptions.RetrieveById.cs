//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Models.Foundations.Accommodations;
using System.Threading.Tasks;
using System;
using Xunit;
using FluentAssertions;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using Sheenam.Api.Models.Foundations.Accommodations;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAccommodationStorageException =
                new FailedAccommodationStorageException(sqlException);

            var expectedAccommodationDependencyException =
                new AccommodationDependencyException(failedAccommodationStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Accommodation> retrieveAccommodationByIdTask =
                this.accommodationService.RetrieveAccommodationByIdAsync(someId);

            AccommodationDependencyException actaulAccommodationDependencyException =
                await Assert.ThrowsAsync<AccommodationDependencyException>(
                    retrieveAccommodationByIdTask.AsTask);

            // then
            actaulAccommodationDependencyException.Should().BeEquivalentTo(
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAccommodationServiceException =
                new FailedAccommodationServiceException(serviceException);

            var expectedAccommodationServiceException =
                new AccommodationServiceException(failedAccommodationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Accommodation> retrieveAccommodationByIdTask =
                this.accommodationService.RetrieveAccommodationByIdAsync(someId);

            AccommodationServiceException actualAccommodationServiceException =
                await Assert.ThrowsAsync<AccommodationServiceException>(
                    retrieveAccommodationByIdTask.AsTask);

            // then
            actualAccommodationServiceException.Should().BeEquivalentTo(expectedAccommodationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

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
