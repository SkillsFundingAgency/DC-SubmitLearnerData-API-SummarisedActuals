using ESFA.DC.PublicApi.FCS.Dtos;
using System;
using System.Threading.Tasks;
using ESFA.DC.Api.Common.Utilities.Interfaces;

namespace ESFA.DC.PublicApi.FCS.Interfaces
{
    public interface ISummarisedActualsRepository
    {
        Task<IPaginatedResult<SummarisedActualDto>> GetSummarisedActuals(string collectionType, string collectionReturnCode, int pageSize = 0, int pageNumber = 0);

        Task<IPaginatedResult<CollectionReturnDto>> GetClosedCollectionEvents(DateTime? closedCollectionsSince = null, int pageSize = 0, int pageNumber = 0);
    }
}
