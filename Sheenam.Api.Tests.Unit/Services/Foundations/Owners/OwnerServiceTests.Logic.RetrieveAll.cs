//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System.Linq;
using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Owners;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Owners
{
    public partial class OwnerServiceTests
    {
        [Fact]
        public void ShouldRetrieveOwners()
        {
            // given
            IQueryable<Owner> randomOwners = CreateRandomOwners();
            IQueryable<Owner> storageOwners = randomOwners;
            IQueryable<Owner> expectedOwners = storageOwners;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOwners())
                    .Returns(storageOwners);

            // when
            IQueryable<Owner> actualOwners =
                this.ownerService.RetrieveAllOwners();

            // then
            actualOwners.Should().BeEquivalentTo(expectedOwners);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOwners(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
