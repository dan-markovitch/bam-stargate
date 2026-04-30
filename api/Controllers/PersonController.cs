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
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly StargateContext _context;

        public PersonController(IMediator mediator, StargateContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var result = await _mediator.Send(new GetPeople());
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = "GetPeople succeeded", IsSuccess = true });
                await _context.SaveChangesAsync();
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"GetPeople failed: {ex.Message}", IsSuccess = false });
                await _context.SaveChangesAsync();
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName() { Name = name });
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"GetPersonByName succeeded for {name}", IsSuccess = true });
                await _context.SaveChangesAsync();
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"GetPersonByName failed for {name}: {ex.Message}", IsSuccess = false });
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
        public async Task<IActionResult> CreatePerson([FromBody] string name)
        {
            try
            {
                var result = await _mediator.Send(new CreatePerson() { Name = name });
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"CreatePerson succeeded for {name}", IsSuccess = true });
                await _context.SaveChangesAsync();
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                await _context.AstronautLogs.AddAsync(new AstronautLog { Message = $"CreatePerson failed for {name}: {ex.Message}", IsSuccess = false });
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