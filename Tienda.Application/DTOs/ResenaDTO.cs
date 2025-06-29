using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.DTOs
{
    public class ResenaDTO
    {
        public int IdResena { get; set; }

        public int IdUsuario { get; set; }

        public int IdProducto { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Comentario { get; set; }

        public byte Valoracion { get; set; }

        public virtual ProductoDTO IdProductoNavigation { get; set; } = null!;

        public virtual UsuarioDTO IdUsuarioNavigation { get; set; } = null!;
    }
}
