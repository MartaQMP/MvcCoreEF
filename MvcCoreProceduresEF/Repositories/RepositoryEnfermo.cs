using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;
using System.Data.Common;
using System.Data;

#region STORED PROCEDURES
/*
--------------------
--MOSTRAR ENFERMOS--
--------------------
    CREATE PROCEDURE SP_ALL_ENFERMOS
    AS
	    SELECT * FROM ENFERMO
    GO

---------------------
--ENCONTRAR ENFERMO--
---------------------
    CREATE PROCEDURE SP_FIND_ENFERMO (@inscripcion nvarchar(50))
    AS
	    SELECT * FROM ENFERMO WHERE INSCRIPCION = @inscripcion
    GO

--------------------
--ELIMINAR ENFERMO--
--------------------
    CREATE PROCEDURE SP_DELETE_ENFERMO (@inscripcion nvarchar(50))
    AS
	    DELETE FROM ENFERMO WHERE INSCRIPCION = @inscripcion
    GO
 */
#endregion
namespace MvcCoreProceduresEF.Repositories
{
    public class RepositoryEnfermo
    {
        EnfermoContext context;

        public RepositoryEnfermo(EnfermoContext context)
        {
            this.context = context;
        }

        public async Task<List<Enfermo>> GetEnfermosAsync()
        {
            /* NECESITAMOS UN COMMAND VAMOS A USAR UN using PARA TODO 
            *  EL COMMAND EN SU CREACION NECESITA UNA CADENA DE CONEXION (OBJETO)
            *  EL OBJETO CONNNECTION NOS LO OFRECE EF */
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_ALL_ENFERMOS";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                // DEBEMOS MAPEAR LOS DATOS MANUALMENTE
                List<Enfermo> enfermos = new List<Enfermo>();
                while(await reader.ReadAsync())
                {
                    Enfermo enf = new Enfermo
                    {
                        Inscripcion = reader["INSCRIPCION"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNac = DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString(),
                        NSS = reader["NSS"].ToString()
                    };
                    enfermos.Add(enf);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
        }
    }
}
