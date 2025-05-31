using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class Resena
{
    public int IdResena { get; set; }

    public int IdUsuario { get; set; }

    public int IdProducto { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Comentario { get; set; }

    public byte Valoracion { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
