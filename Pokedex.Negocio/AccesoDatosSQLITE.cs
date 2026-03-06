namespace Pokedex.Negocio;

using System;
using System.Data;
using Microsoft.Data.Sqlite;

public class AccesoDatosSQLITE : IDisposable
{
    // CAMBIO SQLITE: conexión a base SQLite local.
    // Busca pokedex.db en el directorio actual o en script_db
    private static string CadenaConexion
    {
        get
        {
            // Opción 1 (PRIORIDAD): Subir un nivel y buscar en Pokedex.Negocio/script_db
            var rutaNegocio = Path.Combine(Directory.GetCurrentDirectory(), "..", "Pokedex.Negocio", "script_db", "pokedex.db");
            if (File.Exists(rutaNegocio))
                return $"Data Source={Path.GetFullPath(rutaNegocio)}";

            // Opción 2: En script_db dentro del directorio actual
            var rutaScriptDb = Path.Combine(Directory.GetCurrentDirectory(), "script_db", "pokedex.db");
            if (File.Exists(rutaScriptDb))
                return $"Data Source={rutaScriptDb}";

            // Opción 3: En el directorio actual
            var rutaActual = Path.Combine(Directory.GetCurrentDirectory(), "pokedex.db");
            if (File.Exists(rutaActual))
                return $"Data Source={rutaActual}";

            // Si no encuentra, usa ruta relativa (se creará si no existe)
            return "Data Source=pokedex.db";
        }
    }

    private readonly SqliteConnection conexion;
    private readonly SqliteCommand comando;
    private SqliteDataReader? lector;

    public SqliteDataReader? Lector => lector;

    public AccesoDatosSQLITE()
    {
        conexion = new SqliteConnection(CadenaConexion);
        comando = new SqliteCommand();
    }

    public void setearConsulta(string consulta)
    {
        comando.Parameters.Clear();
        comando.CommandType = CommandType.Text;
        comando.CommandText = consulta;
    }

    public void ejecutarLectura()
    {
        comando.Connection = conexion;
        conexion.Open();
        lector = comando.ExecuteReader();
    }

    public void ejecutarAccion()
    {
        comando.Connection = conexion;
        conexion.Open();
        comando.ExecuteNonQuery();
    }

    public int ejecutarAccionScalar()
    {
        comando.Connection = conexion;
        conexion.Open();
        var result = comando.ExecuteScalar();
        return Convert.ToInt32(result);
    }

    public void setearParametro(string nombre, object valor)
    {
        comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
    }

    public void cerrarConexion()
    {
        if (lector is not null)
        {
            lector.Close();
            lector = null;
        }

        if (conexion.State == ConnectionState.Open)
            conexion.Close();
    }

    public void Dispose()
    {
        cerrarConexion();
        comando.Dispose();
        conexion.Dispose();
    }
}