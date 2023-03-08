//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class NotFoundAccommodationException : Xeption
    {
        public NotFoundAccommodationException(Guid accommodationId)
            : base(message: $"Couldn't find accommodation with id: {accommodationId}")
        {}
    }
}