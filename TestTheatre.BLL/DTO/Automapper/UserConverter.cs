using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using TestTheatre.BLL.DTO.Users.Basic;
using TestTheatre.BLL.Helpers;
using TestTheatre.DAL.Entities;

namespace TestTheatre.BLL.DTO.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, User>();
            CreateMap<LoginDTO, User>();
        }
    }
    public static class UserConverter
    {

        public static User ToEntity<T>(this T item, IMapper mapper) where T : BasicAuthDTO
        {
            var user = mapper.Map<User>(item);
            SetPassword(user, item.Password);
            return user;
        }
        private static void SetPassword(User user, string password)
        {
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }
    }
}
