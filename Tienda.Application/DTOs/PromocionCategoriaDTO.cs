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
    public class PromocionCategoriaDTO
    {
        [Key]
        public int IdPromocion { get; set; }
        [Key]
        public int IdCategoria { get; set; }

        [JsonIgnore]
        public virtual CategoriaDTO? IdCategoriaNavigation { get; set; }

        public virtual PromocionDTO? IdPromocionNavigation { get; set; }
    }
}
