﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Guest> InsertGuestAsync(Guest guest);
        IQueryable<Guest> SelectAllGuests();
        ValueTask<Guest> SelectGuestsByIdAsync(Guid GuestId);
        ValueTask<Guest> UpdateGuestAsync(Guest guest);
        ValueTask<Guest> DeleteGuestAsync(Guest guest);
    }
}
