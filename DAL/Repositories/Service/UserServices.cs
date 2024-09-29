using BCrypt.Net;
using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Service
{
    public class UserServices : IUserServices
    {
        private readonly PeerlandingContext _context;
        private readonly IConfiguration _configuration;

        public UserServices(PeerlandingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<ResUserDto>> GetAllUser()
        {
            return await _context.MstUsers
                .Select(user => new ResUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Balance = user.Balance,
                })
                .Where(user => user.Role != "admin")
                .ToListAsync();
        }

        //public async Task<string> Register(ReqRegisterUseerDto register)
        //{
        //    var isAnyEmail = await _context.MstUsers.SingleOrDefaultAsync(e => e.Email == register.Email);

        //    if (isAnyEmail != null)
        //    {
        //        throw new Exception("Email already used");
        //    }
        //    var newUser = new MstUser
        //    {
        //        Name = register.Name,
        //        Email = register.Email,
        //        Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
        //        Role = register.Role,
        //        Balance = register.Balance,
        //    };

        //    await _context.MstUsers.AddAsync(newUser);
        //    await _context.SaveChangesAsync();

        //    return newUser.Name;
        //}
        public async Task<string> Register(ReqRegisterUseerDto register)
        {
            // Cek apakah email sudah digunakan
            var isAnyEmail = await _context.MstUsers.SingleOrDefaultAsync(e => e.Email == register.Email);
            if (isAnyEmail != null)
            {
                throw new Exception("Email already used");
            }
            // Validasi role, hanya lender, borrower, atau admin yang diizinkan
            var validRoles = new[] { "lender", "borrower", "admin" };
            if (!validRoles.Contains(register.Role.ToLower()))
            {
                throw new Exception("Invalid role. Role must be lender, borrower, or admin.");
            }

            // Set saldo default ke 0 jika tidak disediakan
            var newUser = new MstUser
            {
                Name = register.Name,
                Email = register.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = register.Role,
                Balance = register.Balance ?? 0 // Set saldo default ke 0
            };

            // Tambahkan user baru ke database
            await _context.MstUsers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return "User registered successfully!";
        }


        public async Task<ResLoginDto> Login(ReqLoginDto reqLogin)
        {
               
                var user = await _context.MstUsers.SingleOrDefaultAsync(e => e.Email == reqLogin.Email);
                if (user == null)
                {
                    throw new Exception("Invalid email or password");
                }
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(reqLogin.Password, user.Password);
                if (!isPasswordValid)
                {
                    throw new Exception("Invalid email or password");
                }

                var token = GenerateJwtToken(user);
                var loginResponse = new ResLoginDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Balance = user.Balance,
                    Role = user.Role,
                    Token = token
                };

            return loginResponse;
        }

        private string GenerateJwtToken(MstUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));   
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
              issuer: jwtSettings["ValidIssuer"],
              audience: jwtSettings["ValidAudience"],
              claims: claims,
              expires: DateTime.Now.AddHours(2),
              signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResUserDto> UpdateUser(string id, ReqUpdateUserDto updateUser)
        {
            var user = await _context.MstUsers.FindAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Name = updateUser.Name ?? user.Name;
            user.Role = updateUser.Role ?? user.Role;
            user.Balance = updateUser.Balance;

            await _context.SaveChangesAsync();

            return new ResUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Balance = updateUser.Balance,
            };
        }

        public async Task<string> Delete(string id)
        {
            var user = await _context.MstUsers.FindAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }
         
            _context.MstUsers.Remove(user);
            await _context.SaveChangesAsync();

            return user.Name;
        }

        public async Task<ResUserDtobyId> UserList(string id)
        {
            var userList = await _context.MstUsers
                .Where(x => x.Id.Contains(id))
                  .Select(x => new ResUserDtobyId
                  {
                      UserId = x.Id,
                      Name = x.Name,
                      Balance = x.Balance,
                      Role = x.Role
                  }).FirstOrDefaultAsync();
            return userList;
        }

        public async Task<ResUpdateBalance> UpdateBalance(ReqUpdateBalance reqBalance, string id)
        {
            var user = await _context.MstUsers.SingleOrDefaultAsync(e => e.Id == id);
            if (user == null)
            {
                throw new Exception("User did not exist");
            }
            user.Balance = reqBalance.Balance;

            await _context.SaveChangesAsync();

            var res = new ResUpdateBalance
            {
                Balance = reqBalance.Balance,
            };
            return res;
        }

        public async Task<object> CreateFunding(ReqFunding funding)
        {
            var newFunding = new TrnFunding
            {
                LoanId = funding.LoanId,
                LenderId = funding.LenderId,
                Amount = funding.Amount
            };

            await _context.TrnFundings.AddAsync(newFunding);
            await _context.SaveChangesAsync();

            return newFunding;
        }

    }
}



