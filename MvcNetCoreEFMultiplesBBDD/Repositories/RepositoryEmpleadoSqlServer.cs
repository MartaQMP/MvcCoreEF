using Microsoft.Data.SqlClient;
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

*/
#endregion
#region STORED PROCEDURES
/*
------------------------------------
--PROCEDIMIENTO Q LLAMA A LA VISTA--
------------------------------------
    CREATE PROCEDURE SP_ALL_VIEW_EMPLEADOS
    AS
	    SELECT * FROM V_EMPLEADOS
    GO

---------------------
--INSERTAR EMPLEADO--
---------------------
    CREATE PROCEDURE SP_INSERT_EMPLEADO(@apellido nvarchar(50), @oficio nvarchar(50), @dir int, @salario int, @comision int, @dept nvarchar(50))
    AS
	    DECLARE @id int
	    SELECT @id = MAX(EMP_NO) FROM EMP
	    SET @id = @id + 1
	    DECLARE @dept_no int
	    SELECT @dept_no = DEPT_NO FROM DEPT WHERE DNOMBRE=@dept
	    INSERT INTO EMP VALUES(@id, @apellido, @oficio, @dir, GETDATE(), @salario, @comision, @dept_no)
    GO

*/
#endregion
namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadoSqlServer: IRepositoryEmpleado
    {
        HospitalContext context;

        public RepositoryEmpleadoSqlServer (HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            string sql = "SP_ALL_VIEW_EMPLEADOS";
            return await this.context.Empleados.FromSqlRaw(sql).ToListAsync();
            //return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> GetEmpleadoById(int idEmpleado)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        public async Task CreateEmpleado(string apellido, string oficio, int dir, int salario, int comision, string dept)
        {
            string sql = "SP_INSERT_EMPLEADO @apellido, @oficio, @dir, @salario, @comision, @dept";
            SqlParameter pamApe = new SqlParameter("@apellido", apellido);
            SqlParameter pamOfi = new SqlParameter("@oficio", oficio);
            SqlParameter pamDir = new SqlParameter("@dir", dir);
            SqlParameter pamSal = new SqlParameter("@salario", salario);
            SqlParameter pamCom = new SqlParameter("@comision", comision);
            SqlParameter pamDept = new SqlParameter("@dept", dept);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSal, pamCom, pamDept);
        }
    }
}
