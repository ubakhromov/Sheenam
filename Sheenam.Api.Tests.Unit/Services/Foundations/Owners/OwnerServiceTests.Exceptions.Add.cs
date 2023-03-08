//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Owners;
using Sheenam.Api.Models.Foundations.Owners.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Owner someOwner = CreateRandomOwner(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedOwnerStorageException =
                new FailedOwnerStorageException(sqlException);

            var expectedOwnerDependencyException =
                new OwnerDependencyException(failedOwnerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(someOwner);

            OwnerDependencyException actualOwnerDependencyException =
                await Assert.ThrowsAsync<OwnerDependencyException>(
                    addOwnerTask.AsTask);

            // then
            actualOwnerDependencyException.Should().BeEquivalentTo(
                expectedOwnerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOwnerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfOwnerAlreadyExsitsAndLogItAsync()
        {
            // given
            Owner randomOwner = CreateRandomOwner();
            Owner alreadyExistsOwner = randomOwner;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsOwnerException =
                new AlreadyExistsOwnerException(duplicateKeyException);

            var expectedOwnerDependencyValidationException =
                new OwnerDependencyValidationException(alreadyExistsOwnerException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(alreadyExistsOwner);

            OwnerDependencyValidationException actualOwnerDependencyValidationException =
               await Assert.ThrowsAsync<OwnerDependencyValidationException>(
                   addOwnerTask.AsTask);

            // then
            actualOwnerDependencyValidationException.Should().BeEquivalentTo(
                expectedOwnerDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
            Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Owner someOwner = CreateRandomOwner();

            var databaseUpdateException =
                new DbUpdateException();

            var failedOwnerStorageException =
                new FailedOwnerStorageException(databaseUpdateException);

            var expectedOwnerDependencyException =
                new OwnerDependencyException(failedOwnerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(someOwner);

            OwnerDependencyException actualOwnerDependencyException =
              await Assert.ThrowsAsync<OwnerDependencyException>(
                  addOwnerTask.AsTask);

            // then
            actualOwnerDependencyException.Should().BeEquivalentTo(
                expectedOwnerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Owner someOwner = CreateRandomOwner();
            var serviceException = new Exception();

            var failedOwnerServiceException =
                new FailedOwnerServiceException(serviceException);

            var expectedOwnerServiceException =
                new OwnerServiceException(failedOwnerServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(someOwner);

            OwnerServiceException actualOwnerServiceException =
              await Assert.ThrowsAsync<OwnerServiceException>(
                  addOwnerTask.AsTask);

            // then
            actualOwnerServiceException.Should().BeEquivalentTo(
                expectedOwnerServiceException);

            // then
            await Assert.ThrowsAsync<OwnerServiceException>(() =>
                addOwnerTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
