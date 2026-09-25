use videoGame_db;
---Insert de categorias
INSERT INTO Categoria (categoria, foto)
SELECT 
    'Nintendo', 
    BulkColumn  
FROM OPENROWSET(BULK 'C:\VideoGame\nitendos.jpg', SINGLE_BLOB) AS image;

INSERT INTO Categoria (categoria, foto)
SELECT 
    'PlayStation 5', 
    BulkColumn  
FROM OPENROWSET(BULK 'C:\VideoGame\play5.jpg', SINGLE_BLOB) AS image;

INSERT INTO Categoria (categoria, foto)
SELECT 
    'PlayStation 4', 
    BulkColumn  
FROM OPENROWSET(BULK 'C:\VideoGame\play4.jpg', SINGLE_BLOB) AS image;

INSERT INTO Categoria (categoria, foto)
SELECT 
    'Xbox', 
    BulkColumn  
FROM OPENROWSET(BULK 'C:\VideoGame\xbox.png', SINGLE_BLOB) AS image;

---Insert Producto Nintendo
Insert into Producto(idCategoria, nombre,descripcion,precio,stock) Values
(1,'Control Alámbrico Kirby', 'Mejora tus sesiones durante el juego con mayor comodidad con este control alámbrico, incluye cable de carga.',18000,3),
(1,'Control Alámbrico Minecraft Grass Switch', 'Controlador ergonómico con diseño de botón estándar y diseño de bloques de césped Minecraft.', 18000,3),
(1, 'Control ergonómico Switch','Diseñado con un joystick de efecto hall para cero deriva y zonas muertas, este controlador inalámbrico Switch/Oled ofrece una precisión precisa y un control suave.',31000,3),

--Insert Productos PlayStation 5
(2, 'NBA 2K25 - PlayStation 5', ' Acércate a tus superestrellas favoritas de la NBA y sumérgete en momentos de embrague, ya que ProPLAY potencia la experiencia más auténtica de la NBA hasta la fecha.', 8000,5),
(2, 'The Last of Us Parte 1', 'Completamente reconstruido desde cero utilizando la última tecnología de motor PS5 de Naughty Dog para mejorar cada detalle visual y la experiencia.', 15000,10),
(2, 'The Last of Us Parte II', 'Completamente reconstruido desde cero utilizando la última tecnología de motor PS5 de Naughty Dog para mejorar cada detalle visual y la experiencia.', 15000,10),

--Insert Producto play 4
(3,'Demon Slayer: The Hinokami Chronicles - PlayStation 4', 'Es el período Taisho, un niño de buen corazón que vende carbón para ganarse la vida, encuentra a su familia asesinada por un demonio.', 1000,3),
(3,'One Piece Odyssey - PlayStation 4','ONE PIECE ODYSSEY, elementos únicos de aventura de ONE PIECE que ha sido muy deseado por los fanáticos', 14000,5),
(3,'PlayStation 4', 'Incluye un nuevo sistema PlayStation4 delgado de 500 GB, un controlador inalámbrico DualShock 4 a juego.',115000,2),

--Insert Productos xbox
(4, 'Black – Play on Xbox','Control inalámbrico Xbox, con superficies esculpidas y geometría refinada para mayor comodidad durante el juego con una duración de batería de hasta 40 horas',31000,2),
(4,'Wireless Controller for Xbox', 'Sumérgete en aventuras de juego inmejorables con nuestro control inalámbrico para Xbox.',25000,2),
(4,'Blades of Time - Xbox 360','Experimente lugares de juego enormes y bellamente realizados, incluyendo tierras nevadas, selvas, ciudades antiguas, templos e islas del cielo.', 20000,2);

--Insert ImagenProducto
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    1, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\nitendo1.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    1, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\nitendo2.jpg', SINGLE_BLOB) AS image;

--Insert producto 2
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    2, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\nitendoM1.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    2, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\nitendoM2.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    2, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\nitendoM3.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    3, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\NitendoE1.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    3, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\NitendoE2.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    3, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\NitendoE3.jpg', SINGLE_BLOB) AS image;

--Insert producto 4
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    4, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\nba1.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    4, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\nba2.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    4, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\nba3.jpg', SINGLE_BLOB) AS image;

--Insert producto 5

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    5, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\play51.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    5, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\play52.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    5, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\play53.jpg', SINGLE_BLOB) AS image;

-- Insert Producto 6
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    6, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\play521.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    6, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\play522.jpg', SINGLE_BLOB) AS image;

--Insert Producto 7
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    7, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\play4A1.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    7, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\play4A2.jpg', SINGLE_BLOB) AS image;

---Insert producto 8

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    8, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\play4A12.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    8, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\play4A11.jpg', SINGLE_BLOB) AS image;

--Insert producto 9 
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    9, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\play4V1.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    9, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\play4V2.jpg', SINGLE_BLOB) AS image;

--Insert producto 10 

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    10, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\xboxC1.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    10, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\xboxC2.jpg', SINGLE_BLOB) AS image;

--producto 11
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    11, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\xbxC21.jpg', SINGLE_BLOB) AS image;

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    11, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\xboxC22.jpg', SINGLE_BLOB) AS image;

--Insert producto 12
INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    11, 
    BulkColumn,
	1
FROM OPENROWSET(BULK 'C:\VideoGame\xboxV1.jpg', SINGLE_BLOB) AS image; 

INSERT INTO ImagenProducto(idProducto, foto,principal)
SELECT 
    11, 
    BulkColumn,
	0
FROM OPENROWSET(BULK 'C:\VideoGame\xboxV2.jpg', SINGLE_BLOB) AS image;

---Insert Etiquetas

Insert Into Etiqueta(etiqueta) Values
('Nuevo'), ('Temporada'),('Más Vendidos'), ('+18'),('Deportes');

--Insert Etiquetas Productos
Insert Into EtiquetaProducto(idProducto,idEtiqueta) Values
(5,2), (6,2),(1,1), (4,5),(12,4);

---Insert TipoPromocion

Insert Into TipoPromocion(tipo) Values 
('Producto'), ('Descuento')

--Insert Promoción
Insert Into Promocion(idTipoPromocion,nombre, descripcion, descuento, fechaInicio, fechaFin) Values
(1, 'Descuento por tiempo limitado', '¡Solo por hoy! 15% de descuento', 0.15, '2025-07-25','2025-07-25'),
(1, 'Envío gratuito', 'Envío gratuito en pedidos mayores a la compra de 30000 colones', 0.10, '2025-08-02','2025-08-10'),
(1, 'Promociones de temporada', 'Tendras promociones especiales por temporadas de video juegos', 0.15, '2025-08-02','2025-10-10'),
(2, 'Descuentos en Xbox', 'Descuentos en todos los productos de XBox un 10%', 0.10, '2025-07-25','2025-07-30');

--Insert Promocion x Producto
Insert Into PromocionProducto(idPromocion,idProducto) Values
(1,10),(2,3), (3,5);
--Insert Promocion x Categoria
Insert Into PromocionCategoria(idPromocion,idCategoria) Values
(4,4)


--Insert Rol
Insert into Rol(rol) Values('Administrador'),('Cliente')

--Insert Usuario
Insert into Usuario(idRol, nombre, correo, contrasena, pais, telefono) Values 
(1, 'Henderry Moscat Bendict', 'henderry@gmail.com','123456','Costa Rica', '60524896'),
(2, 'Pablo Arias', 'pablo@gmail.com','123456','Costa Rica', '60584892'),
(2, 'Leonela Alfaro', 'leo@gmail.com','123456','Costa Rica', '65574862')


---Insert Reseñas
Insert into Resena(idUsuario, idProducto, comentario, valoracion) Values
(2,5,'Recomiendo el video juego muy emocionante y efectos especiales', 5),
(3,1, 'El diseño es hermoso y tierno', 4)