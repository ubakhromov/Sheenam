//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================


namespace Sheenam.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
      public DateTimeOffset GetCurrentDateTime() =>
            DateTimeOffset.UtcNow;
    }
}
