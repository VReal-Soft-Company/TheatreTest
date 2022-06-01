using System;
using System.Threading.Tasks;
using TestTheare.Shared.Data;
using TestTheatre.BLL.DTO;
using TestTheatre.BLL.Services;

namespace TestTheatre.API.Seeders
{
    public static class DatabaseSeed
    {
        public static async Task SeedAsync(IShowService showService, IUserService userService)
        {
            if (!await userService.AnyAsync())
            {
                await userService.RegisterAsync(new RegisterDTO() { Email = "userTest1@gmail.com", Password = "qwerty1234"}, Role.ADMINISTRATOR);
                await userService.RegisterAsync(new RegisterDTO() { Email = "userTest2@gmail.com", Password = "qwerty1234" });
            }
            if (!await showService.AnyAsync())
            {
                var today = DateTime.UtcNow;
                await showService.CreateAsync(new DAL.Entities.Show()
                {
                    Name = "Theatre 1",
                    BeginDate = today,
                    EndDate = today.AddHours(2),
                    TicketsCount = 1
                }); ;
                var tomorrow = today.AddDays(1);  
                await showService.CreateAsync(new DAL.Entities.Show()
                {
                    Name = "Theatre 2",
                    BeginDate = tomorrow,
                    EndDate = tomorrow.AddHours(2),
                    TicketsCount = 10000
                });
            }
        }
    }
}
