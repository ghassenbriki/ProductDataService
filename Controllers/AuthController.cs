using Leoni.Services.Interfaces;
using Leoni.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Leoni.Dtos.AuthDto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Leoni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("SignUp")]
        public async Task<ActionResult<ObjWrapper<LoggedInDto>>> SignUp([FromBody] RegistrationDto registrationDto)
        {
            var res = new ObjWrapper<LoggedInDto>() { };
            try
            {

                res.Item = await _authService.signIN(registrationDto);
                return Ok(res);
            }

            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Errors.Add(ex.Message);
                return BadRequest(res);
            }

        }




        [HttpPost("SignIn")]
        public async Task<ActionResult<ObjWrapper<LoggedInDto>>> SignIn([FromBody] RegistrationDto registrationDto)
            {
                var res = new ObjWrapper<LoggedInDto>() { };
                try
                {


                    res.Item = await _authService.signIN(registrationDto);
                    return Ok(res);
                }

                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Errors.Add(ex.Message);
                    return BadRequest(res);
                }



            }

        [HttpPost("SignOut")]
        public async Task<ActionResult> SignOut([FromQuery] string refrehToken , [FromQuery] bool allDevices = false)
        {
     
            try
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

             

                await _authService.SignOut(refrehToken, userId, allDevices);
                return Ok(new BasicResponse{Message = "SignOut Succefully !" });


            }

              catch (Exception ex)
            {

                return BadRequest(new BasicResponse {Error = ex.Message });

            }



        }




        [HttpPost("RotateToken")]
        public async Task<ActionResult<ObjWrapper<LoggedInDto>>> RotateToken([FromQuery] string refreshToken)
        {
            var res = new ObjWrapper<LoggedInDto>() { };
            try
            {


                res.Item = await _authService.RotateTokens(refreshToken);
                return Ok(res);
            }

            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Errors.Add(ex.Message);
                return BadRequest(res);
            }



        }



    }


}
