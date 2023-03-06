//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class AlreadyExistAccommodationException : Xeption
    {
        public AlreadyExistAccommodationException(Exception innerException)
            : base(message: "Accommodation already exist", innerException)
        { }
    }
}
