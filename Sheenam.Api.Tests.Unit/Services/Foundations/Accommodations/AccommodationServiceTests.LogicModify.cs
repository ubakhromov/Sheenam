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
        public async Task ShouldModifyAccommodationAsync()
        {
            // given
            DateTimeOffset randomDate =
                GetRandomDateTimeOffset();

            Accommodation randomAccommodation =
                CreateRandomModifyAccommodation(randomDate);

            Accommodation inputAccommodation =
                randomAccommodation;

            inputAccommodation.UpdatedDate =
                randomDate.AddMinutes(1);

            Accommodation storageAccommodation =
                inputAccommodation;

            Accommodation updatedAccommodation =
                inputAccommodation;

            Accommodation expectedAccommodation =
                updatedAccommodation.DeepClone();

            Guid inputAccommodationId =
                inputAccommodation.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccommodationByIdAsync(
                    inputAccommodationId))
                        .ReturnsAsync(storageAccommodation);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAccommodationAsync(
                    inputAccommodation))
                        .ReturnsAsync(updatedAccommodation);

            // when
            Accommodation actualAccommodation =
                await this.accommodationService.
                    ModifyAccommodationAsync(inputAccommodation);

            // then
            actualAccommodation.Should().BeEquivalentTo(
                expectedAccommodation);
        
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccommodationByIdAsync(inputAccommodationId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAccommodationAsync(inputAccommodation),
                    Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
