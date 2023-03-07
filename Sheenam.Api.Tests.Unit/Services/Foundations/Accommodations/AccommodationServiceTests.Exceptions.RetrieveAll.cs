//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using System;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedAccommodationStorageException(sqlException);

            var expectedAccommodationDependencyException =
                new AccommodationDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAccommodations()).Throws(sqlException);

            // when
            Action retrieveAllAccommodationsAction = () =>
                this.accommodationService.RetrieveAllAccommodations();

            AccommodationDependencyException actualAccommodationDependencyException =
                Assert.Throws<AccommodationDependencyException>(
                    retrieveAllAccommodationsAction);

            // then
            actualAccommodationDependencyException.Should().BeEquivalentTo(
                expectedAccommodationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAccommodations(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccommodationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedAccommodationServiceException =
                new FailedAccommodationServiceException(serviceException);

            var expectedAccommodationServiceException =
                new AccommodationServiceException(failedAccommodationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAccommodations())
                    .Throws(serviceException);

            // when
            Action retrieveAllAccommodationsAction = () =>
                this.accommodationService.RetrieveAllAccommodations();

            AccommodationServiceException actualAccommodationServiceException =
                Assert.Throws<AccommodationServiceException>(
                    retrieveAllAccommodationsAction);

            // then
            actualAccommodationServiceException.Should().BeEquivalentTo(
                expectedAccommodationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAccommodations(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccommodationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
