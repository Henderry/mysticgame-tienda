using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.DTOs
{
    public class ProductoDTO
    {
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Descripcion { get; set; }

        [Required]
        public decimal? Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        public virtual CategoriaDTO? IdCategoriaNavigation { get; set; } = null!;

        public virtual ICollection<ImagenProductoDTO>? ImagenProducto { get; set; }

        public virtual ICollection<ResenaDTO>? Resena { get; set; }
    }
}
