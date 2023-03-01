//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System.Threading.Tasks;
using Sheenam.Api.Models.Foundations.Accommodations;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Accommodation> InsertAccommodationAsync(Accommodation accommodation);
    }
}
