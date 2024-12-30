namespace sistema_soportemp.Models
{
    public class Roles
    {
        public const string Administrador = "Administrador";
        public const string Supervisor = "Supervisor";
        public const string Tecnico = "Técnico";
        public const string Operador = "Operador";

        public static readonly string[] TodosLosRoles = new[]
        {
        Administrador,
        Supervisor,
        Tecnico,
        Operador
    };
    }
}
