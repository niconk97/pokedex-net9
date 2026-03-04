namespace Pokedex.Negocio;
using Pokedex.Dominio;

public class ElementoNegocio
{
    public List<Elemento> listar()
        {
            List<Elemento> lista = new List<Elemento>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Select Id, Descripcion From ELEMENTOS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Elemento aux = new Elemento();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

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
                //datos.cerrarConexion();
                datos.Dispose(); // Asegura que se liberen los recursos de conexión incluso si ocurre una excepción.
            }
        }

    // CAMBIO SQLITE: versión equivalente de listar para SQLite.
    public List<Elemento> listarSQLITE()
    {
        List<Elemento> lista = new List<Elemento>();
        AccesoDatosSQLITE datos = new AccesoDatosSQLITE();

        try
        {
            datos.setearConsulta("Select Id, Descripcion From ELEMENTOS");
            datos.ejecutarLectura();

            while (datos.Lector!.Read())
            {
                Elemento aux = new Elemento();
                aux.Id = Convert.ToInt32(datos.Lector["Id"]);
                aux.Descripcion = Convert.ToString(datos.Lector["Descripcion"])!;

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
}