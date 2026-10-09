namespace Taller2.Models;

/// <summary>
/// Representa un punto discreto de la trayectoria física calculado por GNU Octave cada 0.5 segundos.
/// </summary>
public class PuntoTrayectoria
{
    /// <summary>
    /// Tiempo transcurrido en segundos.
    /// </summary>
    public double Tiempo { get; set; }

    /// <summary>
    /// Posición horizontal en metros.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Posición vertical en metros.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Componente horizontal de la velocidad en m/s.
    /// </summary>
    public double Vx { get; set; }

    /// <summary>
    /// Componente vertical de la velocidad en m/s.
    /// </summary>
    public double Vy { get; set; }

    /// <summary>
    /// Magnitud total de la velocidad en m/s.
    /// </summary>
    public double Velocidad { get; set; }
}
