//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class OwnerServiceException : Xeption
    {
        public OwnerServiceException(Xeption innerException)
           : base(message: "Owner service error occured, please fix the problem and try again", innerException)
        { }
    }
}
