using System;

namespace MChatSDK
{

    public class MChatWorkerConfigurationException : Exception {

        public MChatWorkerConfigurationException()
            : base("MChat Worker not configured, Please configure worker")
        {
        }
    }

    public class MChatWorkerConfiguration
    {
        public enum MChatWorkerType
        {
            MChatWorkerKey,
            MChatWorkerBasic
        }


        internal String apiKey;
        internal MChatWorkerType workerType;
        internal String authorization;
        internal int bnsTimeout;
        Boolean configured = false;


        private static MChatWorkerConfiguration instance = null;

        public static MChatWorkerConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MChatWorkerConfiguration();
                }
                return instance;
            }
        }

        public void Configure(String apiKey, MChatWorkerType workerType, String authorization)
        {
            this.apiKey = apiKey;
            this.workerType = workerType;
            this.authorization = authorization;
            this.configured = true;
        }

        public void setBNSTimeout(int timeout)
        {
            this.bnsTimeout = timeout;
        }

        public String showInfo
        {
            get
            {
                String info = "";
                info += "ApiKey - " + this.apiKey + "\n";
                info += "WorkerType - " + this.workerType + "\n";
                info += "WorkerCredentials - " + this.authorization + "\n";
                return info;
            }
        }

        internal void CheckIsConfigured() {
            if (!this.configured)
            {
                throw new MChatWorkerConfigurationException();
            }
        }
    }
}
