using System.Collections.Generic;

namespace Backups.Services
{
    public interface IJobObject
    {
        string GetObjectName();
        IReadOnlyCollection<byte> GetBytes();
    }
}