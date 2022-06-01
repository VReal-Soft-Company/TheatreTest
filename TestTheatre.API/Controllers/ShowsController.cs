using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTheare.Shared.Data;
using TestTheare.Shared.Data.Pagination;
using TestTheatre.API.Data.Extensions;
using TestTheatre.BLL.DTO.Automapper;
using TestTheatre.BLL.DTO.Shows;
using TestTheatre.BLL.Services;
using TestTheatre.DAL.Entities;

namespace TestTheatre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;
        private readonly IUserShowService _userShowService;
        private readonly IMapper _mapper;

        public ShowsController(IShowService showService, IUserShowService userShowService, IMapper mapper)
        {
            _showService = showService;
            _userShowService = userShowService;
            _mapper = mapper;
        }

        [HttpPost("Buy")]
        public async Task<IActionResult> ScheduleShow([FromBody] ScheduleDTO model)
        {
            if (ModelState.IsValid)
            {
                await _userShowService.ScheduleAsync(model, HttpContext.GetCurrentUserID());
                return Ok();
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [Authorize(Roles = Role.ADMINISTRATOR)]
        public async Task<IActionResult> CreateShow([FromBody] ShowDTO model)
        {
            if (ModelState.IsValid)
            { 
                await _showService.CreateAsync(model.ToEntity(_mapper));
                return Ok();
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteShow(long id)
        {
            if (ModelState.IsValid)
            {
                await _showService.DeleteAsync(id);
                return Ok();
            }
            return BadRequest(ModelState);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetShow(string name=null, int? itemsCount = null, int? pageNumber = null, DateTime? dateTime = null)
        {
            return Ok((await _showService.GetAsync(new PageInfo(pageNumber, itemsCount), new TestTheare.DAL.Filters.ShowFilter() { Name = name, DateTime = dateTime } )).ToDTO(_mapper));
        } 
    }
}
