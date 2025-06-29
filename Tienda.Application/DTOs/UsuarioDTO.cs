using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        public int IdRol { get; set; }

        public string? Nombre { get; set; }

        public string Correo { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string? Pais { get; set; }

        public string? Telefono { get; set; }

        public virtual RolDTO IdRolNavigation { get; set; } = null!;

        public virtual ICollection<ResenaDTO> Resena { get; set; } = new List<ResenaDTO>();
    }
}
