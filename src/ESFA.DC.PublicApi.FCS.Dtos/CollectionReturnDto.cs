using System;

namespace ESFA.DC.PublicApi.FCS.Dtos
{
    public class CollectionReturnDto
    {
        public string CollectionType { get; set; }

        public string CollectionReturnCode { get; set; }

        public DateTime DateTime { get; set; }

        public int TotalItems { get; set; }
    }
}