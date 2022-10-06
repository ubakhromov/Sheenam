//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldModifyGuestAsync()
        {
            //given
            DateTimeOffset randomDate = 
                GetRandomDateTimeOffset();

            Guest randomGuest = 
                CreateRandomGuest(randomDate);

            Guest inputGuest = randomGuest;

            inputGuest.UpdatedDate =
                randomDate.AddMinutes(1);

            Guest storageGuest =
                inputGuest;

            Guest updatedGuest =
                inputGuest;

            Guest expectedGuest =
                updatedGuest.DeepClone();

            Guid inputGuestId =
                inputGuest.Id;

            this.dateTimeBrokerMock.Setup(Brokers =>
                Brokers.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestsByIdAsync(
                    inputGuestId))
                        .ReturnsAsync(storageGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(
                    inputGuest))
                        .ReturnsAsync(updatedGuest);

            //when
            Guest actualGuest =
                await this.guestServices.
                    ModifyGuestAsync(inputGuest);

            //then
            actualGuest.Should().BeEquivalentTo(
                expectedGuest);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(inputGuestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(inputGuest),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
