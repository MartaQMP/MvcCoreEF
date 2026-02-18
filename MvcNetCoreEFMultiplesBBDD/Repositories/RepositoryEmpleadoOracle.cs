using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#region VIEWS
/*
--------------------------
--VISTA EMPLEADOS ORACLE--
--------------------------
    CREATE OR  REPLACE VIEW V_EMPLEADOS
    AS
	    SELECT E.EMP_NO AS ID_EMPLEADO, E.APELLIDO, E.OFICIO, E.SALARIO, D.DEPT_NO AS ID_DEPARTAMENTO, D.DNOMBRE AS DEPARTAMENTO, D.LOC AS LOCALIDAD 
	    FROM EMP E INNER JOIN DEPT D ON E.DEPT_NO = D.DEPT_NO

*/
#endregion
#region STORED PROCEDURES
/*
-------------------------------------------
--PROCEDIMIENTO Q LLAMA A LA VISTA ORACLE--
-------------------------------------------
--SI NECESITAMOS SELECT DENTRO DE UN PROCEDURE DEBEMOS DEVOLVERLO COMO PARAMETRO DE SALIDA--
    CREATE OR REPLACE PROCEDURE SP_ALL_VIEW_EMPLEADOS (p_cursor_empleados out SYS_REFCURSOR)
    AS
    BEGIN
        OPEN p_cursor_empleados for SELECT * FROM V_EMPLEADOS;
    END;

---------------------
--INSERTAR EMPLEADO--
---------------------
    CREATE OR REPLACE PROCEDURE SP_INSERT_EMPLEADO 
    (p_apellido EMP.APELLIDO%TYPE, p_oficio EMP.OFICIO%TYPE, p_dir EMP.DIR%TYPE, p_salario EMP.SALARIO%TYPE, p_comision EMP.COMISION%TYPE, p_dept DEPT.DNOMBRE%TYPE)
    AS
	    p_id EMP.EMP_NO%TYPE;
        p_dept_no EMP.DEPT_NO%TYPE;
    BEGIN
        SELECT MAX(EMP_NO) INTO p_id FROM EMP;
	    p_id := p_id + 1;
	    SELECT DEPT_NO INTO p_dept_no FROM DEPT WHERE DNOMBRE=p_dept;
	    INSERT INTO EMP VALUES(p_id, p_apellido, p_oficio, p_dir, SYSDATE(), P_salario, p_comision, p_dept_no);
        COMMIT;
    END;
*/
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
  
    public class RepositoryEmpleadoOracle : IRepositoryEmpleado
    {
        HospitalContext context;

        public RepositoryEmpleadoOracle(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            string sql = "begin ";
            sql += "SP_ALL_VIEW_EMPLEADOS (:p_cursor_empleados); ";
            sql += " end;";
            OracleParameter pamCur = new OracleParameter();
            pamCur.ParameterName = "p_cursor_empleados";
            pamCur.Value = null;
            pamCur.Direction = ParameterDirection.Output;
            //INDICAMOS EL TIPO DE ORACLE
            pamCur.OracleDbType = OracleDbType.RefCursor;
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamCur);
            return await consulta.ToListAsync();
            //return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> GetEmpleadoById(int idEmpleado)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        public async Task<int> CreateEmpleado(string apellido, string oficio, int dir, int salario, int comision, string dept)
        {
            string sql = "begin ";
            sql += " SP_INSERT_EMPLEADO (:p_apellido, :p_oficio, :p_dir, :p_salario, :p_comision, :p_dept); ";
            sql += " end;";
            OracleParameter pamApe = new OracleParameter(":p_apellido", apellido);
            OracleParameter pamOfi = new OracleParameter(":p_oficio", oficio);
            OracleParameter pamDir = new OracleParameter(":p_dir", dir);
            OracleParameter pamSal = new OracleParameter(":p_salario", salario);
            OracleParameter pamCom = new OracleParameter(":p_comision", comision);
            OracleParameter pamDept = new OracleParameter(":p_dept", dept);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSal, pamCom, pamDept);
            return 1;
        }
    }
}
