namespace Taller2.Models;

/// <summary>
/// Contiene el resultado global de la simulación ejecutada por GNU Octave,
/// incluyendo métricas de impacto, estado final y series de trayectoria y viento.
/// </summary>
public class ResultadoSimulacion
{
    /// <summary>
    /// Estado final de la simulación (e.g. OBJETIVO_ALCANZADO, OBJETIVO_NO_ALCANZADO,
    /// SIMULACION_FINALIZADA_POR_SUELO, SIMULACION_FINALIZADA_POR_LIMITE).
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Tiempo final en segundos del último frame simulado.
    /// </summary>
    public double TiempoFinal { get; set; }

    /// <summary>
    /// Posición horizontal final en metros.
    /// </summary>
    public double XFinal { get; set; }

    /// <summary>
    /// Posición vertical final en metros.
    /// </summary>
    public double YFinal { get; set; }

    /// <summary>
    /// Error absoluto horizontal respecto al objetivo (|xFinal - Xobjetivo|).
    /// </summary>
    public double ErrorX { get; set; }

    /// <summary>
    /// Error absoluto vertical respecto al objetivo (|yFinal - Yobjetivo|).
    /// </summary>
    public double ErrorY { get; set; }

    /// <summary>
    /// Distancia euclidiana entre la posición final y el centro del objetivo.
    /// </summary>
    public double DistanciaObjetivo { get; set; }

    /// <summary>
    /// Tiempo exacto en segundos estimado cuando se cruzó el plano del objetivo, si aplica.
    /// </summary>
    public double? TiempoObjetivo { get; set; }

    /// <summary>
    /// Colección de puntos discretos de la trayectoria física.
    /// </summary>
    public List<PuntoTrayectoria> Trayectoria { get; set; } = new();

    /// <summary>
    /// Colección de mediciones del viento por cada frame.
    /// </summary>
    public List<DatoViento> Vientos { get; set; } = new();

    /// <summary>
    /// Indica si el objetivo fue alcanzado exitosamente.
    /// </summary>
    public bool Exitoso => Estado == "OBJETIVO_ALCANZADO";
}
