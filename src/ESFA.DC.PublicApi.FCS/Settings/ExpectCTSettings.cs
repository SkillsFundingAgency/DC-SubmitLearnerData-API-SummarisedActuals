using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESFA.DC.PublicApi.FCS.Settings
{
    public class ExpectCTSettings
    {
        private const string ReportUriTemplate = "max-age={0}, {1} https://{2}.report-uri.com/r/d/ct/{3}";

        public string Domain { get; set; }

        public int MaxAge { get; set; }

        public bool Enforce { get; set; }

        public string HeaderValue() => string.Format(ReportUriTemplate, MaxAge, Enforce ? "enforce," : string.Empty, Domain, Enforce ? "enforce" : "reportOnly");
    }
}
