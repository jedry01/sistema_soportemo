using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace sistema_soportemp.Models
{
    public class Modelos
    {
        
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int MarcaId { get; set; }
            public int ActivoId { get; set; }

            [ForeignKey("MarcaId")]
            [DeleteBehavior(DeleteBehavior.NoAction)]  // Asegurando que no se haga en cascada
            public virtual Marcas Marca { get; set; }

            [ForeignKey("ActivoId")]
            [DeleteBehavior(DeleteBehavior.NoAction)]  // Asegurando que no se haga en cascada
            public virtual Activos Activo { get; set; }
        

    }

}
