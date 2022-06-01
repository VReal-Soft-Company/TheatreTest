using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTheare.DAL.Filters;
using TestTheare.Shared.Data.Pagination;
using TestTheatre.BLL.Services.Base;
using TestTheatre.DAL.Context;
using TestTheatre.DAL.Entities;

namespace TestTheatre.BLL.Services
{
    public interface IShowService : IBaseService<Show>
    {
        Task<PagedResponse<Show>> GetAsync(PageInfo pageInfo, ShowFilter filter);
    }
    public class ShowService : BaseService<Show>, IShowService
    {
        public ShowService(ApplicationDataContext contex) : base(contex)
        {
        }

        public async Task<PagedResponse<Show>> GetAsync(PageInfo pageInfo, ShowFilter filter)
        {
            var query = GetAll();
            if (filter.DateTime != null)
            {
                query = query.Where(f => f.BeginDate.Date == filter.DateTime.Value.Date);
            }
            if (!String.IsNullOrEmpty(filter.Name))
            {
                var filterName = filter.Name.ToLower();
                query = query.Where(f => f.Name.ToLower().Contains(filterName));
            }
            var totalCount = await query.CountAsync();

            if (pageInfo == null)
            {
                pageInfo = new PageInfo()
                {
                    PageLength = totalCount,
                    PageNumber = 1
                };
            }

            pageInfo.TotalRecords = totalCount;
            var result = new PagedResponse<Show>()
            {
                PageInfo = pageInfo, 
            };
            if (pageInfo.TotalRecords > 0 && pageInfo.PageLength.HasValue && pageInfo.PageNumber.HasValue)
            {
                result.Records= await query
                    .Skip((pageInfo.PageNumber.Value - 1) * pageInfo.PageLength.Value)
                    .Take(pageInfo.PageLength.Value)
                    .ToListAsync();
            }
            else
            {
                result.Records = await query.ToListAsync();
            }
             

            return result; 
        }
    }
}
