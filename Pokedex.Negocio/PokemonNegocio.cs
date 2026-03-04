namespace Pokedex.Negocio;
using Pokedex.Dominio;
using Microsoft.Data.SqlClient;
public class PokemonNegocio
{

    public List<Pokemon> listar(string id = "")
    {
        List<Pokemon> lista = new List<Pokemon>();
        AccesoDatos datos = new AccesoDatos();

        try
        {
            string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id, P.Activo From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad ";
            datos.setearConsulta(consulta);
            datos.ejecutarLectura();

            while (datos.Lector.Read())
            {
                Pokemon aux = new Pokemon();
                aux.Id = (int)datos.Lector["Id"];
                aux.Numero = datos.Lector.GetInt32(0);
                aux.Nombre = (string)datos.Lector["Nombre"];
                aux.Descripcion = (string)datos.Lector["Descripcion"];

                //if(!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))
                //    aux.UrlImagen = (string)lector["UrlImagen"];
                if(!(datos.Lector["UrlImagen"] is DBNull))
                    aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                aux.Tipo = new Elemento();
                aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                aux.Debilidad = new Elemento();
                aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                aux.Activo = bool.Parse(datos.Lector["Activo"].ToString());

                lista.Add(aux);
            }

            return lista;

        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose(); // Asegura que se liberen los recursos de conexión incluso si ocurre una excepción.
        }
    }

    public void agregar(Pokemon nuevo)
    {
        AccesoDatos datos = new AccesoDatos();

        try
        {
            datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @urlImagen)");
            datos.setearParametro("@idTipo", nuevo.Tipo.Id);
            datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
            datos.setearParametro("@urlImagen", nuevo.UrlImagen);
            datos.ejecutarAccion();
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    public void modificar(Pokemon poke)
    {
        AccesoDatos datos = new AccesoDatos();
        try
        {
            datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad Where Id = @id");
            datos.setearParametro("@numero", poke.Numero);
            datos.setearParametro("@nombre", poke.Nombre);
            datos.setearParametro("@desc", poke.Descripcion);
            datos.setearParametro("@img", poke.UrlImagen);
            datos.setearParametro("@idTipo", poke.Tipo.Id);
            datos.setearParametro("@idDebilidad", poke.Debilidad.Id);
            datos.setearParametro("@id", poke.Id);

            datos.ejecutarAccion();
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    public List<Pokemon> filtrar(string campo, string criterio, string filtro, string estado)
    {
        List<Pokemon> lista = new List<Pokemon>();
        AccesoDatos datos = new AccesoDatos();
        try
        {
            string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id, P.Activo From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And ";
            if(campo == "Número")
            {
                switch (criterio)
                {
                    case "Mayor a":
                        consulta += "Numero > " + filtro;
                        break;
                    case "Menor a":
                        consulta += "Numero < " + filtro;
                        break;
                    default:
                        consulta += "Numero = " + filtro;
                        break;
                }
            }
            else if(campo == "Nombre")
            {
                switch (criterio)
                {
                    case "Comienza con":
                        consulta += "Nombre like '" + filtro + "%' ";
                        break;
                    case "Termina con":
                        consulta += "Nombre like '%" + filtro + "'";
                        break;
                    default:
                        consulta += "Nombre like '%" + filtro + "%'";
                        break;
                }
            }
            else
            {
                switch (criterio)
                {
                    case "Comienza con":
                        consulta += "E.Descripcion like '" + filtro + "%' ";
                        break;
                    case "Termina con":
                        consulta += "E.Descripcion like '%" + filtro + "'";
                        break;
                    default:
                        consulta += "E.Descripcion like '%" + filtro + "%'";
                        break;
                }
            }

            if (estado == "Activo")
                consulta += " and P.Activo = 1";
            else if (estado == "Inactivo")
                consulta += " and P.Activo = 0";

            datos.setearConsulta(consulta);
            datos.ejecutarLectura();
            while (datos.Lector.Read())
            {
                Pokemon aux = new Pokemon();
                aux.Id = (int)datos.Lector["Id"];
                aux.Numero = datos.Lector.GetInt32(0);
                aux.Nombre = (string)datos.Lector["Nombre"];
                aux.Descripcion = (string)datos.Lector["Descripcion"];
                if (!(datos.Lector["UrlImagen"] is DBNull))
                    aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                aux.Tipo = new Elemento();
                aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                aux.Debilidad = new Elemento();
                aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                aux.Activo = bool.Parse(datos.Lector["Activo"].ToString());

                lista.Add(aux);
            }

            return lista;
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }
    public void eliminar(int id)
    {
        AccesoDatos datos = new AccesoDatos();
        try
        {
            datos.setearConsulta("delete from pokemons where id = @id");
            datos.setearParametro("@id",id);
            datos.ejecutarAccion();

        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    public void eliminarLogico(int id, bool activo = false)
    {
        AccesoDatos datos = new AccesoDatos();
        try
        {
            datos.setearConsulta("update POKEMONS set Activo = @activo Where id = @id");
            datos.setearParametro("@id", id);
            datos.setearParametro("@activo", activo);
            datos.ejecutarAccion();
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    // CAMBIO SQLITE: versión equivalente de listar para SQLite.
    public List<Pokemon> listarSQLITE(string id = "")
    {
        List<Pokemon> lista = new List<Pokemon>();
        AccesoDatosSQLITE datos = new AccesoDatosSQLITE();

        try
        {
            string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id, P.Activo From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad ";
            datos.setearConsulta(consulta);
            datos.ejecutarLectura();

            while (datos.Lector!.Read())
            {
                Pokemon aux = new Pokemon();
                aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                aux.Numero = Convert.ToInt32(datos.Lector["Numero"]);
                aux.Nombre = Convert.ToString(datos.Lector["Nombre"])!;
                aux.Descripcion = Convert.ToString(datos.Lector["Descripcion"])!;

                if (!(datos.Lector["UrlImagen"] is DBNull))
                    aux.UrlImagen = Convert.ToString(datos.Lector["UrlImagen"]);

                aux.Tipo = new Elemento();
                aux.Tipo.Id = Convert.ToInt32(datos.Lector["IdTipo"]);
                aux.Tipo.Descripcion = Convert.ToString(datos.Lector["Tipo"])!;
                aux.Debilidad = new Elemento();
                aux.Debilidad.Id = Convert.ToInt32(datos.Lector["IdDebilidad"]);
                aux.Debilidad.Descripcion = Convert.ToString(datos.Lector["Debilidad"])!;

                // CAMBIO SQLITE: Activo suele venir como 0/1.
                aux.Activo = Convert.ToInt32(datos.Lector["Activo"]) == 1;

                lista.Add(aux);
            }

            return lista;
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    // CAMBIO SQLITE: versión equivalente de agregar para SQLite.
    public void agregarSQLITE(Pokemon nuevo)
    {
        AccesoDatosSQLITE datos = new AccesoDatosSQLITE();

        try
        {
            // CAMBIO SQLITE: consulta parametrizada completa.
            datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen) values (@numero, @nombre, @descripcion, 1, @idTipo, @idDebilidad, @urlImagen)");
            datos.setearParametro("@numero", nuevo.Numero);
            datos.setearParametro("@nombre", nuevo.Nombre);
            datos.setearParametro("@descripcion", nuevo.Descripcion);
            datos.setearParametro("@idTipo", nuevo.Tipo.Id);
            datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
            datos.setearParametro("@urlImagen", nuevo.UrlImagen);
            datos.ejecutarAccion();
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    // CAMBIO SQLITE: versión equivalente de modificar para SQLite.
    public void modificarSQLITE(Pokemon poke)
    {
        AccesoDatosSQLITE datos = new AccesoDatosSQLITE();
        try
        {
            datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad Where Id = @id");
            datos.setearParametro("@numero", poke.Numero);
            datos.setearParametro("@nombre", poke.Nombre);
            datos.setearParametro("@desc", poke.Descripcion);
            datos.setearParametro("@img", poke.UrlImagen);
            datos.setearParametro("@idTipo", poke.Tipo.Id);
            datos.setearParametro("@idDebilidad", poke.Debilidad.Id);
            datos.setearParametro("@id", poke.Id);

            datos.ejecutarAccion();
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    // CAMBIO SQLITE: versión equivalente de filtrar para SQLite.
    public List<Pokemon> filtrarSQLITE(string campo, string criterio, string filtro, string estado)
    {
        List<Pokemon> lista = new List<Pokemon>();
        AccesoDatosSQLITE datos = new AccesoDatosSQLITE();
        try
        {
            string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id, P.Activo From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And ";

            if (campo == "Número")
            {
                switch (criterio)
                {
                    case "Mayor a":
                        consulta += "Numero > " + filtro;
                        break;
                    case "Menor a":
                        consulta += "Numero < " + filtro;
                        break;
                    default:
                        consulta += "Numero = " + filtro;
                        break;
                }
            }
            else if (campo == "Nombre")
            {
                switch (criterio)
                {
                    case "Comienza con":
                        consulta += "Nombre like '" + filtro + "%' ";
                        break;
                    case "Termina con":
                        consulta += "Nombre like '%" + filtro + "'";
                        break;
                    default:
                        consulta += "Nombre like '%" + filtro + "%'";
                        break;
                }
            }
            else
            {
                switch (criterio)
                {
                    case "Comienza con":
                        consulta += "E.Descripcion like '" + filtro + "%' ";
                        break;
                    case "Termina con":
                        consulta += "E.Descripcion like '%" + filtro + "'";
                        break;
                    default:
                        consulta += "E.Descripcion like '%" + filtro + "%'";
                        break;
                }
            }

            if (estado == "Activo")
                consulta += " and P.Activo = 1";
            else if (estado == "Inactivo")
                consulta += " and P.Activo = 0";

            datos.setearConsulta(consulta);
            datos.ejecutarLectura();
            while (datos.Lector!.Read())
            {
                Pokemon aux = new Pokemon();
                aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                aux.Numero = Convert.ToInt32(datos.Lector["Numero"]);
                aux.Nombre = Convert.ToString(datos.Lector["Nombre"])!;
                aux.Descripcion = Convert.ToString(datos.Lector["Descripcion"])!;

                if (!(datos.Lector["UrlImagen"] is DBNull))
                    aux.UrlImagen = Convert.ToString(datos.Lector["UrlImagen"]);

                aux.Tipo = new Elemento();
                aux.Tipo.Id = Convert.ToInt32(datos.Lector["IdTipo"]);
                aux.Tipo.Descripcion = Convert.ToString(datos.Lector["Tipo"])!;
                aux.Debilidad = new Elemento();
                aux.Debilidad.Id = Convert.ToInt32(datos.Lector["IdDebilidad"]);
                aux.Debilidad.Descripcion = Convert.ToString(datos.Lector["Debilidad"])!;

                // CAMBIO SQLITE: Activo suele venir como 0/1.
                aux.Activo = Convert.ToInt32(datos.Lector["Activo"]) == 1;

                lista.Add(aux);
            }

            return lista;
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    // CAMBIO SQLITE: versión equivalente de eliminar para SQLite.
    public void eliminarSQLITE(int id)
    {
        AccesoDatosSQLITE datos = new AccesoDatosSQLITE();
        try
        {
            datos.setearConsulta("delete from pokemons where id = @id");
            datos.setearParametro("@id", id);
            datos.ejecutarAccion();
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    // CAMBIO SQLITE: versión equivalente de eliminación lógica para SQLite.
    public void eliminarLogicoSQLITE(int id, bool activo = false)
    {
        AccesoDatosSQLITE datos = new AccesoDatosSQLITE();
        try
        {
            datos.setearConsulta("update POKEMONS set Activo = @activo Where id = @id");
            datos.setearParametro("@id", id);
            datos.setearParametro("@activo", activo ? 1 : 0);
            datos.ejecutarAccion();
        }
        catch
        {
            throw;
        }
        finally
        {
            datos.Dispose();
        }
    }

    // TO DO: Implementar métodos con procedimientos almacenados (SP) para listar, agregar y modificar pokemons, manteniendo la lógica de negocio pero delegando la ejecución de consultas a SP definidos en la base de datos. Esto mejora la seguridad y el rendimiento al evitar concatenación de strings para consultas SQL.
    // public List<Pokemon> listarConSP()
    // public void agregarConSP(Pokemon nuevo)
    // public void modificarConSP(Pokemon poke)

}