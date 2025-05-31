using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class ImagenProducto
{
    public int IdImagen { get; set; }

    public int IdProducto { get; set; }

    public byte[]? Foto { get; set; }

    public bool? Principal { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
