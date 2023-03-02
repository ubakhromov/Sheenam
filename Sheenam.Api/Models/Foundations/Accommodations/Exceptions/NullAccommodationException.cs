//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Accommodations.Exceptions
{
    public class NullAccommodationException : Xeption
    {
        public NullAccommodationException()
           : base(message: "Accommodation is null")
        { }
    }
}
