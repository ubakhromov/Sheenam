//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Accommodations
{
    public partial class AccommodationServiceTests
    {
        [Fact]
        public void ShouldRetrieveAccommodations()
        {
            // given
            IQueryable<Accommodation> randomAccommodations = CreateRandomAccommodations();
            IQueryable<Accommodation> storageAccommodations = randomAccommodations;
            IQueryable<Accommodation> expectedAccommodations = storageAccommodations;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAccommodations())
                    .Returns(storageAccommodations);

            // when
            IQueryable<Accommodation> actualAccommodations =
                this.accommodationService.RetrieveAllAccommodations();

            // then
            actualAccommodations.Should().BeEquivalentTo(expectedAccommodations);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAccommodations(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
