namespace Taller2.Models;

/// <summary>
/// Representa el conjunto de parámetros de entrada configurados por el usuario
/// para la simulación del movimiento parabólico con viento aleatorio.
/// </summary>
public class ParametrosSimulacion
{
    /// <summary>
    /// Ángulo de lanzamiento en grados (sugerido: 10° - 45°).
    /// </summary>
    public double Angulo { get; set; } = 25.0;

    /// <summary>
    /// Velocidad inicial en m/s (sugerido: 10 - 30 m/s).
    /// </summary>
    public double VelocidadInicial { get; set; } = 20.0;

    /// <summary>
    /// Aceleración mínima del viento en m/s² (sugerido: 0.0 - 1.5 m/s²).
    /// </summary>
    public double VientoMin { get; set; } = 0.0;

    /// <summary>
    /// Aceleración máxima del viento en m/s² (sugerido: 0.0 - 1.5 m/s²).
    /// </summary>
    public double VientoMax { get; set; } = 1.5;

    /// <summary>
    /// Dirección mínima del viento en grados (sugerido: -30° - 30°).
    /// </summary>
    public double DireccionMin { get; set; } = -30.0;

    /// <summary>
    /// Dirección máxima del viento en grados (sugerido: -30° - 30°).
    /// </summary>
    public double DireccionMax { get; set; } = 30.0;

    /// <summary>
    /// Coordenada horizontal del objetivo en metros (por defecto: 25.0 m).
    /// </summary>
    public double ObjetivoX { get; set; } = 25.0;

    /// <summary>
    /// Coordenada vertical del centro del objetivo en metros (por defecto: 1.5 m).
    /// </summary>
    public double ObjetivoY { get; set; } = 1.5;

    /// <summary>
    /// Tolerancia del objetivo en metros en X y en Y (por defecto: 1.0 m).
    /// </summary>
    public double Tolerancia { get; set; } = 1.0;

    /// <summary>
    /// Semilla entera para reproducibilidad del generador aleatorio en Octave.
    /// </summary>
    public int Semilla { get; set; } = 12345;
}
