using System;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IApplicationLogger
    {
        void Trace(string message);
        void Info(string message);
        void Warn(string message, Exception ex);
        void Error(string message, Exception ex);
    }
}
