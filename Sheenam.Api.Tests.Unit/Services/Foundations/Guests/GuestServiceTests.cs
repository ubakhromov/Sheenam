//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Moq;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public class GuestServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IGuestServices guestServices;

        public GuestServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.guestServices = 
                new GuestServices(storageBroker: this.storageBrokerMock.Object);
        }

       [Fact]
       public async Task ShouldAddGuestAsync()
        {
            //Arrange
            Guest randomGuest = new Guest
            {
                Id = Guid.NewGuid(),
                FirstName = "Tchuameni",
                Lastname = "Aurlien",
                Address = "Beruniy Str. #87",
                DateOfBirth = new DateTimeOffset(),
                Email = "Halls@epam.com",
                Gender = GenderType.Male,
                PhoneNumber = "8527410"
            };

            this.storageBrokerMock.Setup(broker =>
          broker.InsertGuestAsync(randomGuest))
              .ReturnsAsync(randomGuest);

            //Act
            Guest actual = await this.guestServices.AddGuestAsync(randomGuest);
            //Assert
            actual.Should().BeEquivalentTo(randomGuest);
        }
    }
}
