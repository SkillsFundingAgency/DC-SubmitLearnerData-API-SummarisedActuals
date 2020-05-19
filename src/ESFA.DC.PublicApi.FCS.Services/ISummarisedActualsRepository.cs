using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Api.Common.Paging.Interfaces;
using ESFA.DC.PublicApi.FCS.Dtos;

namespace ESFA.DC.PublicApi.FCS.Services
{
    public interface ISummarisedActualsRepository
    {
        Task<IPaginatedResult<SummarisedActualDto>> GetSummarisedActuals(CancellationToken cancellationToken, string collectionType, string collectionReturnCode, int pageSize = 0, int pageNumber = 0);

        Task<IPaginatedResult<CollectionReturnDto>> GetClosedCollectionEvents(CancellationToken cancellationToken, DateTime? closedCollectionsSince = null, int pageSize = 0, int pageNumber = 0);
    }
}
