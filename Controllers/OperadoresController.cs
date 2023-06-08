using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiOperadores.Data;
using apiOperadores.Models;

namespace apiOperadores.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OperadoresController : ControllerBase
    {
        private readonly AztecaGeneralContext _context;
        string QueryUser = "";
        string QueryOperadores = "";

        public OperadoresController(AztecaGeneralContext context)
        {
            _context = context;

           
            QueryOperadores = @"SELECT Operadores.id 
									 , Operadores.fk_sucursal_id 
									 , Sucursal.nombre AS 'Sucursal' 
									 , Operadores.fk_puesto_id 
									 , Puesto.nombre AS 'Puesto'
									 , Operadores.fk_perfil_id 
									 , Perfil.nombre AS 'Perfil' 
									 , Operadores.fk_tipo_id 
									 , TipoEmpleado.nombre AS 'TipoEmpleado' 
									 , Operadores.numero_empleado 
									 , Operadores.contrasena 
									 , Operadores.nombre 
									 , Operadores.apellido_paterno 
									 , Operadores.apellido_materno 
									 , Operadores.foto_perfil 
									 , Operadores.telefono_celular 
									 , Operadores.correo_electronico 
									 , Operadores.rfc 
									 , Operadores.curp 
									 , Operadores.nss 
									 , Operadores.direccion 
									 , Operadores.fk_tipo_contrato_id 
									 , TipoContrato.nombre AS 'TipoContrato' 
									 , Operadores.genero 
									 , Operadores.afore 
									 , Operadores.fk_estado_civil_id 
									 , EstadoCivil.nombre AS 'EstadoCivil' 
									 , Operadores.kms_acumulados 
									 , Operadores.fk_experiencia_id 
									 , ExperienciaLaboral.nombre AS 'ExperienciaLaboral'
									 , Operadores.fk_nivel_ecolaridad_id 
									 , NivelEstudio.nombre AS 'NivelEstudio' 
									 , Operadores.nombre_conyuge 
									 , Operadores.total_hijos 
									 , Operadores.telefono_emergencias 
									 , Operadores.fecha_baja 
									 , Operadores.fk_causa_baja_id 
									 , CausaBaja.nombre AS 'CausaBaja' 
									 , Operadores.status_sesion 
									 , Operadores.fecha_inicio_sesion 
									 , Operadores.dispositivo_inicio_sesion 
									 , Operadores.dispositivo_notificaciones
									 , Operadores.fk_estado_operador_id 
									 , EstadoEmpleado.nombre AS 'EstadoEmpleado' 
									 , Operadores.fk_usuario_registro 
									 , Operadores.fecha_registro 
									 , Operadores.sis_status 
									 , UsuarioRegistro.nombre AS 'UsuarioRegistroNombre' 
									 , UsuarioRegistro.apellido_paterno AS 'UsuarioRegistroApellidoPaterno' 
									 , UsuarioRegistro.apellido_materno AS 'UsuarioRegistroApellidoMaterno' 
									 , UsuarioRegistroPerfil.alias AS 'UsuarioRegistroPerfilAlias' 
									 , UsuarioRegistroDepartamento.color AS 'UsuarioRegistroDepartamentoColor' 
								 FROM modulo.operadores AS Operadores 
										 INNER JOIN modulo.operadores_status AS TipoEmpleado ON TipoEmpleado.id = Operadores.fk_tipo_id AND TipoEmpleado.tipo = 'TipoEmpleado' 
										 INNER JOIN modulo.operadores_status AS TipoContrato ON TipoContrato.id = Operadores.fk_tipo_contrato_id AND TipoContrato.tipo = 'TipoContrato' 
										 INNER JOIN modulo.operadores_status AS EstadoCivil ON EstadoCivil.id = Operadores.fk_estado_civil_id AND EstadoCivil.tipo = 'EstadoCivil' 
										 INNER JOIN modulo.operadores_status AS ExperienciaLaboral ON ExperienciaLaboral.id = Operadores.fk_experiencia_id AND ExperienciaLaboral.tipo = 'ExperienciaLaboral' 
										 INNER JOIN modulo.operadores_status AS NivelEstudio ON NivelEstudio.id = Operadores.fk_nivel_ecolaridad_id AND NivelEstudio.tipo = 'NivelEstudio' 
										 INNER JOIN modulo.operadores_status AS CausaBaja ON CausaBaja.id = Operadores.fk_causa_baja_id AND CausaBaja.tipo = 'CausaBaja' 
										 INNER JOIN modulo.operadores_status AS EstadoEmpleado ON EstadoEmpleado.id = Operadores.fk_estado_operador_id AND EstadoEmpleado.tipo = 'EstadoEmpleado' 
										 INNER JOIN sistema.sucursal AS Sucursal ON Sucursal.id = Operadores.fk_sucursal_id 
										 INNER JOIN sistema.departamento_puestos AS Puesto ON Puesto.id = Operadores.fk_puesto_id 
										 INNER JOIN sistema.perfiles AS Perfil ON Perfil.id = Operadores.fk_perfil_id 
										 INNER JOIN sistema.usuarios AS UsuarioRegistro ON Operadores.fk_usuario_registro = UsuarioRegistro.id 
										 INNER JOIN sistema.perfiles AS UsuarioRegistroPerfil ON UsuarioRegistro.fk_perfil_id = UsuarioRegistroPerfil.id 
										 INNER JOIN sistema.departamento_puestos AS UsuarioRegistroPuesto ON UsuarioRegistro.fk_puesto_id = UsuarioRegistroPuesto.id 
										 INNER JOIN sistema.departamentos AS UsuarioRegistroDepartamento ON UsuarioRegistroPuesto.fk_departamento_id = UsuarioRegistroDepartamento.id";
        }

        // GET: /Operadores/Consultar/idUsuario/ordenarPor/paginaActual/registrosVisibles/
        [HttpPost("Consultar/{idUsuario}/{ordenarPor}/{paginaActual}/{registrosVisibles}")]
		public Result GetOperadores(string idUsuario, string ordenarPor, int paginaActual, int registrosVisibles, Operadore operador)
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

					if ( operador.Id != null && operador.Id != 0 ) WHERE += "Operadores.id = " + operador.Id + " AND ";
					if ( operador.Sucursal != null ) WHERE += "Sucursal.nombre LIKE '%" + operador.Sucursal + "%' AND ";
					if ( operador.Puesto != null ) WHERE += "Puesto.nombre LIKE '%" + operador.Puesto + "%' AND ";
					if ( operador.Perfil != null ) WHERE += "Perfil.nombre LIKE '%" + operador.Perfil + "%' AND ";
					if ( operador.TipoEmpleado != null ) WHERE += "TipoEmpleado.nombre LIKE '%" + operador.Perfil + "%' AND ";
					if ( operador.NumeroEmpleado != null ) WHERE += "Operadores.numero_empleado = " + operador.NumeroEmpleado + " AND ";
					if ( operador.Nombre != null ) WHERE += "Operadores.nombre LIKE '%" + operador.NumeroEmpleado + "%' AND ";
					if ( operador.ApellidoPaterno != null ) WHERE += "Operadores.apellido_paterno LIKE '%" + operador.ApellidoPaterno + "%' AND ";
					if ( operador.ApellidoMaterno != null ) WHERE += "Operadores.apellido_materno LIKE '%" + operador.ApellidoMaterno + "%' AND ";
					if ( operador.TipoContrato != null ) WHERE += "TipoContrato.nombre LIKE '%" + operador.TipoContrato + "%' AND ";
					if ( operador.Genero != null) WHERE += "Operadores.genero = '" + operador.Genero + "' AND ";
					if ( operador.EstadoCivil != null ) WHERE += "EstadoCivil.nombre LIKE '%" + operador.EstadoCivil + "%' AND ";
					if ( operador.ExperienciaLaboral != null ) WHERE += "ExperienciaLaboral.nombre LIKE '%" + operador.ExperienciaLaboral + "%' AND ";
					if ( operador.NivelEstudio != null ) WHERE += "NivelEstudio.nombre LIKE '%" + operador.NivelEstudio + "%' AND ";
					if ( operador.CausaBaja != null ) WHERE += "CausaBaja.nombre LIKE '%" + operador.CausaBaja + "%' AND ";
					if ( operador.EstadoEmpleado != null ) WHERE += "EstadoEmpleado.nombre LIKE '%" + operador.EstadoEmpleado + "%' AND ";

					_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
					var TotalRegistros = _context.Operadores.FromSqlRaw(QueryOperadores + (WHERE.Length > 10 ? WHERE[..^5] : " ")).ToList().Count();

					string PaginationQuery = " ORDER BY Operadores.id " + (ordenarPor == "ultimos_registrados" ? " DESC " : " ASC ") +
											 " OFFSET(" + (paginaActual * registrosVisibles) + ") ROWS " +
											 " FETCH NEXT( " + (registrosVisibles != 0 ? registrosVisibles : 20) + " )  ROWS ONLY";
					
					var TotalOperadores = _context.Operadores.FromSqlRaw(QueryOperadores + (WHERE.Length > 10 ? WHERE[..^5] : " ") + PaginationQuery).ToList();
					
					result.status = true;
					result.message = (TotalRegistros > 0 ? "Consulta exitosa" : "No se encontraron registros");
					result.code = (TotalRegistros > 0 ? "con-G-200" : "con-G-404");
					result.data = TotalOperadores;
					result.total = TotalRegistros;
				}
				catch (Exception ex)
                {
					result.message = "  <--->  (  " + ex.ToString() + "  )";
				}
			}


			return result;
		}

		// POST: /Operadores/Registrar/idUsuario
		[HttpPost("Registrar/{idUsuario}")]
		public Result PostOperadores(string idUsuario, Operadore operador)
        {
			// ----- Variables generales ----- //
			Result result = new Result();
			result.status = false;
			result.message = "Lo sentimos, pero su usuario no tiene acceso a este proceso, si cree que es un error, comuníquese con soporte.";
			result.code = "reg-500";
			string Id = Encrypt.base64Decode(idUsuario.Substring(0, idUsuario.Length - 3));

			// ----- Variables para comprobar si el Operador ya existe ----- //
			Operadore ExistNewOperador = new Operadore();
			ExistNewOperador.NumeroEmpleado = operador.NumeroEmpleado;

			var Permission = PermissionValidation(idUsuario, "Registrar");
			if (Permission != "ok")
			{
				result.message += Permission;
            }
            else
            {
				Result ExistOperador = GetOperadores(idUsuario, "ultimos_registrados", 0, 5, ExistNewOperador);

                if ((ExistOperador.total > 0))
                {
					result.message = "Ya existe un registro con el mismo numero de empleado.";
                }
                else
                {
					if (!(operador.NumeroEmpleado != null  
						&& operador.Nombre != null
						&& operador.ApellidoPaterno != null
						&& operador.ApellidoMaterno != null
						&& operador.FkSucursalId != null
						&& operador.Genero != null
						&& operador.FkEstadoOperadorId != null))
                    {
						result.message = "No se pudo realizar el registro por falta de información obligatoria (Sucursal, Numero de empleado, Nombre y apellidos, Genero, Estado del operador).";
                    }
                    else
                    {
						try
						{
							string QueryInsert = "DECLARE @id INT = (SELECT MAX(id) FROM modulo.operadores);" +
													" SET @id = (CASE WHEN @id is NULL THEN 0 " +
																		   " WHEN @id = 1 THEN 1 " +
																	  " ELSE @id END); " +
													  " DBCC CHECKIDENT('modulo.operadores', RESEED, @id); " +
												 " INSERT INTO modulo.operadores ( " +
														" numero_empleado " +
														" , nombre " +
														" , apellido_paterno " +
														" , apellido_materno " +
														" , fk_sucursal_id " +
														" , genero ";

							string QueryInsertValues = " ) VALUES ( " +
															operador.NumeroEmpleado +
															", '" + operador.Nombre.Trim() + "' " +
															", '" + operador.ApellidoPaterno.Trim() + "' " +
															", '" + operador.ApellidoMaterno.Trim() + "' " +
															", " + operador.FkSucursalId +
															", '" + operador.Genero.Trim() + "'";

							QueryInsert += ", fk_puesto_id ";
							QueryInsertValues += ", " + (operador.FkPuestoId != null ? operador.FkPuestoId : 1);      
;
							QueryInsert += ", fk_perfil_id ";
							QueryInsertValues += ", " + (operador.FkPerfilId != null ? operador.FkPerfilId : 59);

							QueryInsert += ", fk_tipo_id ";
							QueryInsertValues += ", " + (operador.FkTipoId != null ? operador.FkTipoId : "(SELECT TOP 1 id FROM modulo.operadores_status WHERE tipo = 'TipoEmpleado' AND valor_extra = 'Default') ");

							QueryInsert += ", fk_tipo_contrato_id ";
							QueryInsertValues += ", " + (operador.FkTipoContratoId != null ? operador.FkTipoContratoId : "(SELECT TOP 1 id FROM modulo.operadores_status WHERE tipo = 'TipoContrato' AND valor_extra = 'Default') ");

							QueryInsert += ", fk_estado_civil_id ";
							QueryInsertValues += ", " + (operador.FkEstadoCivilId != null ? operador.FkEstadoCivilId : "(SELECT TOP 1 id FROM modulo.operadores_status WHERE tipo = 'EstadoCivil' AND valor_extra = 'Default') ");

							QueryInsert += ", fk_experiencia_id ";
							QueryInsertValues += ", " + (operador.FkExperienciaId != null ? operador.FkExperienciaId : "(SELECT TOP 1 id FROM modulo.operadores_status WHERE tipo = 'ExperienciaLaboral' AND valor_extra = 'Default') ");

							QueryInsert += ", fk_nivel_ecolaridad_id ";
							QueryInsertValues += ", " + (operador.FkNivelEcolaridadId != null ? operador.FkNivelEcolaridadId : "(SELECT TOP 1 id FROM modulo.operadores_status WHERE tipo = 'NivelEstudio' AND valor_extra = 'Default') ");

							QueryInsert += ", fk_causa_baja_id ";
							QueryInsertValues += ", " + (operador.FkCausaBajaId != null ? operador.FkCausaBajaId : "(SELECT TOP 1 id FROM modulo.operadores_status WHERE tipo = 'CausaBaja' AND valor_extra = 'Default') ");

							QueryInsert += ", fk_estado_operador_id ";
							QueryInsertValues += ", " + (operador.FkEstadoOperadorId != null ? operador.FkEstadoOperadorId : "(SELECT TOP 1 id FROM modulo.operadores_status WHERE tipo = 'EstadoEmpleado' AND valor_extra = 'Default') ");

							if (operador.Contrasena != null)
							{
								QueryInsert += ", contrasena ";
								QueryInsertValues += ", '" + Encrypt.GetMD5(operador.Contrasena.Trim()) + "'";
							}
							if (operador.TelefonoCelular != null)
							{
								QueryInsert += ", telefono_celular ";
								QueryInsertValues += ", '" + operador.TelefonoCelular.Trim() + "'";
							}
                            if (operador.CorreoElectronico != null)
                            {
								QueryInsert += ", correo_electronico ";
								QueryInsertValues += ", '" + operador.CorreoElectronico.Trim() + "'";
							}
							if (operador.Rfc != null)
							{
								QueryInsert += ", rfc ";
								QueryInsertValues += ", '" + operador.Rfc.Trim() + "'";
							}
                            if (operador.Curp != null)
                            {
								QueryInsert += ", rfc ";
								QueryInsertValues += ", '" + operador.Curp.Trim() + "'";
							}
							if (operador.Nss != null)
							{
								QueryInsert += ", rfc ";
								QueryInsertValues += ", " + operador.Nss;
							}
							if (operador.Direccion != null)
							{
								QueryInsert += ", direccion ";
								QueryInsertValues += ", '" + operador.Direccion.Trim() + "'";
							}
							if (operador.Afore != null)
                            {
								QueryInsert += ", afore ";
								QueryInsertValues += ", '" + operador.Afore.Trim() + "'";
							}
							if (operador.KmsAcumulados != null)
							{
								QueryInsert += ", kms_acumulados ";
								QueryInsertValues += ", " + operador.KmsAcumulados;
							}
							if (operador.NombreConyuge != null)
							{
								QueryInsert += ", nombre_conyuge ";
								QueryInsertValues += ", '" + operador.NombreConyuge.Trim() + "'";
							}
                            if (operador.TotalHijos != null)
                            {
								QueryInsert += ", total_hijos ";
								QueryInsertValues += ", " + operador.TotalHijos;
							}
							if (operador.TelefonoEmergencias != null)
                            {
								QueryInsert += ", telefono_emergencias ";
								QueryInsertValues += ", '" + operador.TelefonoEmergencias.Trim() + "'";
							}
							if (operador.FechaBaja != null)
                            {
								QueryInsert += ", fecha_baja ";
								QueryInsertValues += ", GETDATE()";
							}
                            if (operador.StatusSesion != null)
                            {
								DateTime Today = DateTime.Now;
								QueryInsert += ", status_sesion,  fecha_inicio_sesion ";
								QueryInsertValues += ", " + Convert.ToInt16(operador.StatusSesion) + ", '" + Today + "'";

							}
                            if (operador.DispositivoInicioSesion != null)
                            {
								QueryInsert += ", dispositivo_inicio_sesion ";
								QueryInsertValues += ", '" + operador.DispositivoInicioSesion + "'";
							}
                            if (operador.DispositivoNotificaciones != null)
                            {
								QueryInsert += ", dispositivo_notificaciones ";
								QueryInsertValues += ", '" + operador.DispositivoNotificaciones + "'";
							}
                            if (operador.SisStatus != null)
                            {
								QueryInsert += ", sis_status ";
								QueryInsertValues += ", " + Convert.ToInt16(operador.SisStatus);
							}

							QueryInsert += ", fk_usuario_registro, fecha_registro ";
							QueryInsertValues += ", " + int.Parse(Id) + ", GETDATE() )";

							string Query = (QueryInsert + QueryInsertValues) + " " + (QueryOperadores + " WHERE Operadores.id = (SELECT MAX(id) FROM modulo.operadores)");
							
							_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
							var Insert = _context.Operadores.FromSqlRaw(Query).ToList();

							result.status = true;
							result.message = "Registro exitoso.";
							result.code = "reg-200";

							if (Insert.Count == 0)
                            {
								result.status = false;
								result.message = "Problema al registrar la información, si este problema persiste comunícate con soporte.";
								result.code = "reg-500";
								result.total = 0;
							}
							else
                            {
								result.data = Insert[0];
								result.total = Insert.Count();
							}
						}
						catch (Exception ex)
						{
							result.message = "Error al registrar información.  <--->  (  " + ex.ToString() + "  )";
						}
					}
				}
			}

			return result;
		}

		// PUT: /Operadores/Editar/idUsuario
		[HttpPost("Editar/{idUsuario}")]
		public Result PutOperadoresStatus(string idUsuario, Operadore operador)
        {
			// ----- Variables generales ----- //
			Result result = new Result();
			result.status = false;
			result.message = "Lo sentimos, pero su usuario no tiene acceso a este proceso, si cree que es un error, comuníquese con soporte.";
			result.code = "act-500";
			string Id = Encrypt.base64Decode(idUsuario.Substring(0, idUsuario.Length - 3));
			
			Operadore ExistEditOperador = new Operadore();
			ExistEditOperador.Id = operador.Id;

			var Permission = PermissionValidation(idUsuario, "Editar");
			if (Permission != "ok")
			{
				result.message += Permission;
            }
            else if(operador.Id == null)
            {
				result.message = "El identificador del operador es necesario para poder editarlo.";
			}
            else if(GetOperadores(idUsuario, "ultimos_registrados", 0, 1, ExistEditOperador).total == 0)
            {
				result.message = "El registro que desea editar no se encuentra en el sistema.";
            }
			else
            {
				result = UpdateOperador(operador);
			}

			return result;
		}

		// DELETE: /Operadores/Eliminar/idUsuario
		[HttpPost("Eliminar/{idUsuario}")]
		public Result DeleteOperadores(string idUsuario, Operadore operador)
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
			else if (operador.Id == null)
            {
				result.message = "El identificador del operador es necesario para poder eliminarlo.";
			}
			else
			{
				var ExistOperador = _context.Operadores.FromSqlRaw(QueryOperadores + " WHERE Operadores.id = " + (operador.Id ?? 0)).ToList();
				if (ExistOperador == null || ExistOperador.Count() == 0)
				{
					result.status = true;
					result.message = "Se ha eliminado con éxito.";
					result.code = "eli-200";
				}
				else
				{
					try
					{
						_context.Operadores.Remove(ExistOperador[0]);
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


		public Result UpdateOperador(Operadore operador)
        {
			Result result = new Result();
			result.status = false;
			result.message = "Lo sentimos, pero su usuario no tiene acceso a este proceso, si cree que es un error, comuníquese con soporte.";
			result.code = "act-500";

			try
			{
				string QueryUpdate = "UPDATE modulo.operadores SET ";

				if (operador.FkSucursalId != null) QueryUpdate += " fk_sucursal_id = " + operador.FkSucursalId;
				if (operador.FkPuestoId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_puesto_id = " + operador.FkPuestoId;
				if (operador.FkPerfilId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_perfil_id = " + operador.FkPerfilId;
				if (operador.FkTipoId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_tipo_id = " + operador.FkTipoId;
				if (operador.NumeroEmpleado != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "numero_empleado = " + operador.NumeroEmpleado;
				if (operador.Contrasena != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "contrasena = '" + Encrypt.GetMD5(operador.Contrasena.Trim()) + "'";
				if (operador.Nombre != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "nombre = '" + operador.Nombre.Trim() + "'";
				if (operador.ApellidoPaterno != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "apellido_paterno = '" + operador.ApellidoPaterno.Trim() + "'";
				if (operador.ApellidoMaterno != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "apellido_materno = '" + operador.ApellidoMaterno.Trim() + "'";
				if (operador.FotoPerfil != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "foto_perfil = '" + operador.FotoPerfil.Trim() + "'";
				if (operador.TelefonoCelular != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "telefono_celular = '" + operador.TelefonoCelular.Trim() + "'";
				if (operador.CorreoElectronico != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "correo_electronico = '" + operador.CorreoElectronico.Trim() + "'";
				if (operador.Rfc != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "rfc = '" + operador.Rfc.Trim() + "'";
				if (operador.Curp != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "curp = '" + operador.Curp.Trim() + "'";
				if (operador.Nss != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "nss = " + operador.Nss;
				if (operador.Direccion != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "direccion = '" + operador.Direccion.Trim() + "'";
				if (operador.FkTipoContratoId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_tipo_contrato_id = " + operador.FkTipoContratoId;
				if (operador.Genero != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "genero = '" + operador.Genero.Trim() + "'";
				if (operador.Afore != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "afore = '" + operador.Afore.Trim() + "'";
				if (operador.FkEstadoCivilId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_estado_civil_id = " + operador.FkEstadoCivilId;
				if (operador.KmsAcumulados != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "kms_acumulados = " + operador.KmsAcumulados;
				if (operador.FkExperienciaId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_experiencia_id = " + operador.FkExperienciaId;
				if (operador.FkNivelEcolaridadId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_nivel_ecolaridad_id = " + operador.FkNivelEcolaridadId;
				if (operador.NombreConyuge != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "nombre_conyuge = '" + operador.NombreConyuge.Trim() + "'";
				if (operador.TotalHijos != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "total_hijos = " + operador.TotalHijos;
				if (operador.TelefonoEmergencias != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "telefono_emergencias = '" + operador.TelefonoEmergencias.Trim() + "'";
				if (operador.FkCausaBajaId != null) {
					QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_causa_baja_id = " + operador.FkCausaBajaId;
					if (operador.FkCausaBajaId == 16) {
						QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fecha_baja = GETDATE()";
					}
				}
				if (operador.StatusSesion != null)
				{
					QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "status_sesion = " + Convert.ToInt32(operador.StatusSesion);

                    if (operador.StatusSesion == true)
                    {
						QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fecha_inicio_sesion = GETDATE()";
                    }
                    else
                    {
						QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fecha_inicio_sesion = null";
					}

				}
				if (operador.DispositivoInicioSesion != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "dispositivo_inicio_sesion = '" + operador.DispositivoInicioSesion.Trim() + "'";
				if (operador.DispositivoNotificaciones != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "dispositivo_notificaciones = '" + operador.DispositivoNotificaciones.Trim() + "'";
				if (operador.FkEstadoOperadorId != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "fk_estado_operador_id = " + operador.FkEstadoOperadorId;
				if (operador.SisStatus != null) QueryUpdate += (QueryUpdate.Length > 30 ? ", " : " ") + "sis_status = " + Convert.ToInt32(operador.SisStatus);

				QueryUpdate += " WHERE id = " + operador.Id + "; " + (QueryOperadores + " WHERE Operadores.id = " + operador.Id);
				_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
				var Update = _context.Operadores.FromSqlRaw(QueryUpdate).ToList();

				result.status = true;
				result.message = "Actualización exitosa.";
				result.code = "act-200";
				result.data = Update[0];
				result.total = Update.Count();
			}
			catch (Exception ex)
			{
				result.message += "  <--->  (  " + ex.ToString() + "  )";
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
				var usuario = _context.Usuarios.FromSqlRaw(Auth.autenticarUsuario(int.Parse(Id))).ToList();

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
