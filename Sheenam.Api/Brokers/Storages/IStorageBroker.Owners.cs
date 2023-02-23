﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Sheenam.Api.Models.Foundations.Owner;
using System.Threading.Tasks;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Owner> InsertOwnerAsync(Owner owner);
    }
}
