//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using System;
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
    }
}
