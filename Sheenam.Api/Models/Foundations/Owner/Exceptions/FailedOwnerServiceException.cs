//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class FailedOwnerServiceException : Xeption
    {
        public FailedOwnerServiceException(Exception innerException)
           : base(message: "Service error occured", innerException)
        { }
    }
}
