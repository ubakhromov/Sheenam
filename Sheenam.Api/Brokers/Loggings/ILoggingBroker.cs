//===================================================
// Copyright (c) Coalition of Good-Hearted Engineers 
// Free To Use To Find Comfort and Peace
// ==================================================



namespace Sheenam.Api.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogError(Exception exception);
        void LogCrtitical (Exception exception);
        void LogTrace(string message);
        void LogWarning(string message);
        void LogDebug (string message);
        void LogInformation(string message);
    }
}
