using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return user;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                return BadRequest("Пароль обязателен");

            var exists = await _context.Users.AnyAsync(u =>
                u.Email.ToLower() == user.Email.ToLower() || u.Username.ToLower() == user.Username.ToLower());

            if (exists)
                return BadRequest("Пользователь с таким email или логином уже существует");

            // Хешируем пароль перед сохранением
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            // Установка только роли "User"
            user.Role = "User";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Role
            });
        }

        // DTO для входа
        // Позже вынести в отдельный файл
        public class LoginRequest
        {
            public string UsernameOrEmail { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UsernameOrEmail) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Логин/почта и пароль обязательны" });

            // Ищем пользователя по логину или почте
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username.ToLower() == request.UsernameOrEmail.ToLower() ||
                    u.Email.ToLower() == request.UsernameOrEmail.ToLower());

            if (user == null)
                return Unauthorized(new { message = "Пользователь не найден" });

            // Проверка пароля 
            if (!VerifyPasswordHash(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Неверный пароль" });

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Role
            });
        }

        // Позже вынести в отдельный файл
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("ID не совпадают");

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            // Обновим только нужные поля (не роль!)
            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;

            if (!string.IsNullOrWhiteSpace(updatedUser.PasswordHash))
                existingUser.PasswordHash = updatedUser.PasswordHash;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
