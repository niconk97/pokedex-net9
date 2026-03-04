-- Script de creación de base de datos SQLite para Pokedex
-- Adaptado desde SQL Server POKEDEX_DB.SQL
-- Cambios principales:
-- - IDENTITY(1,1) -> AUTOINCREMENT
-- - [bit] -> INTEGER (0/1 para booleanos)
-- - Sintaxis de constraints simplificada
-- - Eliminación de opciones específicas de SQL Server (PAD_INDEX, etc.)

-- En SQLite no se usa "CREATE DATABASE", el archivo .db ya representa la base de datos
-- El archivo será: pokedex.db

-- Tabla ELEMENTOS (tipos de pokemon: Planta, Fuego, Agua, etc.)
CREATE TABLE IF NOT EXISTS ELEMENTOS (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Descripcion TEXT
);

-- Tabla POKEMONS
CREATE TABLE IF NOT EXISTS POKEMONS (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Numero INTEGER,
    Nombre TEXT,
    Descripcion TEXT,
    UrlImagen TEXT,
    IdTipo INTEGER,
    IdDebilidad INTEGER,
    IdEvolucion INTEGER,
    Activo INTEGER DEFAULT 1, -- SQLite usa 0/1 para booleanos (bit en SQL Server)
    
    -- Foreign Keys
    FOREIGN KEY (IdTipo) REFERENCES ELEMENTOS(Id),
    FOREIGN KEY (IdDebilidad) REFERENCES ELEMENTOS(Id),
    FOREIGN KEY (IdEvolucion) REFERENCES POKEMONS(Id)
);

-- Datos semilla para ELEMENTOS
INSERT INTO ELEMENTOS (Descripcion) VALUES ('Planta');
INSERT INTO ELEMENTOS (Descripcion) VALUES ('Fuego');
INSERT INTO ELEMENTOS (Descripcion) VALUES ('Agua');

-- Datos semilla para POKEMONS
INSERT INTO POKEMONS (Numero, Nombre, Descripcion, UrlImagen, IdTipo, IdDebilidad, IdEvolucion, Activo) 
VALUES (1, 'Bulbasaur', 'Este Pokémon nace con una semilla en el lomo.', 'https://assets.pokemon.com/assets/cms2/img/pokedex/full/001.png', 1, 2, NULL, 1);

INSERT INTO POKEMONS (Numero, Nombre, Descripcion, UrlImagen, IdTipo, IdDebilidad, IdEvolucion, Activo) 
VALUES (4, 'Charmander', 'Pokemon de fuego', 'https://assets.pokemon.com/assets/cms2/img/pokedex/full/004.png', 2, 3, NULL, 1);

INSERT INTO POKEMONS (Numero, Nombre, Descripcion, UrlImagen, IdTipo, IdDebilidad, IdEvolucion, Activo) 
VALUES (11, 'Butterfree', 'mariposa', 'https://assets.pokemon.com/assets/cms2/img/pokedex/full/012.png', 1, 1, NULL, 1);

INSERT INTO POKEMONS (Numero, Nombre, Descripcion, UrlImagen, IdTipo, IdDebilidad, IdEvolucion, Activo) 
VALUES (15, 'Pidgey', 'Voladorrrrr', 'https://assets.pokemon.com/assets/cms2/img/pokedex/full/016.png', 2, 1, NULL, 1);
