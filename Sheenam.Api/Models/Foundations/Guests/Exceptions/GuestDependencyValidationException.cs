//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestDependencyValidationException :Xeption
    {
        public GuestDependencyValidationException(Xeption innerException)
            : base (message: "Guest dependency validation error occurred, please fix the error and try again",
                  innerException)
        {}
    }
}
