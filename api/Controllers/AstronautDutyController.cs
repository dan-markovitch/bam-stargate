using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly StargateContext _context;

        public AstronautDutyController(IMediator mediator, StargateContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetAstronautDutiesByName() { Name = name });
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"GetAstronautDutiesByName succeeded for {name}", IsSuccess = true });
                await _context.SaveChangesAsync();
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"GetAstronautDutiesByName failed for {name}: {ex.Message}", IsSuccess = false });
                await _context.SaveChangesAsync();
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            try
            {
                var result = await _mediator.Send(request);
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"CreateAstronautDuty succeeded for {request.Name}", IsSuccess = true });
                await _context.SaveChangesAsync();
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"CreateAstronautDuty failed for {request.Name}: {ex.Message}", IsSuccess = false });
                await _context.SaveChangesAsync();
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }
    }
}