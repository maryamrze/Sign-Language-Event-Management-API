using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using a2.Data;
using a2.Models;

namespace a2.Handler{


    public class A2AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>{
        private readonly IA2Repo _repo;
        public A2AuthHandler(IA2Repo repo, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
        {
            _repo = repo;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            if (!Request.Headers.ContainsKey("Authorization")){
                Response.Headers.Add("WWW-Authenticate", "Basic");
                return AuthenticateResult.Fail("Authorization header not found");
            } else {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
                var username = credentials[0];
                var password = credentials[1];
                if (_repo.ValidLoginUser(username, password)){
                    var claims = new[] {new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "RegisteredUser")};
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                } else  if (_repo.ValidLoginOrganiser(username, password)){ 
                    
                    var claims = new[] {new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "RegisteredOrganiser")};
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                } else {
                    return AuthenticateResult.Fail("userName and password do not match");
                }
                
            }
        }
    }
}