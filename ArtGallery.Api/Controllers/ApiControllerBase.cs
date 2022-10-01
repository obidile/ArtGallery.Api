using ArtGallery.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace ArtGallery.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private IConfiguration Config => HttpContext.RequestServices.GetService<IConfiguration>();

    protected (string Controller, string Action) RouteParam
    {
        get
        {
            var param = ControllerContext.RouteData.Values;
            var result = (param["controller"].ToString(), param["action"].ToString());
            return result;
        }
    }

    protected string GetAuthToken
    {
        get
        {
            string token = Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            else
            {
                token = null;
            }
            return token;
        }
    }

    //protected long GetUserId()
    //{
    //    try
    //    {
    //        long userId = 0;
    //        try
    //        {
    //            userId = long.Parse(User.Identity.GetUserId() ?? "0");
    //        }
    //        catch (Exception)
    //        {
    //        }

    //        if (userId == 0)
    //        {
    //            var token = GetAuthToken;
    //            if (string.IsNullOrEmpty(token))
    //            {
    //                throw new AuthenticationException("Not Authenticated!");
    //            }
    //            var principal = JwtExtensions.GetPrincipalFromExpiredToken(token, Config);
    //            userId = long.Parse(principal.Identity.GetUserId() ?? "0");
    //        }

    //        return userId == 0 ? throw new AuthenticationException("Invalid auth token") : userId;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex, "BaseController.GetUserId");
    //        throw;
    //    }
    //}

    //protected AuthTokenModel GetAuthUser()
    //{
    //    try
    //    {
    //        try
    //        {
    //            var user = User.Identity.GetUserData();
    //            return user;
    //        }
    //        catch (Exception)
    //        {
    //            throw new AuthenticationException("Please Login to continue.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex, "BaseController.GetAuthUser");
    //        throw;
    //    }
    //}
}

