using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationMV.API.Models.AuthenticationModels;

namespace WebApplicationMV.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        public ActionResult<string> Authenticate(
            AuthenticationDto authenticationRequestBody)
        {
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }
            var test = _configuration["Authentication:SecretKeyForTesting"];

            if (_configuration["Authentication:SecretKeyForTesting"].IsNullOrEmpty())
            {
                return NotFound("Missing security key");
            }
            var securityKey = new SymmetricSecurityKey(
                Convert.FromBase64String(_configuration["Authentication:SecretKeyForTesting"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("user_name", user.UserName));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("company", user.Company));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private CompanyUserDto ValidateUserCredentials(string userName, string password)
        {
            if (userName != "TestUserName" || password != "TestPassword")
            {
                return null;
            }

            //Dummy data for company user (it should be checked from database)
            return new CompanyUserDto(
                userName ?? "",
                "TestFirstName",
                "TestLastName",
                "TestCompany");
        }
    }
}
