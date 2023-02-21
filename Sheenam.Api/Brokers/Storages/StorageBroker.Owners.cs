﻿//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Owner;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Owner> Owners { get; set; }
    }
}
