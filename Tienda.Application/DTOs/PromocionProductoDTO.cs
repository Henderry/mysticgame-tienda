using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.DTOs
{
    public class PromocionProductoDTO
    {
        [Key]
        public int IdPromocion { get; set; }
        [Key]
        public int IdProducto { get; set; }

        [JsonIgnore]
        public virtual ProductoDTO? IdProductoNavigation { get; set; }

        public virtual PromocionDTO? IdPromocionNavigation { get; set; }
    }
}
