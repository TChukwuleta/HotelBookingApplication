using HotelBooking.Application.Hotel.Commands;
using HotelBooking.Application.Hotel.Queries;
using HotelBooking.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HotelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("addhotel")]
        public async Task<ActionResult<Result>> AddHotel(CreateHotelCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to add a new hotel. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        [HttpPost("changehotelstatus")]
        public async Task<ActionResult<Result>> ChangeHotelStatus(ChangeHotelStatusCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Changing hotel status failed. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        [HttpPost("updatehotelinformation")]
        public async Task<ActionResult<Result>> UpdateHotelInformation(UpdateHotelCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Updating hotel information failed. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        [HttpGet("gethotelbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> GetHotelById(int id)
        {
            try
            {
                return await _mediator.Send(new GetHotelByIdQuery { HotelId = id });
            }
            catch (Exception ex)
            {
                return Result.Failure($"Hotel retrieval failed. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getavailablehotels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> GetAvailableHotels()
        {
            try
            {
                return await _mediator.Send(new GetAvailableHotelsQuery());
            }
            catch (Exception ex)
            {
                return Result.Failure($"Hotels retrieval failed. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getallhotels/{skip}/{take}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> GetAllOrders(int skip, int take)
        {
            try
            {
                return await _mediator.Send(new GetAllHotelsQuery { Skip = skip, Take = take});
            }
            catch (Exception ex)
            {
                return Result.Failure($"Hotels retrieval failed. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
