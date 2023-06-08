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
using Microsoft.AspNetCore.Hosting;


namespace apiOperadores.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppOpAztecaController : ControllerBase
    {
        private readonly AztecaGeneralContext _context;
		private readonly IConfiguration _configuration;
        private static IWebHostEnvironment _environment;

        string QueryUser = "";
        string QueryOperadores = "";
        string QueryViajeStatus = "";
		string QueryOperadorAccesos = "";
        string QueryViajes = "";
        string QueryDocumentos = "";
        //string UrlServ = "http://187.210.224.10";
        string UrlServ = "http://192.168.25.24:80";


        public AppOpAztecaController(AztecaGeneralContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;

            QueryUser = @"SELECT  Usuario.id 
                              ,Usuario.nombre 
                              ,Usuario.apellido_paterno 
                              ,Usuario.apellido_materno 
                              ,Usuario.tipo AS 'tipoUsuario' 
                              ,Usuario.correo_electronico 
                              ,DepartamentoPuesto.nombre AS 'PuestoNombre' 
                              ,Departamento.nombre AS 'DepartamentoNombre' 
                              ,Departamento.color AS 'DepartamentoColor'
                              ,Usuario.sis_status 
                              ,Usuario.status_sesion 
                              ,Perfil.sis_status AS 'PerfilStatus' 
                              ,CONVERT(bit,'1') AS 'tipoPermiso' 
                           FROM  sistema.usuarios AS Usuario 
                              INNER JOIN sistema.perfiles AS Perfil  ON  Perfil.id = Usuario.fk_perfil_id 
                              INNER JOIN sistema.departamento_puestos AS DepartamentoPuesto ON Usuario.fk_puesto_id = DepartamentoPuesto.id 
                              INNER JOIN sistema.departamentos AS Departamento ON DepartamentoPuesto.fk_departamento_id = Departamento.id 
                        WHERE Usuario.id = ";

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

            QueryViajeStatus = @"SELECT ViajeStatus.id 
                                            , ViajeStatus.tipo
                                            , ViajeStatus.nombre
                                            , '' AS 'descripcio'
                                            , ViajeStatus.valor_extra
                                            , ViajeStatus.fecha_registro
                                            , ViajeStatus.fk_usuario_registro
                                            , ViajeStatus.sis_status
                                            , ViajeStatus.valor_extra2
                                            , UsuarioRegistro.nombre AS 'UsuarioRegistroNombre'
                                            , UsuarioRegistro.apellido_paterno AS 'UsuarioRegistroApellidoPaterno'
                                            , UsuarioRegistro.apellido_materno AS 'UsuarioRegistroApellidoMaterno'
                                            , UsuarioRegistroPerfil.alias AS 'UsuarioRegistroPerfilAlias'
                                            , UsuarioRegistroDepartamento.color AS 'UsuarioRegistroDepartamentoColor'
                                     FROM  modulo.controlViajes_status AS ViajeStatus
                                            INNER JOIN sistema.usuarios AS UsuarioRegistro ON ViajeStatus.fk_usuario_registro = UsuarioRegistro.id
                                            INNER JOIN sistema.perfiles AS UsuarioRegistroPerfil ON UsuarioRegistro.fk_perfil_id = UsuarioRegistroPerfil.id
                                            INNER JOIN sistema.departamento_puestos AS UsuarioRegistroPuesto ON UsuarioRegistro.fk_puesto_id = UsuarioRegistroPuesto.id
                                            INNER JOIN sistema.departamentos AS UsuarioRegistroDepartamento ON UsuarioRegistroPuesto.fk_departamento_id = UsuarioRegistroDepartamento.id ";

            QueryOperadorAccesos =  @" SELECT PerfilAcceso.id 
                                            , PerfilAcceso.fk_perfil_id 
                                            , Perfil.sis_status AS 'PerfilStatus' 
                                            , PerfilAcceso.fk_modulo_proceso_id 
                                            , ModuloProceso.sis_status AS 'ProcesoStatus' 
                                            , PerfilAcceso.tipo 
                                       FROM seguridad.perfil_accesos AS PerfilAcceso 
                                              LEFT JOIN sistema.perfiles AS Perfil ON PerfilAcceso.fk_perfil_id = Perfil.id 
                                              LEFT JOIN sistema.modulo_procesos AS ModuloProceso ON PerfilAcceso.fk_modulo_proceso_id = ModuloProceso.id 
                                       WHERE ModuloProceso.clave = '" + _configuration.GetValue<string>("AppiKeys:OpAzteca") + "'";

            QueryViajes = @" SELECT Viaje.id 
                                   , Viaje.numero_viaje 
                                   , Viaje.numero_liquidacion
                                   , ISNULL(Ruta.nombre, CONCAT(PlazaOrigen.nombre, ' - ', PlazaDestino.nombre)) AS 'ruta_nombre'
                                   , 'Sin remitente' AS 'cliente_remitente'
                                   , 'Sin destinatario' AS 'cliente_destinario'
                                   , Viaje.peso_toneladas
                                   , TipoArmado.nombre AS 'Tipo_armado'
                                   , Viaje.clave_unidad
                                   , Viaje.clave_remolque1
                                   , Viaje.clave_dolly 
                                   , Viaje.clave_remolque2 
                                   , Viaje.kilometraje_inicial
                                   , Viaje.kilometraje_final 
                                   , ISNULL(Camino.kilometros_distancia_manual, Viaje.kilometros_ruta_manual) AS 'kilometros_ruta_manual' 
                                   , Viaje.kilometros_recorridos 
                                   , CONVERT(varchar, Viaje.fecha_inicio, 101) AS 'fecha_inicio' 
                                   , ISNULL(CONVERT(varchar, Viaje.fecha_fin, 101), 'Sin finalizar') AS 'fecha_fin' 
                                   , Viaje.fk_estado_documentos_id 
                                   , EstadoDocumento.nombre AS 'estado_documento' 
                                   , EstadoDocumento.valor_extra2 AS 'estado_documento_color' 
                                   , (SELECT COUNT(id) 
                              FROM modulo.controlViajes_documento AS ViajeDocumentos 
                              WHERE ViajeDocumentos.fk_viaje_id = Viaje.id AND ViajeDocumentos.sis_status = 1) AS 'total_documentos' 
                                    , (SELECT COUNT(id) 
                                        FROM modulo.controlViajes_documento AS ViajeDocumentos 
                                            WHERE ViajeDocumentos.fk_viaje_id = Viaje.id AND ViajeDocumentos.sis_status = 1 AND ViajeDocumentos.fk_estado_id = 3) AS 'total_documentos_aprobados' 
                                            , Viaje.fk_estado_viaje 
                                            , EstadoViaje.nombre AS 'estado_viaje' 
                            FROM modulo.controlViajes_viajes AS Viaje 
                                    INNER JOIN modulo.controlViajes_status AS ViajeEstado ON ViajeEstado.valor_extra = Viaje.fk_estado_viaje AND ViajeEstado.tipo = 'EstadoViaje' 
                                    INNER JOIN sistema.plazas AS PlazaOrigen ON PlazaOrigen.id = Viaje.fk_plaza_origen_id 
                                    INNER JOIN sistema.plazas AS PlazaDestino ON PlazaDestino.id = Viaje.fk_plaza_destino_id 
                                    LEFT JOIN sistema.plaza_ruta_caminos AS Camino ON Camino.id = Viaje.fk_ruta_camino_id 
                                    LEFT JOIN sistema.plaza_rutas AS Ruta ON Ruta.id = Camino.fk_plaza_ruta_id 
                                    INNER JOIN modulo.controlViajes_status AS TipoArmado ON TipoArmado.valor_extra = Viaje.fk_tipo_armado_id AND TipoArmado.tipo = 'TipoArmado' 
                                    LEFT JOIN modulo.controlViajes_status AS EstadoDocumento ON EstadoDocumento.valor_extra = Viaje.fk_estado_documentos_id AND EstadoDocumento.tipo = 'EstadoDocumento' 
                                    INNER JOIN modulo.controlViajes_status AS EstadoViaje ON EstadoViaje.valor_extra = Viaje.fk_estado_viaje AND EstadoViaje.tipo = 'EstadoViaje' ";

            QueryDocumentos = " SELECT  ViajeDocumento.id " + 
                                      " , ViajeDocumento.url_documento_operador " +
                                      " , Documento.nombre AS 'DocumentoNombre' " +
		                              " , ISNULL((SELECT nombre FROM modulo.clientes WHERE id = ViajeDocumento.fk_cliente_id),'') AS 'ClienteNombre' " +
                                      " , ViajeDocumento.fk_estado_id " +
                                      " , EstadoDocumento.nombre AS 'EstadoDocumento' " +
                                      " , EstadoDocumento.valor_extra2 AS 'EstadoDocumentoColor' " +
                                " FROM  modulo.controlViajes_documento AS ViajeDocumento " +
                                          " LEFT JOIN modulo.documentos AS Documento ON  Documento.id = ViajeDocumento.fk_documento_id " +
                                          " LEFT JOIN modulo.controlViajes_viajes AS Viaje ON Viaje.id = ViajeDocumento.fk_viaje_id " +
                                          " LEFT JOIN modulo.controlViajes_status AS EstadoDocumento ON EstadoDocumento.valor_extra = ViajeDocumento.fk_estado_id AND EstadoDocumento.tipo = 'EstadoDocumento' " +
                                " WHERE Documento.sis_status = 1 ";

        }


        // POST: /AppOpAzteca/IniciarSesion
        [HttpPost("IniciarSesion")]
		public Result LogIn(Operadore operador)
        {
			// ----- Variables generales ----- //
			Result result = new Result();
			result.status = false;
			result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
			result.code = "LogIn-500";


            if (operador.NumeroEmpleado == null || operador.Contrasena == null)
            {
                result.message = "El numero de empleado y la contaseña son requeridos.";
            }
            else
            {
                try
                {
                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    var Operador = _context.Operadores.FromSqlRaw((QueryOperadores + " WHERE Operadores.numero_empleado = " + operador.NumeroEmpleado + " AND " + " Operadores.contrasena = '" + Encrypt.GetMD5(operador.Contrasena.Trim()) + "'")).ToList();

                    if (Operador.Count == 0)
                    {
                        result.message = "Usuario no encontrado, es necesario estar registrado en el sistema para acceder.";
                    }
                    else if (Operador[0].SisStatus == false)
                    {
                        result.message = "Acceso denegado, este usuario ya esta dado de baja del sistema, si cree que es un error, comuníquese con soporte.";
                    }
                    else if (Operador[0].StatusSesion == true)
                    {
                        result.message = "Este usuario ya cuenta con una sesion iniciada desde otro dispositivo, si cree que es un error comuniquese con soporte.";
                    }
                    else
                    {
                        var OperadorPermisos = _context.AppOpAztecaAccesos.FromSqlRaw(QueryOperadorAccesos + " AND PerfilAcceso.fk_perfil_id = " + Operador[0].FkPerfilId).ToList();
                        if ( OperadorPermisos.Count != 0 && OperadorPermisos[0].PerfilStatus == true && OperadorPermisos[0].ProcesoStatus == true )
                        {
                            Operador[0].Contrasena = null;
                            Operador[0].StatusSesion = true;

                            OperadoresController operadoresController = new OperadoresController(_context);
                            var UpdateOperador = operadoresController.UpdateOperador(Operador[0]);

                            if (UpdateOperador.status)
                            {
                                Operador[0].Contrasena = "";
                                result.status = true;
                                result.message = "Acceso exitoso.";
                                result.code = "LogIn-200";
                                result.data = Operador[0];
                            }
                            else
                            {
                                result.message = "Problema al iniciar sesión Code: " + result.code + "-" + UpdateOperador.code + "). Comuníquese con soporte.";
                            }
                        }
                        else
                        {
                            result.message = "Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.message = "Error al Iniciar sesion.  <--->  (  " + ex.ToString() + "  )";
                }
            }

            return result;
		}

        // POST: /AppOpAzteca/CerrarSesion/NumeroEmpleado
        [HttpGet("CerrarSesion/{NumeroEmpleado}")]
        public Result LogOut(string NumeroEmpleado)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogOut-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {
                try
                {
                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    var Operador = _context.Operadores.FromSqlRaw((QueryOperadores + " WHERE Operadores.numero_empleado = " + int.Parse(NumeroEmpleado))).ToList();

                    Operador[0].Contrasena = null;
                    Operador[0].StatusSesion = false;

                    OperadoresController operadoresController = new OperadoresController(_context);
                    var UpdateOperador = operadoresController.UpdateOperador(Operador[0]);

                    result.status = true;
                    result.message = "Transacción exitosa.";
                    result.code = "LogOut-200";
                }
                catch (Exception ex)
                {
                    result.message = "Error al Cerrar sesion.  <--->  (  " + ex.ToString() + "  )";
                }
            }
            return result;
        }

        // GET: /AppOpAzteca/OperadorStatus/NumeroEmpleado
        [HttpGet("OperadorStatus/{NumeroEmpleado}")]
        public Result OperadorStatus(string NumeroEmpleado)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogIn-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {
                try
                {
                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    var Operador = _context.Operadores.FromSqlRaw((QueryOperadores + " WHERE Operadores.numero_empleado = " + int.Parse(NumeroEmpleado))).ToList();
                    if (Operador.Count == 0)
                    {
                        result.message = "Su usuario ya no está activo en el sistema. Si cree que es un error, comuníquese con el área de soporte.";
                    }
                    else
                    {
                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var OperadorPermisos = _context.AppOpAztecaAccesos.FromSqlRaw(QueryOperadorAccesos + " AND PerfilAcceso.fk_perfil_id = " + Operador[0].FkPerfilId).ToList();
                        if(OperadorPermisos.Count != 0 && OperadorPermisos[0].PerfilStatus == true && OperadorPermisos[0].ProcesoStatus == true){
                            
                            Operador[0].Contrasena = "";
                            result.status = true;
                            result.message = "Consulta exitosa.";
                            result.code = "con-G-200";
                            result.data = Operador[0];

                        } else {
                            result.message = "Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.message += "  <--->  (  " + ex.ToString() + "  )";
                }
            }

            return result;
        }

        // POST: /AppOpAzteca/OperadorViajes/NumeroEmpleado
        [HttpPost("OperadorViajes/{NumeroEmpleado}")]
        public Result OperadorViajes(string NumeroEmpleado, AppOpAztecaViajes viaje)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogIn-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(NumeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    try
                    {
                        string Query = QueryViajes + " WHERE Viaje.numero_empleado_operador = " + int.Parse(NumeroEmpleado);

                        if (viaje.FechaInicio != null)
                        {
                            Query += " AND YEAR(Viaje.fecha_inicio) = YEAR(" + viaje.FechaInicio + ") AND MONTH(Viaje.fecha_inicio) = MONTH(" + viaje.FechaInicio + ")";
                        }
                        else
                        {
                            Query += " AND Viaje.fecha_inicio >= CAST(CONCAT(DATEADD(DAY, -15, DATEADD(DAY, 1, EOMONTH(GETDATE(), -1))), ' 00:00:00:000') AS DATETIME) " +
                                     " AND Viaje.fecha_inicio <= CAST(CONCAT(EOMONTH(GETDATE()), ' 23:59:00:000') AS DATETIME) ";
                        }
                        Query += " AND Viaje.fk_estado_viaje <> 4 ";
                       
                        if (viaje.FkEstadoDocumentosId != null && viaje.FkEstadoDocumentosId != 0) Query += " AND Viaje.fk_estado_documentos_id = " + viaje.FkEstadoDocumentosId;
                        if (viaje.EstadoDocumento != null && viaje.EstadoDocumento != "Ver todos") Query += " AND EstadoDocumento.nombre = '" + viaje.EstadoDocumento + "'";

                        if (viaje.FkEstadoViajeId != null && viaje.FkEstadoViajeId != 0) Query += " AND Viaje.fk_estado_viaje = " + viaje.FkEstadoViajeId;
                        if (viaje.EstadoViaje != null && viaje.EstadoViaje != "Ver todos") Query += " AND EstadoViaje.nombre = '" + viaje.EstadoViaje + "'";

                        if (viaje.NumeroViaje != null && viaje.NumeroViaje.Length > 0) Query += " AND ( Viaje.numero_viaje LIKE '%" + viaje.NumeroViaje + "%' OR Ruta.nombre LIKE '%" + viaje.NumeroViaje + "%')";

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var Viajes = _context.AppOpAztecaViajes.FromSqlRaw(Query);

                        result.status = true;
                        result.message = "Consulta exitosa.";
                        result.code = "con-G-200";
                        result.data = Viajes;
                        result.total = Viajes.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = "Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado. " + ex.ToString();
                    }
                }
            }
            return result;
        }
       
        // GET: /AppOpAzteca/OperadorViajesEnProcesoDocsPendientes/NumeroEmpleado
        [HttpGet("OperadorViajesEnProcesoDocsPendientes/{NumeroEmpleado}")]
        public Result OperadorViajesEnProcesoDocsPendientes(string NumeroEmpleado)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogIn-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {

                string Permission = PermissionValidation(int.Parse(NumeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    try
                    {
                        String Query = QueryViajes + " WHERE Viaje.numero_empleado_operador = " + int.Parse(NumeroEmpleado);
                        Query += " AND Viaje.fecha_inicio >= CAST(CONCAT(DATEADD(DAY, -15, DATEADD(DAY, 1, EOMONTH(GETDATE(), -1))), ' 00:00:00:000') AS DATETIME) " +
                                 " AND Viaje.fecha_inicio <= CAST(CONCAT(EOMONTH(GETDATE()), ' 23:59:00:000') AS DATETIME) " +
                                 " AND (Viaje.fk_estado_viaje = 3 OR Viaje.fk_estado_documentos_id IN(4,2,1))";
                        
                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var Viajes = _context.AppOpAztecaViajes.FromSqlRaw(Query);

                        result.status = true;
                        result.message = "Consulta exitosa.";
                        result.code = "con-G-200";
                        result.data = Viajes;
                        result.total = Viajes.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = " Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado. " + ex.ToString();
                    }
                }
            }
            return result;
        }

        // GET: /AppOpAzteca/OperadorViajesPendientes/NumeroEmpleado
        [HttpGet("OperadorViajesPendientes/{NumeroEmpleado}")]
        public Result OperadorViajesPendientes(string NumeroEmpleado)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogIn-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(NumeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    try
                    {
                        string Query = QueryViajes + " WHERE Viaje.numero_empleado_operador = " + int.Parse(NumeroEmpleado);
                        Query += " AND Viaje.fecha_inicio >= CAST(CONCAT(DATEADD(DAY, -15, DATEADD(DAY, 1, EOMONTH(GETDATE(), -1))), ' 00:00:00:000') AS DATETIME) " +
                                 " AND Viaje.fecha_inicio <= CAST(CONCAT(EOMONTH(GETDATE()), ' 23:59:00:000') AS DATETIME) " +
                                 " AND Viaje.fk_estado_viaje = 2 ";
                       
                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var Viajes = _context.AppOpAztecaViajes.FromSqlRaw(Query).ToList();

                        result.status = true;
                        result.message = "Consulta exitosa.";
                        result.code = "con-G-200";
                        result.data = Viajes;
                        result.total = Viajes.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = " Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado. " + ex.ToString();
                    }
                }
            }
            return result;
        }

        // POST: /AppOpAzteca/ViajesStatus/NumeroEmpleado
        [HttpGet("ViajesStatus/{NumeroEmpleado}")]
        public Result ViajesStatus(string NumeroEmpleado)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogIn-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(NumeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    try{
                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var Operador = _context.Operadores.FromSqlRaw((QueryOperadores + " WHERE Operadores.numero_empleado = " + int.Parse(NumeroEmpleado))).ToList();
                        if (Operador.Count == 0)
                        {
                            result.message = "Su usuario ya no está activo en el sistema. Si cree que es un error, comuníquese con el área de soporte.";
                        }
                        else
                        {
                            var OperadorPermisos = _context.AppOpAztecaAccesos.FromSqlRaw(QueryOperadorAccesos + " AND PerfilAcceso.fk_perfil_id = " + Operador[0].FkPerfilId).ToList();
                            if (OperadorPermisos.Count != 0 && OperadorPermisos[0].PerfilStatus == true && OperadorPermisos[0].ProcesoStatus == true)
                            {
                                var ViajeStatus = _context.OperadoresStatuses.FromSqlRaw(QueryViajeStatus + " WHERE ViajeStatus.sis_status = 1 AND (ViajeStatus.tipo = 'EstadoDocumento' OR ViajeStatus.tipo = 'EstadoViaje')").ToList();

                                Operador[0].Contrasena = "";
                                result.status = true;
                                result.message = "Consulta exitosa.";
                                result.code = "con-G-200";
                                result.data = ViajeStatus;
                                result.total = ViajeStatus.Count;
                            }
                            else
                            {
                                result.message = "Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.message += "  <--->  (  " + ex.ToString() + "  )";
                    }
                }  
            }

            return result;
        }
       
        // GET: /AppOpAzteca/OperadorViajesPendientes/NumeroEmpleado
        [HttpGet("OperadorViajeDocumentos/{NumeroEmpleado}/{Numeroviaje}")]
        public Result OperadorViajesDocumentos(string NumeroEmpleado, int Numeroviaje)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogIn-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(NumeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    try
                    {
                        string Query = QueryDocumentos + " AND Viaje.numero_viaje = '" + Numeroviaje + "'";
                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var ViajeDocumentos = _context.ViajeDocumentos.FromSqlRaw(Query).ToList();

                        result.status = true;
                        result.message = "Consulta exitosa.";
                        result.code = "con-G-200";
                        result.data = ViajeDocumentos;
                        result.total = ViajeDocumentos.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = " Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado. " + ex.ToString();
                    }
                }
            }
            return result;
        }

        // GET: /AppOpAzteca/OperadorViajeDocumentosPendientes/NumeroEmpleado
        [HttpGet("OperadorViajeDocumentosPendientes/{NumeroEmpleado}/{Numeroviaje}")]
        public Result OperadorViajeDocumentosPendientes(string NumeroEmpleado, int Numeroviaje)
        {
            // ----- Variables generales ----- //
            Result result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta información, si cree que es un error, comuníquese con soporte.";
            result.code = "LogIn-500";

            NumeroEmpleado = Encrypt.base64Decode(NumeroEmpleado.Substring(0, NumeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(NumeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(NumeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    try
                    {
                        string Query = QueryDocumentos + " AND Viaje.numero_viaje = '" + Numeroviaje + "' AND ViajeDocumento.fk_estado_id IN(1,4)";
                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var ViajeDocumentos = _context.ViajeDocumentos.FromSqlRaw(Query).ToList();

                        result.status = true;
                        result.message = "Consulta exitosa.";
                        result.code = "con-G-200";
                        result.data = ViajeDocumentos;
                        result.total = ViajeDocumentos.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = " Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado. " + ex.ToString();
                    }
                }
            }
            return result;
        }

        // POST: /AppOpAzteca/IniciarViaje
        [HttpPost("IniciarViaje")]
        public Result IniciarViaje([FromForm] FileUpLoad objFile)
        {
            var result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta transacion, si cree que es un error, comuníquese con soporte.";
            result.code = "InitTravel-500";


            string dir = "\\Viajes\\" + objFile.NumeroViaje + "\\";
            string dirWeb = "/Viajes/" + objFile.NumeroViaje + "/";

            string numeroEmpleado = objFile.Operador;

            numeroEmpleado = Encrypt.base64Decode(numeroEmpleado.Substring(0, numeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(numeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(numeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    try
                    {
                        var update = " INSERT INTO modulo.controlViajes_documento " +
                                        " SELECT '' as 'url_documento_operador' " +
	                                            " , Viaje.id AS 'fk_viaje_id' " +
	                                            " , DocumentoCliente.fk_documento_id AS 'fk_documento_id' " +
	                                            " , NULL AS 'fk_departamento_id' " +
	                                            " , DocumentoCliente.fk_cliente_id AS 'fk_cliente_id' " +
	                                            " , 1 AS 'fk_estado_id' " +
	                                            " , 1 AS 'fk_usuario_registro_id' " + 
	                                            " , GETDATE() AS 'fecha_registro' " + 
	                                            " , 1 AS 'fk_usuario_ultima_actualizacion_id' " +
	                                            " , GETDATE() AS 'ultima_fecha_actualizacion' " +
	                                            " , 1 AS 'ultima_fecha_actualizacion' " +
                                         " FROM modulo.clientes_documentos AS DocumentoCliente " + 
                                                " LEFT JOIN modulo.clientes AS Cliente ON Cliente.id = DocumentoCliente.fk_cliente_id " +
                                                " LEFT JOIN Azteca2021db..trafico_guia AS ZAM ON ZAM.id_cliente = Cliente.fk_cliente_zam " +
                                                " LEFT JOIN modulo.controlViajes_viajes AS Viaje ON Viaje.numero_viaje = ZAM.no_viaje " +
                                         " WHERE ZAM.no_viaje = " + int.Parse(objFile.NumeroViaje) + "; " +
                            
                                   " DECLARE @ESTADODOCUMENTO INT = (SELECT (CASE WHEN COUNT(Documento.id) > 0 THEN 1 ELSE 0 END) " +
                                                                     " FROM modulo.controlViajes_documento AS Documento " +
                                                                              " LEFT JOIN modulo.controlViajes_viajes AS Viaje ON Viaje.id = Documento.fk_viaje_id " +
                                                                    " WHERE numero_viaje = " + int.Parse(objFile.NumeroViaje) + ") " +

                                    " IF @ESTADODOCUMENTO = 0 " +
                                        " BEGIN " +
                                            " DECLARE @TIPOCARGA INT = (SELECT fk_tipo_carga_id FROM modulo.controlViajes_viajes WHERE numero_viaje = " + int.Parse(objFile.NumeroViaje) + "); " +

                                            " IF @TIPOCARGA = 1 " +
                                                " BEGIN " +
                                                    " INSERT INTO modulo.controlViajes_documento " +
                                                    " SELECT '' as 'url_documento_operador' " +
                                                            " , (SELECT id FROM  modulo.controlViajes_viajes WHERE numero_viaje = " + int.Parse(objFile.NumeroViaje) + ") AS 'fk_viaje_id' " +
					                                        " , id AS 'fk_documento_id' " +
					                                        " , NULL AS 'fk_departamento_id' " +
					                                        " , NULL AS 'fk_cliente_id' " +
					                                        " , 1 AS 'fk_estado_id' " +
	                                                        " , 1 AS 'fk_usuario_registro_id' " +
	                                                        " , GETDATE() AS 'fecha_registro' " +
	                                                        " , 1 AS 'fk_usuario_ultima_actualizacion_id' " +
	                                                        " , GETDATE() AS 'ultima_fecha_actualizacion' " +
	                                                        " , 1 AS 'ultima_fecha_actualizacion' " +
                                                     " FROM modulo.documentos " +
                                                    " WHERE tipo_documento in(2, 3) " +
                                                " END "+
                                               " ELSE " +
                                                " BEGIN " +
                                                        " INSERT INTO modulo.controlViajes_documento " +
                                                        " SELECT '' as 'url_documento_operador' " +
                                                                " , (SELECT id FROM  modulo.controlViajes_viajes WHERE numero_viaje = " + int.Parse(objFile.NumeroViaje) + ") AS 'fk_viaje_id' " +
                                                                " , id AS 'fk_documento_id' " +
                                                                " , NULL AS 'fk_departamento_id' " +
                                                                " , NULL AS 'fk_cliente_id' " +
                                                                " , 1 AS 'fk_estado_id' " +
                                                                " , 1 AS 'fk_usuario_registro_id' " +
                                                                " , GETDATE() AS 'fecha_registro' " +
                                                                " , 1 AS 'fk_usuario_ultima_actualizacion_id' " +
                                                                " , GETDATE() AS 'ultima_fecha_actualizacion' " +
                                                                " , 1 AS 'ultima_fecha_actualizacion' " +
                                                            " FROM modulo.documentos " +
                                                        " WHERE tipo_documento in(1, 3) " +
                                                 " END " +

                                                 " SET @ESTADODOCUMENTO = (SELECT (CASE WHEN COUNT(Documento.id) > 0 THEN 1 ELSE 0 END) " +
                                                              " FROM modulo.controlViajes_documento AS Documento " +
                                                                     " LEFT JOIN modulo.controlViajes_viajes AS Viaje ON Viaje.id = Documento.fk_viaje_id " +
                                                              " WHERE numero_viaje = " + int.Parse(objFile.NumeroViaje) + ") " +
                                    " END " +

                                   
                            " UPDATE modulo.controlViajes_viajes " +
                               " SET fk_estado_viaje = 3 " +
                                   " , fk_estado_documentos_id = @ESTADODOCUMENTO " +
                                   " , kilometraje_inicial = " + objFile.Kilometraje +
                                   " , url_img_kilometraje_inicial  = '" + UrlServ + dirWeb + objFile.File.FileName + "' " +
                                   " , observacion = '" + (objFile.Observaciones != null ? objFile.Observaciones : "") + "'" +
                             " WHERE numero_viaje = '" + objFile.NumeroViaje + "' ";

                        var Query = update + " " + QueryViajes + " WHERE Viaje.numero_viaje = '" + objFile.NumeroViaje + "' ";

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var UpdateViaje = _context.AppOpAztecaViajes.FromSqlRaw(Query).ToList();

                        result.message = "Inicio de viaje exitoso.";


                        if (objFile.File.Length > 0)
                        {
                            if (!Directory.Exists(_environment.WebRootPath + dir))
                            {
                                Directory.CreateDirectory(_environment.WebRootPath + dir);
                            }

                            using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + dir + objFile.File.FileName))
                            {
                                objFile.File.CopyTo(fileStream);
                                fileStream.Flush();
                            }
                        }
                        else
                        {
                            result.message = "Inicio del viaje exitoso, pero hubo un problema al cargar la imagen";
                        }

                        result.status = true;
                        result.code = "InitTravel-200";
                        result.data = UpdateViaje[0];
                        result.total = UpdateViaje.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = " Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado. " + ex.ToString();
                    }
                }
            }

            return result;
        }
        
        // POST: /AppOpAzteca/FinalizarViaje
        [HttpPost("FinalizarViaje")]
        public Result FinalizarViaje([FromForm] FileUpLoad objFile)
        {
            var result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta transacion, si cree que es un error, comuníquese con soporte.";
            result.code = "FinishTravel-500";

            string dir = "\\Viajes\\" + objFile.NumeroViaje + "\\";
            string dirWeb = "/Viajes/" + objFile.NumeroViaje + "/";

            string numeroEmpleado = objFile.Operador;

            numeroEmpleado = Encrypt.base64Decode(numeroEmpleado.Substring(0, numeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(numeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(numeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    result.status = true;

                    try
                    {
                        var update = " DECLARE @FK_VIAJE_ID INT = (SELECT id FROM modulo.controlViajes_viajes WHERE numero_viaje = '" + objFile.NumeroViaje + "' ); " +
                            " DECLARE @DOCUMENTOS_PENDIENTES_SUBIR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 1 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_PENDIENTES_APROBAR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 2 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_ACEPTADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 3 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_RECHAZADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 4 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @ESTADO_DOCUMENTO INT = (CASE WHEN @DOCUMENTOS_RECHAZADO > 0 THEN 4 " + 
                                                                  " WHEN @DOCUMENTOS_PENDIENTES_SUBIR > 0 THEN 1 " +
                                                                  " WHEN @DOCUMENTOS_PENDIENTES_APROBAR > 0 THEN 2 " +
                                                                  " WHEN @DOCUMENTOS_ACEPTADO > 0 THEN 3 "+
                                                              " ELSE 0 END); " +

                        " UPDATE modulo.controlViajes_viajes " +
                               " SET fk_estado_viaje = 1 " +
                                   " , fk_estado_documentos_id = @ESTADO_DOCUMENTO" +
                                   " , kilometraje_final = " + objFile.Kilometraje +
                                   " , kilometros_recorridos = ( " + objFile.Kilometraje + " - kilometraje_inicial )" +
                                   " , fecha_fin = GETDATE() " +
                                   " , url_img_kilometraje_final = '" + UrlServ + dirWeb + objFile.File.FileName + "' " +
                                   " , observacion = CONCAT(ISNULL(observacion,''),' <---> ', '" + (objFile.Observaciones != null ? objFile.Observaciones : "") + "" + "')" + 
                             " WHERE numero_viaje = '" + objFile.NumeroViaje + "' ";

                        var Query = update + " " + QueryViajes + " WHERE Viaje.numero_viaje = '" + objFile.NumeroViaje + "' ";

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var UpdateViaje = _context.AppOpAztecaViajes.FromSqlRaw(Query).ToList();
                        result.message = "Finalización exitosa del viaje.";

                        if (objFile.File.Length > 0)
                        {
                            if (!Directory.Exists(_environment.WebRootPath + dir))
                            {
                                Directory.CreateDirectory(_environment.WebRootPath + dir);
                            }

                            using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + dir + objFile.File.FileName))
                            {
                                objFile.File.CopyTo(fileStream);
                                fileStream.Flush();
                            }
                        }
                        else
                        {
                            result.message = "Finalización exitosa del viaje, pero hubo un problema al cargar la imagen";
                        }

                        result.code = "FinishTravel-200";
                        result.data = UpdateViaje[0];
                        result.total = UpdateViaje.Count();
                    }
                    catch (Exception ex)
                    {
                        result.message = " Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado. " + ex.ToString();
                    }
                }
            }
            return result;
        }

        // POST: /AppOpAzteca/SubirImagenViaje
        [HttpPost("SubirImagenViaje")]
        public Result SubirImagenViaje([FromForm] FileUpLoad objFile)
        {
            var result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta transacion, si cree que es un error, comuníquese con soporte.";
            result.code = "UploadImageTravel-500";

            string dir = "\\Viajes\\" + objFile.NumeroViaje + "\\";
            string dirWeb = "/Viajes/" + objFile.NumeroViaje + "/";

            string numeroEmpleado = objFile.Operador;

            numeroEmpleado = Encrypt.base64Decode(numeroEmpleado.Substring(0, numeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(numeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(numeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    result.status = true;
                    try
                    {
                        if (objFile.File.Length > 0)
                        {
                            if (!Directory.Exists(_environment.WebRootPath + dir))
                            {
                                Directory.CreateDirectory(_environment.WebRootPath + dir);
                            }

                            using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + dir + objFile.File.FileName))
                            {
                                objFile.File.CopyTo(fileStream);
                                fileStream.Flush();
                            }

                            string Query = " UPDATE modulo.controlViajes_documento " +
                                              " SET url_documento_operador = '" + UrlServ + dirWeb + objFile.File.FileName + "' " +
                                                  " , fk_usuario_ultima_actualizacion_id = 1 " +
                                                  " , ultima_fecha_actualizacion = GETDATE() " +
                                                  " , fk_estado_id = 2 " +
                                               " WHERE id = " + objFile.DocumentoId + "; ";

                            Query += " DECLARE @FK_VIAJE_ID INT = (SELECT id FROM modulo.controlViajes_viajes WHERE numero_viaje = '" + objFile.File.FileName.Split("_")[0].ToString() + "' ); " +
                            " DECLARE @DOCUMENTOS_PENDIENTES_SUBIR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 1 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_PENDIENTES_APROBAR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 2 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_ACEPTADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 3 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_RECHAZADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 4 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @ESTADO_DOCUMENTO INT = (CASE WHEN @DOCUMENTOS_RECHAZADO > 0 THEN 4 " +
                                                                  " WHEN @DOCUMENTOS_PENDIENTES_SUBIR > 0 THEN 1 " +
                                                                  " WHEN @DOCUMENTOS_PENDIENTES_APROBAR > 0 THEN 2 " +
                                                                  " WHEN @DOCUMENTOS_ACEPTADO > 0 THEN 3 " +
                                                              " ELSE 0 END); " +

                            " UPDATE modulo.controlViajes_viajes " +
                             " SET fk_estado_documentos_id = @ESTADO_DOCUMENTO " +
                           " WHERE numero_viaje = '" + objFile.File.FileName.Split("_")[0].ToString() + "' ";

                            Query += QueryDocumentos + " AND ViajeDocumento.id = " + objFile.DocumentoId;
                            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                            var ViajeDocumentos = _context.ViajeDocumentos.FromSqlRaw(Query).ToList();

                            result.message = "La imagen fue cargada con éxito";
                            result.code = "UploadImage-200";
                            result.data = ViajeDocumentos[0];
                            result.total = ViajeDocumentos.Count;
                        }
                        else
                        {
                            result.message = "Error al subir la imagen.";
                            result.total = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.message = "Error al subir la imagen. <---> " + ex.ToString();
                        result.total = 0;
                    }
                }
            }

            return result;
        }


        // POST: /AppOpAzteca/DocumentoNoRequerido
        [HttpPost("DocumentoNoRequerido")]
        public Result DocumentoNoRequerido([FromForm] FileUpLoad objFile)
        {
            var result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta transacion, si cree que es un error, comuníquese con soporte.";
            result.code = "UploadImageTravel-500";

           
            string numeroEmpleado = objFile.Operador;

            numeroEmpleado = Encrypt.base64Decode(numeroEmpleado.Substring(0, numeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(numeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(numeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    result.status = true;
                    try
                    {
                        

                            string Query = " UPDATE modulo.controlViajes_documento " +
                                              " SET  fk_usuario_ultima_actualizacion_id = 1 " +
                                                  " , ultima_fecha_actualizacion = GETDATE() " +
                                                  " , fk_estado_id = 5 " +
                                               " WHERE id = " + objFile.DocumentoId + "; ";

                            Query += " DECLARE @FK_VIAJE_ID INT = (SELECT id FROM modulo.controlViajes_viajes WHERE numero_viaje = '" + objFile.NumeroViaje.ToString() + "' ); " +
                            " DECLARE @DOCUMENTOS_PENDIENTES_SUBIR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 1 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_PENDIENTES_APROBAR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 2 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_ACEPTADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 3 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_RECHAZADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 4 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @DOCUMENTOS_NO_REQUERIDOS INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 5 AND fk_viaje_id = @FK_VIAJE_ID); " +
                            " DECLARE @ESTADO_DOCUMENTO INT = (CASE WHEN @DOCUMENTOS_RECHAZADO > 0 THEN 4 " +
                                                                  " WHEN @DOCUMENTOS_PENDIENTES_SUBIR > 0 THEN 1 " +
                                                                  " WHEN @DOCUMENTOS_PENDIENTES_APROBAR > 0 THEN 2 " +
                                                                  " WHEN @DOCUMENTOS_ACEPTADO > 0 THEN 3 " +
                                                                  " WHEN @DOCUMENTOS_NO_REQUERIDOS > 0 THEN 5 " +
                                                              " ELSE 0 END); " +

                            " UPDATE modulo.controlViajes_viajes " +
                             " SET fk_estado_documentos_id = @ESTADO_DOCUMENTO " +
                           " WHERE numero_viaje = '" + objFile.NumeroViaje.ToString() + "' ";

                            Query += QueryDocumentos + " AND ViajeDocumento.id = " + objFile.DocumentoId;
                            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                            var ViajeDocumentos = _context.ViajeDocumentos.FromSqlRaw(Query).ToList();

                            result.message = "El documento fue actualizado con éxito";
                            result.code = "UploadDocument-200";
                            result.data = ViajeDocumentos[0];
                            result.total = ViajeDocumentos.Count;
                        
                    }
                    catch (Exception ex)
                    {
                        result.message = "Error al subir el documento. <---> " + ex.ToString();
                        result.total = 0;
                    }
                }
            }

            return result;
        }


        // POST: /AppOpAzteca/DocumentoNoRequerido
        [HttpPost("DocumentoRequerido")]
        public Result DocumentoRequerido([FromForm] FileUpLoad objFile)
        {
            var result = new Result();
            result.status = false;
            result.message = "Lo sentimos, pero su usuario no tiene acceso a esta transacion, si cree que es un error, comuníquese con soporte.";
            result.code = "UploadImageTravel-500";


            string numeroEmpleado = objFile.Operador;

            numeroEmpleado = Encrypt.base64Decode(numeroEmpleado.Substring(0, numeroEmpleado.Length - 3));
            if (!string.IsNullOrEmpty(numeroEmpleado))
            {
                string Permission = PermissionValidation(int.Parse(numeroEmpleado));
                if (Permission != "ok")
                {
                    result.message = Permission;
                }
                else
                {
                    result.status = true;
                    try
                    {


                        string Query = " UPDATE modulo.controlViajes_documento " +
                                          " SET  fk_usuario_ultima_actualizacion_id = 1 " +
                                              " , ultima_fecha_actualizacion = GETDATE() " +
                                              " , fk_estado_id = 1 " +
                                           " WHERE id = " + objFile.DocumentoId + "; ";

                        Query += " DECLARE @FK_VIAJE_ID INT = (SELECT id FROM modulo.controlViajes_viajes WHERE numero_viaje = '" + objFile.NumeroViaje.ToString() + "' ); " +
                        " DECLARE @DOCUMENTOS_PENDIENTES_SUBIR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 1 AND fk_viaje_id = @FK_VIAJE_ID); " +
                        " DECLARE @DOCUMENTOS_PENDIENTES_APROBAR INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 2 AND fk_viaje_id = @FK_VIAJE_ID); " +
                        " DECLARE @DOCUMENTOS_ACEPTADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 3 AND fk_viaje_id = @FK_VIAJE_ID); " +
                        " DECLARE @DOCUMENTOS_RECHAZADO INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 4 AND fk_viaje_id = @FK_VIAJE_ID); " +
                        " DECLARE @DOCUMENTOS_NO_REQUERIDOS INT = (SELECT COUNT(id) FROM modulo.controlViajes_documento WHERE fk_estado_id = 5 AND fk_viaje_id = @FK_VIAJE_ID); " +
                        " DECLARE @ESTADO_DOCUMENTO INT = (CASE WHEN @DOCUMENTOS_RECHAZADO > 0 THEN 4 " +
                                                              " WHEN @DOCUMENTOS_PENDIENTES_SUBIR > 0 THEN 1 " +
                                                              " WHEN @DOCUMENTOS_PENDIENTES_APROBAR > 0 THEN 2 " +
                                                              " WHEN @DOCUMENTOS_ACEPTADO > 0 THEN 3 " +
                                                              " WHEN @DOCUMENTOS_NO_REQUERIDOS > 0 THEN 5 " +
                                                          " ELSE 0 END); " +

                        " UPDATE modulo.controlViajes_viajes " +
                         " SET fk_estado_documentos_id = @ESTADO_DOCUMENTO " +
                       " WHERE numero_viaje = '" + objFile.NumeroViaje.ToString() + "' ";

                        Query += QueryDocumentos + " AND ViajeDocumento.id = " + objFile.DocumentoId;
                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var ViajeDocumentos = _context.ViajeDocumentos.FromSqlRaw(Query).ToList();

                        result.message = "El documento fue actualizado con éxito";
                        result.code = "UploadDocument-200";
                        result.data = ViajeDocumentos[0];
                        result.total = ViajeDocumentos.Count;

                    }
                    catch (Exception ex)
                    {
                        result.message = "Error al subir el documento. <---> " + ex.ToString();
                        result.total = 0;
                    }
                }
            }

            return result;
        }



        private string PermissionValidation(int NumeroEmpleado)
        {
            string Result = "ok";

            try
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var Operador = _context.Operadores.FromSqlRaw((QueryOperadores + " WHERE Operadores.numero_empleado = " + NumeroEmpleado)).ToList();

                if (Operador.Count == 0)
                {
                    Result = "Usuario no encontrado, es necesario estar registrado en el sistema para acceder.";
                }
                else if (Operador[0].SisStatus == false || Operador[0].StatusSesion == false)
                {
                    Result = "Acceso denegado, si cree que es un error, comuníquese con soporte.";
                }
                else
                {
                    var OperadorPermisos = _context.AppOpAztecaAccesos.FromSqlRaw(QueryOperadorAccesos + " AND PerfilAcceso.fk_perfil_id = " + Operador[0].FkPerfilId).ToList();
                    if (OperadorPermisos.Count == 0 && OperadorPermisos[0].PerfilStatus == false && OperadorPermisos[0].ProcesoStatus == false)
                    {
                        Result = "Lo sentimos, su usuario no tiene los permisos para ingresar a la aplicación, si cree que es un error, comuníquese con soporte proporcionando su número de empleado.";
                    }
                }
            }
            catch (Exception ex)
            {
                Result = "Error al validar permisos.  <--->  (  " + ex.ToString() + "  )";
            }

            return Result;
        }
    }
} 
 