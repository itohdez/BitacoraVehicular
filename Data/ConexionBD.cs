using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace BitacoraVehicular.Data;

public class ConexionBD
{
    private readonly string _cadenaConexion;

    public ConexionBD()
    {
        // Guarda el archivo .db junto al ejecutable para asegurar persistencia
        string rutaDb = Path.Combine(AppContext.BaseDirectory, "bitacora.db");
        _cadenaConexion = $"Data Source={rutaDb}";
        InicializarBaseDeDatos();
    }

    public SqliteConnection ObtenerConexion()
    {
        return new SqliteConnection(_cadenaConexion);
    }

    private void InicializarBaseDeDatos()
    {
        using var conexion = ObtenerConexion();
        conexion.Open();

        string sql = @"
            CREATE TABLE IF NOT EXISTS Mantenimientos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL,
                Tipo TEXT NOT NULL,
                Descripcion TEXT NOT NULL,
                Kilometraje INTEGER NOT NULL,
                Costo REAL NOT NULL
            );";

        using var comando = new SqliteCommand(sql, conexion);
        comando.ExecuteNonQuery();
    }
}