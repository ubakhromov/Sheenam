﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public class GuestServices : IGuestServices
    {
        private readonly IStorageBroker storageBroker;

        public GuestServices(IStorageBroker storageBroker) =>
           this.storageBroker = storageBroker;

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
         this.storageBroker.InsertGuestAsync(guest);    
      
    }
}
