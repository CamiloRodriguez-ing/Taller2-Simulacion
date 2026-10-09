namespace Taller2;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.Panel panelParametros;
    private System.Windows.Forms.GroupBox grpLanzamiento;
    private System.Windows.Forms.Label lblAngulo;
    private System.Windows.Forms.NumericUpDown nudAngulo;
    private System.Windows.Forms.Label lblVelocidad;
    private System.Windows.Forms.NumericUpDown nudVelocidad;

    private System.Windows.Forms.GroupBox grpViento;
    private System.Windows.Forms.Label lblVientoMin;
    private System.Windows.Forms.NumericUpDown nudVientoMin;
    private System.Windows.Forms.Label lblVientoMax;
    private System.Windows.Forms.NumericUpDown nudVientoMax;
    private System.Windows.Forms.Label lblDirMin;
    private System.Windows.Forms.NumericUpDown nudDirMin;
    private System.Windows.Forms.Label lblDirMax;
    private System.Windows.Forms.NumericUpDown nudDirMax;

    private System.Windows.Forms.GroupBox grpObjetivo;
    private System.Windows.Forms.Label lblObjetivoX;
    private System.Windows.Forms.NumericUpDown nudObjetivoX;
    private System.Windows.Forms.Label lblObjetivoY;
    private System.Windows.Forms.NumericUpDown nudObjetivoY;
    private System.Windows.Forms.Label lblTolerancia;
    private System.Windows.Forms.NumericUpDown nudTolerancia;

    private System.Windows.Forms.GroupBox grpReproducibilidad;
    private System.Windows.Forms.Label lblSemilla;
    private System.Windows.Forms.NumericUpDown nudSemilla;

    private System.Windows.Forms.Button btnSimular;
    private System.Windows.Forms.Button btnDetener;
    private System.Windows.Forms.Button btnLimpiar;

    private System.Windows.Forms.TabControl tabPrincipal;
    private System.Windows.Forms.TabPage tabAnimacion;
    private System.Windows.Forms.TabPage tabRecorrido;
    private System.Windows.Forms.TabPage tabViento;

    private System.Windows.Forms.Panel panelCanvas;
    private System.Windows.Forms.Panel panelTelemetria;
    private System.Windows.Forms.Label lblTelemetriaTitulo;
    private System.Windows.Forms.Label lblTelemetriaTiempo;
    private System.Windows.Forms.Label lblTelemetriaPosicion;
    private System.Windows.Forms.Label lblTelemetriaVelocidades;
    private System.Windows.Forms.Label lblTelemetriaViento;

    private System.Windows.Forms.DataGridView dgvRecorrido;
    private System.Windows.Forms.DataGridView dgvViento;

    private System.Windows.Forms.Panel panelInferior;
    private System.Windows.Forms.Label lblEstadoBanner;
    private System.Windows.Forms.Label lblResumenResultados;

    private System.Windows.Forms.Timer timerAnimacion;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.splitMain = new System.Windows.Forms.SplitContainer();
        this.panelParametros = new System.Windows.Forms.Panel();
        this.btnLimpiar = new System.Windows.Forms.Button();
        this.btnDetener = new System.Windows.Forms.Button();
        this.btnSimular = new System.Windows.Forms.Button();
        this.grpReproducibilidad = new System.Windows.Forms.GroupBox();
        this.nudSemilla = new System.Windows.Forms.NumericUpDown();
        this.lblSemilla = new System.Windows.Forms.Label();
        this.grpObjetivo = new System.Windows.Forms.GroupBox();
        this.nudTolerancia = new System.Windows.Forms.NumericUpDown();
        this.lblTolerancia = new System.Windows.Forms.Label();
        this.nudObjetivoY = new System.Windows.Forms.NumericUpDown();
        this.lblObjetivoY = new System.Windows.Forms.Label();
        this.nudObjetivoX = new System.Windows.Forms.NumericUpDown();
        this.lblObjetivoX = new System.Windows.Forms.Label();
        this.grpViento = new System.Windows.Forms.GroupBox();
        this.nudDirMax = new System.Windows.Forms.NumericUpDown();
        this.lblDirMax = new System.Windows.Forms.Label();
        this.nudDirMin = new System.Windows.Forms.NumericUpDown();
        this.lblDirMin = new System.Windows.Forms.Label();
        this.nudVientoMax = new System.Windows.Forms.NumericUpDown();
        this.lblVientoMax = new System.Windows.Forms.Label();
        this.nudVientoMin = new System.Windows.Forms.NumericUpDown();
        this.lblVientoMin = new System.Windows.Forms.Label();
        this.grpLanzamiento = new System.Windows.Forms.GroupBox();
        this.nudVelocidad = new System.Windows.Forms.NumericUpDown();
        this.lblVelocidad = new System.Windows.Forms.Label();
        this.nudAngulo = new System.Windows.Forms.NumericUpDown();
        this.lblAngulo = new System.Windows.Forms.Label();
        this.tabPrincipal = new System.Windows.Forms.TabControl();
        this.tabAnimacion = new System.Windows.Forms.TabPage();
        this.panelCanvas = new System.Windows.Forms.Panel();
        this.panelTelemetria = new System.Windows.Forms.Panel();
        this.lblTelemetriaViento = new System.Windows.Forms.Label();
        this.lblTelemetriaVelocidades = new System.Windows.Forms.Label();
        this.lblTelemetriaPosicion = new System.Windows.Forms.Label();
        this.lblTelemetriaTiempo = new System.Windows.Forms.Label();
        this.lblTelemetriaTitulo = new System.Windows.Forms.Label();
        this.tabRecorrido = new System.Windows.Forms.TabPage();
        this.dgvRecorrido = new System.Windows.Forms.DataGridView();
        this.tabViento = new System.Windows.Forms.TabPage();
        this.dgvViento = new System.Windows.Forms.DataGridView();
        this.panelInferior = new System.Windows.Forms.Panel();
        this.lblResumenResultados = new System.Windows.Forms.Label();
        this.lblEstadoBanner = new System.Windows.Forms.Label();
        this.timerAnimacion = new System.Windows.Forms.Timer(this.components);

        ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
        this.splitMain.Panel1.SuspendLayout();
        this.splitMain.Panel2.SuspendLayout();
        this.splitMain.SuspendLayout();
        this.panelParametros.SuspendLayout();
        this.grpReproducibilidad.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudSemilla)).BeginInit();
        this.grpObjetivo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudTolerancia)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudObjetivoY)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudObjetivoX)).BeginInit();
        this.grpViento.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudDirMax)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudDirMin)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudVientoMax)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudVientoMin)).BeginInit();
        this.grpLanzamiento.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudVelocidad)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudAngulo)).BeginInit();
        this.tabPrincipal.SuspendLayout();
        this.tabAnimacion.SuspendLayout();
        this.panelCanvas.SuspendLayout();
        this.panelTelemetria.SuspendLayout();
        this.tabRecorrido.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvRecorrido)).BeginInit();
        this.tabViento.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvViento)).BeginInit();
        this.panelInferior.SuspendLayout();
        this.SuspendLayout();

        // 
        // splitMain
        // 
        this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
        this.splitMain.Location = new System.Drawing.Point(0, 0);
        this.splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        this.splitMain.Panel1.Controls.Add(this.panelParametros);
        this.splitMain.Panel1MinSize = 340;
        // 
        // splitMain.Panel2
        // 
        this.splitMain.Panel2.Controls.Add(this.tabPrincipal);
        this.splitMain.Panel2.Controls.Add(this.panelInferior);
        this.splitMain.Size = new System.Drawing.Size(1260, 780);
        this.splitMain.SplitterDistance = 350;
        this.splitMain.TabIndex = 0;

        // 
        // panelParametros
        // 
        this.panelParametros.AutoScroll = true;
        this.panelParametros.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
        this.panelParametros.Controls.Add(this.btnLimpiar);
        this.panelParametros.Controls.Add(this.btnDetener);
        this.panelParametros.Controls.Add(this.btnSimular);
        this.panelParametros.Controls.Add(this.grpReproducibilidad);
        this.panelParametros.Controls.Add(this.grpObjetivo);
        this.panelParametros.Controls.Add(this.grpViento);
        this.panelParametros.Controls.Add(this.grpLanzamiento);
        this.panelParametros.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelParametros.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.panelParametros.Location = new System.Drawing.Point(0, 0);
        this.panelParametros.Name = "panelParametros";
        this.panelParametros.Padding = new System.Windows.Forms.Padding(12);
        this.panelParametros.Size = new System.Drawing.Size(350, 780);
        this.panelParametros.TabIndex = 0;

        // 
        // grpLanzamiento
        // 
        this.grpLanzamiento.Controls.Add(this.nudVelocidad);
        this.grpLanzamiento.Controls.Add(this.lblVelocidad);
        this.grpLanzamiento.Controls.Add(this.nudAngulo);
        this.grpLanzamiento.Controls.Add(this.lblAngulo);
        this.grpLanzamiento.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpLanzamiento.Location = new System.Drawing.Point(12, 12);
        this.grpLanzamiento.Name = "grpLanzamiento";
        this.grpLanzamiento.Size = new System.Drawing.Size(324, 105);
        this.grpLanzamiento.TabIndex = 0;
        this.grpLanzamiento.TabStop = false;
        this.grpLanzamiento.Text = "Parámetros del lanzamiento";

        // lblAngulo & nudAngulo
        this.lblAngulo.AutoSize = true;
        this.lblAngulo.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblAngulo.Location = new System.Drawing.Point(12, 32);
        this.lblAngulo.Name = "lblAngulo";
        this.lblAngulo.Size = new System.Drawing.Size(155, 15);
        this.lblAngulo.Text = "Ángulo de lanzamiento (°):";

        this.nudAngulo.DecimalPlaces = 1;
        this.nudAngulo.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudAngulo.Location = new System.Drawing.Point(195, 29);
        this.nudAngulo.Maximum = new decimal(new int[] { 89, 0, 0, 0 });
        this.nudAngulo.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.nudAngulo.Name = "nudAngulo";
        this.nudAngulo.Size = new System.Drawing.Size(115, 23);
        this.nudAngulo.Value = new decimal(new int[] { 25, 0, 0, 0 });

        // lblVelocidad & nudVelocidad
        this.lblVelocidad.AutoSize = true;
        this.lblVelocidad.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblVelocidad.Location = new System.Drawing.Point(12, 67);
        this.lblVelocidad.Name = "lblVelocidad";
        this.lblVelocidad.Size = new System.Drawing.Size(134, 15);
        this.lblVelocidad.Text = "Velocidad inicial (m/s):";

        this.nudVelocidad.DecimalPlaces = 1;
        this.nudVelocidad.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudVelocidad.Location = new System.Drawing.Point(195, 64);
        this.nudVelocidad.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        this.nudVelocidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.nudVelocidad.Name = "nudVelocidad";
        this.nudVelocidad.Size = new System.Drawing.Size(115, 23);
        this.nudVelocidad.Value = new decimal(new int[] { 20, 0, 0, 0 });

        // 
        // grpViento
        // 
        this.grpViento.Controls.Add(this.nudDirMax);
        this.grpViento.Controls.Add(this.lblDirMax);
        this.grpViento.Controls.Add(this.nudDirMin);
        this.grpViento.Controls.Add(this.lblDirMin);
        this.grpViento.Controls.Add(this.nudVientoMax);
        this.grpViento.Controls.Add(this.lblVientoMax);
        this.grpViento.Controls.Add(this.nudVientoMin);
        this.grpViento.Controls.Add(this.lblVientoMin);
        this.grpViento.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpViento.Location = new System.Drawing.Point(12, 126);
        this.grpViento.Name = "grpViento";
        this.grpViento.Size = new System.Drawing.Size(324, 175);
        this.grpViento.TabIndex = 1;
        this.grpViento.TabStop = false;
        this.grpViento.Text = "Aceleración del viento (m/s²)";

        // Viento Min
        this.lblVientoMin.AutoSize = true;
        this.lblVientoMin.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblVientoMin.Location = new System.Drawing.Point(12, 30);
        this.lblVientoMin.Name = "lblVientoMin";
        this.lblVientoMin.Size = new System.Drawing.Size(147, 15);
        this.lblVientoMin.Text = "Aceleración mínima (m/s²):";

        this.nudVientoMin.DecimalPlaces = 2;
        this.nudVientoMin.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudVientoMin.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
        this.nudVientoMin.Location = new System.Drawing.Point(195, 27);
        this.nudVientoMin.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        this.nudVientoMin.Name = "nudVientoMin";
        this.nudVientoMin.Size = new System.Drawing.Size(115, 23);
        this.nudVientoMin.Value = new decimal(new int[] { 0, 0, 0, 0 });

        // Viento Max
        this.lblVientoMax.AutoSize = true;
        this.lblVientoMax.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblVientoMax.Location = new System.Drawing.Point(12, 65);
        this.lblVientoMax.Name = "lblVientoMax";
        this.lblVientoMax.Size = new System.Drawing.Size(149, 15);
        this.lblVientoMax.Text = "Aceleración máxima (m/s²):";

        this.nudVientoMax.DecimalPlaces = 2;
        this.nudVientoMax.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudVientoMax.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
        this.nudVientoMax.Location = new System.Drawing.Point(195, 62);
        this.nudVientoMax.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        this.nudVientoMax.Name = "nudVientoMax";
        this.nudVientoMax.Size = new System.Drawing.Size(115, 23);
        this.nudVientoMax.Value = new decimal(new int[] { 15, 0, 0, 65536 });

        // Dir Min
        this.lblDirMin.AutoSize = true;
        this.lblDirMin.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblDirMin.Location = new System.Drawing.Point(12, 100);
        this.lblDirMin.Name = "lblDirMin";
        this.lblDirMin.Size = new System.Drawing.Size(126, 15);
        this.lblDirMin.Text = "Dirección mínima (°):";

        this.nudDirMin.DecimalPlaces = 1;
        this.nudDirMin.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudDirMin.Location = new System.Drawing.Point(195, 97);
        this.nudDirMin.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
        this.nudDirMin.Minimum = new decimal(new int[] { 180, 0, 0, -2147483648 });
        this.nudDirMin.Name = "nudDirMin";
        this.nudDirMin.Size = new System.Drawing.Size(115, 23);
        this.nudDirMin.Value = new decimal(new int[] { 30, 0, 0, -2147483648 });

        // Dir Max
        this.lblDirMax.AutoSize = true;
        this.lblDirMax.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblDirMax.Location = new System.Drawing.Point(12, 135);
        this.lblDirMax.Name = "lblDirMax";
        this.lblDirMax.Size = new System.Drawing.Size(128, 15);
        this.lblDirMax.Text = "Dirección máxima (°):";

        this.nudDirMax.DecimalPlaces = 1;
        this.nudDirMax.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudDirMax.Location = new System.Drawing.Point(195, 132);
        this.nudDirMax.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
        this.nudDirMax.Minimum = new decimal(new int[] { 180, 0, 0, -2147483648 });
        this.nudDirMax.Name = "nudDirMax";
        this.nudDirMax.Size = new System.Drawing.Size(115, 23);
        this.nudDirMax.Value = new decimal(new int[] { 30, 0, 0, 0 });

        // 
        // grpObjetivo
        // 
        this.grpObjetivo.Controls.Add(this.nudTolerancia);
        this.grpObjetivo.Controls.Add(this.lblTolerancia);
        this.grpObjetivo.Controls.Add(this.nudObjetivoY);
        this.grpObjetivo.Controls.Add(this.lblObjetivoY);
        this.grpObjetivo.Controls.Add(this.nudObjetivoX);
        this.grpObjetivo.Controls.Add(this.lblObjetivoX);
        this.grpObjetivo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpObjetivo.Location = new System.Drawing.Point(12, 310);
        this.grpObjetivo.Name = "grpObjetivo";
        this.grpObjetivo.Size = new System.Drawing.Size(324, 140);
        this.grpObjetivo.TabIndex = 2;
        this.grpObjetivo.TabStop = false;
        this.grpObjetivo.Text = "Zona Objetivo (Portería)";

        // Objetivo X
        this.lblObjetivoX.AutoSize = true;
        this.lblObjetivoX.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblObjetivoX.Location = new System.Drawing.Point(12, 30);
        this.lblObjetivoX.Name = "lblObjetivoX";
        this.lblObjetivoX.Size = new System.Drawing.Size(89, 15);
        this.lblObjetivoX.Text = "Objetivo X (m):";

        this.nudObjetivoX.DecimalPlaces = 1;
        this.nudObjetivoX.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudObjetivoX.Location = new System.Drawing.Point(195, 27);
        this.nudObjetivoX.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        this.nudObjetivoX.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.nudObjetivoX.Name = "nudObjetivoX";
        this.nudObjetivoX.Size = new System.Drawing.Size(115, 23);
        this.nudObjetivoX.Value = new decimal(new int[] { 25, 0, 0, 0 });

        // Objetivo Y
        this.lblObjetivoY.AutoSize = true;
        this.lblObjetivoY.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblObjetivoY.Location = new System.Drawing.Point(12, 65);
        this.lblObjetivoY.Name = "lblObjetivoY";
        this.lblObjetivoY.Size = new System.Drawing.Size(89, 15);
        this.lblObjetivoY.Text = "Objetivo Y (m):";

        this.nudObjetivoY.DecimalPlaces = 2;
        this.nudObjetivoY.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudObjetivoY.Location = new System.Drawing.Point(195, 62);
        this.nudObjetivoY.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        this.nudObjetivoY.Name = "nudObjetivoY";
        this.nudObjetivoY.Size = new System.Drawing.Size(115, 23);
        this.nudObjetivoY.Value = new decimal(new int[] { 15, 0, 0, 65536 });

        // Tolerancia
        this.lblTolerancia.AutoSize = true;
        this.lblTolerancia.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblTolerancia.Location = new System.Drawing.Point(12, 100);
        this.lblTolerancia.Name = "lblTolerancia";
        this.lblTolerancia.Size = new System.Drawing.Size(87, 15);
        this.lblTolerancia.Text = "Tolerancia (m):";

        this.nudTolerancia.DecimalPlaces = 2;
        this.nudTolerancia.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudTolerancia.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
        this.nudTolerancia.Location = new System.Drawing.Point(195, 97);
        this.nudTolerancia.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        this.nudTolerancia.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
        this.nudTolerancia.Name = "nudTolerancia";
        this.nudTolerancia.Size = new System.Drawing.Size(115, 23);
        this.nudTolerancia.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // 
        // grpReproducibilidad
        // 
        this.grpReproducibilidad.Controls.Add(this.nudSemilla);
        this.grpReproducibilidad.Controls.Add(this.lblSemilla);
        this.grpReproducibilidad.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.grpReproducibilidad.Location = new System.Drawing.Point(12, 459);
        this.grpReproducibilidad.Name = "grpReproducibilidad";
        this.grpReproducibilidad.Size = new System.Drawing.Size(324, 70);
        this.grpReproducibilidad.TabIndex = 3;
        this.grpReproducibilidad.TabStop = false;
        this.grpReproducibilidad.Text = "Reproducibilidad";

        // Semilla
        this.lblSemilla.AutoSize = true;
        this.lblSemilla.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblSemilla.Location = new System.Drawing.Point(12, 31);
        this.lblSemilla.Name = "lblSemilla";
        this.lblSemilla.Size = new System.Drawing.Size(83, 15);
        this.lblSemilla.Text = "Semilla (rng):";

        this.nudSemilla.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.nudSemilla.Location = new System.Drawing.Point(195, 28);
        this.nudSemilla.Maximum = new decimal(new int[] { 2147483647, 0, 0, 0 });
        this.nudSemilla.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.nudSemilla.Name = "nudSemilla";
        this.nudSemilla.Size = new System.Drawing.Size(115, 23);
        this.nudSemilla.Value = new decimal(new int[] { 12345, 0, 0, 0 });

        // 
        // Botones de acción
        // 
        this.btnSimular.BackColor = System.Drawing.Color.FromArgb(16, 124, 65);
        this.btnSimular.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnSimular.FlatAppearance.BorderSize = 0;
        this.btnSimular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnSimular.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnSimular.ForeColor = System.Drawing.Color.White;
        this.btnSimular.Location = new System.Drawing.Point(12, 545);
        this.btnSimular.Name = "btnSimular";
        this.btnSimular.Size = new System.Drawing.Size(324, 38);
        this.btnSimular.TabIndex = 4;
        this.btnSimular.Text = "SIMULAR LANZAMIENTO";
        this.btnSimular.UseVisualStyleBackColor = false;
        this.btnSimular.Click += new System.EventHandler(this.BtnSimular_Click);

        this.btnDetener.BackColor = System.Drawing.Color.FromArgb(196, 43, 28);
        this.btnDetener.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnDetener.Enabled = false;
        this.btnDetener.FlatAppearance.BorderSize = 0;
        this.btnDetener.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnDetener.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnDetener.ForeColor = System.Drawing.Color.White;
        this.btnDetener.Location = new System.Drawing.Point(12, 591);
        this.btnDetener.Name = "btnDetener";
        this.btnDetener.Size = new System.Drawing.Size(158, 34);
        this.btnDetener.TabIndex = 5;
        this.btnDetener.Text = "DETENER";
        this.btnDetener.UseVisualStyleBackColor = false;
        this.btnDetener.Click += new System.EventHandler(this.BtnDetener_Click);

        this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(100, 110, 120);
        this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnLimpiar.FlatAppearance.BorderSize = 0;
        this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnLimpiar.ForeColor = System.Drawing.Color.White;
        this.btnLimpiar.Location = new System.Drawing.Point(178, 591);
        this.btnLimpiar.Name = "btnLimpiar";
        this.btnLimpiar.Size = new System.Drawing.Size(158, 34);
        this.btnLimpiar.TabIndex = 6;
        this.btnLimpiar.Text = "LIMPIAR";
        this.btnLimpiar.UseVisualStyleBackColor = false;
        this.btnLimpiar.Click += new System.EventHandler(this.BtnLimpiar_Click);

        // 
        // tabPrincipal
        // 
        this.tabPrincipal.Controls.Add(this.tabAnimacion);
        this.tabPrincipal.Controls.Add(this.tabRecorrido);
        this.tabPrincipal.Controls.Add(this.tabViento);
        this.tabPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabPrincipal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.tabPrincipal.Location = new System.Drawing.Point(0, 0);
        this.tabPrincipal.Name = "tabPrincipal";
        this.tabPrincipal.SelectedIndex = 0;
        this.tabPrincipal.Size = new System.Drawing.Size(906, 675);
        this.tabPrincipal.TabIndex = 0;

        // 
        // tabAnimacion
        // 
        this.tabAnimacion.Controls.Add(this.panelCanvas);
        this.tabAnimacion.Location = new System.Drawing.Point(4, 26);
        this.tabAnimacion.Name = "tabAnimacion";
        this.tabAnimacion.Padding = new System.Windows.Forms.Padding(3);
        this.tabAnimacion.Size = new System.Drawing.Size(898, 645);
        this.tabAnimacion.TabIndex = 0;
        this.tabAnimacion.Text = "  Animación  ";
        this.tabAnimacion.UseVisualStyleBackColor = true;

        // 
        // panelCanvas
        // 
        this.panelCanvas.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
        this.panelCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panelCanvas.Controls.Add(this.panelTelemetria);
        this.panelCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelCanvas.Location = new System.Drawing.Point(3, 3);
        this.panelCanvas.Name = "panelCanvas";
        this.panelCanvas.Size = new System.Drawing.Size(892, 639);
        this.panelCanvas.TabIndex = 0;
        this.panelCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelCanvas_Paint);
        this.panelCanvas.Resize += new System.EventHandler(this.PanelCanvas_Resize);

        // 
        // panelTelemetria
        // 
        this.panelTelemetria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.panelTelemetria.BackColor = System.Drawing.Color.FromArgb(220, 255, 255, 255);
        this.panelTelemetria.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panelTelemetria.Controls.Add(this.lblTelemetriaViento);
        this.panelTelemetria.Controls.Add(this.lblTelemetriaVelocidades);
        this.panelTelemetria.Controls.Add(this.lblTelemetriaPosicion);
        this.panelTelemetria.Controls.Add(this.lblTelemetriaTiempo);
        this.panelTelemetria.Controls.Add(this.lblTelemetriaTitulo);
        this.panelTelemetria.Location = new System.Drawing.Point(630, 15);
        this.panelTelemetria.Name = "panelTelemetria";
        this.panelTelemetria.Padding = new System.Windows.Forms.Padding(10);
        this.panelTelemetria.Size = new System.Drawing.Size(248, 160);
        this.panelTelemetria.TabIndex = 0;

        // Telemetría Labels
        this.lblTelemetriaTitulo.AutoSize = true;
        this.lblTelemetriaTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTelemetriaTitulo.Location = new System.Drawing.Point(8, 8);
        this.lblTelemetriaTitulo.Name = "lblTelemetriaTitulo";
        this.lblTelemetriaTitulo.Size = new System.Drawing.Size(124, 15);
        this.lblTelemetriaTitulo.Text = "TELEMETRÍA ACTUAL";

        this.lblTelemetriaTiempo.AutoSize = true;
        this.lblTelemetriaTiempo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        this.lblTelemetriaTiempo.Location = new System.Drawing.Point(8, 32);
        this.lblTelemetriaTiempo.Name = "lblTelemetriaTiempo";
        this.lblTelemetriaTiempo.Size = new System.Drawing.Size(83, 15);
        this.lblTelemetriaTiempo.Text = "Tiempo: 0.00 s";

        this.lblTelemetriaPosicion.AutoSize = true;
        this.lblTelemetriaPosicion.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        this.lblTelemetriaPosicion.Location = new System.Drawing.Point(8, 56);
        this.lblTelemetriaPosicion.Name = "lblTelemetriaPosicion";
        this.lblTelemetriaPosicion.Size = new System.Drawing.Size(147, 15);
        this.lblTelemetriaPosicion.Text = "Posición: X = 0.00, Y = 0.00 m";

        this.lblTelemetriaVelocidades.AutoSize = true;
        this.lblTelemetriaVelocidades.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        this.lblTelemetriaVelocidades.Location = new System.Drawing.Point(8, 82);
        this.lblTelemetriaVelocidades.Name = "lblTelemetriaVelocidades";
        this.lblTelemetriaVelocidades.Size = new System.Drawing.Size(183, 30);
        this.lblTelemetriaVelocidades.Text = "Vx: 0.00 m/s | Vy: 0.00 m/s\nVelocidad total: 0.00 m/s";

        this.lblTelemetriaViento.AutoSize = true;
        this.lblTelemetriaViento.Font = new System.Drawing.Font("Segoe UI", 8.5F);
        this.lblTelemetriaViento.Location = new System.Drawing.Point(8, 122);
        this.lblTelemetriaViento.Name = "lblTelemetriaViento";
        this.lblTelemetriaViento.Size = new System.Drawing.Size(193, 15);
        this.lblTelemetriaViento.Text = "Viento: 0.0° | Acel: 0.00 m/s²";

        // 
        // tabRecorrido
        // 
        this.tabRecorrido.Controls.Add(this.dgvRecorrido);
        this.tabRecorrido.Location = new System.Drawing.Point(4, 26);
        this.tabRecorrido.Name = "tabRecorrido";
        this.tabRecorrido.Padding = new System.Windows.Forms.Padding(3);
        this.tabRecorrido.Size = new System.Drawing.Size(898, 645);
        this.tabRecorrido.TabIndex = 1;
        this.tabRecorrido.Text = "  Tabla de Recorrido  ";
        this.tabRecorrido.UseVisualStyleBackColor = true;

        // dgvRecorrido
        this.dgvRecorrido.AllowUserToAddRows = false;
        this.dgvRecorrido.AllowUserToDeleteRows = false;
        this.dgvRecorrido.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvRecorrido.BackgroundColor = System.Drawing.Color.White;
        this.dgvRecorrido.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvRecorrido.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvRecorrido.Location = new System.Drawing.Point(3, 3);
        this.dgvRecorrido.Name = "dgvRecorrido";
        this.dgvRecorrido.ReadOnly = true;
        this.dgvRecorrido.RowHeadersVisible = false;
        this.dgvRecorrido.Size = new System.Drawing.Size(892, 639);
        this.dgvRecorrido.TabIndex = 0;

        // 
        // tabViento
        // 
        this.tabViento.Controls.Add(this.dgvViento);
        this.tabViento.Location = new System.Drawing.Point(4, 26);
        this.tabViento.Name = "tabViento";
        this.tabViento.Padding = new System.Windows.Forms.Padding(3);
        this.tabViento.Size = new System.Drawing.Size(898, 645);
        this.tabViento.TabIndex = 2;
        this.tabViento.Text = "  Tabla de Viento  ";
        this.tabViento.UseVisualStyleBackColor = true;

        // dgvViento
        this.dgvViento.AllowUserToAddRows = false;
        this.dgvViento.AllowUserToDeleteRows = false;
        this.dgvViento.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvViento.BackgroundColor = System.Drawing.Color.White;
        this.dgvViento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvViento.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvViento.Location = new System.Drawing.Point(3, 3);
        this.dgvViento.Name = "dgvViento";
        this.dgvViento.ReadOnly = true;
        this.dgvViento.RowHeadersVisible = false;
        this.dgvViento.Size = new System.Drawing.Size(892, 639);
        this.dgvViento.TabIndex = 0;

        // 
        // panelInferior
        // 
        this.panelInferior.BackColor = System.Drawing.Color.FromArgb(235, 238, 242);
        this.panelInferior.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panelInferior.Controls.Add(this.lblResumenResultados);
        this.panelInferior.Controls.Add(this.lblEstadoBanner);
        this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelInferior.Location = new System.Drawing.Point(0, 675);
        this.panelInferior.Name = "panelInferior";
        this.panelInferior.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
        this.panelInferior.Size = new System.Drawing.Size(906, 105);
        this.panelInferior.TabIndex = 1;

        // lblEstadoBanner
        this.lblEstadoBanner.AutoSize = true;
        this.lblEstadoBanner.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblEstadoBanner.ForeColor = System.Drawing.Color.FromArgb(80, 90, 100);
        this.lblEstadoBanner.Location = new System.Drawing.Point(12, 8);
        this.lblEstadoBanner.Name = "lblEstadoBanner";
        this.lblEstadoBanner.Size = new System.Drawing.Size(206, 21);
        this.lblEstadoBanner.TabIndex = 0;
        this.lblEstadoBanner.Text = "ESTADO: LISTO PARA INICIAR";

        // lblResumenResultados
        this.lblResumenResultados.AutoSize = true;
        this.lblResumenResultados.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblResumenResultados.ForeColor = System.Drawing.Color.FromArgb(40, 45, 55);
        this.lblResumenResultados.Location = new System.Drawing.Point(14, 38);
        this.lblResumenResultados.Name = "lblResumenResultados";
        this.lblResumenResultados.Size = new System.Drawing.Size(462, 51);
        this.lblResumenResultados.TabIndex = 1;
        this.lblResumenResultados.Text = "Configure los parámetros en el panel izquierdo y pulse 'SIMULAR LANZAMIENTO'.\nOctave calculará la física, la perturbación del viento y evaluará el objetivo.\nC# representará visualmente la animación y cargará las tablas de datos.";

        // 
        // timerAnimacion
        // 
        this.timerAnimacion.Interval = 500;
        this.timerAnimacion.Tick += new System.EventHandler(this.TimerAnimacion_Tick);

        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1260, 780);
        this.Controls.Add(this.splitMain);
        this.MinimumSize = new System.Drawing.Size(1000, 680);
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Simulador de Movimiento Parabólico con Viento Aleatorio (GNU Octave + C# .NET 9)";

        this.splitMain.Panel1.ResumeLayout(false);
        this.splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
        this.splitMain.ResumeLayout(false);
        this.panelParametros.ResumeLayout(false);
        this.grpReproducibilidad.ResumeLayout(false);
        this.grpReproducibilidad.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudSemilla)).EndInit();
        this.grpObjetivo.ResumeLayout(false);
        this.grpObjetivo.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudTolerancia)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudObjetivoY)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudObjetivoX)).EndInit();
        this.grpViento.ResumeLayout(false);
        this.grpViento.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudDirMax)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudDirMin)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudVientoMax)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudVientoMin)).EndInit();
        this.grpLanzamiento.ResumeLayout(false);
        this.grpLanzamiento.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudVelocidad)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudAngulo)).EndInit();
        this.tabPrincipal.ResumeLayout(false);
        this.tabAnimacion.ResumeLayout(false);
        this.panelCanvas.ResumeLayout(false);
        this.panelTelemetria.ResumeLayout(false);
        this.panelTelemetria.PerformLayout();
        this.tabRecorrido.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvRecorrido)).EndInit();
        this.tabViento.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvViento)).EndInit();
        this.panelInferior.ResumeLayout(false);
        this.panelInferior.PerformLayout();
        this.ResumeLayout(false);
    }
}
