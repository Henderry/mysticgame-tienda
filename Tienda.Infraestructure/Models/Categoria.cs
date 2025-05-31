using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string? Categoria1 { get; set; }

    public byte[]? Foto { get; set; }

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
