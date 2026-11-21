using System;
using System.Collections.Generic;
using System.Globalization;
using BitacoraVehicular.Data;
using BitacoraVehicular.Models;

namespace BitacoraVehicular.Services;

public class MantenimientoServicio
{
    private readonly MantenimientoDAO _dao;

    public MantenimientoServicio(MantenimientoDAO dao)
    {
        _dao = dao;
    }

    public List<Mantenimiento> Listar() => _dao.ObtenerTodos();

    public void Registrar(string fecha, string tipo, string descripcion, string kmTexto, string costoTexto)
    {
        Mantenimiento nuevo = ValidarYConstruir(fecha, tipo, descripcion, kmTexto, costoTexto);
        _dao.Insertar(nuevo);
    }

    public void Modificar(int id, string fecha, string tipo, string descripcion, string kmTexto, string costoTexto)
    {
        if (id <= 0)
            throw new ArgumentException("Debe seleccionar un registro de la lista para modificar.");

        Mantenimiento actualizado = ValidarYConstruir(fecha, tipo, descripcion, kmTexto, costoTexto);
        actualizado.Id = id;
        _dao.Actualizar(actualizado);
    }

    public void Eliminar(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Debe seleccionar un registro de la lista para eliminar.");

        _dao.Eliminar(id);
    }

    private Mantenimiento ValidarYConstruir(string fecha, string tipo, string descripcion, string kmTexto, string costoTexto)
    {
        if (string.IsNullOrWhiteSpace(fecha) ||
            !DateTime.TryParseExact(fecha.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new ArgumentException("La fecha es obligatoria y debe tener formato AAAA-MM-DD (ej. 2026-11-20).");
        }

        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("Debe seleccionar un tipo de mantenimiento.");

        if (string.IsNullOrWhiteSpace(descripcion) || descripcion.Trim().Length < 3)
            throw new ArgumentException("La descripción debe tener al menos 3 caracteres.");

        if (!int.TryParse(kmTexto.Trim(), out int kilometraje) || kilometraje <= 0)
            throw new ArgumentException("El kilometraje debe ser un número entero mayor que cero.");

        if (!decimal.TryParse(costoTexto.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal costo) || costo < 0)
            throw new ArgumentException("El costo debe ser un número válido (igual o mayor que cero).");

        return new Mantenimiento(fecha.Trim(), tipo.Trim(), descripcion.Trim(), kilometraje, costo);
    }
}