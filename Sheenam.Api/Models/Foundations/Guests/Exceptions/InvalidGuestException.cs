//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class InvalidGuestException : Xeption
    {
        public InvalidGuestException(string parameterName, object parameterValue)
            : base(message: $"Invalid guest, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidGuestException()
        : base(message: "Guest is invalid")
        { }
    }
}
