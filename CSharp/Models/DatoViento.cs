namespace Taller2.Models;

/// <summary>
/// Representa la perturbación del viento generada aleatoriamente por GNU Octave en cada intervalo de tiempo.
/// </summary>
public class DatoViento
{
    /// <summary>
    /// Tiempo transcurrido en segundos.
    /// </summary>
    public double Tiempo { get; set; }

    /// <summary>
    /// Dirección del viento en grados sexagesimales.
    /// </summary>
    public double Direccion { get; set; }

    /// <summary>
    /// Magnitud modelada como aceleración del viento en m/s² (columna 'fuerza' en viento.csv).
    /// </summary>
    public double Magnitud { get; set; }

    /// <summary>
    /// Componente horizontal de la aceleración del viento (m/s²).
    /// </summary>
    public double AxViento { get; set; }

    /// <summary>
    /// Componente vertical de la aceleración del viento (m/s²).
    /// </summary>
    public double AyViento { get; set; }
}
