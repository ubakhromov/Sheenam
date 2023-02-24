//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class OwnerValidationException : Xeption
    {
        public OwnerValidationException(Xeption innerException)
        : base(message: "Owner validation error occured, fix the errors and try again",
              innerException)
        { }
    }
}
