using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UrFU_WorkSpace.enums;

namespace UrFU_WorkSpace.Helpers;

public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(params AuthorizationStatus[] claims) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { claims };
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        readonly RedirectToRouteResult DefualtToRouteResult = new RedirectToRouteResult(new RouteValueDictionary
        {
            { "controller", "Home" },   
            { "action", "" }
        });
        readonly AuthorizationStatus[] _claim;

        public AuthorizeFilter(params AuthorizationStatus[] claims)
        {
            _claim = claims;
        }
        
        private static int? GetIdUser(HttpContext context)
        {
            var routeValues = context.GetRouteData().Values;
        
            if (!routeValues.TryGetValue("idUser", out var idUserValue)) 
                return null;
        
            if (int.TryParse(idUserValue.ToString(), out var idUser))
            {
                return idUser;
            }
            return null;
        }
        
        private static bool IsCorrectUserId(string token,AuthorizationFilterContext context)
        {
            var correctId = GetIdUser(context.HttpContext);
            var currentId = int.Parse(JwtTokenDecoder.GetUserId(token));

            return correctId == null || currentId == correctId;
        }
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Session.GetString("JwtToken");
            if (token == null)
            {
                context.Result = DefualtToRouteResult;
            }
            var accessLevel = JwtTokenDecoder.GetUserAccessLevel(token) ;
            var status = (AuthorizationStatus)Enum.ToObject(typeof(AuthorizationStatus), accessLevel);
            if (_claim.All(x => x != status) || !IsCorrectUserId(token, context))
            {
                context.Result = DefualtToRouteResult;
            };
        }
        
    }