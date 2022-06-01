using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTheare.Shared.Data.Pagination;
using TestTheatre.BLL.DTO.Shows;
using TestTheatre.DAL.Entities;

namespace TestTheatre.BLL.DTO.Automapper
{
    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
            CreateMap<Show, ShowDTO>().ReverseMap();
            CreateMap<PagedResponse<Show>, PagedResponse<ShowDTO>>().ReverseMap();
        }
    }
    public static class ShowConverter
    {
        public static Show ToEntity<T>(this T item, IMapper mapper) where T : ShowDTO
        {
            return mapper.Map<Show>(item);
        }
        public static ShowDTO ToDTO<T>(this T item, IMapper mapper) where T : Show
        {
            return mapper.Map<ShowDTO>(item);
        }
        public static PagedResponse<ShowDTO> ToDTO<T>(this PagedResponse<T> item, IMapper mapper) where T : Show
        {
            return mapper.Map<PagedResponse<ShowDTO>>(item);
        }
    }
}
