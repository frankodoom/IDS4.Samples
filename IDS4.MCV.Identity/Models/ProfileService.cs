using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IDS4.AspIdentity.Models
{
    //custom profile service
    //https://damienbod.com/2017/04/14/asp-net-core-identityserver4-resource-owner-password-flow-with-custom-userrepository/

    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;
        protected readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        public ProfileService(UserManager<IdentityUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No sub claim present");
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                //Logger?.LogWarning("No user found matching subject Id: {0}", sub);
            }
            else
            {
              var claims = await getClaims(user);
              if (claims == null) throw new Exception("ClaimsFactory failed to create a principal");
                context.AddRequestedClaims(claims);
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (sub == null) throw new Exception("No subject Id claim present");
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
               // Logger?.LogWarning("No user found matching subject Id: {0}", sub);
            }
            context.IsActive = user != null;

        }

        private async Task<List<Claim>> getClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, await _userManager.GetUserIdAsync(user)),
                new Claim(JwtClaimTypes.Name, await _userManager.GetUserNameAsync(user))
            };
            if (_userManager.SupportsUserEmail)
            {
                var email = await _userManager.GetEmailAsync(user);
                if (!string.IsNullOrWhiteSpace(email))
                {
                    claims.AddRange(new[]
                    {
                     new Claim(JwtClaimTypes.Email, email),
                     new Claim(JwtClaimTypes.EmailVerified,
                     await _userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)

                    });
                }
            }

            if (_userManager.SupportsUserPhoneNumber)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    claims.AddRange(new[]
                    {
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(JwtClaimTypes.PhoneNumberVerified,
                        await _userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }


            if (_userManager.SupportsUserClaim)
            {
                claims.AddRange(await _userManager.GetClaimsAsync(user));
            }



            if (_userManager.SupportsUserRole)

            {

                var roles = await _userManager.GetRolesAsync(user);

                claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

            }
            return claims;

        }

    }
}

