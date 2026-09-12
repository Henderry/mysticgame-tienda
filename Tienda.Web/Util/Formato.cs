using System.Globalization;

namespace Tienda.Web.Util
{
    /// <summary>Formatos de presentación usados en las vistas.</summary>
    public static class Formato
    {
        private static readonly NumberFormatInfo Colon = new()
        {
            NumberGroupSeparator = " ",
            NumberDecimalSeparator = ",",
            NumberGroupSizes = new[] { 3 }
        };

        private static readonly CultureInfo Espanol = new("es-CR");

        /// <summary>18000 → "₡18 000"</summary>
        public static string Colones(decimal? monto) =>
            monto is null ? string.Empty : "₡" + monto.Value.ToString("#,0", Colon);

        /// <summary>2025-08-10 → "10 ago 2025"</summary>
        public static string Fecha(DateTime? fecha) =>
            fecha is null ? string.Empty : fecha.Value.ToString("dd MMM yyyy", Espanol).Replace(".", string.Empty);

        /// <summary>Iniciales para el avatar de un usuario: "Pablo Arias" → "PA"</summary>
        public static string Iniciales(string? nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "?";
            var partes = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(partes.Take(2).Select(p => char.ToUpperInvariant(p[0])));
        }

        /// <summary>Color estable (no aleatorio) para el avatar según el nombre.</summary>
        public static string ColorAvatar(string? nombre)
        {
            string[] paleta = { "#00abf0", "#7c5cff", "#ff4f8b", "#22c55e", "#f59e0b", "#14b8a6" };
            var suma = (nombre ?? string.Empty).Sum(c => c);
            return paleta[suma % paleta.Length];
        }

        public static string Base64(byte[]? foto) =>
            foto is null || foto.Length == 0 ? string.Empty : "data:image/jpeg;base64," + Convert.ToBase64String(foto);
    }
}
