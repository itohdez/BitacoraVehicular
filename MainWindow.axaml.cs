using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using BitacoraVehicular.Data;
using BitacoraVehicular.Models;
using BitacoraVehicular.Services;

namespace BitacoraVehicular;

public partial class MainWindow : Window
{
    private readonly MantenimientoServicio _servicio;
    private int? _idSeleccionado = null;

    public MainWindow()
    {
        InitializeComponent();

        // Inyección manual de dependencias: ConexionBD -> DAO -> Servicio
        var conexion = new ConexionBD();
        var dao = new MantenimientoDAO(conexion);
        _servicio = new MantenimientoServicio(dao);

        TxtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        CargarRegistros();
    }

    private void CargarRegistros()
    {
        LstMantenimientos.ItemsSource = _servicio.Listar();
    }

    private string ObtenerTipoSeleccionado()
    {
        return (CmbTipo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
    }

    private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            _servicio.Registrar(
                TxtFecha.Text ?? "",
                ObtenerTipoSeleccionado(),
                TxtDescripcion.Text ?? "",
                TxtKilometraje.Text ?? "",
                TxtCosto.Text ?? ""
            );

            CargarRegistros();
            LimpiarFormulario();
            MostrarMensaje("Registro guardado correctamente en SQLite.", esError: false);
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", esError: true);
        }
    }

    private void BtnActualizar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (_idSeleccionado is null)
                throw new ArgumentException("Selecciona primero un registro de la lista.");

            _servicio.Modificar(
                _idSeleccionado.Value,
                TxtFecha.Text ?? "",
                ObtenerTipoSeleccionado(),
                TxtDescripcion.Text ?? "",
                TxtKilometraje.Text ?? "",
                TxtCosto.Text ?? ""
            );

            CargarRegistros();
            LimpiarFormulario();
            MostrarMensaje("Registro actualizado correctamente.", esError: false);
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", esError: true);
        }
    }

    private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (_idSeleccionado is null)
                throw new ArgumentException("Selecciona primero un registro de la lista.");

            _servicio.Eliminar(_idSeleccionado.Value);

            CargarRegistros();
            LimpiarFormulario();
            MostrarMensaje("Registro eliminado correctamente.", esError: false);
        }
        catch (Exception ex)
        {
            MostrarMensaje($"Error: {ex.Message}", esError: true);
        }
    }

    private void BtnLimpiar_Click(object? sender, RoutedEventArgs e)
    {
        LimpiarFormulario();
        MostrarMensaje("Formulario limpio. Listo para un nuevo registro.", esError: false);
    }

    private void LstMantenimientos_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (LstMantenimientos.SelectedItem is Mantenimiento seleccionado)
        {
            _idSeleccionado = seleccionado.Id;
            TxtFecha.Text = seleccionado.Fecha;
            TxtDescripcion.Text = seleccionado.Descripcion;
            TxtKilometraje.Text = seleccionado.Kilometraje.ToString();
            TxtCosto.Text = seleccionado.Costo.ToString("0.##");

            for (int i = 0; i < CmbTipo.Items.Count; i++)
            {
                if (CmbTipo.Items[i] is ComboBoxItem item && item.Content?.ToString() == seleccionado.Tipo)
                {
                    CmbTipo.SelectedIndex = i;
                    break;
                }
            }

            MostrarMensaje($"Editando registro #{seleccionado.Id}.", esError: false);
        }
    }

    private void LimpiarFormulario()
    {
        _idSeleccionado = null;
        TxtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        CmbTipo.SelectedIndex = 0;
        TxtDescripcion.Text = string.Empty;
        TxtKilometraje.Text = string.Empty;
        TxtCosto.Text = string.Empty;
        LstMantenimientos.SelectedItem = null;
    }

    private void MostrarMensaje(string texto, bool esError)
    {
        TxtMensaje.Text = texto;
        TxtMensaje.Foreground = esError ? Brushes.Crimson : Brushes.DarkGreen;
    }
}