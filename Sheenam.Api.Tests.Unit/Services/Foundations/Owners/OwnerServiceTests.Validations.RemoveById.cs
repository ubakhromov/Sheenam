//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.Extensions.Hosting;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using Sheenam.Api.Models.Foundations.Owner;
using FluentAssertions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidOwnerId = Guid.Empty;

            var invalidOwnerException =
                new InvalidOwnerException();

            invalidOwnerException.AddData(
                key: nameof(Owner.Id),
                values: "Id is required");

            var expectedOwnerValidationException =
                new OwnerValidationException(invalidOwnerException);

            // when
            ValueTask<Owner> removeOwnerByIdTask =
                this.ownerService.RemoveOwnerByIdAsync(invalidOwnerId);

            OwnerValidationException actualOwnerValidationException =
                await Assert.ThrowsAsync<OwnerValidationException>(() =>
                    removeOwnerByIdTask.AsTask());

            // then
            actualOwnerValidationException.Should()
                .BeEquivalentTo(expectedOwnerValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOwnerValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOwnerAsync(It.IsAny<Owner>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}