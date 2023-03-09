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
using Microsoft.EntityFrameworkCore;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTimeOffset();
            Accommodation randomAccommodation = CreateRandomAccommodation(someDateTime);
            Accommodation someAccommodation = randomAccommodation;
            Guid postId = someAccommodation.Id;
            SqlException sqlException = GetSqlException();

            var failedAccommodationStorageException =
                new FailedAccommodationStorageException(sqlException);

            var expectedAccommodationDependencyException =
                new AccommodationDependencyException(failedAccommodationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Accommodation> modifyAccommodationTask =
                this.accommodationService.ModifyAccommodationAsync(someAccommodation);

            AccommodationDependencyException actualAccommodationDependencyException =
              await Assert.ThrowsAsync<AccommodationDependencyException>(
                  modifyAccommodationTask.AsTask);

            // then
            actualAccommodationDependencyException.Should().BeEquivalentTo(
                expectedAccommodationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(postId),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAccommodationAsync(someAccommodation),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Accommodation randomAccommodation = CreateRandomAccommodation(randomDateTime);
            Accommodation SomeAccommodation = randomAccommodation;
            Guid AccommodationId = SomeAccommodation.Id;
            SomeAccommodation.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedAccommodationException =
                new FailedAccommodationStorageException(databaseUpdateException);

            var expectedAccommodationDependencyException =
                new AccommodationDependencyException(failedAccommodationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(AccommodationId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Accommodation> modifyAccommodationTask =
                this.accommodationService.ModifyAccommodationAsync(SomeAccommodation);

            AccommodationDependencyException actualAccommodationDependencyException =
              await Assert.ThrowsAsync<AccommodationDependencyException>(
                  modifyAccommodationTask.AsTask);

            // then
            actualAccommodationDependencyException.Should().BeEquivalentTo(
                expectedAccommodationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(AccommodationId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Accommodation randomAccommodation = CreateRandomAccommodation(randomDateTime);
            Accommodation someAccommodation = randomAccommodation;
            someAccommodation.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid postId = someAccommodation.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAccommodationException =
                new LockedAccommodationException(databaseUpdateConcurrencyException);

            var expectedAccommodationDependencyValidationException =
                new AccommodationDependencyValidationException(lockedAccommodationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(postId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Accommodation> modifyAccommodationTask =
                this.accommodationService.ModifyAccommodationAsync(someAccommodation);

            AccommodationDependencyValidationException actualAccommodationDependencyValidationException =
                await Assert.ThrowsAsync<AccommodationDependencyValidationException>(
                    modifyAccommodationTask.AsTask);

            // then
            actualAccommodationDependencyValidationException.Should().BeEquivalentTo(
                expectedAccommodationDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(postId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Accommodation randomAccommodation = CreateRandomAccommodation(randomDateTime);
            Accommodation someAccommodation = randomAccommodation;
            someAccommodation.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedAccommodationException =
                new FailedAccommodationServiceException(serviceException);

            var expectedAccommodationServiceException =
                new AccommodationServiceException(failedAccommodationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(someAccommodation.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Accommodation> modifyAccommodationTask =
                this.accommodationService.ModifyAccommodationAsync(someAccommodation);

            AccommodationServiceException actualAccommodationServiceException =
                await Assert.ThrowsAsync<AccommodationServiceException>(
                    modifyAccommodationTask.AsTask);

            // then
            actualAccommodationServiceException.Should().BeEquivalentTo(
                expectedAccommodationServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(someAccommodation.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
