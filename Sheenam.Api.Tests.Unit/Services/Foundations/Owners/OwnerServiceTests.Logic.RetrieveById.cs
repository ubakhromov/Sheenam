//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Owners;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOwnerByIdAsync()
        {
            // given
            Guid randomOwnerId = Guid.NewGuid();
            Guid inputOwnerId = randomOwnerId;
            Owner randomOwner = CreateRandomOwner();
            Owner storageOwner = randomOwner;
            Owner expectedOwner = storageOwner.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(inputOwnerId))
                    .ReturnsAsync(storageOwner);

            // when
            Owner actualOwner =
                await this.ownerService.RetrieveOwnerByIdAsync(inputOwnerId);

            // then
            actualOwner.Should().BeEquivalentTo(expectedOwner);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(inputOwnerId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
