using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.Api.Common.Extensions;
using ESFA.DC.Api.Common.Paging.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.PublicApi.FCS.Constants;
using ESFA.DC.PublicApi.FCS.Dtos;
using ESFA.DC.PublicApi.FCS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESFA.DC.PublicApi.FCS.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = PolicyNameConstants.FCS)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/summarised-actuals")]
    [ApiController]
    public class SummarisedActualsController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISummarisedActualsRepository _summarisedActualsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummarisedActualsController"/> class.
        /// </summary>
        /// <param name="logger">instance of logger</param>
        /// <param name="appSettings">instance of appsettings</param>
        /// <param name="summarisedActualsRepository">instance of summarised actuals repository</param>
        /// <param name="mapper">instance of automapper mapper</param>
        public SummarisedActualsController(ILogger logger, ISummarisedActualsRepository summarisedActualsRepository)
        {
            _logger = logger;
            _summarisedActualsRepository = summarisedActualsRepository;
        }

        /// <summary>
        /// Get summarised actuals based on the filter critiera
        /// </summary>
        /// <param name="collectionReturnCode"></param>
        /// <param name="collectionType"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>List of SummarisedActualDto with response header named "X-pagination" for paging information containing following
        ///  int TotalItems
        ///  int PageNumber
        ///  int PageSize
        ///  int TotalPages.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("{collectionReturnCode}/{collectionType}")]
        public async Task<ActionResult<IEnumerable<SummarisedActualDto>>> Get(string collectionReturnCode, string collectionType, [FromQuery]int? pageSize = null, [FromQuery]int? pageNumber = null)
        {
            if (string.IsNullOrWhiteSpace(collectionReturnCode) ||
                string.IsNullOrWhiteSpace(collectionType))
            {
                return BadRequest();
            }

            IPaginatedResult<SummarisedActualDto> summarisedActuals = await _summarisedActualsRepository.GetSummarisedActuals(collectionType, collectionReturnCode, pageSize ?? DefaultConstants.DefaultPageSize, pageNumber ?? DefaultConstants.DefaultPageNumber);

            _logger.LogVerbose($"Exiting Get Summarised Actuals, data count : {summarisedActuals.TotalItems}");

            if (summarisedActuals?.TotalItems > 0)
            {
                Response.AddPaginationHeader(summarisedActuals);
                return Ok(summarisedActuals.List);
            }

            return NoContent();
        }

        /// <summary>
        /// Get the list of collection codes based on filter criteria
        /// </summary>
        /// <param name="closedCollectionsSince"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>List of CollectionReturnDto with response header named "X-pagination" for paging information containing following
        ///  int TotalItems
        ///  int PageNumber
        ///  int PageSize
        ///  int TotalPages.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("collections")]
        public async Task<ActionResult<IEnumerable<CollectionReturnDto>>> Get([FromQuery]DateTime? closedCollectionsSince = null, [FromQuery]int? pageSize = null, [FromQuery]int? pageNumber = null)
        {
            _logger.LogVerbose("Call made to Get Collection Returns");

            var collectionEvents = await _summarisedActualsRepository.GetClosedCollectionEvents(closedCollectionsSince, pageSize ?? DefaultConstants.DefaultPageSize, pageNumber ?? DefaultConstants.DefaultPageNumber);

            _logger.LogVerbose($"Exiting Get Collection Returns, data count : {collectionEvents.TotalItems}");

            if (collectionEvents?.TotalItems > 0)
            {
                Response.AddPaginationHeader(collectionEvents);
                return Ok(collectionEvents.List);
            }

            return NoContent();
        }
    }
}