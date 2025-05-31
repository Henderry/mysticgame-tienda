using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string? Nombre { get; set; }

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? Pais { get; set; }

    public string? Telefono { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Resena> Resena { get; set; } = new List<Resena>();
}
