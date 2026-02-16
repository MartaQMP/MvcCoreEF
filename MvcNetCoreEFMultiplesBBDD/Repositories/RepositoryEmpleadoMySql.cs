using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using MySql.Data.MySqlClient;

#region VIEWS
/*
-------------------------
--VISTA EMPLEADOS MySQL--
-------------------------
    CREATE VIEW V_EMPLEADOS
    AS
	    SELECT E.EMP_NO AS ID_EMPLEADO, E.APELLIDO, E.OFICIO, E.SALARIO, D.DEPT_NO AS ID_DEPARTAMENTO, D.DNOMBRE AS DEPARTAMENTO, D.LOC AS LOCALIDAD 
	    FROM EMP E INNER JOIN DEPT D ON E.DEPT_NO = D.DEPT_NO

*/
#endregion
#region STORED PROCEDURES
/*
----------------------------------------
--PROCEDIMIENTO Q LLAMA A LA VISTA SQL--
----------------------------------------
    DELIMITER $$
    CREATE PROCEDURE SP_ALL_VIEW_EMPLEADOS()
    BEGIN
        SELECT * FROM V_EMPLEADOS
    END $$
    DELIMITER ;

---------------------
--INSERTAR EMPLEADO--
---------------------
    DELIMITER $$
    CREATE PROCEDURE SP_INSERT_EMPLEADO(apellido nvarchar(50), oficio nvarchar(50), dir int, salario int, comision int, dept nvarchar(50))
    BEGIN
	        DECLARE id int;
	        DECLARE dept_no int;
	        SELECT id = MAX(EMP_NO) FROM EMP;
            SET id = id + 1;
	        SELECT dept_no = DEPT_NO FROM DEPT WHERE DNOMBRE=@dept;
	        INSERT INTO EMP VALUES(id, apellido, oficio, dir, NOW(), salario, comision, dept_no);
    END $$
    DELIMITER ;

*/
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadoMySql: IRepositoryEmpleado
    {
        HospitalContext context;

        public RepositoryEmpleadoMySql(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            string sql = "CALL SP_ALL_VIEW_EMPLEADOS()";
            return await this.context.Empleados.FromSqlRaw(sql).ToListAsync();
            //return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> GetEmpleadoById(int idEmpleado)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        public async Task CreateEmpleado(string apellido, string oficio, int dir, int salario, int comision, string dept)
        {
            string sql = "CALL SP_INSERT_EMPLEADO (@apellido, @oficio, @dir, @salario, @comision, @dept)";
            MySqlParameter pamApe = new MySqlParameter("@apellido", apellido);
            MySqlParameter pamOfi = new MySqlParameter("@oficio", oficio);
            MySqlParameter pamDir = new MySqlParameter("@dir", dir);
            MySqlParameter pamSal = new MySqlParameter("@salario", salario);
            MySqlParameter pamCom = new MySqlParameter("@comision", comision);
            MySqlParameter pamDept = new MySqlParameter("@dept", dept);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSal, pamCom, pamDept);
        }
    }
}
