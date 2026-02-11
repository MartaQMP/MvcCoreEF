using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;

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

---------------------------------------------
--INSERTAR ENFERMO (INSCRIPCION AUTOMATICA)--
---------------------------------------------
    CREATE PROCEDURE SP_INSERT_ENFERMO (@apellido nvarchar(50), @direccion nvarchar(50), @fecha_nac datetime, @genero nvarchar(50), @nss nvarchar(50))
    AS
	    DECLARE @inscripcion nvarchar(50)
	    SELECT @inscripcion = MAX(INSCRIPCION) FROM ENFERMO 
	    INSERT INTO ENFERMO VALUES (@inscripcion + 1, @apellido, @direccion, @fecha_nac, @genero, @nss)
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

        /* IMPORTANTE: ESTE ES EL METODO SI EL MODEL NO ESTA MAPEADO */
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
                while (await reader.ReadAsync())
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

        /* IMPORTANTE: ESTE ES EL METODO SI EL MODEL ESTA MAPEADO */
        public async Task<Enfermo> GetEnfermoById(string inscripcion)
        {
            /* PARA LLAMAR A UN PROCEDIMIENTO QUE CONTIENE PARAMETROS LA LLAMADA SE REALIZA 
             * MEDIANTE EL NOMBRE DEL PROCEDURE Y CADA PARAMETRO A CONTINUACION EN LA DECLARACION
             * DEL SQL: SP_PROCEDURE @PAM1, @PAM2 */
            string sql = "SP_FIND_ENFERMO @inscripcion";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            /* SI LOS DATOS QUE DEVUELVE EL PROCEDURE ESTAN MAPEADOS CON UN MODEL PODEMOS UTILIZAR EL METODO 
            * FromSqlRaw PARA RECUPERAR DIRECTAMENTE EL MODEL/S
            * NO PODEMOS CONSULTAR Y EXTRAER A LA VEZ CON LINQ, SE DEBE REALIZAR SIEMPRE EN 2 PASOS */
            var consulta = this.context.Enfermos.FromSqlRaw(sql, pamIns);
            /* DEBEMOS UTILIZAR AsEnumerable() PARA EXTRAER LOS DATOS */
            Enfermo enfermo = await consulta.AsAsyncEnumerable().FirstOrDefaultAsync();
            return enfermo;
        }

        /* ESTA ES LA MANERA CLASICA */
        public async Task DeleteEnfermoAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamIns);
                await com.Connection.OpenAsync();
                await com.ExecuteNonQueryAsync();
                await com.Connection.CloseAsync();
                com.Parameters.Clear();
            }
        }

        /* ESTA ES LA MANERA MODERNA */
        public async Task DeleteEnfermoRawAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @inscripcion";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamIns);
        }


        public async Task CreateEnfermo(string apellido, string direccion, DateTime fecha_nac, string genero, string nss)
        {
            string sql = "SP_INSERT_ENFERMO @apellido, @direccion, @fecha_nac, @genero, @nss";
            SqlParameter pamApe = new SqlParameter("@apellido", apellido);
            SqlParameter pamDir = new SqlParameter("@direccion", direccion);
            SqlParameter pamFec = new SqlParameter("@fecha_nac", fecha_nac);
            SqlParameter pamGen = new SqlParameter("@genero", genero);
            SqlParameter pamNss = new SqlParameter("@nss", nss);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamDir, pamFec, pamGen, pamNss);
        }
    }
}
