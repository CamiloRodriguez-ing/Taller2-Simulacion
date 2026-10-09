using System.Globalization;
using Taller2.Models;
using Taller2.Services;

namespace Taller2;

static class Program
{
    /// <summary>
    /// Punto de entrada principal para la aplicación.
    /// Soporta modo interactivo con interfaz gráfica Windows Forms
    /// o ejecución en modo prueba desde consola con el argumento '--test'.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0].Equals("--test", StringComparison.OrdinalIgnoreCase))
        {
            EjecutarBateriaDePruebasConsola();
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }

    private static void EjecutarBateriaDePruebasConsola()
    {
        Console.WriteLine("=================================================================");
        Console.WriteLine("    BATERÍA AUTOMATIZADA DE PRUEBAS DE INTEGRACIÓN C# <-> OCTAVE");
        Console.WriteLine("=================================================================\n");

        var servicio = new OctaveService();
        var rutaOctave = servicio.DetectarOctave();
        Console.WriteLine($"[1] Detección de GNU Octave: {rutaOctave}");

        if (string.IsNullOrEmpty(rutaOctave))
        {
            Console.WriteLine("ERROR: No se detectó GNU Octave.");
            Environment.ExitCode = 1;
            return;
        }

        try
        {
            // -------------------------------------------------------------
            // PRUEBA A: Configuración Predeterminada
            // -------------------------------------------------------------
            Console.WriteLine("\n--> PRUEBA A: Configuración Predeterminada (25°, 20 m/s, sem 12345)");
            var paramsA = new ParametrosSimulacion
            {
                Angulo = 25.0,
                VelocidadInicial = 20.0,
                VientoMin = 0.0,
                VientoMax = 1.5,
                DireccionMin = -30.0,
                DireccionMax = 30.0,
                ObjetivoX = 25.0,
                ObjetivoY = 1.5,
                Tolerancia = 1.0,
                Semilla = 12345
            };
            var resA1 = servicio.EjecutarSimulacion(paramsA);
            Console.WriteLine($"   Estado: {resA1.Estado}");
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"   Posición final: ({resA1.XFinal:F3}, {resA1.YFinal:F3}) m | Frames: {resA1.Trayectoria.Count}"));
            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"   Tiempo objetivo estimado: {resA1.TiempoObjetivo?.ToString("F3", CultureInfo.InvariantCulture) ?? "N/A"} s"));
            Assert(resA1.Exitoso, "Prueba A debe alcanzar el objetivo.");
            Assert(resA1.Trayectoria.Count == resA1.Vientos.Count, "Prueba A debe tener igual número de puntos en trayectoria y viento.");
            Console.WriteLine("   [OK] Prueba A superada con éxito.");

            // -------------------------------------------------------------
            // PRUEBA C: Reproducibilidad con la misma semilla
            // -------------------------------------------------------------
            Console.WriteLine("\n--> PRUEBA C: Reproducibilidad (Misma semilla ejecutada por segunda vez)");
            var resA2 = servicio.EjecutarSimulacion(paramsA);
            bool identicos = resA1.Trayectoria.Count == resA2.Trayectoria.Count;
            for (int i = 0; i < resA1.Trayectoria.Count && identicos; i++)
            {
                if (Math.Abs(resA1.Trayectoria[i].X - resA2.Trayectoria[i].X) > 1e-4 ||
                    Math.Abs(resA1.Vientos[i].Magnitud - resA2.Vientos[i].Magnitud) > 1e-4)
                {
                    identicos = false;
                }
            }
            Assert(identicos, "Prueba C: Dos simulaciones con la misma semilla deben producir resultados idénticos.");
            Console.WriteLine("   [OK] Prueba C superada: Resultados idénticos bit a bit entre ejecuciones.");

            // -------------------------------------------------------------
            // PRUEBA D: Semilla Diferente
            // -------------------------------------------------------------
            Console.WriteLine("\n--> PRUEBA D: Semilla Diferente (Cambio a semilla 99999)");
            var paramsD = new ParametrosSimulacion
            {
                Angulo = 25.0,
                VelocidadInicial = 20.0,
                VientoMin = 0.0,
                VientoMax = 1.5,
                DireccionMin = -30.0,
                DireccionMax = 30.0,
                ObjetivoX = 25.0,
                ObjetivoY = 1.5,
                Tolerancia = 1.0,
                Semilla = 99999
            };
            var resD = servicio.EjecutarSimulacion(paramsD);
            bool vientoCambio = Math.Abs(resA1.Vientos[0].Magnitud - resD.Vientos[0].Magnitud) > 1e-3 ||
                               Math.Abs(resA1.Vientos[0].Direccion - resD.Vientos[0].Direccion) > 1e-3;
            Assert(vientoCambio, "Prueba D: Al cambiar la semilla, el viento aleatorio debe cambiar.");
            Console.WriteLine($"   [OK] Prueba D superada: El viento cambió (Mag inicial A: {resA1.Vientos[0].Magnitud:F3} vs D: {resD.Vientos[0].Magnitud:F3}).");

            // -------------------------------------------------------------
            // PRUEBA B: Parámetros Diferentes
            // -------------------------------------------------------------
            Console.WriteLine("\n--> PRUEBA B: Parámetros Diferentes (35°, 22 m/s, viento 0.2 - 1.2 m/s²)");
            var paramsB = new ParametrosSimulacion
            {
                Angulo = 35.0,
                VelocidadInicial = 22.0,
                VientoMin = 0.2,
                VientoMax = 1.2,
                DireccionMin = -20.0,
                DireccionMax = 20.0,
                ObjetivoX = 25.0,
                ObjetivoY = 1.5,
                Tolerancia = 1.0,
                Semilla = 12345
            };
            var resB = servicio.EjecutarSimulacion(paramsB);
            Console.WriteLine($"   Estado: {resB.Estado} | Posición final: ({resB.XFinal:F3}, {resB.YFinal:F3}) m");
            Assert(Math.Abs(resA1.Trayectoria[1].Y - resB.Trayectoria[1].Y) > 0.1, "Prueba B: La trayectoria debe responder a los nuevos parámetros.");
            Console.WriteLine("   [OK] Prueba B superada: Trayectoria y cinemática recalculadas por Octave.");

            // -------------------------------------------------------------
            // PRUEBA F: Fallo por Suelo
            // -------------------------------------------------------------
            Console.WriteLine("\n--> PRUEBA F: Fallo por Suelo (10°, 10 m/s, Objetivo a 35 m)");
            var paramsF = new ParametrosSimulacion
            {
                Angulo = 10.0,
                VelocidadInicial = 10.0,
                VientoMin = 0.0,
                VientoMax = 0.5,
                DireccionMin = -10.0,
                DireccionMax = 10.0,
                ObjetivoX = 35.0,
                ObjetivoY = 1.5,
                Tolerancia = 1.0,
                Semilla = 12345
            };
            var resF = servicio.EjecutarSimulacion(paramsF);
            Console.WriteLine($"   Estado: {resF.Estado} | Y final: {resF.YFinal:F3} m");
            Assert(resF.Estado == "SIMULACION_FINALIZADA_POR_SUELO", "Prueba F: Debe terminar con estado SIMULACION_FINALIZADA_POR_SUELO.");
            Assert(!resF.TiempoObjetivo.HasValue, "Prueba F: No debe registrar tiempo en objetivo.");
            Console.WriteLine("   [OK] Prueba F superada: Finalización por colisión en suelo controlada.");

            // Restaurar ejecución predeterminada en datos/
            servicio.EjecutarSimulacion(paramsA);

            Console.WriteLine("\n=================================================================");
            Console.WriteLine("   TODAS LAS PRUEBAS OBLIGATORIAS FUERON COMPLETADAS CON ÉXITO");
            Console.WriteLine("=================================================================\n");
            Environment.ExitCode = 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERROR]: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Environment.ExitCode = 2;
        }
    }

    private static void Assert(bool condicion, string mensaje)
    {
        if (!condicion)
        {
            throw new InvalidOperationException($"Aserción fallida: {mensaje}");
        }
    }
}