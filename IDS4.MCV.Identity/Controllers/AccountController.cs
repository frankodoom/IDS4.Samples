using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IDS4.AspIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace IDS4.AspIdentity.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly PersistedGrantDbContext _persistedGrantContext;
        private readonly ConfigurationDbContext _configurationContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Register> _logger;

        public AccountController(//ApplicationDbContext context,
            PersistedGrantDbContext persistedGrantContext,
            ConfigurationDbContext configurationContext, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<Register> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _persistedGrantContext = persistedGrantContext;
            _configurationContext = configurationContext;
        }

        //Register User
        [HttpPost]

        public async Task<IActionResult> Register(Register model)
        {

            //if (ModelState.IsValid)
            //{
                //var endpoint = await DiscoveryClient.GetAsync("http://localhost:61011");
                //if (endpoint.IsError)
                //{
                //    _logger.LogInformation(endpoint.Error);
                //}

                //check if client is registered on the server
                //var tokenClient = new TokenClient(endpoint.TokenEndpoint, model.Client, model.Secret);
                //var tokenResponse = await tokenClient.RequestClientCredentialsAsync(model.Scope);
                //if (!tokenResponse.IsError)
                //{
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //_configurationContext.Clients.Add(new IdentityServer4.EntityFramework.Entities.Client { ClientId = "Cloud911Api", ClientName = "Cloud911Api" });
                    //await _configurationContext.SaveChangesAsync();

                    // send SMS Verification                
                    return Json(new { status = StatusCode(201) });
                }
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
            return BadRequest();
            }
        }
    }
        
          //  return BadRequest();
        

        //Authenticate User 
    //    [HttpPost]
    //    public async Task<IActionResult> Authenticate(Login model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            //check if user exists in identity
    //            var user = await _userManager.FindByNameAsync(model.Email);
    //            //check if user and password exists in identity
    //            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
    //            {
    //                var endpoint = await DiscoveryClient.GetAsync("http://localhost:61011");
    //                if (endpoint.IsError)
    //                {
    //                    return Json(new {message = endpoint.Error });
    //                }
    //                //Grab a bearer token
    //                var tokenClient = new TokenClient(endpoint.TokenEndpoint, model.Client, model.Secret);
                    
    //                var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(model.Email, model.Password, model.Scope);
    //                if (tokenResponse.IsError)
    //                {
    //                    //return the token error message
    //                    return Json(new { message = tokenResponse.ErrorDescription }); ;
    //                }

    //                return Json(new { message = tokenResponse.IdentityToken });
    //            }
    //            else
    //            {
    //                //return error message if user does not exist
    //                return Json(new { message = "username or password is incorrect" });
    //            }
    //        }
    //        return BadRequest();
    //    }
    //}

