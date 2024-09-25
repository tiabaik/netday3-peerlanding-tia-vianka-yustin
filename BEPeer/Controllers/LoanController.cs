using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Service;
using DAL.Repositories.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("rest/v1/loan/[action]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanSercvices _loanServices;

        public LoanController(ILoanSercvices loanServices)
        {
            _loanServices = loanServices;
        }
        [HttpPost]
        public async Task<IActionResult> NewLoan(ReqLoanDto loan)
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

                var res = await _loanServices.CreateLoan(loan);

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

        [HttpPut("id")]

        public async Task<IActionResult> UpdateLoan(string id, ReqUpdateLoan updateLoan)
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
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = "Validation errors occurred!",
                        Data = errors
                    });
                }



                var updatedLoan = await _loanServices.UpdateLoan(id, updateLoan);

                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User updated successfully",
                    Data = updatedLoan
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> LoanList(string Status = "")
        {
            try
            {
                var users = await _loanServices.LoanList(Status);

                return Ok(new ResBaseDto<List<ResListLoadDtocs>>
                {
                    Success = true,
                    Message = "List of LOands",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });

            }
        }

    }

}


