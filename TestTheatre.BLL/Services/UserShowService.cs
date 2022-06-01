using System;
using System.Linq;
using System.Threading.Tasks;
using TestTheare.Shared.Data.Exceptions;
using TestTheatre.BLL.DTO.Shows;
using TestTheatre.BLL.Services.Base;
using TestTheatre.DAL.Context;
using TestTheatre.DAL.Entities;

namespace TestTheatre.BLL.Services
{
    public interface IUserShowService 
    {
        Task ScheduleAsync(ScheduleDTO scheduleDTO, long UserId);
    }
    public class UserShowService:IUserShowService
    {
        private readonly IShowService _showService;
        private readonly ApplicationDataContext _context;
        public UserShowService(ApplicationDataContext contex, IShowService showService)  
        {
            _showService = showService;
            _context = contex;
        }

        public async Task ScheduleAsync(ScheduleDTO scheduleDTO, long UserId)
        {
            var show = await _showService.FirstOrDefaultAsync(f => f.Id == scheduleDTO.ShowId);
            if(show.TicketsCount>= scheduleDTO.CountOfTickets)
            {
                await _context.UserShows.AddAsync(new UserShows() { UserId = UserId, ShowId = scheduleDTO.ShowId, CountOfTickets = scheduleDTO.CountOfTickets, DateTime = DateTime.UtcNow  });

            }
            else
            {
                throw new AppException("ERROR. Tickets count is less, then you want to buy. ");
            }
            show.TicketsCount -= scheduleDTO.CountOfTickets;
            _context.Attach(show);
            _context.Entry(show).Property(f => f.TicketsCount).IsModified = true;
            await _context.SaveChangesAsync();
        }
    }
}
