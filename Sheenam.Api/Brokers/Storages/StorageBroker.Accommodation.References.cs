//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Accommodations;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddAccommodationsReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accommodation>()
                .HasOne(accommodation => accommodation.Owner)
                .WithMany(owner => owner.Accommodations)
                .HasForeignKey(accommodation => accommodation.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
