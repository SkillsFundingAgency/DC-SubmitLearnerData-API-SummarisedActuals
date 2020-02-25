using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.Api.Common.Identity.EF.Entities;
using ESFA.DC.Api.Common.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ESFA.DC.PublicApi.FCS.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public TokenController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var token = await _identityService.AuthenticateUser(user.UserName, user.Password);

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("invalid user name or password");
            }

            return Ok(token);
        }
    }
}