using System;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.Api.Common.Paging.Interfaces;
using ESFA.DC.Api.Common.Paging.Pagination;
using ESFA.DC.PublicApi.FCS.Dtos;
using ESFA.DC.Summarisation.Model;
using ESFA.DC.Summarisation.Model.Interface;
using Microsoft.EntityFrameworkCore;
using SummarisedActualDto = ESFA.DC.PublicApi.FCS.Dtos.SummarisedActualDto;

namespace ESFA.DC.PublicApi.FCS.Services
{
    public class SummarisedActualsRepository : ISummarisedActualsRepository
    {
        private readonly Func<ISummarisationContext> _summarisationFactory;

        public SummarisedActualsRepository(Func<ISummarisationContext> summarisationFactory)
        {
            _summarisationFactory = summarisationFactory;
        }

        public async Task<IPaginatedResult<SummarisedActualDto>> GetSummarisedActuals(string collectionType, string collectionReturnCode, int pageSize = 0, int pageNumber = 0)
        {
            IPaginatedResult<SummarisedActualDto> result;

            using (var context = _summarisationFactory())
            {
                var collectionReturnId = (await context.CollectionReturns.FirstOrDefaultAsync(x => x.CollectionType == collectionType &&
                                                                                                   x.CollectionReturnCode == collectionReturnCode))?.Id;

                var data = context.SummarisedActuals
                    .Where(x => x.CollectionReturnId == collectionReturnId)
                    .Select(x => new SummarisedActualDto
                    {
                        CollectionReturnCode = collectionReturnCode,
                        CollectionType = collectionType,
                        ActualValue = x.ActualValue,
                        ActualVolume = x.ActualVolume == 0 ? null : (int?)x.ActualVolume,
                        ContractAllocationNumber = x.ContractAllocationNumber,
                        DeliverableCode = x.DeliverableCode,
                        FundingStreamPeriodCode = x.FundingStreamPeriodCode,
                        OrganisationId = x.OrganisationId,
                        Period = x.Period,
                        PeriodTypeCode = x.PeriodTypeCode,
                        UopCode = x.UoPCode,
                        Id = x.ID,
                    })
                    .OrderBy(x => x.Id);

                result = await PaginatedResultFactory<SummarisedActualDto>.CreatePaginatedResult(data, pageSize, pageNumber);
            }

            return result;
        }

        public async Task<IPaginatedResult<CollectionReturnDto>> GetClosedCollectionEvents(
            DateTime? closedCollectionsSince = null, int pageSize = 0, int pageNumber = 0)
        {
            IPaginatedResult<CollectionReturnDto> result;

            closedCollectionsSince = closedCollectionsSince ?? new DateTime(1900, 1, 1);

            using (var context = _summarisationFactory())
            {
                var data = context.SummarisedActuals.Include(x => x.CollectionReturn)
                    .Where(x => x.CollectionReturn.DateTime > closedCollectionsSince.Value)
                    .GroupBy(y => new
                    {
                        y.CollectionReturn.DateTime, y.CollectionReturn.CollectionType,
                        y.CollectionReturn.CollectionReturnCode
                    })
                    .Select(grp => new CollectionReturnDto
                    {
                        DateTime = grp.Key.DateTime.GetValueOrDefault(),
                        CollectionReturnCode = grp.Key.CollectionReturnCode,
                        CollectionType = grp.Key.CollectionType,
                        TotalItems = grp.Count()
                    });

                result = await PaginatedResultFactory<CollectionReturnDto>.CreatePaginatedResult(data, pageSize, pageNumber);
            }

            return result;
        }
    }
}