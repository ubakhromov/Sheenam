//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Owner;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public async Task ShouldAddOwnerAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Owner randomOwner = CreateRandomOwner(randomDateTime);
            Owner inputOwner = randomOwner;
            Owner insertedOwner = inputOwner;
            Owner expectedOwner = insertedOwner.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOwnerAsync(inputOwner))
                    .ReturnsAsync(insertedOwner);

            // when
            Owner actualOwner =
                await this.ownerService.AddOwnerAsync(inputOwner);

            // then
            actualOwner.Should().BeEquivalentTo(expectedOwner);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOwnerAsync(inputOwner),
                    Times.Once());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
