using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.DTOs
{
    public class RolDTO
    {
        public int IdRol { get; set; }

        public string Rol1 { get; set; } = null!;

        public virtual ICollection<UsuarioDTO> Usuario { get; set; } = new List<UsuarioDTO>();
    }
}
