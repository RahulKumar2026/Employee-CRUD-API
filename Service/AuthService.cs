using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Repository.Interface;
using Employee_CRUD_API.Service.Interface;
using Employee_CRUD_API.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee_CRUD_API.Service
{
    public class AuthService : IAuthService
    {
        private IUserRepository _userRepository;
        private ILogger<AuthService> _logger;
        private readonly JwtSettings _jwtSettings;
        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger, JwtSettings jwtSettings) 
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request) 
        {
            try
            {
                //Vaildate request 
                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password)) 
                {
                    throw new ArgumentException("Username and password require.");
                }
                //Get user from database
                var user = await _userRepository.GetByUsernameAsync(request.Username);

                //Checking whether user exists

                if (user == null) 
                {
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }
                //Verify Password 
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash);
                if (!isPasswordValid)
                {
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }
                //create JWT claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                //key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                //creating signins credentails 
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Calculatate expiration
                var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

                //Create JWT token
                var token = new JwtSecurityToken(issuer:_jwtSettings.Issuer, audience: _jwtSettings.Audience,claims: claims,expires: expiresAt,signingCredentials: credentials);

                //Convert token to string 
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation("User {Username} logged in successfully.",user.Username);
                return new LoginResponseDto
                {
                    Token = tokenString,
                    ExpiresAt = expiresAt
                };
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"Error while accessing repository layer. StatusCode: {StatusCode}", HTTPResponseWrapper.Constants.BadRequestCode);
                throw;
            }
        }
    }
}
