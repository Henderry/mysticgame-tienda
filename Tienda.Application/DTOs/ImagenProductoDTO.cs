using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.DTOs
{
    public class ImagenProductoDTO
    {
        public int IdImagen { get; set; }

        public int IdProducto { get; set; }

        public byte[]? Foto { get; set; }

        public bool Principal { get; set; }

        public virtual ProductoDTO IdProductoNavigation { get; set; } = null!;
    }
}
