using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

[PrimaryKey(nameof(IdPromocion), nameof(IdCategoria))]
public partial class PromocionCategoria
{
    public int IdPromocion { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Promocion IdPromocionNavigation { get; set; } = null!;
}
