using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Service.Interfaces
{
    public interface IUserServices
    {
        Task<string> Register(ReqRegisterUseerDto resgiste);
        Task<List<ResUserDto>> GetAllUser();
        Task<ResLoginDto> Login(ReqLoginDto reqLoginDto);
        Task<ResUserDto> UpdateUser(string id, ReqUpdateUserDto updateUser);
        Task<string> Delete(string id);

        Task<ResUserDtobyId> UserList(string id);
        Task<ResUpdateBalance> UpdateBalance(ReqUpdateBalance reqUpdateBalance, string id);

        Task<object> CreateFunding(ReqFunding funding);
    }
}
