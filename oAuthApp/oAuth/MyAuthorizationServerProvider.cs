using Microsoft.Owin.Security.OAuth;
using oAuthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace oAuthApp.oAuth
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {


            //string UserName;
            //string Password;
            //context.TryGetFormCredentials(out UserName, out Password);
            //if (!string.IsNullOrEmpty(UserName))
            //{
            //    context.Validated(UserName);
            //}
            //else
            //{
            //    context.Validated();
            //}
            //return base.ValidateClientAuthentication(context);
            context.Validated(); // 
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            User user = new User();
            using (PracticeEntities _dbContext = new PracticeEntities())
            {
                user =  _dbContext.Users.Where(x => x.UserName == context.UserName && x.PassWord == context.Password).FirstOrDefault();

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if(user.Role=="Admin")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim("username", "admin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Vinod Pal"));
                context.Validated(identity);
            }
            else if (user.Role == "User")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("username", "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Vivek Kuamr"));
                context.Validated(identity);
            }
                                                              
        }
    }
}