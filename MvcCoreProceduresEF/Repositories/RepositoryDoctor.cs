using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreEF.Models;
using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;
using System.Data;
using System.Data.Common;

#region STORED PROCEDURES
/*
--------------------------
--MOSTRAR ESPECIALIDADES--
--------------------------
    CREATE PROCEDURE SP_MOSTRAR_ESPECIALIDADES
    AS
	    SELECT DISTINCT ESPECIALIDAD FROM DOCTOR
    GO

---------------------------------------
--ACTUALIZAR SALARIO POR ESPECIALIDAD--
---------------------------------------
    CREATE PROCEDURE SP_UPDATE_SALARIO_POR_ESPECIALIDAD (@especialidad nvarchar(50), @salario int)
    AS
	    UPDATE DOCTOR SET SALARIO=(SALARIO + @salario) WHERE ESPECIALIDAD=@especialidad
    GO

---------------------------------
--MOSTRAR DOCTORES ESPECIALIDAD--
---------------------------------
    CREATE PROCEDURE SP_DOCTORES_ESPECIALIDAD (@especialidad nvarchar(50))
    AS
	    SELECT * FROM DOCTOR WHERE ESPECIALIDAD=@especialidad
    GO
*/
#endregion

namespace MvcCoreProceduresEF.Repositories
{
    public class RepositoryDoctor
    {
        EnfermoContext context;

        public RepositoryDoctor(EnfermoContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> GetEspecialidadesAsync()
        {
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_MOSTRAR_ESPECIALIDADES";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                // DEBEMOS MAPEAR LOS DATOS MANUALMENTE
                List<string> especialidades = new List<string>();
                while (await reader.ReadAsync())
                {
                    especialidades.Add(reader["ESPECIALIDAD"].ToString());
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return especialidades;
            }
        }

        public async Task<List<Doctor>> GetDoctoresEspecialidadAsync(string especialidad)
        {
            string sql = "SP_DOCTORES_ESPECIALIDAD @especialidad";
            SqlParameter pamIns = new SqlParameter("@especialidad", especialidad);
            var consulta = this.context.Doctores.FromSqlRaw(sql, pamIns);
            List<Doctor> doctores = await consulta.AsAsyncEnumerable().ToListAsync();
            return doctores;
        }

        public async Task UpdateSalarioProcedureAsync(string especialidad, int salario)
        {
            string sql = "SP_UPDATE_SALARIO_POR_ESPECIALIDAD @especialidad, @salario";
            SqlParameter pamEsp = new SqlParameter("@especialidad", especialidad);
            SqlParameter pamSal = new SqlParameter("@salario", salario);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamEsp, pamSal);
        }

        public async Task<List<Doctor>> FindDoctorByEspecialidadAsync(string especialidad)
        {
            var consulta = from datos in this.context.Doctores
                           where datos.Especialidad == especialidad
                           select datos;
            return await consulta.ToListAsync();
        }


        public async Task UpdateSalarioAsync(string especialidad, int salario)
        {
            List<Doctor> doctores = await this.FindDoctorByEspecialidadAsync(especialidad);
            foreach (Doctor doc in doctores)
            {
                doc.Salario = doc.Salario + salario;
            }
            await this.context.SaveChangesAsync();
        }
    }
}
