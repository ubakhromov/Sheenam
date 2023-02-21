//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class InvalidOwnerException : Xeption
    {
        public InvalidOwnerException(string parameterName, object parameterValue)
            : base(message: $"Invalid owner, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }

        public InvalidOwnerException()
        : base(message: "Owner is invalid")
        { }
    }
}
