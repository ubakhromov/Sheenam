//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class InvalidGuestReferenceException : Xeption
    {
        public InvalidGuestReferenceException(Exception innerException)
            : base(message: "Invalid guest reference error occurred.", innerException)
        { }
    }
}
