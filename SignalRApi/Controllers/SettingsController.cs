using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR.DtoLayer.Dtos.SettingDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public SettingsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserInfo(string username)
        {
            var value = await _userManager.FindByNameAsync(username);
            return Ok(new ResultUserInfoDto
            {
                Name = value.Name,
                Surname = value.Surname,
                Username = value.UserName,
                Email = value.Email
            });
        }
        [HttpPut]
        public async Task<IActionResult> EditUserInfo(ResultUserInfoDto dto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.UserName = dto.Username;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, dto.Password);
            await _userManager.UpdateAsync(user);
            return Ok("İşlem başarılı");
        }
    }
}