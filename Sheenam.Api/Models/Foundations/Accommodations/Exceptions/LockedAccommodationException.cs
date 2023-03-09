//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class LockedAccommodationException : Xeption
    {
        public LockedAccommodationException(Exception innerException)
            : base(message: "Locked accommodation record excpetion, contact support", innerException)
        { }
    }
}
