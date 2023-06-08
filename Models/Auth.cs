namespace apiOperadores.Models
{
    public class Auth
    {
        public static string autenticarUsuario(int id)
        {
            
            return @"SELECT  u.id 
                           , u.nombre 
                           , u.apellido_paterno 
                           , u.apellido_materno 
                           , u.tipo AS 'tipoUsuario' 
                           , p.alias AS 'PerfilAlias' 
                           , u.correo_electronico 
                           , dp.nombre AS 'PuestoNombre' 
                           , d.nombre AS 'DepartamentoNombre' 
                           , d.color AS 'DepartamentoColor' 
                           , u.sis_status 
                           , u.status_sesion 
                           , p.sis_status AS 'PerfilStatus' 
                           , CONVERT(bit,'1') AS 'tipoPermiso' 
                     FROM  sistema.usuarios AS u 
                           INNER JOIN sistema.perfiles AS p  ON  p.id = u.fk_perfil_id 
                           INNER JOIN sistema.departamento_puestos AS dp ON u.fk_puesto_id = dp.id 
                           INNER JOIN sistema.departamentos AS d ON dp.fk_departamento_id = d.id 
                     WHERE u.id = " + id;
        }
    }
}
