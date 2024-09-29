using DAL.Repositories.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using DAL.DTO.Req;
using DAL.DTO.Res;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DAL.Repositories.Service;

namespace BEPeer.Controllers
{
    [Route("rest/v1/funding/[action]")]
    [ApiController]
    public class FundingController : Controller
    {
        private readonly IFundingServices _fundingServices;

        public FundingController(IFundingServices fundingService)
        {
            _fundingServices =  fundingService;
        }

        [HttpPost]
        [Authorize(Roles = "lender")]
        public async Task<IActionResult> Funding(ReqFundingApprove funding)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Message = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();

                    var errorMessage = new StringBuilder("Validation errors occured!");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }

                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;

                var res = await _fundingServices.FundingApproove(funding, userId);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "Success give funding",
                    Data = res
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

    }
}



