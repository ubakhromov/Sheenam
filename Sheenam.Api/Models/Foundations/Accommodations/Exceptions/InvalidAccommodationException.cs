//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class InvalidAccommodationException : Xeption
    {
        public InvalidAccommodationException(string parameterName, object parameterValue)
           : base(message: $"Invalid accommodation, " +
                 $"parameter name: {parameterName}, " +
                 $"parameter value: {parameterValue}.")
        { }

        public InvalidAccommodationException()
        : base(message: "Accommodation is invalid")
        { }
    }
}
