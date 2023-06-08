using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiOperadores.Data;
using apiOperadores.Models;
using System.Collections;
using Microsoft.Data.SqlClient;

namespace apiOperadores.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly AztecaGeneralContext _context;
        string QueryUser = "";
        string QueryOperadoresStatus = "";

        public StatusController(AztecaGeneralContext context)
        {
            _context = context;
            QueryUser = "SELECT  u.id " +
                           ", u.nombre " +
                           ", u.apellido_paterno " +
                           ", u.apellido_materno " +
                           ", u.tipo AS 'tipoUsuario' " +
                           ", p.alias AS 'PerfilAlias' " +
                           ", u.correo_electronico " +
                           ", dp.nombre AS 'PuestoNombre' " +
                           ", d.nombre AS 'DepartamentoNombre' " +
                           ", d.color AS 'DepartamentoColor' " +
                           ", u.sis_status " +
                           ", u.status_sesion " +
                           ", p.sis_status AS 'PerfilStatus' " +
                           ", CONVERT(bit,'1') AS 'tipoPermiso' " +
                     "FROM  sistema.usuarios AS u " +
                           "INNER JOIN sistema.perfiles AS p  ON  p.id = u.fk_perfil_id " +
                           "INNER JOIN sistema.departamento_puestos AS dp ON u.fk_puesto_id = dp.id " +
                           "INNER JOIN sistema.departamentos AS d ON dp.fk_departamento_id = d.id " +
                     "WHERE u.id = ";

            QueryOperadoresStatus = "SELECT OperadoresStatus.id " +
                                         ", OperadoresStatus.tipo" +
                                         ", OperadoresStatus.nombre" +
                                         ", OperadoresStatus.descripcio" +
                                         ", OperadoresStatus.valor_extra" +
                                         ", OperadoresStatus.fecha_registro" +
                                         ", OperadoresStatus.fk_usuario_registro" +
                                         ", OperadoresStatus.sis_status" +
                                         ", OperadoresStatus.valor_extra2" +
                                         ", UsuarioRegistro.nombre AS 'UsuarioRegistroNombre'" +
                                         ", UsuarioRegistro.apellido_paterno AS 'UsuarioRegistroApellidoPaterno'" +
                                         ", UsuarioRegistro.apellido_materno AS 'UsuarioRegistroApellidoMaterno'" +
                                         ", UsuarioRegistroPerfil.alias AS 'UsuarioRegistroPerfilAlias'" +
                                         ", UsuarioRegistroDepartamento.color AS 'UsuarioRegistroDepartamentoColor'" +
                                     " FROM modulo.operadores_status AS OperadoresStatus" +
                                            " INNER JOIN sistema.usuarios AS UsuarioRegistro ON OperadoresStatus.fk_usuario_registro = UsuarioRegistro.id" +
                                            " INNER JOIN sistema.perfiles AS UsuarioRegistroPerfil ON UsuarioRegistro.fk_perfil_id = UsuarioRegistroPerfil.id" +
                                            " INNER JOIN sistema.departamento_puestos AS UsuarioRegistroPuesto ON UsuarioRegistro.fk_puesto_id = UsuarioRegistroPuesto.id" +
                                            " INNER JOIN sistema.departamentos AS UsuarioRegistroDepartamento ON UsuarioRegistroPuesto.fk_departamento_id = UsuarioRegistroDepartamento.id ";
        }

        // GET: /Status/Consultar/idUsuario/ordenarPor/paginaActual/registrosVisibles/
        [HttpGet("Consultar/{idUsuario}/{ordenarPor}/{paginaActual}/{registrosVisibles}")]
        public Result GetOperadoresStatuses(string idUsuario, string ordenarPor, int paginaActual, int registrosVisibles, OperadoresStatus operadoresStatus)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "con-G-500";

            var Permission = PermissionValidation(idUsuario, "Registrar");
            if (Permission != "ok")
            {
                result.message += Permission;
            }
            else
            {
                try
                {
                    string WHERE = " WHERE ";
                    if ( operadoresStatus.Id != null && operadoresStatus.Id != 0 )  WHERE += " OperadoresStatus.id = " + operadoresStatus.Id + " AND ";
                    if ( operadoresStatus.Tipo != null ) WHERE += "OperadoresStatus.tipo LIKE '%" + operadoresStatus.Tipo + "%' AND ";
                    if ( operadoresStatus.Nombre != null ) WHERE += "OperadoresStatus.nombre LIKE '%" + operadoresStatus.Nombre + "%' AND ";
                    if ( operadoresStatus.Descripcio != null ) WHERE += "OperadoresStatus.descripcio LIKE '%" + operadoresStatus.Descripcio + "%' AND ";
                    if ( operadoresStatus.UsuarioRegistroNombre != null ) WHERE += "(UsuarioRegistro.nombre LIKE '%" + operadoresStatus.UsuarioRegistroNombre + "%' OR UsuarioRegistro.apellido_paterno LIKE '%" + operadoresStatus.UsuarioRegistroNombre + "%' OR UsuarioRegistro.apellido_materno LIKE '%" + operadoresStatus.UsuarioRegistroNombre + "%') AND ";
                    if ( operadoresStatus.SisStatus != null ) WHERE += "OperadoresStatus.sis_status = " + Convert.ToInt16(operadoresStatus.SisStatus) + " AND ";

                    var TotalRegistros = _context.OperadoresStatuses.FromSqlRaw(QueryOperadoresStatus + (WHERE.Length > 10 ? WHERE[..^5] : " ")).ToList().Count();

                    string PaginationQueryOperadoresStatus = " ORDER BY OperadoresStatus.id " + (ordenarPor == "ultimos_registrados" ? " DESC " : " ASC ") + ", OperadoresStatus.tipo " +
                                                " OFFSET(" + (paginaActual * registrosVisibles) + ") ROWS " +
                                                " FETCH NEXT( " + (registrosVisibles != 0 ? registrosVisibles : 20) + " )  ROWS ONLY";

                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    var TotalOperadoresStatus = _context.OperadoresStatuses.FromSqlRaw(QueryOperadoresStatus + (WHERE.Length > 10 ? WHERE[..^5] : " ") + PaginationQueryOperadoresStatus).ToList();
                    result.status = true;
                    result.message = (TotalRegistros > 0 ? "Consulta exitosa" : "No se encontraron registros");
                    result.code = (TotalRegistros > 0 ? "con-G-200" : "con-G-404");
                    result.data = TotalOperadoresStatus;
                    result.total = TotalRegistros;

                }
                catch (Exception ex)
                {
                    result.message = "  <--->  (  " + ex.ToString() + "  )";
                } 
            }

            return result;
        }

        // PUT: /Status/Editar/idUsuario
        [HttpPut("Editar/{idUsuario}")]
        public Result PutOperadoresStatus(string idUsuario, OperadoresStatus operadoresStatus)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a este proceso, si cree que es un error, comuníquese con soporte.";
            result.code = "act-500";

            // ----- Variables para comprobar variables si el estado ya existe ----- //
            OperadoresStatus ExistNewStatus = new OperadoresStatus();
            ExistNewStatus.Tipo = operadoresStatus.Tipo;
            ExistNewStatus.Nombre = operadoresStatus.Nombre;

            var Permission = PermissionValidation(idUsuario, "Editar");
            if (Permission != "ok")
            {
                result.message += Permission;
            }
            else if (operadoresStatus.Id == null || operadoresStatus.Id == 0)
            {
                result.message = "El registro que desea editar no se encuentra en el sistema.";
            }
            else
            {
                bool ValidExistStatus = false;
                
                if(operadoresStatus.Tipo != null && operadoresStatus.Nombre != null)
                {
                    Result ExistStatus = GetOperadoresStatuses(idUsuario, "ultimos_registrados", 0, 20, ExistNewStatus);
                    if (ExistStatus.total != 0)
                    {
                        IList<OperadoresStatus> StatusList = ExistStatus.data as IList<OperadoresStatus>;
                        ValidExistStatus = (StatusList.FirstOrDefault(s => s.Tipo.Trim() == operadoresStatus.Tipo.Trim() && s.Nombre.Trim() == operadoresStatus.Nombre.Trim() && s.Id != operadoresStatus.Id) != null);
                    }

                    ExistNewStatus.Tipo = null;
                    ExistNewStatus.Nombre = null;
                }
                
                ExistNewStatus.Id = operadoresStatus.Id;

                if (GetOperadoresStatuses(idUsuario, "ultimos_registrados", 0, 20, ExistNewStatus).total == 0)
                {
                    result.message = "El registro que desea editar no se encuentra en el sistema.";
                }
                else if (ValidExistStatus)
                {
                    result.message = "Ya existe un registro con la misma información.";
                }
                else
                {
                    try
                    {
                        string QueryUpdatetStatus = "UPDATE modulo.operadores_status SET ";
                                                           
                        if (operadoresStatus.Tipo != null ) QueryUpdatetStatus += (QueryUpdatetStatus.Length > 37 ? ", " : " ") + "tipo = '" + operadoresStatus.Tipo.Trim() + "' ";
                        if (operadoresStatus.Nombre != null ) QueryUpdatetStatus += (QueryUpdatetStatus.Length > 37 ? ", " : " ") + "nombre = '" + operadoresStatus.Nombre.Trim() + "' ";
                        if ( operadoresStatus.Descripcio != null ) QueryUpdatetStatus += (QueryUpdatetStatus.Length > 37 ? ", " : " ") + "descripcio = '" + operadoresStatus.Descripcio.Trim() + "' ";
                        if ( operadoresStatus.ValorExtra != null ) QueryUpdatetStatus += (QueryUpdatetStatus.Length > 37 ? ", " : " ") + "valor_extra = '" + operadoresStatus.ValorExtra.Trim() + "' ";
                        if ( operadoresStatus.ValorExtra2 != null ) QueryUpdatetStatus += (QueryUpdatetStatus.Length > 37 ? ", " : " ") + "valor_extra2 = '" + operadoresStatus.ValorExtra2.Trim() + "' ";
                        if (operadoresStatus.SisStatus != null) QueryUpdatetStatus += (QueryUpdatetStatus.Length > 37 ? ", " : " ") + "sis_status = " + Convert.ToInt16(operadoresStatus.SisStatus) + " ";

                        QueryUpdatetStatus += " WHERE id = " + operadoresStatus.Id + "; ";
                        string Query = (QueryUpdatetStatus + " " + (QueryOperadoresStatus + " WHERE OperadoresStatus.id = " + operadoresStatus.Id));

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var UpdateOperadoresStatus = _context.OperadoresStatuses.FromSqlRaw(Query).ToList();
                        result.status = true;
                        result.message = "Actualización exitosa.";
                        result.code = "act-200";
                        result.data = UpdateOperadoresStatus[0];
                        result.total = UpdateOperadoresStatus.Count();

                    }
                    catch (Exception ex)
                    {
                        result.message += "  <--->  (  " + ex.ToString() + "  )";
                    }
                }
            }

            return result;
        }

        // POST: /Status/Registrar/idUsuario
        [HttpPost("Registrar/{idUsuario}")]
        public Result PostOperadoresStatus(string idUsuario, OperadoresStatus operadoresStatus)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a este proceso, si cree que es un error, comuníquese con soporte.";
            result.code = "reg-500";
            string Id = Encrypt.base64Decode(idUsuario.Substring(0, idUsuario.Length - 3));

            // ----- Variables para comprobar variables si el estado ya existe ----- //
            OperadoresStatus ExistNewStatus = new OperadoresStatus();
            ExistNewStatus.Tipo = operadoresStatus.Tipo;
            ExistNewStatus.Nombre = operadoresStatus.Nombre;

            var Permission = PermissionValidation(idUsuario, "Registrar");
            if (Permission != "ok")
            {
                result.message += Permission;
            }
            else if (operadoresStatus.Tipo == null || operadoresStatus.Nombre == null)
            {
                result.message = "Los campos Tipo y Nombre son obligatorios.";
            }
            else
            {
                bool ValidExistStatus = false;
                Result ExistStatus = GetOperadoresStatuses(idUsuario, "ultimos_registrados", 0, 5, ExistNewStatus);
                if (ExistStatus.total != 0){
                    IList<OperadoresStatus> StatusList = ExistStatus.data as IList<OperadoresStatus>;

                    ValidExistStatus = (StatusList.FirstOrDefault(s => s.Tipo.Trim() == operadoresStatus.Tipo.Trim() && s.Nombre.Trim() == operadoresStatus.Nombre.Trim()) != null);
                }

                if (ValidExistStatus)
                {
                    result.message = "Ya existe un registro con la misma información.";
                }
                else
                {
                    try
                    {

                        string QueryInsertStatus = "DECLARE @idStatus INT = (SELECT MAX(id) FROM modulo.operadores_status);" +
                                                    " SET @idStatus = (CASE WHEN @idStatus is NULL THEN 0 " +
                                                                           " WHEN @idStatus = 1 THEN 1 " +
                                                                      " ELSE @idStatus END); " +
                                                      " DBCC CHECKIDENT('modulo.operadores_status', RESEED, @idStatus); " +
                                                    " INSERT INTO modulo.operadores_status ( " +
                                                            " tipo " +
                                                            " , nombre ";

                        string QueryValuesStatus = " ) VALUES ( " +
                                            "'" + operadoresStatus.Tipo.Trim() + "'" + // tipo
                                            ", '" + operadoresStatus.Nombre.Trim() + "'"; // nombre

                        if (operadoresStatus.Descripcio != null)
                        {
                            QueryInsertStatus += " , descripcio ";
                            QueryValuesStatus += " , '" + operadoresStatus.Descripcio.Trim() + "'";
                        }

                        if (operadoresStatus.ValorExtra != null)
                        {
                            QueryInsertStatus += " , valor_extra ";
                            QueryValuesStatus += ", '" + operadoresStatus.ValorExtra.Trim() + "'";
                        }

                        if (operadoresStatus.ValorExtra2 != null)
                        {
                            QueryInsertStatus += " , valor_extra ";
                            QueryValuesStatus += ", '" + operadoresStatus.ValorExtra2.Trim() + "'";
                        }

                        QueryInsertStatus += " , fk_usuario_registro ";
                        QueryValuesStatus += " , " + Id + " )";

                        string Query = (QueryInsertStatus + QueryValuesStatus) + " " + (QueryOperadoresStatus + " WHERE OperadoresStatus.id = (SELECT MAX(id) FROM modulo.operadores_status)");

                        var InsertOperadoresStatus = _context.OperadoresStatuses.FromSqlRaw(Query).ToList();
                        result.status = true;
                        result.message = "Registro exitoso";
                        result.code = "reg-200";
                        result.data = InsertOperadoresStatus[0];
                        result.total = InsertOperadoresStatus.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = "  <--->  (  " + ex.ToString() + "  )";
                    }
                }                
            }
            
            return result;
        }

        // DELETE: /Status/Eliminar/idUsuario
        [HttpDelete("Eliminar/{idUsuario}")]
        public Result DeleteOperadoresStatus(string idUsuario, OperadoresStatus opStatus)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "con-G-500";

            var Permission = PermissionValidation(idUsuario, "Eliminar");
            if (Permission != "ok")
            {
                result.message += Permission;
            }
            else if (opStatus.Id == null)
            {
                result.message = "El identificador es necesario para poder eliminarlo.";
            }
            else
            {
                var operadoresStatus = _context.OperadoresStatuses.FromSqlRaw(QueryOperadoresStatus + "WHERE OperadoresStatus.id = " + (opStatus.Id ?? 0)).ToList();
                if (operadoresStatus == null || operadoresStatus.Count() == 0)
                {
                    result.status = true;
                    result.message = "Se ha eliminado con éxito.";
                    result.code = "eli-200";
                }
                else
                {
                    try
                    {
                        _context.OperadoresStatuses.Remove(operadoresStatus[0]);
                        _context.SaveChangesAsync();

                        result.status = true;
                        result.message = "Se ha eliminado con éxito.";
                        result.code = "eli-200";
                    }
                    catch (Exception ex)
                    {
                        result.message = "  <--->  (  " + ex.ToString() + "  )";
                    }
                }
            }

            return result;
        }
        private string PermissionValidation(string idUsuario, string TypeQuery)
        {
            string Result = "ok";
            string Id = Encrypt.base64Decode(idUsuario.Substring(0, idUsuario.Length - 3));
            if (string.IsNullOrEmpty(Id))
            {
                Result = "  <--->  No se pudo descifrar el usuario.";
            }
            else
            {
                var usuario = _context.Usuarios.FromSqlRaw(QueryUser + int.Parse(Id)).ToList();

                if (usuario == null || usuario.Count == 0)
                {
                    Result = "  <--->  Usuario no existe en la base de datos.";
                }
                else if (usuario[0].SisStatus == false || usuario[0].PerfilStatus == false || usuario[0].StatusSesion == false || usuario[0].tipoPermiso == null)
                {
                    Result = "  <--->  {'usuarioSisStatus': true, 'perfil_Status': true, 'StatusSesion': " + usuario[0].StatusSesion + " 'tipoPermiso': " + usuario[0].tipoPermiso + "}";
                }
            }

            return Result;
        }
    }
}
