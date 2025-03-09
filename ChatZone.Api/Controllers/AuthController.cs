﻿using System.Security.Claims;
using ChatZone.DTO.Requests;
using ChatZone.DTO.Responses;
using ChatZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request)
    {
        var result = await authService.RegisterPersonAsync(request);

        return result.Match<IActionResult>(e => Ok("Completed!"), x => throw x);
    }
    
    [Authorize(Roles = "Unconfirmed")]
    [HttpPost]
    [Route("/confirm")]
    public async Task<RegisterResponse> Confirm()
    {
        var username = User.FindFirst(ClaimTypes.Name).Value;
        
        var result = await authService.ConfirmEmailAsync(username);

        return result.Match<RegisterResponse>(e => e, x => throw x);
    }
}