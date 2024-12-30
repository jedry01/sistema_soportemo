using sistema_soportemp.Models;

namespace sistema_soportemp.Services
{
    public class PermisosService
    {
        public static class Permisos
        {
            public static class Equipos
            {
                public const string Ver = "Equipos.Ver";
                public const string Crear = "Equipos.Crear";
                public const string Editar = "Equipos.Editar";
                public const string Eliminar = "Equipos.Eliminar";
            }

            public static class Repuestos
            {
                public const string Ver = "Repuestos.Ver";
                public const string Crear = "Repuestos.Crear";
                public const string Editar = "Repuestos.Editar";
                public const string Eliminar = "Repuestos.Eliminar";
            }

            public static class Reportes
            {
                public const string Ver = "Reportes.Ver";
                public const string Generar = "Reportes.Generar";
                public const string Exportar = "Reportes.Exportar";
            }

            public static class Usuarios
            {
                public const string Ver = "Usuarios.Ver";
                public const string Crear = "Usuarios.Crear";
                public const string Editar = "Usuarios.Editar";
                public const string Eliminar = "Usuarios.Eliminar";
            }
        }

        public static class PermisosRoles
        {
            public static Dictionary<string, string[]> ObtenerPermisosRol()
            {
                return new Dictionary<string, string[]>
        {
            {
                Roles.Administrador,
                new[] {
                    Permisos.Equipos.Ver, Permisos.Equipos.Crear, Permisos.Equipos.Editar, Permisos.Equipos.Eliminar,
                    Permisos.Repuestos.Ver, Permisos.Repuestos.Crear, Permisos.Repuestos.Editar, Permisos.Repuestos.Eliminar,
                    Permisos.Reportes.Ver, Permisos.Reportes.Generar, Permisos.Reportes.Exportar,
                    Permisos.Usuarios.Ver, Permisos.Usuarios.Crear, Permisos.Usuarios.Editar, Permisos.Usuarios.Eliminar
                }
            },
            {
                Roles.Supervisor,
                new[] {
                    Permisos.Equipos.Ver, Permisos.Equipos.Crear, Permisos.Equipos.Editar,
                    Permisos.Repuestos.Ver, Permisos.Repuestos.Crear, Permisos.Repuestos.Editar,
                    Permisos.Reportes.Ver, Permisos.Reportes.Generar, Permisos.Reportes.Exportar,
                    Permisos.Usuarios.Ver
                }
            },
            {
                Roles.Tecnico,
                new[] {
                    Permisos.Equipos.Ver, Permisos.Equipos.Editar,
                    Permisos.Repuestos.Ver, Permisos.Repuestos.Editar,
                    Permisos.Reportes.Ver
                }
            },
            {
                Roles.Operador,
                new[] {
                    Permisos.Equipos.Ver,
                    Permisos.Repuestos.Ver,
                    Permisos.Reportes.Ver
                }
            }
        };
            }
        }
    }
}
