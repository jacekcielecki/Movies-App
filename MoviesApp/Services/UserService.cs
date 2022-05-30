using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Exceptions;
using MoviesApi.Models;
using MoviesApp.Exceptions;
using MoviesApp.Models;
using MoviesApp.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApp.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetUsers();
        UserDto GetUserById(int userId);
        void DeleteUserById(int userId);
        int CreateUser(CreateUserDto newUserDto);
        void ChangePasswordById(int userId, ChangePasswordDto changePasswordDto);

        string UserLogin(UsernamePasswordDto usernamePasswordDto);

    }
    public class UserService : IUserService
    {
        private readonly MoviesDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        public UserService(MoviesDbContext moviesDbContext, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = moviesDbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.OrderBy(d => d.Id);
            if (users == null)
                throw new NotFoundException("Users not found");

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return userDtos;
        }

        public UserDto GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new NotFoundException("User not found");

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public void ChangePasswordById(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new NotFoundException("User not found");

            var oldPassword =  _passwordHasher.HashPassword(user, changePasswordDto.OldPassword);
            if (!(user.PasswordHash == oldPassword))
                throw new WrongPasswordException("Password is incorrect");

            user.PasswordHash = _passwordHasher.HashPassword(user, changePasswordDto.NewPassword);
            _context.SaveChanges();
        }

        public void DeleteUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new NotFoundException("User not found");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public int CreateUser(CreateUserDto createUserDto)
        {
            if (createUserDto == null)
                throw new NotFoundException("New user not given");

            var newUser = new User()
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                Joined = DateTime.Now,
                PermissionId = 1 //createUserDto.PermissionId,
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, createUserDto.Password);

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser.Id;
        }

        public string UserLogin(UsernamePasswordDto usernamePasswordDto)
        {
            var user = _context.Users
                .Include(u => u.Permission)
                .FirstOrDefault((u => u.Username == usernamePasswordDto.Username || u.Email == usernamePasswordDto.Username));
            if (user is null)
                throw new NotFoundException("This user does not exist");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, usernamePasswordDto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Inwalid username or password");

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
                new Claim("Joined", user.Joined.ToString("yyyy-MM-dd")),
                new Claim(ClaimTypes.Role, $"{user.Permission.Name}"),
                //new Claim("PermissionId", user.PermissionId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.JwtIssuer,
                audience: _authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
