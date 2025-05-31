using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class Promocion
{
    public int IdPromocion { get; set; }

    public int IdTipoPromocion { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Descuento { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public virtual TipoPromocion IdTipoPromocionNavigation { get; set; } = null!;
}
