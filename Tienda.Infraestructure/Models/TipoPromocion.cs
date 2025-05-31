using System;
using System.Collections.Generic;

namespace Tienda.Infraestructure.Models;

public partial class TipoPromocion
{
    public int IdTipoPromocion { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Promocion> Promocion { get; set; } = new List<Promocion>();
}
