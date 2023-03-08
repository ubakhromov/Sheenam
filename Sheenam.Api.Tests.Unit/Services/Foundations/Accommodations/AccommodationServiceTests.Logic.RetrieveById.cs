//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Moq;
using Sheenam.Api.Models.Foundations.Accommodations;
using System.Threading.Tasks;
using System;
using Xunit;
using Force.DeepCloner;
using FluentAssertions;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAccommodationByIdAsync()
        {
            // given
            Guid randomAccommodationId = Guid.NewGuid();
            Guid inputAccommodationId = randomAccommodationId;
            Accommodation randomAccommodation = CreateRandomAccommodation();
            Accommodation storageAccommodation = randomAccommodation;
            Accommodation expectedAccommodation = storageAccommodation.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(inputAccommodationId))
                    .ReturnsAsync(storageAccommodation);

            // when
            Accommodation actualAccommodation =
                await this.accommodationService.RetrieveAccommodationByIdAsync(inputAccommodationId);

            // then
            actualAccommodation.Should().BeEquivalentTo(expectedAccommodation);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(inputAccommodationId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
