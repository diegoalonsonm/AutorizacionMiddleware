using Autorizacion.Abstracciones.BW;
using Autorizacion.Abstracciones.Modelos;
using Microsoft.AspNetCore.Builder;
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
            httpContext.User.AddIdentity(appIdentity);

            await _next(httpContext);
        }

        private async Task<ClaimsIdentity> verificarAutorizacion(HttpContext httpContext)
        {
            List<Claim> claims = new List<Claim>();

            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                await ObtenerUsuario(httpContext, claims);
                await ObtenerPerfiles(httpContext, claims);
            }

            ClaimsIdentity appIdentity = new ClaimsIdentity(claims);

            return appIdentity;
        }

        private async Task ObtenerPerfiles(HttpContext httpContext, List<Claim> claims)
        {
            IEnumerable<Perfil> perfiles = await ObtenerInformacionPerfiles(httpContext);

            if (perfiles != null && perfiles.Any())
            {
                foreach(Perfil perfil in perfiles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, perfil.IdPerfil.ToString()));
                }
            }
        }

        private async Task<IEnumerable<Perfil>> ObtenerInformacionPerfiles(HttpContext httpContext)
        {
            return await _autorizacionBW.ObtenerPerfilesPorUsuario(new Usuario
            {
                Nombre = httpContext.User.Claims.FirstOrDefault(c => c.Type == "usuario")?.Value
            });
        }

        private async Task ObtenerUsuario(HttpContext httpContext, List<Claim> claims)
        {
            Usuario usuario = await ObtenerInformacionUsuario(httpContext); 

            if(usuario != null && !string.IsNullOrEmpty(usuario.IdUsuario.ToString()) && !string.IsNullOrEmpty(usuario.Nombre.ToString()) 
                && !string.IsNullOrEmpty(usuario.Correo.ToString()))
            {
                claims.Add(new Claim(ClaimTypes.Email, usuario.Correo));
                claims.Add(new Claim(ClaimTypes.Name, usuario.Nombre));
                claims.Add(new Claim("idUsuario", usuario.IdUsuario.ToString()));
            }
        }

        private async Task<Usuario> ObtenerInformacionUsuario(HttpContext httpContext)
        {
            return await _autorizacionBW.ObtenerUsuario(new Usuario {
                Nombre = httpContext.User.Claims.FirstOrDefault(c => c.Type == "usuario")?.Value
            });
        }
    }
}