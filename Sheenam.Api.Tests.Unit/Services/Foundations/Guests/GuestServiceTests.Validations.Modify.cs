//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestIsNullAndLogItAsync()
        {
            //given
            Guest nullGuest = null;
            var nullGuestException = new NullGuestException();

            var expectedGuestValidationException = 
                new GuestValidationException(nullGuestException);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(nullGuest);

            //then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                modifyGuestTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => 
                broker.SelectGuestsByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();            
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestIsInvalidAndLogItAsync(string invalidText)
        {
            //given
            var invalidGuest = new Guest
            {
                FirstName = invalidText
            };

            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");

            invalidGuestException.AddData(
                key: nameof(Guest.FirstName),
                values: "Text is required");

           
            invalidGuestException.AddData(
                key: nameof(Guest.LastName),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.DateOfBirth),
                values: "Date is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Email),
                values: "Text is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Address),
                values: "Text is required");

            invalidGuestException.AddData(
               key: nameof(Guest.CreatedDate),
               values: "Date is required");

            invalidGuestException.AddData(
                key: nameof(Guest.UpdatedDate),
                    values: new[]
                    {
                        "Date is required",
                        $"Date is the same as {nameof(Guest.CreatedDate)}",
                        "Date is not recent"
                    }
                );

            var expectedGuestValidationException = 
                new GuestValidationException(invalidGuestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(GetRandomDateTimeOffset);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(invalidGuest);   
            
            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(() =>
                    modifyGuestTask.AsTask());

            //then
            actualGuestValidationException.Should().BeEquivalentTo(
                expectedGuestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guest randomGuest = CreateRandomGuest(randomDateTime);
            Guest invalidGuest = randomGuest;
            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.UpdatedDate),
                values: $"Date is the same as {nameof(Guest.CreatedDate)}");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(invalidGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(() =>
                    modifyGuestTask.AsTask());

            //then
            actualGuestValidationException.Should().BeEquivalentTo(
                expectedGuestValidationException);

            this.dateTimeBrokerMock.Verify(broker => 
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(invalidGuest.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Guest randomGuest = CreateRandomGuest(dateTime);
            Guest inputGuest = randomGuest;
            inputGuest.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidGuestException = 
                new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.UpdatedDate),
                values: "Date is not recent");

            var expectedGuestValidationException = 
                new GuestValidationException(invalidGuestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(inputGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(() =>
                    modifyGuestTask.AsTask());

            //then
            actualGuestValidationException.Should().BeEquivalentTo(
                expectedGuestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuestDoesNotExistAndLogItAsync()
        {
            //given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Guest randomGuest = CreateRandomGuest(dateTime);
            Guest nonExistGuest = randomGuest;
            nonExistGuest.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Guest nullGuest = null;

            var notFoundGuestException =
                new NotFoundGuestException(nonExistGuest.Id);

            var expectedGuestValidationException = 
                new GuestValidationException(notFoundGuestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestsByIdAsync(nonExistGuest.Id))
                    .ReturnsAsync(nullGuest);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(nonExistGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(() =>
                    modifyGuestTask.AsTask());

            //then
            actualGuestValidationException.Should().BeEquivalentTo(
                expectedGuestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(nonExistGuest.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
