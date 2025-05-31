using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

[PrimaryKey(nameof(IdProducto), nameof(IdEtiqueta))]
public partial class EtiquetaProducto
{
    public int IdProducto { get; set; }

    public int IdEtiqueta { get; set; }

    public virtual Etiqueta IdEtiquetaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
