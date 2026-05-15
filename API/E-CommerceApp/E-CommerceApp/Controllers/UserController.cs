using E_CommerceApp.Domain;
using E_CommerceApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static E_CommerceApp.Domain.DTOs.UserDTO;

namespace E_CommerceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        } 

        [HttpGet]
        public async Task<List<User>> getallusers()
        {
            var users = await _userService.GetAllUsers();
            return users;
        }

        [HttpPost("/createuser")]
        public async Task<IActionResult> CreateUser(CreateUser user)
        {
            try
            {
                var id = await _userService.CreateUser(user);
                return Ok(new { userId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/getuserdata/{id}")]
        public async Task<IActionResult> Getuserdata(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("/updatedetails/{id}")]
        public async Task<IActionResult> UpdateUserData(int id, UpdateUser user)
        {
            try
            {
                var updatedUser = await _userService.UpdateUser(id, user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("/deleteuser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok("User Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(Loggedin user)
        {
            try
            {
                var result = await _userService.Login(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/updatepassword/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, UpdatePassword updatePassword)
        {
            try
            {
                return Ok(await _userService.UpdatePassword(id, updatePassword));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
