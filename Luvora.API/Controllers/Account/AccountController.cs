using System.Security.Claims;
using Luvora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luvora.API.Controllers.Account
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var deleted = await _accountService.DeleteAccountAsync(userId);

            if (!deleted)
                return NotFound("Account not found");

            return Ok("Account deleted successfully");
        }
    }
}
