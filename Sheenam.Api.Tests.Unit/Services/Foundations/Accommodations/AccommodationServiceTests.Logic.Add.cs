//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Owners;
using System.Threading.Tasks;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public async Task ShouldAddAccommodationAsync()
        {
            // given
            Accommodation randomAccommodation = 
                CreateRandomAccommodation();
            Accommodation inputAccommodation = randomAccommodation;
            Accommodation insertedAccommodation = inputAccommodation;
            Accommodation expectedAccommodation = insertedAccommodation.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAccommodationAsync(inputAccommodation))
                    .ReturnsAsync(insertedAccommodation);

            // when
            Accommodation actualAccommodation =
                await this.accommodationService.AddAccommodationAsync(inputAccommodation);

            // then
            actualAccommodation.Should().BeEquivalentTo(expectedAccommodation);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccommodationAsync(inputAccommodation),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
