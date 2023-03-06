//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class AccommodationDependencyException : Xeption
    {
        public AccommodationDependencyException(Xeption innerException)
         : base(message: "Accommodation dependency error occured, contact support",
                  innerException)
        { }
    }
}
