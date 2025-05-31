using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string Rol1 { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
