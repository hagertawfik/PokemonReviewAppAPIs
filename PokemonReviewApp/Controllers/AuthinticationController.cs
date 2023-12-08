using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Configuration;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokemonReviewApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    public class AuthinticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
       
        public AuthinticationController(UserManager<IdentityUser> userManager , IConfiguration configuration) 
        {
           
            _userManager = userManager;
            _configuration = configuration;
        }
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] userRegistrationRequestDto requestDto)
        {
            //validate the incoming request
            if (ModelState.IsValid)
            {
                // if email already exist
                var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);
                if (user_exist != null)
                {
                    var errorObject = new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Email already exist"
                        }

                    };
                    return BadRequest(errorObject);
                   

                }
                //creating user
                var new_user = new IdentityUser()
                {
                    Email = requestDto.Email,
                    UserName = requestDto.Email,

                };
                var Is_created = await _userManager.CreateAsync(new_user , requestDto.password);
                if (Is_created.Succeeded)
                {
                    //generate the token 
                    var token = GenerateJwtToken(new_user);
                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token

                    }) ;
                }
                return BadRequest(new AuthResult()
                {
                   Result = false,
                    Errors = new List<string>()
                        {
                            "server error"
                        }

                });



            }
            return BadRequest();
        }

        //login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] userLoginRequestDto LoginRequest)
        {
            if (ModelState.IsValid)
            {
                //if the user exsist
                var exsisting_User = await _userManager.FindByEmailAsync(LoginRequest.Email);
                if (exsisting_User == null)
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "invaild payload"
                        }

                    });

                var isCorrect= await _userManager.CheckPasswordAsync(exsisting_User, LoginRequest.Password);
                if (!isCorrect)
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "invaild credintials"
                        }

                    });
                var jwtToken = GenerateJwtToken(exsisting_User);
                return Ok(new AuthResult()
                {
                    Result = true,
                    Token = jwtToken

                });


            }

            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                        {
                            "Invaild payload"
                        }

            });

        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwtconfig:Secret").Value);
            //token discriptor
            var tokenDiscriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim( "Id" , user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub , user.Email),
                    new Claim(JwtRegisteredClaimNames.Email , user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat , DateTime.Now.ToUniversalTime().ToString()),

                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256),

            };

            var token = JwtTokenHandler.CreateToken(tokenDiscriptor);
            return JwtTokenHandler.WriteToken(token);

           
        }

       

    }
}
