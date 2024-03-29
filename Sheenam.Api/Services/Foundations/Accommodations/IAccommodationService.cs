﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Accommodations;

namespace Sheenam.Api.Services.Foundations.Accommodations
{
    public interface IAccommodationService
    {
        ValueTask<Accommodation> AddAccommodationAsync(Accommodation accommodation);
        IQueryable<Accommodation> RetrieveAllAccommodations();
        ValueTask<Accommodation> RetrieveAccommodationByIdAsync(Guid accommodationId);
        ValueTask<Accommodation> ModifyAccommodationAsync(Accommodation accommodation);
        ValueTask<Accommodation> RemoveAccommodationByIdAsync(Guid accommodationId);
    }
}
