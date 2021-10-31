using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.IdentityAuth;
using Library.LibraryData;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private LibraryContext _libraryContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(LibraryContext libraryContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _libraryContext = libraryContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("register - User")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.email,
                Name = model.name,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.username
            };
            
            var result = await userManager.CreateAsync(user, model.password);

            var DbUser =  await userManager.FindByNameAsync(model.username);

            User user2 = new User()
            {
                Name = model.name,
                Id = DbUser.Id,
                Username = model.username,
                UserTypeId = 2
            };

            _libraryContext.User.Add(user2);
            _libraryContext.SaveChanges();

            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = message });
            }

            if (await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route("register - VIPUser")]
        public async Task<IActionResult> RegisterVIPUser([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.email,
                Name = model.name,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.username
            };

            var result = await userManager.CreateAsync(user, model.password);

            var DbUser = await userManager.FindByNameAsync(model.username);

            User user2 = new User()
            {
                Name = model.name,
                Id = DbUser.Id,
                UserTypeId = 3,
                Username = model.username
            };

            _libraryContext.User.Add(user2);
            _libraryContext.SaveChanges();

            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = message });
            }

            if (await roleManager.RoleExistsAsync(UserRoles.VIPUser))
            {
                await userManager.AddToRoleAsync(user, UserRoles.VIPUser);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }



        [HttpPost]
        [Route("register - admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.email,
                Name = model.name,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.username
            };
            var result = await userManager.CreateAsync(user, model.password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if (!await roleManager.RoleExistsAsync(UserRoles.VIPUser))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.VIPUser));
            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }


    }
}