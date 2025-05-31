using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Data;

public partial class VideoGameContext : DbContext
{
    public VideoGameContext(DbContextOptions<VideoGameContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Etiqueta> Etiqueta { get; set; }

    public virtual DbSet<EtiquetaProducto> EtiquetaProducto { get; set; }

    public virtual DbSet<ImagenProducto> ImagenProducto { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Promocion> Promocion { get; set; }

    public virtual DbSet<PromocionCategoria> PromocionCategoria { get; set; }

    public virtual DbSet<PromocionProducto> PromocionProducto { get; set; }

    public virtual DbSet<Resena> Resena { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<TipoPromocion> TipoPromocion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__8A3D240CFFB0D3FD");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Categoria1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Foto).HasColumnName("foto");
        });

        modelBuilder.Entity<Etiqueta>(entity =>
        {
            entity.HasKey(e => e.IdEtiqueta).HasName("PK__Etiqueta__3C1526A742B7CC64");

            entity.HasIndex(e => e.Etiqueta1, "UQ__Etiqueta__6EEABC063DFC2E8A").IsUnique();

            entity.Property(e => e.IdEtiqueta).HasColumnName("idEtiqueta");
            entity.Property(e => e.Etiqueta1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("etiqueta");
        });

        modelBuilder.Entity<EtiquetaProducto>(entity =>
        {
            entity.ToTable("EtiquetaProducto");

            entity.Property(e => e.IdEtiqueta).HasColumnName("idEtiqueta");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");

            entity.HasOne(d => d.IdEtiquetaNavigation).WithMany()
                .HasForeignKey(d => d.IdEtiqueta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EtiquetaP__idEti__4316F928");

            entity.HasOne(d => d.IdProductoNavigation).WithMany()
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EtiquetaP__idPro__4222D4EF");
        });

        modelBuilder.Entity<ImagenProducto>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("PK__ImagenPr__EA9A71361C2E1158");

            entity.Property(e => e.IdImagen).HasColumnName("idImagen");
            entity.Property(e => e.Foto).HasColumnName("foto");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Principal).HasColumnName("principal");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ImagenProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImagenPro__idPro__3D5E1FD2");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__07F4A1326DC5A044");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Producto__idCate__3A81B327");
        });

        modelBuilder.Entity<Promocion>(entity =>
        {
            entity.HasKey(e => e.IdPromocion).HasName("PK__Promocio__811C0F99DD171914");

            entity.Property(e => e.IdPromocion).HasColumnName("idPromocion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("descuento");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.IdTipoPromocion).HasColumnName("idTipoPromocion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdTipoPromocionNavigation).WithMany(p => p.Promocion)
                .HasForeignKey(d => d.IdTipoPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promocion__idTip__47DBAE45");
        });

        modelBuilder.Entity<PromocionCategoria>(entity =>
        {
            entity.ToTable("PromocionCategoria");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdPromocion).HasColumnName("idPromocion");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany()
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promocion__idCat__4CA06362");

            entity.HasOne(d => d.IdPromocionNavigation).WithMany()
                .HasForeignKey(d => d.IdPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promocion__idPro__4D94879B");
        });

        modelBuilder.Entity<PromocionProducto>(entity =>
        {
            entity.ToTable("PromocionProducto");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdPromocion).HasColumnName("idPromocion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany()
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promocion__idPro__49C3F6B7");

            entity.HasOne(d => d.IdPromocionNavigation).WithMany()
                .HasForeignKey(d => d.IdPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promocion__idPro__4AB81AF0");
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.IdResena).HasName("PK__Resena__9177AE465435567C");

            entity.Property(e => e.IdResena).HasColumnName("idResena");
            entity.Property(e => e.Comentario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Valoracion).HasColumnName("valoracion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Resena)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resena__idProduc__59063A47");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Resena)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resena__idUsuari__5812160E");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__3C872F765AD0C9C2");

            entity.HasIndex(e => e.Rol1, "UQ__Rol__C2B79D264D5EF1E4").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Rol1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<TipoPromocion>(entity =>
        {
            entity.HasKey(e => e.IdTipoPromocion).HasName("PK__TipoProm__C553ECC85E17A117");

            entity.Property(e => e.IdTipoPromocion).HasColumnName("idTipoPromocion");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A63F760505");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__2A586E0BD14B83E7").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Pais)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pais");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__idRol__5441852A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
