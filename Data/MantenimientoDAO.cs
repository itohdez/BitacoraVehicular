using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BitacoraVehicular.Models;

namespace BitacoraVehicular.Data;

public class MantenimientoDAO
{
    private readonly ConexionBD _conexionBD;

    public MantenimientoDAO(ConexionBD conexionBD)
    {
        _conexionBD = conexionBD;
    }

    // CREATE
    public void Insertar(Mantenimiento m)
    {
        using var conexion = _conexionBD.ObtenerConexion();
        conexion.Open();

        string sql = @"
            INSERT INTO Mantenimientos (Fecha, Tipo, Descripcion, Kilometraje, Costo)
            VALUES (@fecha, @tipo, @descripcion, @kilometraje, @costo);";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@fecha", m.Fecha);
        comando.Parameters.AddWithValue("@tipo", m.Tipo);
        comando.Parameters.AddWithValue("@descripcion", m.Descripcion);
        comando.Parameters.AddWithValue("@kilometraje", m.Kilometraje);
        comando.Parameters.AddWithValue("@costo", (double)m.Costo);

        comando.ExecuteNonQuery();
    }

    // READ
    public List<Mantenimiento> ObtenerTodos()
    {
        var lista = new List<Mantenimiento>();

        using var conexion = _conexionBD.ObtenerConexion();
        conexion.Open();

        string sql = "SELECT Id, Fecha, Tipo, Descripcion, Kilometraje, Costo FROM Mantenimientos ORDER BY Id DESC;";
        using var comando = new SqliteCommand(sql, conexion);
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            lista.Add(new Mantenimiento
            {
                Id = lector.GetInt32(0),
                Fecha = lector.GetString(1),
                Tipo = lector.GetString(2),
                Descripcion = lector.GetString(3),
                Kilometraje = lector.GetInt32(4),
                Costo = (decimal)lector.GetDouble(5)
            });
        }

        return lista;
    }

    // UPDATE
    public void Actualizar(Mantenimiento m)
    {
        using var conexion = _conexionBD.ObtenerConexion();
        conexion.Open();

        string sql = @"
            UPDATE Mantenimientos
            SET Fecha = @fecha,
                Tipo = @tipo,
                Descripcion = @descripcion,
                Kilometraje = @kilometraje,
                Costo = @costo
            WHERE Id = @id;";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@fecha", m.Fecha);
        comando.Parameters.AddWithValue("@tipo", m.Tipo);
        comando.Parameters.AddWithValue("@descripcion", m.Descripcion);
        comando.Parameters.AddWithValue("@kilometraje", m.Kilometraje);
        comando.Parameters.AddWithValue("@costo", (double)m.Costo);
        comando.Parameters.AddWithValue("@id", m.Id);

        comando.ExecuteNonQuery();
    }

    // DELETE
    public void Eliminar(int id)
    {
        using var conexion = _conexionBD.ObtenerConexion();
        conexion.Open();

        string sql = "DELETE FROM Mantenimientos WHERE Id = @id;";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }
}