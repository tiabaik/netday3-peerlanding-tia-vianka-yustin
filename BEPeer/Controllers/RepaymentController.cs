using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Service;
using DAL.Repositories.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[Route("rest/v1/repayment/[action]")]
[ApiController]
public class RepaymentController : ControllerBase
{
    private readonly IRepaymentServices _repaymentService;

    public RepaymentController(IRepaymentServices repaymentService)
    {
        _repaymentService = repaymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRepayment(ReqCreateRepaymentDto pay)
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
            var res = await _repaymentService.CreateRepayment(pay);

            return Ok(new ResBaseDto<string>
            {
                Success = true,
                Message = "Success add loan data",
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

    [HttpGet]
    public async Task<IActionResult> ListHistoryLender()
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
            var response = await _repaymentService.GetRepaymentByUserId(userId);
            return Ok(new ResBaseDto<object>
            {
                Data = response,
                Success = true,
                Message = "Success retrieve list funding!"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<List<ResLoginDto>>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }
}
