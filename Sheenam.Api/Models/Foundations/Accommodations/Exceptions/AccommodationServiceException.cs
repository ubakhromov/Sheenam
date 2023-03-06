//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class AccommodationServiceException : Xeption
    {
        public AccommodationServiceException(Xeption innerException)
            : base(message: "Accommodation service error occured, please fix the problem and try again",
                  innerException)
        { }
        
    }
}
