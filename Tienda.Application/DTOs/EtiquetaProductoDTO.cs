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
    public class EtiquetaProductoDTO
    {
        [Key]
        public int IdProducto { get; set; }
        [Key]
        public int IdEtiqueta { get; set; }
        [JsonIgnore]
        public virtual EtiquetaDTO? IdEtiquetaNavigation { get; set; }

        public virtual ProductoDTO? IdProductoNavigation { get; set; }
    }
}
