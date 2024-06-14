using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeTorch.Core.Interfaces
{
    public interface IServiceLogProvider
    {
        void Initialize(string config);
        void Log(ServiceLogEntry serviceLogEntryDTO);
    }
}
