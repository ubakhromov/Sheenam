//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class AccommodationDependencyValidationException : Xeption
    {
        public AccommodationDependencyValidationException(Xeption innerException)
            : base(message: "Accommodation dependency validation error occurred, please fix the error and try again",
                  innerException)
        { }
    }
}
