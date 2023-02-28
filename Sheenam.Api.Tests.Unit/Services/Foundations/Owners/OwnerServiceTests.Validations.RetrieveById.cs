//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.Extensions.Hosting;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using Sheenam.Api.Models.Foundations.Owners.Exceptions;
using Sheenam.Api.Models.Foundations.Owners;
using FluentAssertions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidOwnerId = Guid.Empty;

            var invalidOwnerException =
                new InvalidOwnerException();

            invalidOwnerException.AddData(
                key: nameof(Owner.Id),
                values: "Id is required");

            var expectedOwnerValidationException = new
                OwnerValidationException(invalidOwnerException);

            // when
            ValueTask<Owner> retrieveOwnerByIdTask =
                this.ownerService.RetrieveOwnerByIdAsync(invalidOwnerId);

            OwnerValidationException actualOwnerValidationException =
                await Assert.ThrowsAsync<OwnerValidationException>(
                    retrieveOwnerByIdTask.AsTask);

            // then
            actualOwnerValidationException.Should().BeEquivalentTo(expectedOwnerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfOwnerIsNotFoundAndLogItAsync()
        {
            //given
            Guid someOwnerId = Guid.NewGuid();
            Owner noOwner = null;

            var notFoundOwnerException =
                new NotFoundOwnerException(someOwnerId);

            var expectedOwnerValidationException =
                new OwnerValidationException(notFoundOwnerException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noOwner);

            //when
            ValueTask<Owner> retrieveOwnerByIdTask =
                this.ownerService.RetrieveOwnerByIdAsync(someOwnerId);

            OwnerValidationException actualOwnerValidationException =
                await Assert.ThrowsAsync<OwnerValidationException>(
                    retrieveOwnerByIdTask.AsTask);

            // then
            actualOwnerValidationException.Should().BeEquivalentTo(expectedOwnerValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}