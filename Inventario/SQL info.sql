--Para  crear la Base de Datos Sql Server, sentencias.
user_id='sa';
password='3572022@';
-- base de datos
create database CrudMVCRazor;

-- //cliente

CREATE TABLE cliente (
  id INT NOT NULL IDENTITY(1,1), -- Clave primaria con autoincremento
  cedula VARCHAR(255) NULL,      -- Número de cédula del cliente
  direccion VARCHAR(255) NULL,   -- Dirección del cliente
  nombre VARCHAR(255) NULL,      -- Nombre del cliente
  telefono VARCHAR(255) NULL,    -- Teléfono del cliente
  apellido VARCHAR(255) NULL,    -- Apellido del cliente
  email VARCHAR(255) NULL,       -- Correo electrónico del cliente
  fecha DATE NULL,               -- Fecha asociada
  fechaup DATE NULL,             -- Fecha de última actualización
  PRIMARY KEY (id)               -- Definición de clave primaria
);


-- //proveedor

CREATE TABLE proveedor (
  id INT NOT NULL IDENTITY(1,1),       -- Clave primaria con autoincremento
  celular VARCHAR(255) NULL,           -- Número de teléfono móvil del proveedor
  direccion VARCHAR(255) NULL,         -- Dirección del proveedor
  nombre VARCHAR(255) NULL,            -- Nombre del proveedor
  nit VARCHAR(255) NULL,               -- Número de identificación tributaria (NIT)
  email VARCHAR(255) NULL,             -- Correo electrónico del proveedor
  telefono_fijo VARCHAR(255) NULL,     -- Número de teléfono fijo del proveedor
  sitio_web VARCHAR(255) NULL,         -- Sitio web del proveedor
  ciudad VARCHAR(255) NULL,            -- Ciudad donde se encuentra el proveedor
  pais VARCHAR(255) NULL,              -- País del proveedor
  fecha_registro DATE NULL,            -- Fecha de registro del proveedor
  PRIMARY KEY (id)                     -- Definición de clave primaria
);


-- //producto 

CREATE TABLE producto (
  cantidad DECIMAL(38,2) NULL,         -- Cantidad del producto con precisión decimal
  id INT NOT NULL IDENTITY(1,1),       -- Clave primaria con autoincremento
  proveedor_id INT NULL,               -- Clave externa al proveedor
  valor DECIMAL(38,2) NULL,            -- Valor del producto con precisión decimal
  descripcion VARCHAR(255) NULL,       -- Descripción del producto
  estado VARCHAR(255) NULL,            -- Estado del producto
  fecha DATE NULL,                     -- Fecha asociada, ahora como tipo DATE
  nombre VARCHAR(255) NULL,            -- Nombre del producto
  barras VARCHAR(255) NULL,            -- Código de barras del producto
  categoria VARCHAR(255) NULL,         -- Categoría del producto
  costo DECIMAL(38,2) NULL,            -- Costo del producto con precisión decimal
  PRIMARY KEY (id),                    -- Definición de clave primaria
  FOREIGN KEY (proveedor_id) REFERENCES proveedor(id) -- Clave foránea hacia la tabla proveedor
);


-- //encabezado factura

CREATE TABLE factura (
  id INT NOT NULL IDENTITY(1,1),         -- Clave primaria con autoincremento
  cliente_id INT NULL,                   -- Clave externa que referencia al cliente
  fecha DATE NULL,                       -- Fecha de emisión de la factura
  fecha_pago DATE NULL,                      -- Fecha de pago en credito
  estado VARCHAR(255) NULL,              -- Estado de la factura (por ejemplo, 'pagada', 'pendiente')
  numero_factura VARCHAR(50) NULL,       -- Número de factura
  metodo_pago VARCHAR(255) NULL,         -- Método de pago (por ejemplo, 'efectivo', 'tarjeta de crédito')
  observaciones TEXT NULL,               -- Observaciones adicionales sobre la factura
  iva DECIMAL(18,2) NULL,          -- Monto de iva aplicado a la factura
  descuento DECIMAL(18,2) NULL,          -- Descuento aplicado a la factura
  base DECIMAL(18,2) NOT NULL,          -- Base de la factura
  total DECIMAL(18,2) NOT NULL,          -- Total de la factura
  PRIMARY KEY (id),                      -- Definición de clave primaria
  FOREIGN KEY (cliente_id) REFERENCES cliente(id) -- Clave foránea hacia la tabla cliente
);


-- //items de la factura
CREATE TABLE facturahasproducto (
  id INT NOT NULL IDENTITY(1,1),         -- Clave primaria con autoincremento
  factura_id INT NULL,                   -- Clave foránea que referencia a la factura
  producto_id INT NULL,                  -- Clave foránea que referencia al producto
  cantidad INT NOT NULL,                 -- Cantidad de productos
  precio_unitario DECIMAL(18,2) NOT NULL,-- Precio unitario del producto
  base DECIMAL(18,2) NULL,               -- Base imponible (cantidad * precio unitario)
  iva DECIMAL(5,2) NULL,                 -- Porcentaje de IVA aplicado
  valor_iva DECIMAL(18,2) NULL,          -- Monto de IVA aplicado
  descuento DECIMAL(5,2) NULL,           -- Porcentaje de descuento aplicado al producto
  valor_descuento DECIMAL(18,2) NULL,    -- Descuento aplicado al producto
  precio_total DECIMAL(18,2) NULL,       -- Precio total con descuento e impuestos
  PRIMARY KEY (id),                      -- Definición de clave primaria
  FOREIGN KEY (factura_id) REFERENCES factura(id), -- Clave foránea hacia la tabla factura
  FOREIGN KEY (producto_id) REFERENCES producto(id) -- Clave foránea hacia la tabla producto
);

-- TRIGGER para calculos de los items 

CREATE TRIGGER trg_calculo_factura_producto
ON facturahasproducto
AFTER INSERT, UPDATE
AS
BEGIN
  -- Actualiza los valores calculados después de una inserción o actualización
  UPDATE f
  SET 
    base = i.base,
    valor_iva = i.valor_iva,
    valor_descuento = i.valor_descuento,
    precio_total = i.precio_total
  FROM 
    facturahasproducto f
  INNER JOIN
    (
      SELECT 
        id,
        cantidad * precio_unitario AS base,
        (cantidad * precio_unitario) * (iva / 100) AS valor_iva,
        (cantidad * precio_unitario) * (descuento / 100) AS valor_descuento,
        (cantidad * precio_unitario) + ((cantidad * precio_unitario) * (iva / 100)) - ((cantidad * precio_unitario) * (descuento / 100)) AS precio_total
      FROM inserted
    ) i ON f.id = i.id;
END;
