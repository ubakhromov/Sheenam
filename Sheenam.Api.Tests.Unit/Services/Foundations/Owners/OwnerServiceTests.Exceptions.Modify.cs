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
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTimeOffset();
            Owner randomOwner = CreateRandomOwner(someDateTime);
            Owner someOwner = randomOwner;
            Guid postId = someOwner.Id;
            SqlException sqlException = GetSqlException();

            var failedOwnerStorageException =
                new FailedOwnerStorageException(sqlException);

            var expectedOwnerDependencyException =
                new OwnerDependencyException(failedOwnerStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Owner> modifyOwnerTask =
                this.ownerService.ModifyOwnerAsync(someOwner);

            OwnerDependencyException actualOwnerDependencyException =
              await Assert.ThrowsAsync<OwnerDependencyException>(
                  modifyOwnerTask.AsTask);

            // then
            actualOwnerDependencyException.Should().BeEquivalentTo(
                expectedOwnerDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(postId),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOwnerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOwnerAsync(someOwner),
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
            Owner randomOwner = CreateRandomOwner(randomDateTime);
            Owner SomeOwner = randomOwner;
            Guid ownerId = SomeOwner.Id;
            SomeOwner.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedOwnerException =
                new FailedOwnerStorageException(databaseUpdateException);

            var expectedOwnerDependencyException =
                new OwnerDependencyException(failedOwnerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(ownerId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Owner> modifyOwnerTask =
                this.ownerService.ModifyOwnerAsync(SomeOwner);

            OwnerDependencyException actualOwnerDependencyException =
              await Assert.ThrowsAsync<OwnerDependencyException>(
                  modifyOwnerTask.AsTask);

            // then
            actualOwnerDependencyException.Should().BeEquivalentTo(
                expectedOwnerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(ownerId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
