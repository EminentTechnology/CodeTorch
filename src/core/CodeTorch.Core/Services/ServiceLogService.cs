using CodeTorch.Core.Interfaces;

namespace CodeTorch.Core.Services
{
    public partial class ServiceLogService
    {
        static readonly ServiceLogService instance = new ServiceLogService();

        public static ServiceLogService GetInstance()
        {
            return instance;
        }

        private ServiceLogService()
        {
        }

        IServiceLogProvider serviceLogProvider = null;

        public IServiceLogProvider ServiceLogProvider
        {
            get
            {
                if (serviceLogProvider == null)
                {
                    serviceLogProvider = GetProvider();
                }
                return serviceLogProvider;
            }
            set
            {
                serviceLogProvider = value;
            }
        }

        private IServiceLogProvider GetProvider()
        {
            var provider = new AppServiceLogProvider();
            provider.Initialize(null);
            return provider;
        }
    }
}
