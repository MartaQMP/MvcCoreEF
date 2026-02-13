using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

#region VIEWS
/*
------------------------------
--VISTA EMPLEADOS SQL SERVER--
------------------------------
    CREATE VIEW V_EMPLEADOS
    AS
	    SELECT E.EMP_NO AS ID_EMPLEADO, E.APELLIDO, E.OFICIO, E.SALARIO, D.DEPT_NO AS ID_DEPARTAMENTO, D.DNOMBRE AS DEPARTAMENTO, D.LOC AS LOCALIDAD 
	    FROM EMP E INNER JOIN DEPT D ON E.DEPT_NO = D.DEPT_NO
    GO

--------------------------
--VISTA EMPLEADOS ORACLE--
--------------------------
    CREATE OR  REPLACE VIEW V_EMPLEADOS
    AS
	    SELECT E.EMP_NO AS ID_EMPLEADO, E.APELLIDO, E.OFICIO, E.SALARIO, D.DEPT_NO AS ID_DEPARTAMENTO, D.DNOMBRE AS DEPARTAMENTO, D.LOC AS LOCALIDAD 
	    FROM EMP E INNER JOIN DEPT D ON E.DEPT_NO = D.DEPT_NO

*/
#endregion
namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleado
    {
        HospitalContext context;

        public RepositoryEmpleado (HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> GetEmpleadoById(int idEmpleado)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }
    }
}
