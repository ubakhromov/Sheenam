//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class OwnerDependencyValidationException : Xeption
    {
        public OwnerDependencyValidationException(Xeption innerException)
            : base(message: "Owner dependency validation error occurred, please fix the error and try again",
                  innerException)
        { }
    }
}
