# Bitácora de Mantenimiento Vehicular (`BitacoraVehicular`)

## 1. Información general

- **Nombre del proyecto:** Bitácora de Mantenimiento Vehicular (`BitacoraVehicular`)
- **Estudiante:** Andrés Felipe Hernández Díaz
- **Asignatura:** Programación con C# y .NET
- **Descripción:** Aplicación de escritorio desarrollada en C# con Avalonia UI y ADO.NET (SQLite) para registrar, consultar, modificar y eliminar el historial de mantenimientos preventivos y correctivos de un vehículo.
- **Problema que resuelve:** Evita llevar el control de servicios, fechas, kilometrajes y costos en notas sueltas o papel, centralizando toda la información en una base de datos local persistente y fácil de administrar.
- **Objetivo principal:** Diseñar, desarrollar, probar y documentar una aplicación de escritorio que integre Programación Orientada a Objetos (POO), interfaz gráfica en Avalonia UI y persistencia de datos mediante ADO.NET y SQLite.

## 2. Funcionalidades

- **Registrar mantenimiento (Create):** Permite ingresar fecha, tipo de servicio, descripción, kilometraje y costo desde el formulario gráfico.
- **Consultar historial (Read):** Carga y muestra automáticamente todos los registros guardados en SQLite al abrir la aplicación, ordenados del más reciente al más antiguo.
- **Actualizar registro (Update):** Al seleccionar un elemento de la lista, sus datos se cargan en el formulario para editarlos y guardar los cambios en la base de datos.
- **Eliminar registro (Delete):** Borra el registro seleccionado tanto de la base de datos SQLite como de la vista.
- **Validación y notificaciones:** Valida el formato de fecha (`AAAA-MM-DD`), campos vacíos y valores numéricos antes de guardar, mostrando mensajes claros de éxito o error en la barra inferior sin que el programa se cierre.

## 3. Tecnologías utilizadas

- **Lenguaje:** C#
- **Framework:** .NET 10.0
- **Interfaz gráfica:** Avalonia UI 11 (XAML `.axaml`)
- **Acceso a datos:** ADO.NET (`Microsoft.Data.Sqlite`)
- **Base de datos:** SQLite
- **Entorno de desarrollo:** Visual Studio Code

## 4. Estructura del proyecto

```text
BitacoraVehicular/
├── Data/
│   ├── ConexionBD.cs              # Gestión de conexión a SQLite e inicialización de tabla
│   └── MantenimientoDAO.cs        # Operaciones CRUD con ADO.NET y SQL parametrizado
├── Models/
│   └── Mantenimiento.cs           # Entidad principal del dominio
├── Services/
│   └── MantenimientoServicio.cs   # Reglas de negocio y validaciones de entrada
├── evidencias/                    # Capturas de pantalla de pruebas funcionales
├── App.axaml                      # Configuración general de Avalonia
├── App.axaml.cs
├── MainWindow.axaml               # Diseño de la interfaz gráfica
├── MainWindow.axaml.cs            # Manejo de eventos y comunicación con la capa de servicio
├── Program.cs                     # Punto de entrada de la aplicación
├── BitacoraVehicular.csproj       # Archivo de proyecto y dependencias NuGet
├── .gitignore                     # Exclusión de bin/, obj/ y archivos temporales
└── README.md                      # Documentación técnica
```

### Clases principales y aplicación de POO

- **`Mantenimiento` (`Models/`):** Representa cada registro de servicio. Aplica **encapsulamiento** mediante propiedades, **sobrecarga de constructores** (vacío y parametrizado) y una propiedad calculada (`DetalleResumen`) para formatear la salida en la interfaz.
- **`ConexionBD` (`Data/`):** Encapsula la cadena de conexión (`private readonly`) y garantiza la creación automática de la base de datos y la tabla.
- **`MantenimientoDAO` (`Data/`):** Centraliza el acceso a datos mediante ADO.NET. Recibe `ConexionBD` por constructor y ejecuta las sentencias SQL parametrizadas.
- **`MantenimientoServicio` (`Services/`):** Contiene la lógica de validación para asegurar que ningún dato inválido llegue a la base de datos.
- **`MainWindow` (`MainWindow.axaml.cs`):** Gestiona únicamente los eventos de la interfaz gráfica (`Click`, `SelectionChanged`) y delega toda operación a `MantenimientoServicio`, manteniendo una separación clara de responsabilidades.

## 5. Instalación de dependencias

### Requisitos previos

- SDK de **.NET 10.0** o superior instalado en el equipo.

### Paquetes NuGet utilizados

- `Avalonia` / `Avalonia.Desktop` / `Avalonia.Themes.Fluent` (interfaz gráfica)
- `Microsoft.Data.Sqlite` (proveedor ADO.NET para SQLite)

### Pasos para preparar el entorno

1. Clonar el repositorio desde GitHub:

   ```bash
   git clone [https://github.com/itohdez/BitacoraVehicular.git](https://github.com/itohdez/BitacoraVehicular.git)
   ```

2. Ingresar a la carpeta del proyecto:

   ```bash
   cd BitacoraVehicular
   ```

3. Restaurar las dependencias del proyecto:

   ```bash
   dotnet restore
   ```

No se requiere instalar motores externos ni realizar configuraciones adicionales.

## 6. Ejecución del programa

Desde la raíz de la carpeta `BitacoraVehicular`, ejecutar:

```bash
dotnet restore
dotnet run
```

Al iniciar, se abrirá la ventana principal de la aplicación lista para usar.

## 7. Base de datos

- **Información almacenada:** Historial de mantenimientos vehiculares (fecha, tipo de servicio, descripción, kilometraje y costo).
- **Motor y archivo:** SQLite almacenado localmente en el archivo `bitacora.db` (ubicado en el directorio de ejecución de la aplicación).
- **Inicialización automática:** No es necesario ejecutar scripts externos. Al instanciar `ConexionBD`, se ejecuta `CREATE TABLE IF NOT EXISTS Mantenimientos (...)`, creando la base de datos y la tabla automáticamente en el primer arranque.
- **Tabla `Mantenimientos`:**

  | Campo | Tipo SQLite | Descripción |
  | :--- | :--- | :--- |
  | `Id` | `INTEGER PRIMARY KEY AUTOINCREMENT` | Identificador único del registro |
  | `Fecha` | `TEXT NOT NULL` | Fecha del servicio (`AAAA-MM-DD`) |
  | `Tipo` | `TEXT NOT NULL` | Categoría (Preventivo, Correctivo, Revisión General) |
  | `Descripcion` | `TEXT NOT NULL` | Detalle del trabajo realizado |
  | `Kilometraje` | `INTEGER NOT NULL` | Kilometraje del vehículo al momento del servicio |
  | `Costo` | `REAL NOT NULL` | Costo total del mantenimiento |

- **Conexión y CRUD:** La conexión se gestiona en `Data/ConexionBD.cs` con `SqliteConnection`, y las cuatro operaciones CRUD (`Insertar`, `ObtenerTodos`, `Actualizar`, `Eliminar`) están implementadas en `Data/MantenimientoDAO.cs` usando `SqliteCommand`, parámetros `@` y bloques `using` para liberar recursos correctamente.

## 8. Evidencias de funcionamiento

### 1. Registro (Create)

Se completan los campos del formulario con datos válidos y se presiona **Guardar**. El registro se guarda en SQLite, aparece en la lista y se muestra el mensaje de confirmación.
![Prueba de registro](evidencias/01_registro.png)

### 2. Consulta (Read)

Se visualizan en la lista los registros almacenados en la base de datos con su descripción, fecha, tipo, kilometraje y costo.
![Prueba de consulta](evidencias/02_consulta.png)

### 3. Actualización (Update)

Se selecciona un registro existente de la lista, se modifican sus datos en el formulario y se presiona **Actualizar**. Los cambios se guardan y se reflejan de inmediato.
![Prueba de actualización](evidencias/03_actualizacion.png)

### 4. Eliminación (Delete)

Se selecciona un registro y se presiona **Eliminar**. El registro se borra de la base de datos y desaparece de la lista.
![Prueba de eliminación](evidencias/04_eliminacion.png)

### 5. Validación de errores

Se intenta registrar información inválida (por ejemplo, kilometraje negativo o texto en campos numéricos). El sistema intercepta la excepción y muestra el mensaje de error en rojo sin cerrarse.
![Prueba de validación](evidencias/05_validacion.png)

### 6. Persistencia

Se cierra completamente la aplicación y se vuelve a ejecutar con `dotnet run`. Los registros guardados previamente siguen disponibles al cargar la ventana.
![Prueba de persistencia](evidencias/06_persistencia.png)

## 9. Problemas encontrados y soluciones

- **Problema 1 (Compiled Bindings en Avalonia 11):** Al enlazar las propiedades del modelo `Mantenimiento` dentro del `DataTemplate` del `ListBox` en `MainWindow.axaml`, el compilador arrojaba error porque Avalonia habilita los *Compiled Bindings* por defecto.
  - **Solución:** Se declaró el espacio de nombres `xmlns:models="using:BitacoraVehicular.Models"` en la etiqueta `<Window>` y se agregó `x:DataType="models:Mantenimiento"` en el `DataTemplate`, permitiendo compilar con verificación de tipos.
- **Problema 2 (Error `CS1733` en cadena interpolada):** Al definir la propiedad `DetalleResumen` en `Mantenimiento.cs`, un separador mal formateado provocó llaves vacías `{}` dentro de la interpolación de cadenas (`$"..."`), impidiendo la compilación.
  - **Solución:** Se corrigieron los separadores por el carácter `|` estándar y el formato de moneda, eliminando los errores de sintaxis.
