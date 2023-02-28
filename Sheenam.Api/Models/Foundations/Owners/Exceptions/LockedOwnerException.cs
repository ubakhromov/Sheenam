//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owners.Exceptions
{
    public class LockedOwnerException : Xeption
    {
        public LockedOwnerException(Exception innerException)
           : base(message: "Locked owner record excpetion, please try again later", innerException)
        { }
    }
}
