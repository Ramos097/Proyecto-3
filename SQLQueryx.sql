-- Crear la base de datos
CREATE DATABASE SeguridadMejorada2;
GO

-- Usar la base de datos
USE SeguridadMejorada2;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PrimerNombre VARCHAR(50),
    SegundoNombre VARCHAR(50),
    PrimerApellido VARCHAR(50),
    SegundoApellido VARCHAR(50),
    Direccion VARCHAR(255),
    Telefono VARCHAR(20),
    Clave VARCHAR(255),
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Tabla de FotosUsuarios
CREATE TABLE FotosUsuarios (
    FotoID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    NombreArchivo VARCHAR(255),
    Foto VARBINARY(MAX),
    FechaSubida DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE
);

-- Tabla de Roles
CREATE TABLE Roles (
    RolID INT PRIMARY KEY IDENTITY(1,1),
    NombreRol VARCHAR(50) NOT NULL UNIQUE
);

-- Tabla de Menús
CREATE TABLE Menus (
    MenuID INT PRIMARY KEY IDENTITY(1,1),
    NombreMenu VARCHAR(50) NOT NULL UNIQUE
);

-- Tabla de Permisos para Menús
CREATE TABLE Permisos_Menus (
    PermisoID INT PRIMARY KEY IDENTITY(1,1),
    MenuID INT NOT NULL,
    PermisoLectura BIT NOT NULL DEFAULT 0,
    PermisoEscritura BIT NOT NULL DEFAULT 0,
    PermisoModificacion BIT NOT NULL DEFAULT 0,
    PermisoEliminacion BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (MenuID) REFERENCES Menus(MenuID)
);

-- Relación entre Roles y Permisos
CREATE TABLE Roles_Permisos (
    RolID INT NOT NULL,
    PermisoID INT NOT NULL,
    PRIMARY KEY (RolID, PermisoID),
    FOREIGN KEY (RolID) REFERENCES Roles(RolID),
    FOREIGN KEY (PermisoID) REFERENCES Permisos_Menus(PermisoID)
);

-- Relación entre Usuarios y Roles
CREATE TABLE Usuario_Rol (
    UsuarioID INT NOT NULL,
    RolID INT NOT NULL,
    PRIMARY KEY (UsuarioID, RolID),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE,
    FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);

-- Tabla de Historial de Acciones de Usuarios
CREATE TABLE HistorialUsuarios (
    HistorialID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    Accion VARCHAR(255),
    FechaAccion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Auditoría de Cambios
CREATE TABLE AuditoriaCambios (
    AuditoriaID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT,
    Accion VARCHAR(255) NOT NULL,
    Detalles NVARCHAR(MAX),
    FechaHora DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

-- Insertar datos de prueba en Usuarios
INSERT INTO Usuarios (NombreUsuario, Email, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Direccion, Telefono, Clave)
VALUES 
('jgarcia', 'jgarcia@mail.com', 'Juan', NULL, 'Garcia', NULL, 'Calle Falsa 123', '555-1234', 'clave123'),
('mgonzalez', 'mgonzalez@mail.com', 'Maria', NULL, 'Gonzalez', NULL, 'Avenida Siempreviva 456', '555-5678', 'clave456'),
('alopez', 'alopez@mail.com', 'Ana', NULL, 'Lopez', NULL, 'Calle Luna 789', '555-8765', 'clave789');

-- Insertar datos de prueba en FotosUsuarios
INSERT INTO FotosUsuarios (UsuarioID, NombreArchivo)
VALUES 
(1, 'juan_garcia_foto.jpg'),
(2, 'maria_gonzalez_foto.jpg'),
(3, 'ana_lopez_foto.jpg');

-- Insertar datos de prueba en Roles
INSERT INTO Roles (NombreRol)
VALUES 
('Administrador'),
('Usuario'),
('Supervisor'),
('Auditor');

-- Insertar datos de prueba en Menús
INSERT INTO Menus (NombreMenu)
VALUES 
('Control de Menús'),
('Reportes'),
('Gestión de Usuarios'),
('Administración de Roles'),
('Auditoría de Cambios');

-- Insertar datos de prueba en Permisos_Menus
INSERT INTO Permisos_Menus (MenuID, PermisoLectura, PermisoEscritura, PermisoModificacion, PermisoEliminacion)
VALUES 
(1, 1, 1, 1, 1),
(2, 1, 0, 0, 0),
(3, 1, 1, 0, 0),
(4, 1, 1, 1, 0),
(5, 1, 0, 0, 0);

-- Relacionar Roles con Permisos
INSERT INTO Roles_Permisos (RolID, PermisoID)
SELECT 1, PermisoID FROM Permisos_Menus;

-- Relacionar Usuarios con Roles
INSERT INTO Usuario_Rol (UsuarioID, RolID)
VALUES 
(1, 1),
(2, 2),
(3, 3);

go

-- Consulta final para verificar los datos
SELECT * FROM Usuarios;
SELECT * FROM FotosUsuarios;
SELECT * FROM Roles;
SELECT * FROM Menus;
SELECT * FROM Permisos_Menus;
SELECT * FROM Roles_Permisos;
SELECT * FROM Usuario_Rol;
SELECT * FROM HistorialUsuarios;
SELECT * FROM AuditoriaCambios;

-- Modificación de la tabla Usuarios: agregar columna Foto
ALTER TABLE Usuarios
ADD Foto VARCHAR(255);

-- Eliminación de la tabla FotosUsuarios
DROP TABLE FotosUsuarios;
GO