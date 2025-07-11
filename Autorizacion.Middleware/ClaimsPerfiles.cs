using Autorizacion.Abstracciones.BW;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Autorizacion.Middleware
{
    public class ClaimsPerfiles
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private IAutorizacionBW _autorizacionBW;

        public ClaimsPerfiles(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAutorizacionBW autorizacionBW)
        {
            _autorizacionBW = autorizacionBW;
            ClaimsIdentity appIdentity = await verificarAutorizacion(httpContext);
            httpContext.Response.ContentType = "application/json";
        }

        private async Task<ClaimsIdentity> verificarAutorizacion(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }
    }
}
