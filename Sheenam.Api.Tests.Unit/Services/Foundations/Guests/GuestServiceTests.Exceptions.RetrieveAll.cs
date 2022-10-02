//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlError();

            var failedGuestStorageException = 
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException = 
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests())
                    .Throws(sqlException);

            //when
            Action retrieveAllGuestAction = () =>
                this.guestServices.RetrieveAllGuests();

            //then
            Assert.Throws<GuestDependencyException>(
                retrieveAllGuestAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests());

            this.loggingBrokerMock.Verify(broker =>
             broker.LogCritical(It.Is(SameExceptionAs(
                 expectedGuestDependencyException))),
                     Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            //given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedGuestServiceException = 
                new FailedGuestServiceException(serviceException);

            var expectedGuestServiceException = 
                new GuestServiceException(failedGuestServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests())
                    .Throws(serviceException);

            //when
            Action retrieveAllGuestsAction = () =>
                this.guestServices.RetrieveAllGuests();

            //then
            Assert.Throws<GuestServiceException>(
                retrieveAllGuestsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
