namespace BitacoraVehicular.Models;

public class Mantenimiento
{
    public int Id { get; set; }
    public string Fecha { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // Preventivo, Correctivo, Revisión
    public string Descripcion { get; set; } = string.Empty;
    public int Kilometraje { get; set; }
    public decimal Costo { get; set; }

    public Mantenimiento() { }

    public Mantenimiento(string fecha, string tipo, string descripcion, int kilometraje, decimal costo)
    {
        Fecha = fecha;
        Tipo = tipo;
        Descripcion = descripcion;
        Kilometraje = kilometraje;
        Costo = costo;
    }

    // Propiedad auxiliar para formatear el resumen en la interfaz
    public string DetalleResumen => $"{Fecha} | {Tipo} | {Kilometraje:N0} km | COP {Costo:N0}";
}