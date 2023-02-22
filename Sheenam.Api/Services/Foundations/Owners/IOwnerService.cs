﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Sheenam.Api.Models.Foundations.Owner;

namespace Sheenam.Api.Services.Foundations.Owners
{
    public interface IOwnerService
    {
        ValueTask<Owner> AddOwnerAsync(Owner owner);
    }
}