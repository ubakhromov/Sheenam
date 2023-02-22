//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================



using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class FailedOwnerStorageException : Xeption
    {
        public FailedOwnerStorageException(Exception innerException)
           : base(message: "Failed owner storage error occured, contact support",
                 innerException)
        { }
    }
}
