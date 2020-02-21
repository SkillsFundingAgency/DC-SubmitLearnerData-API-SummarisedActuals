using Newtonsoft.Json;

namespace ESFA.DC.PublicApi.FCS.Settings
{
    public class ConnectionStrings
    {
        [JsonRequired]
        public string AppLogs { get; set; }

        [JsonRequired]
        public string WebApiExternalIdentity { get; set; }

        [JsonRequired]
        public string SummarisedActualsDatastore { get; set; }
    }
}
