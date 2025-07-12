using Humanizer;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using Tienda.Application.DTOs;
using Tienda.Application.Services.Implementations;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Tienda.Infraestructure.Data;

namespace Tienda.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceEtiqueta _serviceEtiqueta;
        private readonly IServiceEtiquetaProducto _serviceProductoEtiqueta;
        private readonly IServiceResena _serviceResena;
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceCategoria _serviceCategoria;
        private readonly IServiceImagenProducto _serviceImagenProducto;
        private readonly VideoGameContext _contex;

        public ProductoController(
            IServiceEtiqueta serviceEtiqueta,
            IServiceEtiquetaProducto serviceProductoEtiqueta,
            IServiceResena serviceResena,
            IServiceProducto serviceProducto,
            IServiceImagenProducto serviceImagenProducto,
            IServiceCategoria serviceCategoria,
            VideoGameContext contex
            )
        {
            _serviceEtiqueta = serviceEtiqueta;
            _serviceProducto = serviceProducto;
            _serviceProductoEtiqueta = serviceProductoEtiqueta;
            _serviceResena = serviceResena;
            _serviceImagenProducto = serviceImagenProducto;
            _serviceCategoria = serviceCategoria;
            _contex = contex;
        }
        public async Task<IActionResult> Index()
        {
            var productos = await _serviceProducto.ListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var producto = await _serviceProducto.FindByIdAsync(id);
            if (producto == null) return NotFound();

            await DatosViewBag(id);
            return View(producto);
        }

        private async Task DatosViewBag(int productoId)
        {
            // Obtener las relaciones producto etiqueta
            var relaciones = await _serviceProductoEtiqueta.ListAsync();

            // Filtrar y obtener los nombres de las etiquetas del producto
            ViewBag.EtiquetasDelProducto = relaciones
                .Where(x => x.IdProducto == productoId)
                .Select(x => x.IdEtiquetaNavigation?.Etiqueta1)
                .Where(nombre => !string.IsNullOrEmpty(nombre))
                .ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarResena(ResenaDTO resenaDTO)
        {
            resenaDTO.Fecha = DateTime.Now;

            var idResena = await _serviceResena.AddAsync(resenaDTO);

            if (idResena <= 0)
            {
                ModelState.AddModelError("", "No se registró la reseña");
                await DatosViewBag(resenaDTO.IdProducto);
                return View("Details", await _serviceProducto.FindByIdAsync(resenaDTO.IdProducto));
            }

            TempData["SuccessMessage"] = "¡Gracias por tu opinión!";
            return RedirectToAction("Details", new { id = resenaDTO.IdProducto });
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarDatosViewBag();
            return View(new ProductoDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
          ProductoDTO productoDto,
          List<int> selectedEtiqueta,
          List<IFormFile> imageFiles)
        {
            await CargarDatosViewBag();

            if (!ModelState.IsValid)
            {
                return View(productoDto);
            }

            using var transaction = await _contex.Database.BeginTransactionAsync();
            try
            {
                //Crear producto
                var producto = new Producto
                {
                    Nombre = productoDto.Nombre,
                    Descripcion = productoDto.Descripcion,
                    Precio = productoDto.Precio,
                    Stock = productoDto.Stock,
                    IdCategoria = productoDto.IdCategoria
                };

                await _contex.Producto.AddAsync(producto);
                await _contex.SaveChangesAsync();
                var productoId = producto.IdProducto;

                // Asociar etiquetas
                if (selectedEtiqueta != null && selectedEtiqueta.Any())
                {
                    foreach (var id in selectedEtiqueta)
                    {
                        var relationEntity = new EtiquetaProductoDTO();
                        relationEntity.IdProducto = productoId;
                        relationEntity.IdEtiqueta = id;
                        await _serviceProductoEtiqueta.AddAsync(relationEntity);
                    }

                }

                // Guardar imágenes
                if (imageFiles?.Any() == true)
                {
                    foreach (var file in imageFiles.Where(f => f.Length > 0))
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);

                        await _contex.ImagenProducto.AddAsync(new ImagenProducto
                        {
                            IdProducto = productoId,
                            Foto = ms.ToArray(),
                            Principal = (file == imageFiles.First())
                        });
                    }
                    await _contex.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                TempData["SuccessMessage"] = "Producto creado exitosamente!";
                return RedirectToAction("Crear");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error al crear el producto: {ex.Message}");
                return View(productoDto);
            }
        }




        private async Task CargarDatosViewBag()
        {
            ViewBag.ListCategorias = await _serviceCategoria.ListAsync() ?? new List<CategoriaDTO>();
            ViewBag.ListEtiquetas = await _serviceEtiqueta.ListAsync() ?? new List<EtiquetaDTO>();
            ViewBag.Productos = await _serviceProducto.ListAsync() ?? new List<ProductoDTO>();
        }

        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var producto = await _serviceProducto.FindByIdAsync(id);
            if (producto == null) return NotFound();

            await DatosEViewBag(id);
            await CargarDatosViewBag();
            return View(producto);
        }

        private async Task DatosEViewBag(int productoId)
        {
            var relaciones = await _serviceProductoEtiqueta.ListAsync();

            ViewBag.EtiquetasDelProducto = relaciones
                .Where(x => x.IdProducto == productoId)
                .Select(x => x.IdEtiqueta) 
                .ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(
            ProductoDTO productoDto,
            List<int> selectedEtiquetas,
            List<IFormFile> imageFiles

            )
        {
            if (!ModelState.IsValid)
            {
                await DatosEViewBag(productoDto.IdProducto);
                return View(productoDto);
            }
            try
            {
                // 1. Actualizar producto
                var productoId = await _serviceProducto.UpdateAsync(productoDto);
                if (productoId <= 0)
                {
                    ModelState.AddModelError("", "Error al actualizar!");
                    return View(productoDto);
                }

                // Eliminar
                var actuales = await _serviceProductoEtiqueta.ListAsync();
                var etiquetasAsociadas = actuales
                    .Where(e => e.IdProducto == productoDto.IdProducto)
                    .ToList();

                foreach (var etiqueta in etiquetasAsociadas)
                {
                    await _serviceProductoEtiqueta.DeleteAsync(etiqueta);
                }

            
                // Asociar etiquetas
                if (selectedEtiquetas != null && selectedEtiquetas.Any())
                {
                    foreach (var id in selectedEtiquetas)
                    {
                        var relationEntity = new EtiquetaProductoDTO();
                        relationEntity.IdProducto = productoId;
                        relationEntity.IdEtiqueta = id;
                        await _serviceProductoEtiqueta.AddAsync(relationEntity);
                    }

                }

                if (imageFiles != null && imageFiles.Count > 0)
                {
                   
                    var imagenesCreadas = productoDto.ImagenProducto ?? new List<ImagenProductoDTO>();

                    // Verfica existe una imagen principal
                    bool PrimerPrincipal = imagenesCreadas.Any(i =>i.Principal);

                    foreach (var imageFile in imageFiles)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);

                            // La primera imagen será principal solo si no hay ninguna principal ya
                            bool esPrincipal = !PrimerPrincipal && imageFiles.IndexOf(imageFile) == 0;

                            await _serviceImagenProducto.AddAsync(new ImagenProductoDTO
                            {
                                IdProducto = productoId,
                                Foto = memoryStream.ToArray(),
                                Principal = esPrincipal
                            });

                            // Si acabamos de asignar una principal, actualizamos la bandera
                            if (esPrincipal) PrimerPrincipal = true;
                        }
                    }
                }


                TempData["SuccessMessage"] = "Felicidades se actualizó!";
                return RedirectToAction("Editar", new { id = productoId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar: {ex.Message}");
                await DatosEViewBag(productoDto.IdProducto);
                return View(productoDto);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEImagen(int idProducto, int idImagen)
        {
            await _serviceImagenProducto.DeleteAsync(idImagen);
            TempData["SuccessMessage"] = "Se eliminó una imagen!";
            return RedirectToAction("Editar", new { id = idProducto });
        }

    }
}
