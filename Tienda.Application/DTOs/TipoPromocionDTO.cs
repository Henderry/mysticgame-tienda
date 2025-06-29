using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Application.DTOs
{
    public class TipoPromocionDTO
    {
        public int IdTipoPromocion { get; set; }

        public string? Tipo { get; set; }

        public virtual ICollection<PromocionDTO> Promocion { get; set; } = new List<PromocionDTO>();
    }
}
