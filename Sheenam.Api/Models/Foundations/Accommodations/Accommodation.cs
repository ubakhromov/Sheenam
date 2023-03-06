//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================

using System;
using Sheenam.Api.Models.Foundations.Owners;

namespace Sheenam.Api.Models.Foundations.Accommodations
{
    public class Accommodation
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Address { get; set; }
        public string AdditionalInfo { get; set; }
        public bool IsVacant { get; set; }
        public int NoOfBedrooms { get; set; }
        public int NoOfBathrooms { get; set; }
        public double Area { get; set; }
        public bool IsPetAllowed { get; set; }
        public HouseType House { get; set; }
        public decimal Price { get; set; }
        public bool IsShared { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
