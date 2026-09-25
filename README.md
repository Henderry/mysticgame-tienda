# MysticGame, tienda de videojuegos

Tienda en línea de videojuegos hecha con ASP.NET Core 8 MVC, Entity Framework Core y SQL Server,
en una arquitectura de tres capas (Web, Application e Infraestructure).

Proyecto de curso de la UTN.

## Funcionalidades

**Tienda**
- Portada con carrusel de portadas, categorías, ofertas, novedades y promociones vigentes.
- Catálogo con búsqueda, filtro por categoría, opción de ver solo ofertas y orden por nombre o precio.
- Detalle de producto con galería, etiquetas, stock, precio con descuento, productos
  relacionados y reseñas.
- Promociones agrupadas por estado (activa, próxima y finalizada).
- Carrito lateral guardado en el navegador. El pago es simulado.
- Página 404 y manejo de errores con middleware.

**Administración**
- Productos: crear y editar con etiquetas y varias imágenes (vista previa y arrastrar y soltar,
  máximo 5 MB por imagen).
- Promociones por producto o por categoría, con descuento en porcentaje y validación de fechas.

**Reglas de negocio**
- Cada producto muestra la mejor promoción vigente, ya sea propia o de su categoría
  (servicio `CatalogoOfertas`).
- Una promoción está activa entre su fecha de inicio y de fin, y solo si el descuento es mayor a 0.
- Precios en colones con la cultura `es-CR`.

## Tecnologías

- ASP.NET Core 8 MVC, Razor, Bootstrap 5.3 y JavaScript.
- AutoMapper, servicios y DTOs en la capa de aplicación.
- Entity Framework Core con SQL Server y patrón repositorio.
- Serilog para los registros.

## Arquitectura

```
Tienda.sln
├── Tienda.Web              Controladores, vistas, view models, estilos y scripts
├── Tienda.Application      Servicios, DTOs y perfiles de AutoMapper
├── Tienda.Infraestructure  DbContext, entidades y repositorios
└── database                Scripts SQL: esquema y datos de ejemplo
```

Los controladores usan los servicios (`IService*`), los servicios usan los repositorios
(`IRepository*`) y solo la capa de infraestructura conoce Entity Framework. Todo se registra
con inyección de dependencias en `Program.cs`.

## Ejecutar en local

Requisitos: .NET 8 SDK (o Visual Studio 2022) y SQL Server (Express funciona).

1. Ejecutar en SQL Server los scripts de `database/`: `01_esquema.sql` y luego `02_datos.sql`.
   Los datos de ejemplo cargan las imágenes desde `C:\VideoGame\`.
2. Revisar la cadena de conexión `SqlServerDataBase` en `Tienda.Web/appsettings.json`.
3. Ejecutar:
   ```bash
   dotnet run --project Tienda.Web --launch-profile https
   ```
   La tienda abre en https://localhost:7010.

Las reseñas se guardan con un usuario de ejemplo (`IdUsuario = 2`) porque todavía no hay
inicio de sesión.

## Autor

Henderry Moscat Benedict
