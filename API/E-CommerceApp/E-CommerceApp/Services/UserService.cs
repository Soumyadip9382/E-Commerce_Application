using Azure.Core;
using E_CommerceApp.Domain;
using E_CommerceApp.Infrastructure;
using E_CommerceApp.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static E_CommerceApp.Domain.DTOs.UserDTO;

public class UserService : IUserService
{
    private readonly AppDBContext _context;

    public UserService(AppDBContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<int> CreateUser(CreateUser user)
    {
        var email = user.Email?.Trim().ToLower();
        var phone = user.Phone?.Trim();

        if (!string.IsNullOrWhiteSpace(email))
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new Exception("Email already exists");
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (await _context.Users.AnyAsync(u => u.Phone == phone))
                throw new Exception("Phone already exists");
        }

        var textInfo = CultureInfo.CurrentCulture.TextInfo;

        var newuser = new User
        {
            FirstName = textInfo.ToTitleCase(user.FirstName),
            LastName = textInfo.ToTitleCase(user.LastName),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Phone = phone,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(newuser);
        await _context.SaveChangesAsync();

        return newuser.UserId;
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User?> UpdateUser(int id, UpdateUser user)
    {
        var existing = await _context.Users.FindAsync(id);
        if (existing == null) return null;
        var email = user.Email?.Trim().ToLower();
        var phone = user.Phone?.Trim();

        if (!string.IsNullOrWhiteSpace(email))
        {
            if (await _context.Users.AnyAsync(u => u.Email == email && u.UserId != id))
                throw new Exception("Email already exists");

            existing.Email = email;
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            if (await _context.Users.AnyAsync(u => u.Phone == phone && u.UserId != id))
                throw new Exception("Phone already exists");

            existing.Phone = phone;
        }

        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User> Login(Loggedin user)
    {
        var input = user.PhoneOrEmail.Trim();

        var existing = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == input.ToLower() ||
            u.Phone == input
        );

        if (existing == null)
            throw new Exception("User not registered");

        if (!BCrypt.Net.BCrypt.Verify(user.Password, existing.PasswordHash))
            throw new Exception("Wrong password");

        return existing;
    }

    public async Task<string> UpdatePassword(int id, UpdatePassword model)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new Exception("User not found");

        if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
            throw new Exception("Invalid current password");

        if (model.NewPassword != model.ConfirmNewPassword)
            throw new Exception("Passwords do not match");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

        await _context.SaveChangesAsync();

        return "Password updated successfully";
    }
}