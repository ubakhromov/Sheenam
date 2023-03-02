//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Sheenam.Api.Models.Foundations.Accommodations;
using Sheenam.Api.Models.Foundations.Accommodations.Exceptions;

namespace Sheenam.Api.Services.Foundations.Accommodations
{
    public partial class AccommodationService
    {
        public static void ValidateAccommodationOnAdd(Accommodation accommodation)
        {
            ValidateAccommodationIsNotNull(accommodation);
        }

        private static void ValidateAccommodationIsNotNull(Accommodation accommodation)
        {
            if (accommodation is null)
            {
                throw new NullAccommodationException();
            }
        }
    }
}
