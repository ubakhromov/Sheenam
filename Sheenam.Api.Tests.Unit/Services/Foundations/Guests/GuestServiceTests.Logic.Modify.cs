//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Force.DeepCloner;
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
            DateTimeOffset someDate = 
                GetRandomDateTimeOffset();

            Guest randomGuest = 
                CreateRandomGuest(someDate);

            Guest inputGuest = randomGuest;

            Guest storageGuest = 
                inputGuest.DeepClone();

            storageGuest.UpdatedDate = 
                randomGuest.CreatedDate;

            Guest updatedGuest = inputGuest;

            Guest expectedGuest = 
                updatedGuest.DeepClone();

            Guid guestId = inputGuest.Id;




            //when

            //then
        }
    }
}
