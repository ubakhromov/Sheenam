//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Moq;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someOwnerId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedOwnerException =
                new LockedOwnerException(databaseUpdateConcurrencyException);

            var expectedOwnerDependencyValidationException =
                new OwnerDependencyValidationException(lockedOwnerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Owner> removeOwnerByIdTask =
                this.ownerService.RemoveOwnerByIdAsync(someOwnerId);

            OwnerDependencyValidationException actualOwnerDependencyValidationException =
                await Assert.ThrowsAsync<OwnerDependencyValidationException>(
                    removeOwnerByIdTask.AsTask);

            // then
            actualOwnerDependencyValidationException.Should().BeEquivalentTo(
                expectedOwnerDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someOwnerId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedOwnerStorageException =
                new FailedOwnerStorageException(sqlException);

            var expectedOwnerDependencyException =
                new OwnerDependencyException(failedOwnerStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);
            // when
            ValueTask<Owner> deleteOwnerTask =
                this.ownerService.RemoveOwnerByIdAsync(someOwnerId);

            OwnerDependencyException actualOwnerDependencyException =
                await Assert.ThrowsAsync<OwnerDependencyException>(
                    deleteOwnerTask.AsTask);

            // then
            actualOwnerDependencyException.Should().BeEquivalentTo(
                expectedOwnerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOwnerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someOwnerId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedOwnerServiceException =
                new FailedOwnerServiceException(serviceException);

            var expectedOwnerServiceException =
                new OwnerServiceException(failedOwnerServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Owner> removeOwnerByIdTask =
                this.ownerService.RemoveOwnerByIdAsync(someOwnerId);

            OwnerServiceException actualOwnerServiceException =
                await Assert.ThrowsAsync<OwnerServiceException>(
                    removeOwnerByIdTask.AsTask);

            // then
            actualOwnerServiceException.Should().BeEquivalentTo(
                expectedOwnerServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
