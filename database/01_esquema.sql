CREATE DATABASE videoGame_db;
use videoGame_db;


CREATE TABLE Categoria (
idCategoria INT PRIMARY KEY IDENTITY(1,1),
categoria VARCHAR(50),
foto varbinary(max) Null,
);

CREATE TABLE Producto (
idProducto INT PRIMARY KEY IDENTITY(1,1),
idCategoria INT NOT NULL,
nombre VARCHAR(100),
descripcion VARCHAR(255),
precio DECIMAL(10,2),
stock INT NOT NULL DEFAULT 0,
FOREIGN KEY (idCategoria) REFERENCES Categoria(idCategoria)
);

CREATE TABLE ImagenProducto (
idImagen INT PRIMARY KEY IDENTITY (1,1),
idProducto INT NOT NULL,
foto varbinary(max) Null,
principal Bit,
FOREIGN KEY (idProducto) REFERENCES Producto(idProducto)
);

CREATE TABLE Etiqueta (
idEtiqueta INT PRIMARY KEY IDENTITY (1,1),
etiqueta VARCHAR(50) NOT NULL UNIQUE,
);

--Tabla muchos a muchos
Create table EtiquetaProducto(
idProducto int NOT NULL,
idEtiqueta INT NOT NULL,
FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
FOREIGN KEY (idEtiqueta) REFERENCES Etiqueta(idEtiqueta)
)

Create table TipoPromocion(
idTipoPromocion int Primary key Identity(1,1),
tipo varchar(50),
);

CREATE TABLE Promocion (
idPromocion INT PRIMARY KEY IDENTITY(1,1),
idTipoPromocion int Not null,
nombre VARCHAR(100),
descripcion VARCHAR(255),
descuento DECIMAL(5,2), 
fechaInicio DATETIME,
fechaFin DATETIME,
FOREIGN KEY (idTipoPromocion) REFERENCES TipoPromocion(idTipoPromocion),
);

-- Tabla muchos a muchos
CREATE TABLE PromocionProducto (
idPromocion INT NOT NULL, 
idProducto INT NOT NULL,
FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
FOREIGN KEY (idPromocion) REFERENCES Promocion(idPromocion)
);

-- Tabla muchos a muchos
CREATE TABLE PromocionCategoria (
idPromocion INT NOT NULL, 
idCategoria INT NOT NULL,
FOREIGN KEY (idCategoria) REFERENCES Categoria(idCategoria),
FOREIGN KEY (idPromocion) REFERENCES Promocion(idPromocion)
);

CREATE TABLE Rol (
idRol INT PRIMARY KEY IDENTITY(1,1),
rol VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Usuario (
idUsuario INT PRIMARY KEY IDENTITY(1,1),
idRol INT NOT NULL,
nombre VARCHAR(100),
correo VARCHAR(100) NOT NULL UNIQUE,
contrasena VARCHAR(255) NOT NULL, 
pais varchar(100),
telefono VARCHAR(20),
FOREIGN KEY (idRol) REFERENCES Rol(idRol)
);

CREATE TABLE Resena (
idResena INT PRIMARY KEY IDENTITY(1,1),
idUsuario INT NOT NULL,
idProducto INT NOT NULL,
fecha DATETIME DEFAULT GETDATE(),
comentario VARCHAR(255),
valoracion TINYINT NOT NULL,
FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario),
FOREIGN KEY (idProducto) REFERENCES Producto(idProducto)
);