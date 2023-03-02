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
        public async Task ShouldRemoveOwnerById()
        {
            //given
            Guid randomId = Guid.NewGuid();
            Guid inputOwnerId = randomId;
            Owner randomOwner = CreateRandomOwner();
            Owner storageOwner = randomOwner;
            Owner expectedInputOwner = storageOwner;
            Owner deletedOwner = expectedInputOwner;
            Owner expectedOwner = deletedOwner.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOwnerByIdAsync(inputOwnerId))
                    .ReturnsAsync(storageOwner);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteOwnerAsync(expectedInputOwner))
                    .ReturnsAsync(deletedOwner);

            //when
            Owner actualOwner = await this.ownerService
                .RemoveOwnerByIdAsync(inputOwnerId);

            //then
            actualOwner.Should().BeEquivalentTo(expectedOwner);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOwnerByIdAsync(inputOwnerId),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOwnerAsync(expectedInputOwner),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
