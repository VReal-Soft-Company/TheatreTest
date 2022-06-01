using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTheare.Shared.Data;
using TestTheare.Shared.Data.Exceptions;
using TestTheatre.BLL.DTO;
using TestTheatre.BLL.DTO.Automapper;
using TestTheatre.BLL.DTO.Users.Response;
using TestTheatre.BLL.Helpers;
using TestTheatre.BLL.Services.Base;
using TestTheatre.DAL.Context;
using TestTheatre.DAL.Entities;

namespace TestTheatre.BLL.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<UserSuccessLoginDTO> LoginAsync(LoginDTO loginDTO);
        Task<UserSuccessLoginDTO> RegisterAsync(RegisterDTO registerDTO, string role = null);
    }
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IJWTHelper _JWTHelper;
        private readonly IMapper _mapper;
        public UserService(ApplicationDataContext contex, IJWTHelper jWTHelper, IMapper mapper) : base(contex)
        {
            _JWTHelper = jWTHelper;
            _mapper = mapper;
        }

        public async Task<UserSuccessLoginDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Email == loginDTO.Email);
            if (user == null)
            {
                throw new AppException("User are not exist", 404);
            }
            if (!PasswordHelper.VerifyPasswordHash(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AppException("Wrong credentials");
            }
            return ConvertToUserSuccessDTO(user);
        }

        public async Task<UserSuccessLoginDTO> RegisterAsync(RegisterDTO registerDTO, string role = null)
        {
            if (await _context.Users.AnyAsync(f => f.Email == registerDTO.Email))
            {
                throw new AppException("User are already exist");
            }
            var user = registerDTO.ToEntity(_mapper);
            user.Role = role;
            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = Role.USER;
            }
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return ConvertToUserSuccessDTO(user);
        }
        private UserSuccessLoginDTO ConvertToUserSuccessDTO(User user)
        {
            return new UserSuccessLoginDTO() { Email = user.Email, UserId = user.Id, Token = _JWTHelper.GenerateJwtToken(user) };
        }
    }
}
