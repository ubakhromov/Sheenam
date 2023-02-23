//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class AlreadyExistsOwnerException : Xeption
    {
        public AlreadyExistsOwnerException(Exception innerException)
           : base(message: "Owner already exist", innerException)
        {}
    }
}
