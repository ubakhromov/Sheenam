//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Owners.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedOwnerStorageException(sqlException);

            var expectedOwnerDependencyException =
                new OwnerDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOwners())
                    .Throws(sqlException);

            // when
            Action retrieveAllOwnersAction = () =>
                this.ownerService.RetrieveAllOwners();

            OwnerDependencyException actualOwnerDependencyException =
                Assert.Throws<OwnerDependencyException>(
                    retrieveAllOwnersAction);

            // then
            actualOwnerDependencyException.Should().BeEquivalentTo(
                expectedOwnerDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOwners(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOwnerDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedOwnerServiceException =
                new FailedOwnerServiceException(serviceException);

            var expectedOwnerServiceException =
                new OwnerServiceException(failedOwnerServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOwners())
                    .Throws(serviceException);

            // when
            Action retrieveAllOwnersAction = () =>
                this.ownerService.RetrieveAllOwners();

            OwnerServiceException actualOwnerServiceException =
                Assert.Throws<OwnerServiceException>(
                    retrieveAllOwnersAction);

            // then
            actualOwnerServiceException.Should().BeEquivalentTo(
                expectedOwnerServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOwners(),
                    Times.Once);

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
