using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Models.Database;
using WebAPI.Misc;
using WebAPI.Models.API.Requests;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Services
{
    public interface IUserService
    {
        Task<(User, ClaimsPrincipal)?> Authenticate(string username, string password);
        Task<User> Create(string username, string password, string email, bool admin = false);
        Task<bool> Edit(int userId, EditUser data);
        Task<bool> SetCurrentQuestionnaire(int userId, int questionnaireId);
        Task<bool> ConfirmMail(int userId, string code);
        Task<User> Get(int userId);
        Task<(User, ClaimsPrincipal)?> RefreshSession(int userId, string token);
    }

    public class UserService : IUserService
    {
        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";
        public const string RolePremium = "Premium";
        public const string RolePending = "Pending";
        
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> Get(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        
        public async Task<(User, ClaimsPrincipal)?> Authenticate(string username, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);

            if (user == null) return null;
            if (user.Password != new PasswordHasher(user.Salt).HashPassword(password)) return null;

            var (session, claims) = CreateSession(user);
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            
            return (user, claims);
        }

        public async Task<(User, ClaimsPrincipal)?> RefreshSession(int userId, string token)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;
            var currentSession = await _context.Sessions
                .FirstOrDefaultAsync(s => s.Id == token && s.UserId == userId);
            if (currentSession == null) return null;
            if (currentSession.IsExpired()) return null;

            var (session, claims) = CreateSession(user);

            _context.Sessions.Remove(currentSession);
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            
            return (user, claims);
        }

        private (Session, ClaimsPrincipal) CreateSession(User user)
        {
            var token = GenerateToken();
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, token),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            if (!user.EmailConfirmed && user.Role != ERole.Admin)
                claims.Add(new Claim(ClaimTypes.Role, RolePending));
            
            if (user.Role == ERole.Admin)
                claims.Add(new Claim(ClaimTypes.Role, RoleAdmin));
            if (user.Role == ERole.Admin || user.Role == ERole.Premium)
                claims.Add(new Claim(ClaimTypes.Role, RolePremium));
            if (user.Role == ERole.Admin || user.Role == ERole.User || user.Role == ERole.Premium)
                claims.Add(new Claim(ClaimTypes.Role, RoleUser));
            
            ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            Session session = new Session(token, DateTime.Now, user);

            return (session, principal);
        }
        
        public async Task<User> Create(string username, string password, string email, bool admin = false)
        {
            if (await UserExists(username)) return null;
            
            PasswordHasher hasher = new PasswordHasher();
            User user = new User(username, hasher.HashPassword(password), email, hasher.Salt);
            if (admin) user.Role = ERole.Admin;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> Edit(int userId, EditUser data)
        {

            User user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            if (user.Password != new PasswordHasher(user.Salt).HashPassword(data.OldPassword)) return false;
            
            if (!string.IsNullOrEmpty(data.Email)) user.Email = data.Email;
            if (!string.IsNullOrEmpty(data.Username)) user.Name = data.Username;
            
            if (!string.IsNullOrEmpty(data.Password))
            {
                PasswordHasher hasher = new PasswordHasher();
                user.Password = hasher.HashPassword(data.Password);
                user.Salt = hasher.Salt;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetCurrentQuestionnaire(int userId, int questionnaireId)
        {
            User user = await _context.Users.FindAsync(userId);
            Questionnaire questionnaire = await _context.Questionnaires.FindAsync(questionnaireId);

            if (user == null) return false;
            if (questionnaire == null) return false;

            user.CurrentQuestionnaire = questionnaire;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ConfirmMail(int userId, string code)
        {
            User user = await _context.Users.FindAsync(userId);

            if (user == null) return false;
            if (string.IsNullOrEmpty(user.VerificationCode)) return false;
            if (user.VerificationCode != code) return false;

            user.VerificationCode = null;
            user.EmailConfirmed = true;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
        
        private async Task<bool> UserExists(string username)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
            return user != null;
        }

        private string GenerateToken() => KeyGenerator.Generate();

    }
}