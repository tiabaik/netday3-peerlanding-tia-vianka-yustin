using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Internal.Postgres;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("rest/v1/user/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Register(ReqRegisterUseerDto register)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();

                    var errorMessages = new StringBuilder("Validation Errors Occured!");
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = true,
                        Message = errorMessages.ToString(),
                        Data = errors
                    });
                }
                var res = await _userServices.Register(register);

                return Ok(new ResBaseDto<string>
                {
                    Success =true,
                    Message = "User Registee",
                    Data = res
                });
            }
            catch (Exception ex)
            {
                if(ex.Message == "Email alredy used")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userServices.GetAllUser();

                return Ok(new ResBaseDto<List<ResUserDto>>
                {
                    Success = true,
                    Message = "List of users",
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

        [HttpPost]
        
        public async Task<IActionResult>Login(ReqLoginDto loginDto)
        {
            try
            {
                var res = await _userServices.Login(loginDto);

                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = res
                });
            }
            catch (Exception e)
            {
                if (e.Message == "Invalid email or password")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = e.Message,
                        Data = null
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<object>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpPut("id")]
        [Authorize(Roles = "admin")] 
        public async Task<IActionResult> UpdateUser(string id, ReqUpdateUserDto updateUser)
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


                
                var updatedUser = await _userServices.UpdateUser(id, updateUser);

                return Ok(new ResBaseDto<ResUserDto>
                {
                    Success = true,
                    Message = "User updated successfully",
                    Data = updatedUser
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

        [HttpDelete("id")]
        [Authorize(Roles = "admin")] 
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
              
                // Call the service to update the user
                var deleteUser = await _userServices.Delete(id);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "User Deleted successfully",
                    Data = deleteUser
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


