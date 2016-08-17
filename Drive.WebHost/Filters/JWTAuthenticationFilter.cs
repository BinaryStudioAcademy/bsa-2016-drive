﻿using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Drive.Identity.Entities;
using Drive.Identity.Services;
using Drive.Identity.Services.Abstract;
using Drive.WebHost.Services;
using Driver.Shared.Dto;

namespace Drive.WebHost.Filters
{
    public class JWTAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        //private readonly IUsersService _usersService;

        //public JWTAuthenticationFilter(IUsersService usersService)
        //{
        //    _usersService = usersService;
        //}

        //private void CreateUser(IPrincipal principal)
        //{
        //    _usersService.CreateAsync(new UserDto() { id = ((BSIdentity)principal.Identity).UserId });
        //}

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var mockToken = bool.Parse(ConfigurationManager.AppSettings["MockToken"]);
            var token = mockToken ? ConfigurationManager.AppSettings["TestToken"] : 
                filterContext.RequestContext.HttpContext.Request.Cookies["x-access-token"]?.Value;

            if (token != null)
            {
                var secret = ConfigurationManager.AppSettings["JWTSecret"];
                ITokenAuthenticationService authService = new JWTAuthService();
                var principal = authService.VerifyToken(token, secret);
                var checkExpiracy = bool.Parse(ConfigurationManager.AppSettings["CheckExpiracy"]);
                if (checkExpiracy && ((BSIdentity)principal.Identity).IsExpired)
                {
                    var clearCookie = new HttpCookie("x-access-token", "") { Expires = DateTime.Now.AddDays(-1) };
                    filterContext.RequestContext.HttpContext.Response.SetCookie(clearCookie);
                    return;
                }
                var idManager = new BSIdentityManager();
                idManager.SetPrincipal(principal);
                filterContext.Principal = principal;
                //CreateUser(principal);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                var needAuth = bool.Parse(ConfigurationManager.AppSettings["NeedAuth"]);
                if (!needAuth) return;
                var authServer = ConfigurationManager.AppSettings["AuthServer"];
                if (filterContext.HttpContext.Request.Url != null)
                {
                    var url = HttpUtility.UrlDecode(filterContext.HttpContext.Request.Url.ToString());
                    var myCookie = new HttpCookie("referer", url);
                    filterContext.RequestContext.HttpContext.Response.SetCookie(myCookie);
                }
                if (authServer != null) filterContext.Result = new RedirectResult(authServer);
            }
        }
    }
}