using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using Taller2.Models;
using Taller2.Services;

namespace Taller2;

public partial class MainForm : Form
{
    private readonly OctaveService _octaveService = new();
    private ResultadoSimulacion? _resultadoActual;
    private ParametrosSimulacion? _parametrosActuales;

    private int _indiceAnimacion = 0;
    private bool _estaAnimando = false;

    public MainForm()
    {
        InitializeComponent();

        // Habilitar DoubleBuffered en panelCanvas para evitar parpadeo gráfico
        HabilitarDobleBuffer(panelCanvas);

        ConfigurarTablas();
        ActualizarResumenInicial();
    }

    private static void HabilitarDobleBuffer(Control control)
    {
        typeof(Control).InvokeMember(
            "DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            control,
            new object[] { true });
    }

    private void ConfigurarTablas()
    {
        // 1. Tabla de Recorrido (Sección 22)
        dgvRecorrido.Columns.Clear();
        dgvRecorrido.Columns.Add("Tiempo", "Tiempo (s)");
        dgvRecorrido.Columns.Add("X", "X (m)");
        dgvRecorrido.Columns.Add("Y", "Y (m)");
        dgvRecorrido.Columns.Add("Vx", "Vx (m/s)");
        dgvRecorrido.Columns.Add("Vy", "Vy (m/s)");
        dgvRecorrido.Columns.Add("Velocidad", "Velocidad total (m/s)");

        // 2. Tabla de Viento (Sección 23)
        dgvViento.Columns.Clear();
        dgvViento.Columns.Add("Tiempo", "Tiempo (s)");
        dgvViento.Columns.Add("Direccion", "Dirección (°)");
        dgvViento.Columns.Add("Magnitud", "Aceleración viento (m/s²)");
        dgvViento.Columns.Add("AxViento", "Ax viento (m/s²)");
        dgvViento.Columns.Add("AyViento", "Ay viento (m/s²)");
    }

    private void ActualizarResumenInicial()
    {
        lblEstadoBanner.Text = "ESTADO: LISTO PARA INICIAR";
        lblEstadoBanner.ForeColor = Color.FromArgb(60, 70, 80);
        lblResumenResultados.Text =
            "Bienvenido al Simulador de Tiro Parabólico con Viento Aleatorio.\n" +
            "GNU Octave ejecutará la integración física completa cada 0.5 s y la generación aleatoria de viento.\n" +
            "Presione 'SIMULAR LANZAMIENTO' para iniciar.";
    }

    private ParametrosSimulacion CapturarParametros()
    {
        return new ParametrosSimulacion
        {
            Angulo = (double)nudAngulo.Value,
            VelocidadInicial = (double)nudVelocidad.Value,
            VientoMin = (double)nudVientoMin.Value,
            VientoMax = (double)nudVientoMax.Value,
            DireccionMin = (double)nudDirMin.Value,
            DireccionMax = (double)nudDirMax.Value,
            ObjetivoX = (double)nudObjetivoX.Value,
            ObjetivoY = (double)nudObjetivoY.Value,
            Tolerancia = (double)nudTolerancia.Value,
            Semilla = (int)nudSemilla.Value
        };
    }

    private bool ValidarEntradas(ParametrosSimulacion p, out string mensajeError)
    {
        mensajeError = string.Empty;

        if (p.Angulo <= 0 || p.Angulo >= 90)
        {
            mensajeError = "El ángulo de lanzamiento debe ser mayor a 0° y menor a 90°.";
            return false;
        }

        if (p.VelocidadInicial <= 0)
        {
            mensajeError = "La velocidad inicial debe ser mayor a 0 m/s.";
            return false;
        }

        if (p.VientoMin < 0)
        {
            mensajeError = "La aceleración mínima del viento debe ser mayor o igual a 0 m/s².";
            return false;
        }

        if (p.VientoMax < p.VientoMin)
        {
            mensajeError = "La aceleración máxima del viento no puede ser menor a la mínima.";
            return false;
        }

        if (p.DireccionMin >= p.DireccionMax)
        {
            mensajeError = "La dirección mínima del viento debe ser menor a la dirección máxima.";
            return false;
        }

        if (p.ObjetivoX <= 0)
        {
            mensajeError = "La posición horizontal del objetivo (X) debe ser mayor a 0 m.";
            return false;
        }

        if (p.Tolerancia <= 0)
        {
            mensajeError = "La tolerancia del objetivo debe ser mayor a 0 m.";
            return false;
        }

        if (p.Semilla <= 0)
        {
            mensajeError = "La semilla debe ser un número entero positivo.";
            return false;
        }

        return true;
    }

    private async void BtnSimular_Click(object? sender, EventArgs e)
    {
        var parametros = CapturarParametros();

        if (!ValidarEntradas(parametros, out var error))
        {
            MessageBox.Show(error, "Validación de Parámetros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnSimular.Enabled = false;
            btnDetener.Enabled = true;
            btnLimpiar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            lblEstadoBanner.Text = "ESTADO: EJECUTANDO SIMULACIÓN EN GNU OCTAVE...";
            lblEstadoBanner.ForeColor = Color.FromArgb(0, 102, 204);
            lblResumenResultados.Text = "C# escribió 'parametros.txt' y está esperando que GNU Octave complete los cálculos físicos...";

            // Ejecución desacoplada en GNU Octave
            var resultado = await _octaveService.EjecutarSimulacionAsync(parametros);

            _resultadoActual = resultado;
            _parametrosActuales = parametros;

            // Poblar tablas DataGridView sin recalcular datos (Secciones 22 y 23)
            CargarTablaRecorrido(resultado.Trayectoria);
            CargarTablaViento(resultado.Vientos);

            // Preparar inicio de la animación
            _indiceAnimacion = 0;
            _estaAnimando = true;
            tabPrincipal.SelectedTab = tabAnimacion;

            lblEstadoBanner.Text = "ESTADO: ANIMACIÓN EN CURSO (0.5 s por frame)...";
            lblEstadoBanner.ForeColor = Color.FromArgb(16, 124, 65);

            ActualizarTelemetria(0);
            panelCanvas.Invalidate();

            timerAnimacion.Start();
        }
        catch (FileNotFoundException ex)
        {
            var respuesta = MessageBox.Show(
                ex.Message + "\n\n¿Desea examinar y seleccionar manualmente la ruta a 'octave-cli.exe'?",
                "GNU Octave No Detectado Automáticamente",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                using var ofd = new OpenFileDialog
                {
                    Title = "Seleccionar ejecutable de GNU Octave (octave-cli.exe)",
                    Filter = "GNU Octave CLI (octave-cli*.exe)|octave-cli*.exe|Ejecutables (*.exe)|*.exe|Todos los archivos (*.*)|*.*",
                    CheckFileExists = true
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _octaveService.RutaOctaveConfigurada = ofd.FileName;
                    MessageBox.Show(
                        $"Ruta configurada correctamente:\n{ofd.FileName}\n\nPresione 'SIMULAR LANZAMIENTO' nuevamente.",
                        "GNU Octave Configurado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

            RestablecerControles();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error durante la ejecución:\n\n{ex.Message}", "Error de Simulación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            RestablecerControles();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void CargarTablaRecorrido(List<PuntoTrayectoria> trayectoria)
    {
        dgvRecorrido.Rows.Clear();
        foreach (var p in trayectoria)
        {
            dgvRecorrido.Rows.Add(
                p.Tiempo.ToString("F3", CultureInfo.InvariantCulture),
                p.X.ToString("F3", CultureInfo.InvariantCulture),
                p.Y.ToString("F3", CultureInfo.InvariantCulture),
                p.Vx.ToString("F3", CultureInfo.InvariantCulture),
                p.Vy.ToString("F3", CultureInfo.InvariantCulture),
                p.Velocidad.ToString("F3", CultureInfo.InvariantCulture));
        }
    }

    private void CargarTablaViento(List<DatoViento> vientos)
    {
        dgvViento.Rows.Clear();
        foreach (var v in vientos)
        {
            dgvViento.Rows.Add(
                v.Tiempo.ToString("F3", CultureInfo.InvariantCulture),
                v.Direccion.ToString("F2", CultureInfo.InvariantCulture),
                v.Magnitud.ToString("F3", CultureInfo.InvariantCulture),
                v.AxViento.ToString("F3", CultureInfo.InvariantCulture),
                v.AyViento.ToString("F3", CultureInfo.InvariantCulture));
        }
    }

    private void TimerAnimacion_Tick(object? sender, EventArgs e)
    {
        if (_resultadoActual == null || _resultadoActual.Trayectoria.Count == 0)
        {
            FinalizarAnimacion();
            return;
        }

        if (_indiceAnimacion >= _resultadoActual.Trayectoria.Count - 1)
        {
            // Se alcanzó el último frame entregado por Octave
            _indiceAnimacion = _resultadoActual.Trayectoria.Count - 1;
            ActualizarTelemetria(_indiceAnimacion);
            panelCanvas.Invalidate();
            FinalizarAnimacion();
            return;
        }

        _indiceAnimacion++;
        ActualizarTelemetria(_indiceAnimacion);
        panelCanvas.Invalidate();
    }

    private void ActualizarTelemetria(int indice)
    {
        if (_resultadoActual == null || indice >= _resultadoActual.Trayectoria.Count)
        {
            return;
        }

        var p = _resultadoActual.Trayectoria[indice];
        var v = indice < _resultadoActual.Vientos.Count ? _resultadoActual.Vientos[indice] : null;

        lblTelemetriaTiempo.Text = string.Create(CultureInfo.InvariantCulture, $"Tiempo: {p.Tiempo:F2} s");
        lblTelemetriaPosicion.Text = string.Create(CultureInfo.InvariantCulture, $"Posición: X = {p.X:F2} m, Y = {p.Y:F2} m");
        lblTelemetriaVelocidades.Text = string.Create(CultureInfo.InvariantCulture,
            $"Vx: {p.Vx:F2} m/s | Vy: {p.Vy:F2} m/s\nVelocidad total: {p.Velocidad:F2} m/s");

        if (v != null)
        {
            lblTelemetriaViento.Text = string.Create(CultureInfo.InvariantCulture,
                $"Viento: {v.Direccion:F1}° | Magnitud: {v.Magnitud:F2} m/s²");
        }
    }

    private void FinalizarAnimacion()
    {
        timerAnimacion.Stop();
        _estaAnimando = false;
        btnSimular.Enabled = true;
        btnDetener.Enabled = false;
        btnLimpiar.Enabled = true;

        if (_resultadoActual == null) return;

        // Mostrar estado final determinado por Octave (Sección 24)
        if (_resultadoActual.Estado == "OBJETIVO_ALCANZADO")
        {
            lblEstadoBanner.Text = "ESTADO: ¡OBJETIVO ALCANZADO!";
            lblEstadoBanner.ForeColor = Color.FromArgb(16, 124, 65); // Verde
        }
        else if (_resultadoActual.Estado == "SIMULACION_FINALIZADA_POR_SUELO")
        {
            lblEstadoBanner.Text = "ESTADO: OBJETIVO NO ALCANZADO (Impactó en el suelo antes de la meta)";
            lblEstadoBanner.ForeColor = Color.FromArgb(196, 43, 28); // Rojo
        }
        else if (_resultadoActual.Estado == "OBJETIVO_NO_ALCANZADO")
        {
            lblEstadoBanner.Text = "ESTADO: OBJETIVO NO ALCANZADO (Sobrepasó la zona objetivo)";
            lblEstadoBanner.ForeColor = Color.FromArgb(196, 43, 28); // Rojo
        }
        else
        {
            lblEstadoBanner.Text = $"ESTADO: {_resultadoActual.Estado}";
            lblEstadoBanner.ForeColor = Color.FromArgb(216, 59, 1); // Ámbar/Naranja
        }

        var tObjStr = _resultadoActual.TiempoObjetivo.HasValue
            ? $"{_resultadoActual.TiempoObjetivo.Value:F3} s"
            : "N/A";

        var tolStr = _parametrosActuales != null
            ? $"{_parametrosActuales.Tolerancia:F2} m"
            : "1.00 m";

        lblResumenResultados.Text = string.Create(CultureInfo.InvariantCulture,
            $"Tiempo final: {_resultadoActual.TiempoFinal:F3} s | Tiempo en objetivo: {tObjStr}\n" +
            $"Posición final: ({_resultadoActual.XFinal:F3}, {_resultadoActual.YFinal:F3}) m | Tolerancia: ±{tolStr}\n" +
            $"Error horizontal: {_resultadoActual.ErrorX:F3} m | Error vertical: {_resultadoActual.ErrorY:F3} m | Distancia euclidiana: {_resultadoActual.DistanciaObjetivo:F3} m");
    }

    private void BtnDetener_Click(object? sender, EventArgs e)
    {
        if (_estaAnimando)
        {
            timerAnimacion.Stop();
            _estaAnimando = false;
            btnSimular.Enabled = true;
            btnDetener.Enabled = false;
            btnLimpiar.Enabled = true;
            lblEstadoBanner.Text = "ESTADO: ANIMACIÓN DETENIDA POR EL USUARIO";
            lblEstadoBanner.ForeColor = Color.FromArgb(100, 110, 120);
        }
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        timerAnimacion.Stop();
        _estaAnimando = false;
        _resultadoActual = null;
        _parametrosActuales = null;
        _indiceAnimacion = 0;

        dgvRecorrido.Rows.Clear();
        dgvViento.Rows.Clear();

        ActualizarResumenInicial();
        ActualizarTelemetria(0);
        RestablecerControles();
        panelCanvas.Invalidate();
    }

    private void RestablecerControles()
    {
        btnSimular.Enabled = true;
        btnDetener.Enabled = false;
        btnLimpiar.Enabled = true;
    }

    private void PanelCanvas_Resize(object? sender, EventArgs e)
    {
        panelCanvas.Invalidate();
    }

    // -------------------------------------------------------------------------
    // DIBUJO CON GDI+ (Secciones 18, 19 y 20)
    // -------------------------------------------------------------------------
    private void PanelCanvas_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var ancho = panelCanvas.ClientSize.Width;
        var alto = panelCanvas.ClientSize.Height;

        if (ancho <= 50 || alto <= 50) return;

        // 1. Fondo cielo suave
        using (var brushCielo = new LinearGradientBrush(
                   new Point(0, 0), new Point(0, alto),
                   Color.FromArgb(230, 243, 255), Color.FromArgb(250, 253, 255)))
        {
            g.FillRectangle(brushCielo, 0, 0, ancho, alto);
        }

        // Márgenes del lienzo en píxeles
        float margenIzq = 50f;
        float margenDer = 50f;
        float margenInf = 60f;
        float margenSup = 50f;

        // Parámetros de referencia para la escala
        double objetivoX = _parametrosActuales?.ObjetivoX ?? (double)nudObjetivoX.Value;
        double objetivoY = _parametrosActuales?.ObjetivoY ?? (double)nudObjetivoY.Value;
        double tolerancia = _parametrosActuales?.Tolerancia ?? (double)nudTolerancia.Value;

        // Cálculo dinámico de escala automática (Sección 19)
        double maxX = Math.Max(30.0, objetivoX + tolerancia + 5.0);
        double maxY = Math.Max(10.0, objetivoY + tolerancia + 3.0);

        if (_resultadoActual != null && _resultadoActual.Trayectoria.Count > 0)
        {
            var maxTrajX = _resultadoActual.Trayectoria.Max(p => p.X);
            var maxTrajY = _resultadoActual.Trayectoria.Max(p => p.Y);
            maxX = Math.Max(maxX, maxTrajX + 4.0);
            maxY = Math.Max(maxY, maxTrajY + 2.5);
        }

        float escalaX = (ancho - margenIzq - margenDer) / (float)maxX;
        float escalaY = (alto - margenInf - margenSup) / (float)maxY;
        float escala = Math.Min(escalaX, escalaY);

        // Funciones locales de transformación de coordenadas (física -> píxel)
        float ToPixelX(double x) => margenIzq + (float)x * escala;
        float ToPixelY(double y) => (alto - margenInf) - (float)y * escala;

        float sueloPixelY = ToPixelY(0.0);

        // 2. Dibujar césped / suelo verde
        using (var brushSuelo = new SolidBrush(Color.FromArgb(50, 160, 65)))
        {
            g.FillRectangle(brushSuelo, 0, sueloPixelY, ancho, alto - sueloPixelY);
        }
        using (var penSuelo = new Pen(Color.FromArgb(30, 120, 45), 3f))
        {
            g.DrawLine(penSuelo, 0, sueloPixelY, ancho, sueloPixelY);
        }

        // 3. Ejes coordenados y marcas de escala en metros
        using (var penEje = new Pen(Color.FromArgb(140, 150, 165), 1.5f))
        using (var fontEje = new Font("Segoe UI", 8f))
        using (var brushTextoEje = new SolidBrush(Color.FromArgb(80, 90, 100)))
        {
            // Marcas en X cada 5 metros
            for (double mx = 0; mx <= maxX; mx += 5.0)
            {
                float px = ToPixelX(mx);
                if (px > ancho - margenDer + 10) break;

                g.DrawLine(penEje, px, sueloPixelY - 4, px, sueloPixelY + 6);
                var etiqueta = $"{mx:0}m";
                var tam = g.MeasureString(etiqueta, fontEje);
                g.DrawString(etiqueta, fontEje, brushTextoEje, px - tam.Width / 2, sueloPixelY + 8);
            }

            // Marcas en Y cada 2 metros
            for (double my = 2.0; my <= maxY; my += 2.0)
            {
                float py = ToPixelY(my);
                if (py < margenSup - 10) break;

                g.DrawLine(penEje, margenIzq - 6, py, margenIzq + 4, py);
                var etiqueta = $"{my:0}m";
                var tam = g.MeasureString(etiqueta, fontEje);
                g.DrawString(etiqueta, fontEje, brushTextoEje, margenIzq - tam.Width - 8, py - tam.Height / 2);
            }
        }

        // 4. Dibujar Zona Objetivo y Portería
        float objPixelX = ToPixelX(objetivoX);
        float objPixelY = ToPixelY(objetivoY);

        // Caja rectangular de tolerancia (Sección 5)
        float zonaMinX = ToPixelX(Math.Max(0, objetivoX - tolerancia));
        float zonaMaxX = ToPixelX(objetivoX + tolerancia);
        float zonaMaxY = ToPixelY(Math.Max(0, objetivoY - tolerancia)); // Más cerca del suelo
        float zonaMinY = ToPixelY(objetivoY + tolerancia);             // Más alto en píxeles

        float anchoZona = zonaMaxX - zonaMinX;
        float altoZona = zonaMaxY - zonaMinY;

        using (var brushZona = new SolidBrush(Color.FromArgb(45, 0, 150, 255)))
        using (var penZona = new Pen(Color.FromArgb(0, 120, 215), 1.5f))
        {
            penZona.DashStyle = DashStyle.Dash;
            g.FillRectangle(brushZona, zonaMinX, zonaMinY, anchoZona, altoZona);
            g.DrawRectangle(penZona, zonaMinX, zonaMinY, anchoZona, altoZona);
        }

        // Portería física (postes blancos con borde)
        float alturaPorteria = (float)(objetivoY + tolerancia);
        float postePixelY = ToPixelY(alturaPorteria);
        float grosorPoste = 4f;

        using (var penPoste = new Pen(Color.White, grosorPoste))
        using (var penBordePoste = new Pen(Color.FromArgb(50, 50, 50), 1f))
        {
            // Poste vertical
            g.DrawLine(penPoste, objPixelX, sueloPixelY, objPixelX, postePixelY);
            g.DrawLine(penBordePoste, objPixelX - grosorPoste / 2, sueloPixelY, objPixelX - grosorPoste / 2, postePixelY);
            g.DrawLine(penBordePoste, objPixelX + grosorPoste / 2, sueloPixelY, objPixelX + grosorPoste / 2, postePixelY);

            // Travesaño superior hacia atrás
            float profundidadRed = 18f;
            g.DrawLine(penPoste, objPixelX, postePixelY, objPixelX + profundidadRed, postePixelY + 5);

            // Red punteada
            using (var penRed = new Pen(Color.FromArgb(160, 200, 200, 200), 1f))
            {
                penRed.DashStyle = DashStyle.Dot;
                g.DrawLine(penRed, objPixelX + profundidadRed, postePixelY + 5, objPixelX + profundidadRed + 6, sueloPixelY);
            }
        }

        // Centro del objetivo (cruz de referencia)
        using (var penCentro = new Pen(Color.FromArgb(220, 30, 30), 2f))
        {
            g.DrawLine(penCentro, objPixelX - 6, objPixelY, objPixelX + 6, objPixelY);
            g.DrawLine(penCentro, objPixelX, objPixelY - 6, objPixelX, objPixelY + 6);
        }

        // 5. Dibujar Trayectoria física calculada por Octave hasta el frame actual
        if (_resultadoActual != null && _resultadoActual.Trayectoria.Count > 0)
        {
            var puntosVisibles = _resultadoActual.Trayectoria
                .Take(_indiceAnimacion + 1)
                .ToList();

            if (puntosVisibles.Count > 1)
            {
                var screenPoints = puntosVisibles
                    .Select(p => new PointF(ToPixelX(p.X), ToPixelY(p.Y)))
                    .ToArray();

                // Trayectoria continua trazada
                using (var penTrayectoria = new Pen(Color.FromArgb(20, 90, 220), 2.5f))
                {
                    g.DrawLines(penTrayectoria, screenPoints);
                }

                // Nodos en cada frame discreto de 0.5 s
                using (var brushPunto = new SolidBrush(Color.FromArgb(0, 102, 204)))
                {
                    for (int k = 0; k < screenPoints.Length; k++)
                    {
                        var pt = screenPoints[k];
                        g.FillEllipse(brushPunto, pt.X - 3, pt.Y - 3, 6, 6);
                    }
                }
            }

            // 6. Dibujar Balón de fútbol en el frame actual (sin emojis)
            var actual = puntosVisibles.Last();
            float balonPx = ToPixelX(actual.X);
            float balonPy = ToPixelY(actual.Y);
            float radioBalon = 8f;

            DibujarBalon(g, balonPx, balonPy, radioBalon);

            // 7. Dibujar Indicador de perturbación del viento actual
            if (_indiceAnimacion < _resultadoActual.Vientos.Count)
            {
                var vientoActual = _resultadoActual.Vientos[_indiceAnimacion];
                DibujarIndicadorViento(g, balonPx, balonPy - 22, vientoActual);
            }
        }
    }

    /// <summary>
    /// Dibuja un balón de fútbol con primitivas geométricas de GDI+ (Sección 18).
    /// </summary>
    private static void DibujarBalon(Graphics g, float cx, float cy, float r)
    {
        // Fondo blanco del balón con borde negro
        using (var brushBalon = new SolidBrush(Color.White))
        using (var penBorde = new Pen(Color.Black, 1.5f))
        {
            g.FillEllipse(brushBalon, cx - r, cy - r, r * 2, r * 2);
            g.DrawEllipse(penBorde, cx - r, cy - r, r * 2, r * 2);
        }

        // Pentágono interior negro representativo
        using (var brushGajo = new SolidBrush(Color.FromArgb(30, 30, 30)))
        {
            float rp = r * 0.45f;
            PointF[] pentagono = new PointF[5];
            for (int k = 0; k < 5; k++)
            {
                double ang = -Math.PI / 2 + k * (2 * Math.PI / 5);
                pentagono[k] = new PointF(cx + (float)(rp * Math.Cos(ang)), cy + (float)(rp * Math.Sin(ang)));
            }
            g.FillPolygon(brushGajo, pentagono);
        }
    }

    /// <summary>
    /// Dibuja la flecha vectorial del viento indicando aceleración y orientación.
    /// </summary>
    private static void DibujarIndicadorViento(Graphics g, float cx, float cy, DatoViento v)
    {
        if (v.Magnitud <= 0.01) return;

        // Longitud proporcional de la flecha
        float longitud = (float)Math.Clamp(v.Magnitud * 20.0, 15.0, 45.0);
        double rad = v.Direccion * Math.PI / 180.0;

        float dx = (float)(longitud * Math.Cos(rad));
        float dy = -(float)(longitud * Math.Sin(rad)); // Invertir en eje Y de pantalla

        using (var penFlecha = new Pen(Color.FromArgb(220, 80, 0), 2f))
        {
            penFlecha.CustomEndCap = new AdjustableArrowCap(4, 4);
            g.DrawLine(penFlecha, cx, cy, cx + dx, cy + dy);
        }
    }
}
