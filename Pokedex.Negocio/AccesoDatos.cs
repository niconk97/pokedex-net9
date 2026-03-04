namespace Pokedex.Negocio;

using System;
using System.Data;
using Microsoft.Data.SqlClient;


public class AccesoDatos : IDisposable // Implementamos IDisposable para asegurar que los recursos se liberen correctamente.
{
    // En .NET 9 usamos Microsoft.Data.SqlClient (paquete moderno y recomendado).
    // En .NET Framework normalmente se usaba System.Data.SqlClient.
    private readonly SqlConnection conexion;

    // Objeto comando reutilizable para ejecutar consultas SQL y procedimientos almacenados.
    private readonly SqlCommand comando;

    // Lector de solo avance para resultados de consultas (SELECT).
    private SqlDataReader? lector;

    // Propiedad de lectura para exponer el SqlDataReader a otras clases.
    // Igual que en la versión anterior, pero respetando nullable reference types de .NET moderno.
    public SqlDataReader? Lector => lector;

    // Cadena de conexión hardcodeada (solo para desarrollo/pruebas) en MVC debería ser inyectada desde configuración, pero aca la dejamos así para mantener una estructura similar a la versión .NET framework.
    private const string CadenaConexion = "Server=.\\SQLEXPRESS;Database=POKEDEX_DB;Integrated Security=True;TrustServerCertificate=True";

    // Constructor sin parámetros: usa la cadena hardcodeada
    public AccesoDatos()
    {
        conexion = new SqlConnection(CadenaConexion);
        comando = new SqlCommand();
    }

    // Configura una consulta SQL de texto plano (ej: "SELECT * FROM ...").
    // Similar al método de antes, agregando limpieza de parámetros para evitar arrastre entre ejecuciones.
    public void setearConsulta(string consulta)
    {
        // Limpia parámetros previos si el mismo objeto se reutiliza en otra consulta.
        comando.Parameters.Clear();
        comando.CommandType = CommandType.Text; // Asegura que el tipo de comando sea texto plano.
        comando.CommandText = consulta; // Asigna la consulta SQL al comando.
    }

    // Configura un procedimiento almacenado.
    // Misma idea que en .NET Framework: cambia CommandType y define nombre del SP.
    public void setearProcedimiento(string sp)
    {
        comando.Parameters.Clear(); // Limpia parámetros previos para evitar conflictos.
        comando.CommandType = CommandType.StoredProcedure; // Cambia el tipo de comando a StoredProcedure para indicar que se ejecutará un SP.
        comando.CommandText = sp; // Asigna el nombre del procedimiento almacenado al comando.
    }

    // Ejecuta una lectura (SELECT) y deja disponible el SqlDataReader en la propiedad Lector.
    // Patrón equivalente al anterior, solo actualizado al cliente SQL moderno.
    public void ejecutarLectura()
    {
        comando.Connection = conexion; // Asigna la conexión al comando justo antes de ejecutar.
        try
        {
            // Abre conexión justo antes de ejecutar.
            conexion.Open();
            lector = comando.ExecuteReader();
        }
        catch
        {
            // Conserva el stack trace original (mejor que throw ex; de .NET Framework clásico porque no pierde información de la excepción original).
            throw;
        }
    }

    // Ejecuta una acción que no devuelve filas (INSERT, UPDATE, DELETE).
    // Equivalente directo de ExecuteNonQuery en la implementación en .NET framework.
    public void ejecutarAccion()
    {
        comando.Connection = conexion;
        try
        {
            conexion.Open();
            comando.ExecuteNonQuery();
        }
        catch
        {
            throw;
        }
    }

    // Ejecuta una acción escalar (retorna un único valor, por ejemplo un ID).
    // Antes probablemente hacías int.Parse(ExecuteScalar().ToString());
    // aquí usamos Convert.ToInt32 para manejar conversiones de forma más robusta.
    public int ejecutarAccionScalar()
    {
        comando.Connection = conexion;
        try
        {
            conexion.Open();
            var result = comando.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        catch
        {
            throw;
        }
    }

    // Agrega un parámetro al comando actual.
    // En .NET Framework se usaba igual AddWithValue; aquí además convertimos null a DBNull.Value para evitar errores comunes al pasar valores nulos.
    public void setearParametro(string nombre, object valor)
    {
        comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value); // Si valor es null, se convierte a DBNull.Value para que SQL lo interprete correctamente como NULL.
    }

    // Cierra lector y conexión de forma segura.
    // Mantiene la lógica de la versión anterior, agregando chequeo de estado de conexión.
    public void cerrarConexion()
    {
        if (lector is not null)
        {
            lector.Close();
            lector = null;
        }

        if (conexion.State == ConnectionState.Open)
        {
            conexion.Close();
        }
    }

    // Implementación de IDisposable.
    // En .NET Framework muchas veces esto no se implementaba explícitamente;
    // en .NET moderno es una buena práctica para liberar recursos no administrados rápidamente.
    public void Dispose()
    {
        cerrarConexion();
        comando.Dispose();
        conexion.Dispose();
    }
}
