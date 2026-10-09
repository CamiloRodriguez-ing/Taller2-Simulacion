using System.Diagnostics;
using System.Globalization;
using Taller2.Models;

namespace Taller2.Services;

/// <summary>
/// Servicio responsable de la comunicación, ejecución y lectura de resultados
/// generados por el motor de física en GNU Octave.
/// Cumple la regla fundamental: C# NO recalcula la física ni genera el viento aleatorio.
/// </summary>
public class OctaveService
{
    private string? _rutaOctavePersonalizada;

    /// <summary>
    /// Permite configurar o sobrescribir manualmente la ruta al ejecutable de Octave (octave-cli.exe).
    /// </summary>
    public string? RutaOctaveConfigurada
    {
        get => _rutaOctavePersonalizada;
        set => _rutaOctavePersonalizada = value;
    }

    /// <summary>
    /// Detecta la ubicación del ejecutable octave-cli.exe en el sistema.
    /// Busca primero en la ruta configurada, luego en el PATH y finalmente en directorios típicos de instalación.
    /// </summary>
    /// <returns>Ruta absoluta al ejecutable octave-cli.exe o null si no se encuentra.</returns>
    public string? DetectarOctave()
    {
        // 1. Ruta configurada manualmente
        if (!string.IsNullOrWhiteSpace(_rutaOctavePersonalizada) && File.Exists(_rutaOctavePersonalizada))
        {
            return Path.GetFullPath(_rutaOctavePersonalizada);
        }

        // 2. Buscar en la variable de entorno PATH
        var pathVariable = Environment.GetEnvironmentVariable("PATH");
        if (!string.IsNullOrEmpty(pathVariable))
        {
            var directorios = pathVariable.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dir in directorios)
            {
                try
                {
                    var ejecutableCli = Path.Combine(dir.Trim(), "octave-cli.exe");
                    if (File.Exists(ejecutableCli))
                    {
                        return ejecutableCli;
                    }
                }
                catch
                {
                    // Ignorar directorios inválidos en PATH
                }
            }
        }

        // 3. Buscar en rutas estándar de instalación de Windows
        var rutasCandidatas = new List<string>
        {
            @"C:\Program Files\GNU Octave",
            @"C:\Program Files (x86)\GNU Octave",
            @"C:\Octave"
        };

        foreach (var rutaBase in rutasCandidatas)
        {
            if (!Directory.Exists(rutaBase))
            {
                continue;
            }

            // Buscar en subcarpetas Octave-*
            try
            {
                var subdirs = Directory.GetDirectories(rutaBase, "Octave-*", SearchOption.TopDirectoryOnly);
                foreach (var subdir in subdirs.OrderByDescending(d => d)) // Preferir versión más reciente
                {
                    var posibles = new[]
                    {
                        Path.Combine(subdir, "mingw64", "bin", "octave-cli.exe"),
                        Path.Combine(subdir, "ucrt64", "bin", "octave-cli.exe"),
                        Path.Combine(subdir, "clang64", "bin", "octave-cli.exe"),
                        Path.Combine(subdir, "bin", "octave-cli.exe")
                    };

                    foreach (var candidato in posibles)
                    {
                        if (File.Exists(candidato))
                        {
                            return candidato;
                        }
                    }
                }
            }
            catch
            {
                // Ignorar errores de acceso
            }
        }

        return null;
    }

    /// <summary>
    /// Determina la carpeta raíz del proyecto donde residen las carpetas 'datos' y 'Octave'.
    /// </summary>
    public string ObtenerRutaBaseProyecto()
    {
        // 1. Verificar si simulador.m está en el directorio base del ensamblado o en sus padres
        var dirEnsamblado = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        var actual = dirEnsamblado;
        while (actual != null)
        {
            if (File.Exists(Path.Combine(actual.FullName, "Octave", "simulador.m")) ||
                Directory.Exists(Path.Combine(actual.FullName, "Octave")))
            {
                return actual.FullName;
            }
            actual = actual.Parent;
        }

        // 2. Verificar el directorio de trabajo actual
        var dirActual = Directory.GetCurrentDirectory();
        if (File.Exists(Path.Combine(dirActual, "Octave", "simulador.m")) ||
            Directory.Exists(Path.Combine(dirActual, "Octave")))
        {
            return dirActual;
        }

        // Por defecto usar el directorio del ejecutable
        return AppDomain.CurrentDomain.BaseDirectory;
    }

    /// <summary>
    /// Escribe los parámetros de la simulación en el formato clave=valor requerido por Octave.
    /// </summary>
    public void CrearArchivoParametros(ParametrosSimulacion parametros, string rutaArchivo)
    {
        var dir = Path.GetDirectoryName(rutaArchivo);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var lineas = new List<string>
        {
            string.Create(CultureInfo.InvariantCulture, $"angulo={parametros.Angulo}"),
            string.Create(CultureInfo.InvariantCulture, $"velocidad={parametros.VelocidadInicial}"),
            string.Create(CultureInfo.InvariantCulture, $"viento_min={parametros.VientoMin}"),
            string.Create(CultureInfo.InvariantCulture, $"viento_max={parametros.VientoMax}"),
            string.Create(CultureInfo.InvariantCulture, $"direccion_min={parametros.DireccionMin}"),
            string.Create(CultureInfo.InvariantCulture, $"direccion_max={parametros.DireccionMax}"),
            string.Create(CultureInfo.InvariantCulture, $"objetivo_x={parametros.ObjetivoX}"),
            string.Create(CultureInfo.InvariantCulture, $"objetivo_y={parametros.ObjetivoY}"),
            string.Create(CultureInfo.InvariantCulture, $"tolerancia={parametros.Tolerancia}"),
            string.Create(CultureInfo.InvariantCulture, $"semilla={parametros.Semilla}")
        };

        File.WriteAllLines(rutaArchivo, lineas);
    }

    /// <summary>
    /// Ejecuta la simulación completa en GNU Octave de forma desacoplada y no interactiva,
    /// asegurando la limpieza previa de resultados viejos y validando los datos leídos.
    /// </summary>
    public async Task<ResultadoSimulacion> EjecutarSimulacionAsync(ParametrosSimulacion parametros)
    {
        return await Task.Run(() => EjecutarSimulacion(parametros));
    }

    /// <summary>
    /// Versión sincrónica de ejecución de la simulación.
    /// </summary>
    public ResultadoSimulacion EjecutarSimulacion(ParametrosSimulacion parametros)
    {
        var rutaOctave = DetectarOctave();
        if (string.IsNullOrEmpty(rutaOctave) || !File.Exists(rutaOctave))
        {
            throw new FileNotFoundException(
                "No se encontró GNU Octave.\nInstale GNU Octave o configure la ruta a octave-cli.exe.");
        }

        var rutaBase = ObtenerRutaBaseProyecto();
        var dirDatos = Path.Combine(rutaBase, "datos");
        var dirOctave = Path.Combine(rutaBase, "Octave");
        var rutaScript = Path.Combine(dirOctave, "simulador.m");

        if (!File.Exists(rutaScript))
        {
            throw new FileNotFoundException($"No se encontró el script de simulación de Octave: {rutaScript}");
        }

        if (!Directory.Exists(dirDatos))
        {
            Directory.CreateDirectory(dirDatos);
        }

        var rutaParametros = Path.Combine(dirDatos, "parametros.txt");
        var rutaRecorrido = Path.Combine(dirDatos, "recorrido.csv");
        var rutaViento = Path.Combine(dirDatos, "viento.csv");
        var rutaEstado = Path.Combine(dirDatos, "estado.txt");

        // 1. Escribir parametros.txt
        CrearArchivoParametros(parametros, rutaParametros);

        // 2. Seguridad contra resultados viejos: limpiar archivos antes de ejecutar (Sección 29)
        LimpiarResultadosPrevios(rutaRecorrido, rutaViento, rutaEstado);

        // 3. Configurar proceso de Octave
        var startInfo = new ProcessStartInfo
        {
            FileName = rutaOctave,
            Arguments = $"--no-gui --quiet \"{rutaScript}\" \"{rutaParametros}\" \"{dirDatos}\"",
            WorkingDirectory = rutaBase,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        string stdout;
        string stderr;
        int exitCode;

        using (var proceso = new Process { StartInfo = startInfo })
        {
            proceso.Start();
            stdout = proceso.StandardOutput.ReadToEnd();
            stderr = proceso.StandardError.ReadToEnd();
            
            // Esperar con límite de seguridad (30 s)
            if (!proceso.WaitForExit(30000))
            {
                proceso.Kill();
                throw new TimeoutException("La simulación en GNU Octave excedió el tiempo límite de ejecución (30 segundos).");
            }

            exitCode = proceso.ExitCode;
        }

        // 4. Comprobar código de salida
        if (exitCode != 0)
        {
            var detalleError = !string.IsNullOrWhiteSpace(stderr) ? stderr.Trim() : stdout.Trim();
            throw new InvalidOperationException($"Octave terminó con un código de error ({exitCode}).\n{detalleError}");
        }

        // 5. Verificar que se hayan generado todos los archivos esperados
        VerificarResultados(rutaRecorrido, rutaViento, rutaEstado);

        // 6. Leer y validar resultados
        var resultado = LeerEstado(rutaEstado);
        resultado.Trayectoria = LeerRecorrido(rutaRecorrido);
        resultado.Vientos = LeerViento(rutaViento);

        // 7. Control de calidad e integridad de datos (Sección 27)
        ValidarIntegridadDatos(resultado);

        return resultado;
    }

    /// <summary>
    /// Elimina archivos de simulaciones anteriores para garantizar aislamiento (Sección 29).
    /// </summary>
    private static void LimpiarResultadosPrevios(string rutaRecorrido, string rutaViento, string rutaEstado)
    {
        try
        {
            if (File.Exists(rutaRecorrido)) File.Delete(rutaRecorrido);
            if (File.Exists(rutaViento)) File.Delete(rutaViento);
            if (File.Exists(rutaEstado)) File.Delete(rutaEstado);
        }
        catch
        {
            // Ignorar excepciones menores de limpieza si algún archivo está temporalmente retenido
        }
    }

    /// <summary>
    /// Comprueba la existencia de los tres archivos obligatorios tras la ejecución de Octave.
    /// </summary>
    public void VerificarResultados(string rutaRecorrido, string rutaViento, string rutaEstado)
    {
        if (!File.Exists(rutaRecorrido))
        {
            throw new FileNotFoundException("No se generó recorrido.csv.");
        }

        if (!File.Exists(rutaViento))
        {
            throw new FileNotFoundException("No se generó viento.csv.");
        }

        if (!File.Exists(rutaEstado))
        {
            throw new FileNotFoundException("No se generó estado.txt.");
        }
    }

    /// <summary>
    /// Lee y parsea recorrido.csv generado por Octave.
    /// </summary>
    public List<PuntoTrayectoria> LeerRecorrido(string rutaRecorrido)
    {
        var lineas = File.ReadAllLines(rutaRecorrido);
        if (lineas.Length < 2)
        {
            throw new FormatException("recorrido.csv está vacío o no contiene suficientes registros.");
        }

        // Validar encabezado obligatorio
        var encabezado = lineas[0].Trim();
        if (!encabezado.Equals("tiempo,x,y,vx,vy,velocidad", StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException($"El encabezado de recorrido.csv no coincide con el formato esperado. Encontrado: '{encabezado}'");
        }

        var lista = new List<PuntoTrayectoria>();
        for (int i = 1; i < lineas.Length; i++)
        {
            var linea = lineas[i].Trim();
            if (string.IsNullOrEmpty(linea)) continue;

            var partes = linea.Split(',');
            if (partes.Length < 6)
            {
                throw new FormatException($"Fila incompleta en recorrido.csv (línea {i + 1}): '{linea}'");
            }

            try
            {
                var punto = new PuntoTrayectoria
                {
                    Tiempo = double.Parse(partes[0], CultureInfo.InvariantCulture),
                    X = double.Parse(partes[1], CultureInfo.InvariantCulture),
                    Y = double.Parse(partes[2], CultureInfo.InvariantCulture),
                    Vx = double.Parse(partes[3], CultureInfo.InvariantCulture),
                    Vy = double.Parse(partes[4], CultureInfo.InvariantCulture),
                    Velocidad = double.Parse(partes[5], CultureInfo.InvariantCulture)
                };
                lista.Add(punto);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error al parsear valores numéricos en recorrido.csv (línea {i + 1}): {ex.Message}", ex);
            }
        }

        return lista;
    }

    /// <summary>
    /// Lee y parsea viento.csv generado por Octave.
    /// </summary>
    public List<DatoViento> LeerViento(string rutaViento)
    {
        var lineas = File.ReadAllLines(rutaViento);
        if (lineas.Length < 2)
        {
            throw new FormatException("viento.csv está vacío o no contiene suficientes registros.");
        }

        // Validar encabezado obligatorio
        var encabezado = lineas[0].Trim();
        if (!encabezado.Equals("tiempo,direccion,fuerza,ax_viento,ay_viento", StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException($"El encabezado de viento.csv no coincide con el formato esperado. Encontrado: '{encabezado}'");
        }

        var lista = new List<DatoViento>();
        for (int i = 1; i < lineas.Length; i++)
        {
            var linea = lineas[i].Trim();
            if (string.IsNullOrEmpty(linea)) continue;

            var partes = linea.Split(',');
            if (partes.Length < 5)
            {
                throw new FormatException($"Fila incompleta en viento.csv (línea {i + 1}): '{linea}'");
            }

            try
            {
                var dato = new DatoViento
                {
                    Tiempo = double.Parse(partes[0], CultureInfo.InvariantCulture),
                    Direccion = double.Parse(partes[1], CultureInfo.InvariantCulture),
                    Magnitud = double.Parse(partes[2], CultureInfo.InvariantCulture),
                    AxViento = double.Parse(partes[3], CultureInfo.InvariantCulture),
                    AyViento = double.Parse(partes[4], CultureInfo.InvariantCulture)
                };
                lista.Add(dato);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error al parsear valores numéricos en viento.csv (línea {i + 1}): {ex.Message}", ex);
            }
        }

        return lista;
    }

    /// <summary>
    /// Lee y parsea estado.txt generado por Octave.
    /// </summary>
    public ResultadoSimulacion LeerEstado(string rutaEstado)
    {
        var lineas = File.ReadAllLines(rutaEstado);
        var resultado = new ResultadoSimulacion();

        var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var linea in lineas)
        {
            var trimmed = linea.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#") || trimmed.StartsWith("%"))
            {
                continue;
            }

            var idx = trimmed.IndexOf('=');
            if (idx >= 0)
            {
                var clave = trimmed.Substring(0, idx).Trim();
                var valor = trimmed.Substring(idx + 1).Trim();
                dic[clave] = valor;
            }
        }

        if (dic.TryGetValue("estado", out var estadoVal))
        {
            resultado.Estado = estadoVal;
        }

        if (dic.TryGetValue("tiempo_final", out var tFinStr) && double.TryParse(tFinStr, CultureInfo.InvariantCulture, out var tFin))
        {
            resultado.TiempoFinal = tFin;
        }

        if (dic.TryGetValue("x_final", out var xFinStr) && double.TryParse(xFinStr, CultureInfo.InvariantCulture, out var xFin))
        {
            resultado.XFinal = xFin;
        }

        if (dic.TryGetValue("y_final", out var yFinStr) && double.TryParse(yFinStr, CultureInfo.InvariantCulture, out var yFin))
        {
            resultado.YFinal = yFin;
        }

        if (dic.TryGetValue("error_x", out var errXStr) && double.TryParse(errXStr, CultureInfo.InvariantCulture, out var errX))
        {
            resultado.ErrorX = errX;
        }

        if (dic.TryGetValue("error_y", out var errYStr) && double.TryParse(errYStr, CultureInfo.InvariantCulture, out var errY))
        {
            resultado.ErrorY = errY;
        }

        if (dic.TryGetValue("distancia_objetivo", out var distStr) && double.TryParse(distStr, CultureInfo.InvariantCulture, out var dist))
        {
            resultado.DistanciaObjetivo = dist;
        }

        if (dic.TryGetValue("tiempo_objetivo", out var tObjStr) && !string.IsNullOrWhiteSpace(tObjStr))
        {
            if (double.TryParse(tObjStr, CultureInfo.InvariantCulture, out var tObj))
            {
                resultado.TiempoObjetivo = tObj;
            }
        }

        return resultado;
    }

    /// <summary>
    /// Realiza el control de calidad estricto exigido por la sección 27 de la especificación:
    /// - Mismo número de registros entre recorrido y viento.
    /// - Tiempos perfectamente alineados.
    /// - Tiempo inicial igual a 0.0 s.
    /// - Incremento esperado de 0.5 s por paso.
    /// </summary>
    private static void ValidarIntegridadDatos(ResultadoSimulacion resultado)
    {
        var nRec = resultado.Trayectoria.Count;
        var nViento = resultado.Vientos.Count;

        if (nRec == 0)
        {
            throw new InvalidDataException("recorrido.csv no contiene registros de trayectoria.");
        }

        if (nRec != nViento)
        {
            throw new InvalidDataException(
                $"Control de calidad fallido: recorrido.csv contiene {nRec} registros mientras que viento.csv contiene {nViento} registros. Los recuentos deben coincidir.");
        }

        // Validar inicio en t = 0.0 s
        if (Math.Abs(resultado.Trayectoria[0].Tiempo) > 1e-4)
        {
            throw new InvalidDataException($"Control de calidad fallido: el tiempo inicial no es 0.0 s (encontrado: {resultado.Trayectoria[0].Tiempo}).");
        }

        // Validar incremento de 0.5 s y alineación temporal fila a fila
        for (int i = 0; i < nRec; i++)
        {
            var tRec = resultado.Trayectoria[i].Tiempo;
            var tViento = resultado.Vientos[i].Tiempo;

            if (Math.Abs(tRec - tViento) > 1e-4)
            {
                throw new InvalidDataException(
                    $"Control de calidad fallido: desalineación temporal en la fila {i + 1} (Recorrido: {tRec} s vs Viento: {tViento} s).");
            }

            if (i > 0)
            {
                var dt = tRec - resultado.Trayectoria[i - 1].Tiempo;
                if (Math.Abs(dt - 0.5) > 1e-3)
                {
                    throw new InvalidDataException(
                        $"Control de calidad fallido: el incremento temporal entre los puntos {i} y {i + 1} no es de 0.5 s (detectado: {dt:F3} s).");
                }
            }
        }
    }
}
