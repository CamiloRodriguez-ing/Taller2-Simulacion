# Simulador de Movimiento Parabólico con Viento Aleatorio
## Integración C# Windows Forms (.NET 9) + GNU Octave

> **Taller 2 — Simulación y Computación Gráfica**  
> **Institución:** Universidad Pedagógica y Tecnológica de Colombia (UPTC) — Seccional Sogamoso  
> **Escuela:** Escuela de Ingeniería de Sistemas y Computación  

---

## Índice

1. [Descripción General](#1-descripción-general)
2. [Regla Fundamental de Arquitectura](#2-regla-fundamental-de-arquitectura)
3. [Requisitos del Sistema e Instalación](#3-requisitos-del-sistema-e-instalación)
   - [3.1. Requisitos de Hardware y Sistema Operativo](#31-requisitos-de-hardware-y-sistema-operativo)
   - [3.2. Instalación del SDK de .NET 9](#32-instalación-del-sdk-de-net-9)
   - [3.3. Instalación de GNU Octave](#33-instalación-de-gnu-octave)
   - [3.4. IDE Recomendado](#34-ide-recomendado)
4. [Estructura del Proyecto](#4-estructura-del-proyecto)
5. [Instrucciones de Ejecución Paso a Paso](#5-instrucciones-de-ejecución-paso-a-paso)
   - [Método 1: Desde Terminal (PowerShell / CMD)](#método-1-desde-terminal-powershell--cmd)
   - [Método 2: Ejecutable Directo (.exe)](#método-2-ejecutable-directo-exe)
   - [Método 3: Desde Visual Studio Community](#método-3-desde-visual-studio-community)
   - [Método 4: Batería Automatizada de Pruebas (--test)](#método-4-batería-automatizada-de-pruebas---test)
   - [Método 5: Ejecución Independiente del Script de Octave](#método-5-ejecución-independiente-del-script-de-octave)
6. [Guía de Uso de la Interfaz Gráfica](#6-guía-de-uso-de-la-interfaz-gráfica)
7. [Detección Automática de GNU Octave](#7-detección-automática-de-gnu-octave)
8. [Casos de Prueba para Sustentación](#8-casos-de-prueba-para-sustentación)
9. [Modelo Físico y Matemático](#9-modelo-físico-y-matemático)
10. [Criterio de Alcance de la Meta y Casos Límite](#10-criterio-de-alcance-de-la-meta-y-casos-límite)
11. [Solución de Problemas (FAQ / Troubleshooting)](#11-solución-de-problemas-faq--troubleshooting)

---

## 1. Descripción General

El simulador recrea el lanzamiento parabólico de un balón de fútbol hacia una portería situada a una distancia horizontal de $25.0\text{ m}$ y una altura de $1.5\text{ m}$, con una tolerancia de éxito de $\pm 1.0\text{ m}$.

Durante el vuelo, el balón es sometido a perturbaciones dinámicas de viento generadas estocásticamente en cada intervalo temporal fijo de **0.5 segundos** ($\Delta t = 0.5\text{ s}$).

---

## 2. Regla Fundamental de Arquitectura

Para garantizar el cumplimiento de los estándares científicos y de ingeniería de software:

* **GNU Octave es la ÚNICA autoridad física y matemática:**
  * Lee el archivo `parametros.txt`.
  * Configura la semilla pseudoaleatoria (`rng(semilla)`).
  * Genera en cada fotograma la magnitud ($W$) y dirección ($\phi$) del viento.
  * Resuelve la integración temporal numérica (posición, velocidad y velocidad total).
  * Evalúa el cruce por la vertical de la meta, el impacto en la zona de tolerancia y todos los casos límite (caída al suelo, sobrepaso, límite de tiempo).
  * Exporta los resultados a `recorrido.csv`, `viento.csv` y `estado.txt`.

* **C# Windows Forms es EXCLUSIVAMENTE la capa de usuario y presentación:**
  * Captura y valida los parámetros ingresados.
  * Escribe el archivo de intercambio `datos/parametros.txt`.
  * Ejecuta `octave-cli` en segundo plano sin intervención del usuario.
  * Valida la integridad y calidad de los archivos de salida generados.
  * Presenta las tablas de datos (`DataGridView`) y reproduce la animación fotograma a fotograma (**500 ms por tick**) mediante **GDI+**.
  * **PROHIBICIÓN ESTRICTA:** C# **NO** recalcula la física, **NO** genera viento aleatorio y **NO** interpola puntos durante la animación.

---

## 3. Requisitos del Sistema e Instalación

### 3.1. Requisitos de Hardware y Sistema Operativo
* **Sistema Operativo:** Windows 10 o Windows 11 (64-bit).
* **Memoria RAM:** Mínimo 4 GB (8 GB recomendado).
* **Espacio en disco:** ~1 GB libre (para .NET 9 SDK, Octave y archivos del proyecto).

---

### 3.2. Instalación del SDK de .NET 9
Para compilar y ejecutar el proyecto se requiere el **.NET 9 SDK (x64)**.

1. Descarga el instalador oficial desde [Microsoft .NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
2. Ejecuta el instalador y sigue las instrucciones.
3. Para comprobar la instalación, abre una terminal (PowerShell o CMD) y escribe:
   ```powershell
   dotnet --version
   ```
   *Debe responder con una versión `9.0.xxx` (por ejemplo, `9.0.101`).*

---

### 3.3. Instalación de GNU Octave
El motor físico requiere **GNU Octave** (versiones 8.x, 9.x, 10.x u 11.x).

1. Descarga el instalador oficial de Windows (archivo `.exe` o `.zip`) desde [GNU Octave Windows Download](https://octave.org/download#ms-windows).
2. Instálalo en una de las rutas predeterminadas de Windows, por ejemplo:
   * `C:\Program Files\GNU Octave\Octave-11.3.0\`
   * `C:\Program Files\GNU Octave\Octave-9.4.0\`
   * o `C:\Octave\`
3. **Verificación:**
   Abre una terminal y ejecuta:
   ```powershell
   & "C:\Program Files\GNU Octave\Octave-11.3.0\mingw64\bin\octave-cli.exe" --version
   ```
   *(Si agregaste Octave al `PATH` de Windows, basta con ejecutar `octave-cli --version`).*

> **Nota:** La aplicación de C# detecta automáticamente `octave-cli.exe` tanto si está en el `PATH` como si está instalado en cualquiera de las subcarpetas típicas de `C:\Program Files\GNU Octave\`. No requiere configuraciones manuales complejas.

---

### 3.4. IDE Recomendado
* **Visual Studio 2022 Community** (versión 17.12 o superior) con la carga de trabajo:
  * **"Desarrollo de escritorio de .NET"** activada.
* Opcionalmente, se puede utilizar **Visual Studio Code** con la extensión *C# Dev Kit* o simplemente la consola con la CLI de `dotnet`.

---

## 4. Estructura del Proyecto

```text
Taller 2/
├── CSharp/
│   ├── Taller2.csproj                # Archivo de proyecto .NET 9 WinForms
│   ├── Program.cs                    # Punto de entrada (modo GUI y CLI --test)
│   ├── MainForm.cs                   # Lógica de interfaz, eventos y render GDI+
│   ├── MainForm.Designer.cs          # Definición visual de controles
│   ├── Models/                       # Modelos de datos desacoplados
│   │   ├── ParametrosSimulacion.cs   # DTO de parámetros de entrada
│   │   ├── PuntoTrayectoria.cs       # Puntos de cinemática (t, x, y, vx, vy, vel)
│   │   ├── DatoViento.cs             # Datos de viento (t, dir, mag, ax, ay)
│   │   └── ResultadoSimulacion.cs    # Resumen y métricas de impacto
│   └── Services/
│       └── OctaveService.cs          # Integración, ejecución y control de calidad
│
├── Octave/
│   ├── simulador.m                   # Script científico de Octave (física + viento)
│   └── resultados/                   # Copia de respaldo de resultados
│
├── datos/                            # Directorio de intercambio desacoplado
│   ├── parametros.txt                # Archivo de entrada creado por C#
│   ├── recorrido.csv                 # Trayectoria física calculada cada 0.5 s
│   ├── viento.csv                    # Viento aleatorio generado cada 0.5 s
│   └── estado.txt                    # Resultado de impacto y errores
│
├── evidencias/                       # 20 Figuras organizadas para la sustentación
│   ├── 01_contexto/
│   ├── 02_modelo/
│   ├── 03_octave/
│   ├── 04_integracion/
│   ├── 05_gui/
│   ├── 06_animacion/
│   ├── 07_tablas/
│   └── 08_pruebas/
│
├── Taller2.sln                       # Solución de Visual Studio
├── README.md                         # Esta guía técnica de requisitos y uso
├── INFORME_ACADEMICO.md              # Informe académico formal en Markdown
└── INFORME_ACADEMICO.docx            # Informe académico formal en Word (con figuras)
```

---

## 5. Instrucciones de Ejecución Paso a Paso

### Método 1: Desde Terminal (PowerShell / CMD)
Este es el método más rápido y directo:

1. Abre una terminal de PowerShell en la carpeta raíz del proyecto (`Taller 2`).
2. Compila la solución:
   ```powershell
   dotnet build
   ```
3. Ejecuta la aplicación gráfica:
   ```powershell
   dotnet run --project CSharp/Taller2.csproj
   ```

---

### Método 2: Ejecutable Directo (.exe)
Una vez compilado el proyecto con `dotnet build`, el ejecutable standalone queda listo en:
```text
CSharp\bin\Debug\net9.0-windows\Taller2.exe
```
Haz doble clic sobre `Taller2.exe` para abrir el simulador directamente.

---

### Método 3: Desde Visual Studio Community
1. Haz doble clic en el archivo `Taller2.sln`.
2. En la barra superior, confirma que la configuración sea `Debug` o `Release` y la plataforma `Any CPU`.
3. Presiona la tecla **F5** (o haz clic en el botón verde **Iniciar**).

---

### Método 4: Batería Automatizada de Pruebas (--test)
El proyecto cuenta con un modo de prueba por consola integrado en `Program.cs`, diseñado para verificar automáticamente todos los casos requeridos por la rúbrica sin necesidad de abrir la GUI:

```powershell
dotnet run --project CSharp/Taller2.csproj -- --test
```

Esta orden ejecuta en segundos:
* **Prueba A:** Parámetros por defecto (25°, 20 m/s, semilla 12345) $\rightarrow$ Objetivo alcanzado.
* **Prueba C:** Reproducibilidad $\rightarrow$ Ejecuta dos veces con la misma semilla y verifica que los CSVs sean 100% idénticos.
* **Prueba D:** Semilla diferente $\rightarrow$ Verifica que el viento cambie al usar la semilla 99999.
* **Prueba B:** Parámetros diferentes (35°, 22 m/s) $\rightarrow$ Recalcula cinemática y verifica sobrepaso.
* **Prueba F:** Fallo por suelo (10°, 10 m/s) $\rightarrow$ Verifica terminación con `SIMULACION_FINALIZADA_POR_SUELO`.

---

### Método 5: Ejecución Independiente del Script de Octave
Para demostrar durante la sustentación que Octave funciona de manera totalmente autónoma:

```powershell
& "C:\Program Files\GNU Octave\Octave-11.3.0\mingw64\bin\octave-cli.exe" --no-gui --quiet Octave/simulador.m datos/parametros.txt datos
```

Esto generará inmediatamente `datos/recorrido.csv`, `datos/viento.csv` y `datos/estado.txt`.

---

## 6. Guía de Uso de la Interfaz Gráfica

Al iniciar la aplicación, la pantalla se organiza en tres zonas principales:

```text
┌────────────────────────────────┬────────────────────────────────────────────────────────┐
│      PANEL DE PARÁMETROS       │                  PESTAÑAS DE CONTENIDO                 │
│                                │   [ Animación ]   [ Tabla Recorrido ]   [ Tabla Viento ]│
│  Lanzamiento:                  ├────────────────────────────────────────────────────────┤
│    - Ángulo de tiro (°)        │                                                        │
│    - Velocidad inicial (m/s)   │                                                        │
│                                │                   CANVAS DE ANIMACIÓN                  │
│  Aceleración del Viento:       │                        (GDI+)                          │
│    - Mínima / Máxima (m/s²)    │         Trayectoria, Portería, Balón y Viento          │
│    - Dirección Mín / Máx (°)   │                                                        │
│                                │                                                        │
│  Zona Objetivo:                │                                                        │
│    - Objetivo X / Y (m)        │                                                        │
│    - Tolerancia (m)            │                                                        │
│                                ├────────────────────────────────────────────────────────┤
│  Semilla (Reproducibilidad):   │               PANEL DE ESTADO Y RESULTADOS             │
│    - Entero positivo           │  Banner: ¡OBJETIVO ALCANZADO! (Color Verde / Rojo)     │
│                                │  Métricas: Tiempo final, error X, error Y, distancias  │
│  [SIMULAR] [DETENER] [LIMPIAR] │                                                        │
└────────────────────────────────┴────────────────────────────────────────────────────────┘
```

### Pasos de Operación:
1. **Configuración:** La interfaz viene precargada con los valores por defecto del taller (Ángulo 25°, Velocidad 20 m/s, Viento 0 a 1.5 m/s², Dirección -30° a 30°, Objetivo en (25, 1.5) m con tolerancia 1 m, Semilla 12345).
2. **Simular:** Pulsa el botón verde **SIMULAR LANZAMIENTO**.
   * C# valida que las entradas sean lógicas.
   * C# escribe `datos/parametros.txt` y lanza GNU Octave.
   * Octave calcula todo y genera los archivos.
   * C# lee los resultados, los valida e inicia automáticamente la **animación a 0.5 s por fotograma**.
3. **Telemetría en Vivo:** Durante la animación, observa en la esquina superior derecha la tarjeta flotante con el tiempo actual, la posición $(x, y)$, las componentes de velocidad $(v_x, v_y)$, la velocidad total y el vector de viento activo.
4. **Pestañas de Tablas:**
   * Haz clic en **`[ Tabla de Recorrido ]`** para ver los registros numéricos exactos de `recorrido.csv`.
   * Haz clic en **`[ Tabla de Viento ]`** para inspeccionar la dirección y magnitud de cada ráfaga en `viento.csv`.
5. **Estado Final:** Al concluir la animación, el panel inferior muestra si se alcanzó el objetivo, el tiempo estimado de cruce, las coordenadas finales y las distancias euclidianas de error.

---

## 7. Detección Automática de GNU Octave

El servicio `CSharp/Services/OctaveService.cs` implementa una estrategia de búsqueda inteligente:

1. **Ruta personalizada:** Revisa si se configuró explícitamente en la propiedad `RutaOctaveConfigurada`.
2. **Variable de entorno PATH:** Comprueba si `octave-cli.exe` está registrado en las variables de entorno de Windows.
3. **Carpetas estándar del sistema:** Escanea automáticamente las instalaciones en:
   * `C:\Program Files\GNU Octave\Octave-*\mingw64\bin\octave-cli.exe`
   * `C:\Program Files\GNU Octave\Octave-*\ucrt64\bin\octave-cli.exe`
   * `C:\Program Files\GNU Octave\Octave-*\bin\octave-cli.exe`
   * `C:\Program Files (x86)\GNU Octave\...`
   * `C:\Octave\...`
4. **Mensaje de diagnóstico claro:** Si no se encuentra ninguna instalación, la aplicación muestra una ventana modal amigable con la instrucción exacta para instalar Octave.

---

## 8. Casos de Prueba para Sustentación

Para evidenciar el correcto funcionamiento durante la calificación:

| Escenario | Parámetros Clave | Resultado Esperado |
|---|---|---|
| **Prueba A (Por defecto)** | 25°, 20 m/s, Semilla 12345 | **OBJETIVO ALCANZADO** (Cruce en vertical a $t = 1.341\text{ s}$, altura $1.989\text{ m}$). |
| **Prueba B (Tiro alto)** | 35°, 22 m/s, Viento 0.2–1.2 m/s² | **OBJETIVO NO ALCANZADO** (El balón supera la portería por encima a $Y = 7.69\text{ m}$). |
| **Prueba C (Reproducibilidad)** | Misma configuración ejecutada 2 veces | Archivos CSV **100% idénticos bit a bit**. |
| **Prueba D (Viento variable)** | Misma física, cambiar Semilla a 99999 | Las ráfagas de viento y la trayectoria cambian inmediatamente. |
| **Prueba F (Caída al suelo)** | 10°, 10 m/s, Objetivo a 35 m | **SIMULACION FINALIZADA POR SUELO** ($y < 0$ en $t = 0.5\text{ s}$). |
| **Prueba G (Validación)** | Ángulo = 95° o Velocidad = -5 m/s | Cuadro de advertencia; C# **no** ejecuta Octave. |

---

## 9. Modelo Físico y Matemático

1. **Descomposición inicial:**
   $$\theta_{rad} = \theta \cdot \frac{\pi}{180}, \quad v_{x0} = v_0 \cos(\theta_{rad}), \quad v_{y0} = v_0 \sin(\theta_{rad})$$

2. **Generación del viento en cada $\Delta t = 0.5\text{ s}$:**
   $$W \sim \mathcal{U}(W_{min}, W_{max}), \quad \phi \sim \mathcal{U}(\phi_{min}, \phi_{max})$$
   $$W_x = W \cos(\phi_{rad}), \quad W_y = W \sin(\phi_{rad})$$

3. **Aceleraciones totales:**
   $$a_x = W_x, \quad a_y = -g + W_y \quad (g = 9.81\text{ m/s}^2)$$

4. **Integración numérica (Taylor / Verlet 2º orden):**
   $$x(i+1) = x(i) + v_x(i)\Delta t + \frac{1}{2}a_x(i)\Delta t^2$$
   $$y(i+1) = y(i) + v_y(i)\Delta t + \frac{1}{2}a_y(i)\Delta t^2$$
   $$v_x(i+1) = v_x(i) + a_x(i)\Delta t, \quad v_y(i+1) = v_y(i) + a_y(i)\Delta t$$
   $$v_{total}(i+1) = \sqrt{v_x(i+1)^2 + v_y(i+1)^2}$$

---

## 10. Criterio de Alcance de la Meta y Casos Límite

* **Zona rectangular:** $[X_{obj} - T, X_{obj} + T] \times [Y_{obj} - T, Y_{obj} + T]$ (con $y \ge 0$).
* **Cruce entre dos fotogramas (Sección 7):**
  Si en el paso $i \rightarrow i+1$ se cumple $x_i < X_{obj}$ y $x_{i+1} \ge X_{obj}$:
  $$\alpha = \frac{X_{obj} - x_i}{x_{i+1} - x_i}, \quad y_{cruce} = y_i + \alpha (y_{i+1} - y_i), \quad t_{objetivo} = t_i + \alpha \Delta t$$
  Si $|y_{cruce} - Y_{obj}| \le T$ y $y_{cruce} \ge 0 \implies \text{OBJETIVO\_ALCANZADO}$.
* **Caída al suelo:** Si $y_{i+1} < 0 \implies \text{SIMULACION\_FINALIZADA\_POR\_SUELO}$.
* **Sobrepaso:** Si $x_{i+1} > X_{obj} + T$ sin impacto previo $\implies \text{OBJETIVO\_NO\_ALCANZADO}$.
* **Límite de seguridad:** Si $t \ge 30\text{ s} \implies \text{SIMULACION\_FINALIZADA\_POR\_LIMITE}$.

---

## 11. Solución de Problemas (FAQ / Troubleshooting)

### 1. ¿Qué hacer si aparece el error "No se encontró GNU Octave"?
* Comprueba que hayas instalado GNU Octave para Windows.
* Si lo instalaste en una carpeta no convencional (como `D:\Herramientas\Octave`), asegúrate de agregar la subcarpeta `bin` o `mingw64\bin` a la variable de entorno `PATH` de Windows.
* Abre PowerShell y confirma que `octave-cli --version` responda correctamente.

### 2. ¿Por qué la animación avanza cada 500 ms?
* El requisito central del taller exige un paso de simulación y animación estricto de **0.5 segundos** por cuadro, reproduciendo los puntos exactos calculados por Octave sin inventar ni interpolar fotogramas intermedios en C#.

### 3. ¿Dónde se guardan los archivos de salida?
* Cada simulación escribe o actualiza los archivos en la carpeta `datos/`:
  * `datos/parametros.txt`
  * `datos/recorrido.csv`
  * `datos/viento.csv`
  * `datos/estado.txt`
* Adicionalmente, el proyecto mantiene una copia de respaldo en `Octave/resultados/`.

### 4. ¿Cómo compilar en modo Release para entrega final?
```powershell
dotnet build -c Release
```
El ejecutable optimizado se generará en:
```text
CSharp\bin\Release\net9.0-windows\Taller2.exe
```
