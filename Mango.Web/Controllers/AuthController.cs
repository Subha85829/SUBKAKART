﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            ResponseDTO responseDTO = await _authService.LoginAsync(obj);

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                LoginResponseDTO loginResponseDTO = 
                    JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(responseDTO.Result));


                await SignInUser(loginResponseDTO);
                _tokenProvider.SetToken(loginResponseDTO.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDTO.Message;
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new SelectListItem{Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer}
            };

            ViewBag.RoleList = rolelist;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj)
        {
            ResponseDTO result = await _authService.RegisterAsync(obj);
            ResponseDTO assignRole;

            if(result != null && result.IsSuccess) 
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = StaticDetails.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(obj);
                if(assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful!";
                    return RedirectToAction(nameof(Login));
                }                
            }
            else
            {
                TempData["error"] = result.Message;
            }
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new SelectListItem{Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer}
            };
            ViewBag.RoleList = rolelist;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }

        // Cookie to save the token 
        private async Task SignInUser(LoginResponseDTO model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
