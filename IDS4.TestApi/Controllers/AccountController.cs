using IdentityModel.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IDS4.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IDS4.WebApi.Controllers
{
    //Account Controller Web Api Core
    //http://jasonwatmore.com/post/2018/06/26/aspnet-core-21-simple-api-for-authentication-registration-and-user-management

    [Route("api/[Controller]/[action]")]
    [ApiController]
    public class AccountController: Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Register> _logger;

        public AccountController(//ApplicationDbContext context,
           UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager,
           ILogger<Register> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

     [HttpGet]
     public string Test()
     {
         return   "Works";
     }
      

     [HttpPost]
     [Authorize] //client authorization
       public async Task<IActionResult> Register(Register model)
       {
            if (ModelState.IsValid)
            {
                //Option: call identity data api to create user and save to database

                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // send SMS Verification                
                    return StatusCode(201);
                }
            }     
            return BadRequest();
       }
  
    }
}
