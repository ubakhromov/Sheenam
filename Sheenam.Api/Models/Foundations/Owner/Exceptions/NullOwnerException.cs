//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class NullOwnerException : Xeption
    {
        public NullOwnerException()
           : base(message: "Owner is null")
        {}
    }
}
