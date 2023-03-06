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
using EFxceptions.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Accommodation someAccommodation = CreateRandomAccommodation(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedAccommodationStorageException =
                new FailedAccommodationStorageException(sqlException);

            var expectedAccommodationDependencyException =
                new AccommodationDependencyException(failedAccommodationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Accommodation> addAccommodationTask =
                this.accommodationService.AddAccommodationAsync(someAccommodation);

            AccommodationDependencyException actualAccommodationDependencyException =
                await Assert.ThrowsAsync<AccommodationDependencyException>(
                    addAccommodationTask.AsTask);

            // then
            actualAccommodationDependencyException.Should().BeEquivalentTo(
                expectedAccommodationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfAccommodationAlreadyExsitsAndLogItAsync()
        {
            // given
            Accommodation randomAccommodation = CreateRandomAccommodation();
            Accommodation alreadyExistsAccommodation = randomAccommodation;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistAccommodationException =
                new AlreadyExistAccommodationException(duplicateKeyException);

            var expectedAccommodationDependencyValidationException =
                new AccommodationDependencyValidationException(alreadyExistAccommodationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Accommodation> addAccommodationTask =
                this.accommodationService.AddAccommodationAsync(alreadyExistsAccommodation);

            AccommodationDependencyValidationException actualAccommodationDependencyValidationException =
               await Assert.ThrowsAsync<AccommodationDependencyValidationException>(
                   addAccommodationTask.AsTask);

            // then
            actualAccommodationDependencyValidationException.Should().BeEquivalentTo(
                expectedAccommodationDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccommodationAsync(It.IsAny<Accommodation>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Accommodation someAccommodation = CreateRandomAccommodation();

            var databaseUpdateException =
                new DbUpdateException();

            var failedAccommodationStorageException =
                new FailedAccommodationStorageException(databaseUpdateException);

            var expectedAccommodationDependencyException =
                new AccommodationDependencyException(failedAccommodationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Accommodation> addAccommodationTask =
                this.accommodationService.AddAccommodationAsync(someAccommodation);

            AccommodationDependencyException actualAccommodationDependencyException =
              await Assert.ThrowsAsync<AccommodationDependencyException>(
                  addAccommodationTask.AsTask);

            // then
            actualAccommodationDependencyException.Should().BeEquivalentTo(
                expectedAccommodationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyException))),
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
