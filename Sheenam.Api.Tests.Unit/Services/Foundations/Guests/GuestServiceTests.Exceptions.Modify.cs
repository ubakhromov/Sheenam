//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            SqlException sqlException = GetSqlError();

            var failedGuestStorageException = 
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(randomGuest);

            GuestDependencyException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(() =>
                    modifyGuestTask.AsTask());

            //then
            actualGuestDependencyException.Should().BeEquivalentTo(
                expectedGuestDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(randomGuest.Id),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(randomGuest),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();              
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            //given
            Guest someGuest = CreateRandomGuest();
            Guest foreignKeyConflictedGuest = someGuest;
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidGuestReferenceException = 
                new InvalidGuestReferenceException(foreignKeyConstraintConflictException);

            var guestDependencyValidationException = 
                new GuestDependencyValidationException(invalidGuestReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(foreignKeyConstraintConflictException);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(foreignKeyConflictedGuest);

            GuestDependencyValidationException actualGuestDependencyValidationException =
                await Assert.ThrowsAsync<GuestDependencyValidationException>(() =>
                    modifyGuestTask.AsTask());

            //then
            actualGuestDependencyValidationException.Should().BeEquivalentTo(
                guestDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(foreignKeyConflictedGuest.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   guestDependencyValidationException))),
                       Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(foreignKeyConflictedGuest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            var databaseUpdateException = new DbUpdateException();

            var failedGuestStorageException =
                new FailedGuestStorageException(databaseUpdateException);

            var expectedGuestDependencyException = 
                new GuestDependencyException(failedGuestStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Throws(databaseUpdateException);

            //when
            ValueTask<Guest> modifyGuestTask =
                this.guestServices.ModifyGuestAsync(randomGuest);

            GuestDependencyException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(() =>
                    modifyGuestTask.AsTask());

            //then
            actualGuestDependencyException.Should().BeEquivalentTo(
                expectedGuestDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestsByIdAsync(randomGuest.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(randomGuest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
