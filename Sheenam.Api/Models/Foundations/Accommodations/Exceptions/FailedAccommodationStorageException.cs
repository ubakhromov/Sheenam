//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class FailedAccommodationStorageException : Xeption
    {
        public FailedAccommodationStorageException(Exception innerException)
           : base(message: "Failed guest storage error occured, contact support",
                 innerException)
        { }
    }
}
