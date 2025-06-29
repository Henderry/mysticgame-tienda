using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.DTOs
{
    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }

        public string? Categoria1 { get; set; }

        public byte[]? Foto { get; set; }

        public virtual ICollection<ProductoDTO> Producto { get; set; } = new List<ProductoDTO>();
    }
}
