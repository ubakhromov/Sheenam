//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Owner.Exceptions
{
    public class NotFoundOwnerException : Xeption
    {
        public NotFoundOwnerException(Guid ownerId)
           : base(message: $"Couldn't find guest with id: {ownerId}.")
        { }
    }
}
