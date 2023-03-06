//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class FailedAccommodationServiceException : Xeption
    {
        public FailedAccommodationServiceException(Exception innerException)
                : base(message: "Service error occured", innerException)
        { }
    }
}
