using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int Stock { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<ImagenProducto> ImagenProducto { get; set; } = new List<ImagenProducto>();

    public virtual ICollection<Resena> Resena { get; set; } = new List<Resena>();
}
