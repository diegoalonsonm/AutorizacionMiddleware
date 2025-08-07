using Autorizacion.Abstracciones.DA;
using Autorizacion.Abstracciones.Modelos;
using Dapper;
using Helpers;
using Microsoft.Data.SqlClient;

namespace Autorizacion.DA
{
    public class SeguridadDA : ISeguridadDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _conexion;

        public SeguridadDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _conexion = _repositorioDapper.ObtenerRepositorioDapper();
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesPorUsuario(Usuario usuario)
        {
            string sqlQuery = @"[ObtenerPerfilesPorUsuario]";
            var consulta = await _conexion.QueryAsync<Perfil>(sqlQuery, new
            {
                correo = usuario.Correo,
                nombre = usuario.Nombre,
            });

            return consulta;
        }

        public async Task<Usuario> ObtenerUsuario(Usuario usuario)
        {
            string sqlQuery = @"[ObtenerUsuario]";
            var consulta = await _conexion.QueryAsync<Usuario>(sqlQuery, new
            {
                correo = usuario.Correo,
                nombre = usuario.Nombre
            });

            //return Convertidor.Convertir<Abstracciones.Entidades.Usuario, Abstracciones.Modelos.Usuario>(consulta.FirstOrDefault());
            return consulta.FirstOrDefault();
        }
    }
}