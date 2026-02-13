using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;
using System.Data;

#region VIEWS
/*
--------------------------------
--VISTA EMPLEADOS DEPARTAMENTO--
--------------------------------
    CREATE VIEW V_EMPLEADOS_DEPARTAMENTOS
    AS
	    SELECT CAST(ISNULL(ROW_NUMBER() OVER (ORDER BY E.APELLIDO), 0) AS INT) AS ID, E.APELLIDO, E.OFICIO, E.SALARIO, D.DNOMBRE AS DEPARTAMENTO, D.LOC AS LOCALIDAD 
	    FROM EMP E INNER JOIN DEPT D ON E.DEPT_NO = D.DEPT_NO
    GO

----------------------
--VISTA TRABAJADORES--
----------------------
    CREATE VIEW V_TRABAJADORES
    AS
	    SELECT EMP_NO AS IDTRABAJADOR, APELLIDO, OFICIO, SALARIO
	    FROM EMP 
	    UNION 
	    SELECT DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO
	    FROM DOCTOR
	    UNION
	    SELECT EMPLEADO_NO, APELLIDO, FUNCION, SALARIO
	    FROM PLANTILLA
    GO
*/
#endregion 
#region STORED PROCEDURES
/*
------------------------------------------------------------
--PROCEDIMIENTO PARA SABER INFORMACION DE LOS TRABAJADORES--
------------------------------------------------------------
    CREATE PROCEDURE SP_TRABAJADORES_OFICIO (@oficio nvarchar(50), @personas nvarchar(50) out, @media int out, @suma int out)
    AS
	    SELECT * FROM V_TRABAJADORES
	    WHERE OFICIO = @oficio
	    SELECT @personas = COUNT(IDTRABAJADOR), @media = AVG(SALARIO), @suma = SUM(SALARIO) FROM V_TRABAJADORES 
	    WHERE OFICIO = @oficio
    GO
*/
#endregion


namespace MvcCoreProceduresEF.Repositories
{
    public class RepositoryEmpleado
    {
        HospitalContext context;

        public RepositoryEmpleado(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<VistaEmpleadoDepartamento>> GetEmpleadoDepartamentosAsync()
        {
            return await this.context.VistaEmpleados.ToListAsync();
        }

        public async Task<TrabajadoresDatos> GetTrabajadoresDatosAsync()
        {
            // PRIMERO CON LINQ
            var consulta = from datos in this.context.Trabajadores
                           select datos;
            TrabajadoresDatos datosTrabajador = new TrabajadoresDatos
            {
                Trabajadores = await consulta.ToListAsync(),
                Personas = await consulta.CountAsync(),
                SumaSalarial = await consulta.SumAsync(z => z.Salario),
                MediaSalarial = (int) await consulta.AverageAsync(z => z.Salario)
            };
            return datosTrabajador;
        }

        public async Task<List<string>> GetOficios()
        {
            return await this.context.Trabajadores.Select(z => z.Oficio).Distinct().ToListAsync();
        }

        public async Task<TrabajadoresDatos> GetTrabajadoresDatosOficioAsync(string oficio)
        {
            /* YA QUE TENEMOS MODEL, VAMOS A LLAMARLO CON EF LA UNICA DIFERENCIA CUANDO TENEMOS PARAMETROS
             * DE SALIDA ES INDICAR LA PALABRA OUT EN LA DECLARACION DE LAS VARIABLES */
            string sql = "SP_TRABAJADORES_OFICIO @oficio, @personas out, @media out, @suma out";
            SqlParameter pamOfi = new SqlParameter("@oficio", oficio);
            SqlParameter pamPer = new SqlParameter("@personas", -1);
            pamPer.Direction = ParameterDirection.Output;
            SqlParameter pamMed = new SqlParameter("@media", -1);
            pamMed.Direction = ParameterDirection.Output;
            SqlParameter pamSum = new SqlParameter("@suma", -1);
            pamSum.Direction = ParameterDirection.Output;
            // EJECUTAMOS LA CONSULTA CON EL MODEL FromSqlRaw
            var consulta = this.context.Trabajadores.FromSqlRaw(sql, pamOfi, pamPer, pamMed, pamSum);
            TrabajadoresDatos trabajadoresDatos = new TrabajadoresDatos
            {
                Trabajadores = await consulta.ToListAsync(),
                Personas = int.Parse(pamPer.Value.ToString()),
                MediaSalarial = int.Parse(pamMed.Value.ToString()),
                SumaSalarial = int.Parse(pamSum.Value.ToString())
            };
            return trabajadoresDatos;
        }

    }
}
