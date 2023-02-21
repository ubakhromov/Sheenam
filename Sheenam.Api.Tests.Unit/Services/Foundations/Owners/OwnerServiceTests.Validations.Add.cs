//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.Extensions.Hosting;
using Moq;
using Sheenam.Api.Models.Foundations.Owner;
using Sheenam.Api.Models.Foundations.Owner.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOwnerIsNullAndLogItAsync()
        {
            // given
            Owner nullOwner = null;

            var nullOwnerException =
                new NullOwnerException();

            var expectedOwnerValidationException =
                new OwnerValidationException(nullOwnerException);

            // when
            ValueTask<Owner> addOwnerTask =
                this.ownerService.AddOwnerAsync(nullOwner);

            // then
            await Assert.ThrowsAsync<OwnerValidationException>(() =>
                addOwnerTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                  broker.LogError(It.Is(SameExceptionAs(
                      expectedOwnerValidationException))),
                          Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
