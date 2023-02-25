﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedOwnerStorageException =
                new FailedOwnerStorageException(sqlException);

            var expectedOwnerDependencyException =
                new OwnerDependencyException(failedOwnerStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Owner> retrieveOwnerByIdTask =
                this.ownerService.RetrieveOwnerByIdAsync(someId);

            OwnerDependencyException actaulOwnerDependencyException =
                await Assert.ThrowsAsync<OwnerDependencyException>(
                    retrieveOwnerByIdTask.AsTask);

            // then
            actaulOwnerDependencyException.Should().BeEquivalentTo(
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
    }
}
