using HotelBooking.Application.Model;
using HotelBooking.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        public UtilityController()
        {

        }

        [HttpGet("getfacilitytypes")]
        public async Task<ActionResult<Result>> GetFacilityTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                 ((FacilityType[])Enum.GetValues(typeof(FacilityType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}.");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get facility types enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getstatus")]
        public async Task<ActionResult<Result>> GetStatus()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                 ((Status[])Enum.GetValues(typeof(Status))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}.");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
