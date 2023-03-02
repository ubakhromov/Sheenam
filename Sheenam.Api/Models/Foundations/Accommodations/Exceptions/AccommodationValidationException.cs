//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class AccommodationValidationException :Xeption
    {
        public AccommodationValidationException(Xeption innerException)
        : base(message: "Accommodation validation error occured, fix the errors and try again",
              innerException)
        { }
    }
}
