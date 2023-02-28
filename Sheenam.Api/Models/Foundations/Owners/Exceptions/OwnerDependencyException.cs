//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owners.Exceptions
{
    public class OwnerDependencyException : Xeption
    {
        public OwnerDependencyException(Xeption innerException)
            : base(message: "Owner dependency error occured, contact support",
                  innerException)
        { }
    }
}
