using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

[PrimaryKey(nameof(IdPromocion), nameof(IdProducto))]
public partial class PromocionProducto
{
    public int IdPromocion { get; set; }

    public int IdProducto { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Promocion IdPromocionNavigation { get; set; } = null!;
}
