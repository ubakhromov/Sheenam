//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Threading.Tasks;
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
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;

            DateTimeOffset randomDate =
                GetRandomDateTimeOffset();

            Guest randomGuest =
                CreateRandomGuest();

            Guest inputGuest = randomGuest;

            Guest afterUpdateStorageGuest =
                inputGuest;

            Guest expectedGuest =
                afterUpdateStorageGuest;

            Guest beforeUpdateStorageGuest =
                randomGuest.DeepClone();

            inputGuest.UpdatedDate = randomDate;
            Guid guestId = inputGuest.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestsByIdAsync(guestId))
                    .ReturnsAsync(beforeUpdateStorageGuest);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(inputGuest))
                    .ReturnsAsync(afterUpdateStorageGuest);

            //when
            Guest actualGuest =
                await this.guestServices.ModifyGuestAsync(inputGuest);

            //then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(guestId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(inputGuest),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
