using Newtonsoft.Json;

namespace ESFA.DC.PublicApi.FCS.Settings
{
    public class ConnectionStrings
    {
        [JsonRequired]
        public string AppLogs { get; set; }

        [JsonRequired]
        public string FundingClaims { get; set; }

        [JsonRequired]
        public string EAS1819 { get; set; }

        [JsonRequired]
        public string EAS1920 { get; set; }

        [JsonRequired]
        public string ILR1920DataStore { get; set; }

        [JsonRequired]
        public string WebApiExternalIdentity { get; set; }

        [JsonRequired]
        public string SummarisedActualsDatastore { get; set; }

        [JsonRequired]
        public string JobManagement { get; set; }
    }
}
