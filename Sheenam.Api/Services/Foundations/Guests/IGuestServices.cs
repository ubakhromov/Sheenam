﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Sheenam.Api.Models.Foundations.Guests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public interface IGuestServices
    {
        ValueTask<Guest> AddGuestAsync(Guest guest);
        IQueryable<Guest> RetrieveAllGuests();
        ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId);
        ValueTask<Guest> ModifyGuestAsync(Guest guest);
        ValueTask<Guest> RemoveGuestByIdAsync(Guid guestId);
    }
}
