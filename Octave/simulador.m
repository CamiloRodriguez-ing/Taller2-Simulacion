% =========================================================================
% SIMULADOR DE MOVIMIENTO PARABÓLICO CON VIENTO ALEATORIO
% Taller 2 - Simulación y Computación Gráfica
%
% Autoridad de Física y Generación Aleatoria: GNU Octave
% Paso temporal fijo: dt = 0.5 s
% =========================================================================

1; % Asegurar que Octave reconozca este archivo como un script

% -------------------------------------------------------------------------
% 1. LOCALIZACIÓN Y RECEPCIÓN DE PARÁMETROS
% -------------------------------------------------------------------------
args = argv();
ruta_parametros = "";
dir_salida = "";

if (length(args) >= 1 && ~isempty(args{1}))
    ruta_parametros = args{1};
else
    % Rutas relativas por defecto según la ubicación de ejecución
    posibles_rutas = {
        "datos/parametros.txt",
        "../datos/parametros.txt",
        "parametros.txt"
    };
    for k = 1:length(posibles_rutas)
        if (exist(posibles_rutas{k}, "file") == 2)
            ruta_parametros = posibles_rutas{k};
            break;
        end
    end
end

if (isempty(ruta_parametros) || exist(ruta_parametros, "file") != 2)
    fprintf(stderr, "Error: No se encontró el archivo de parámetros: %s\n", ruta_parametros);
    exit(1);
end

% Determinar directorio de salida
if (length(args) >= 2 && ~isempty(args{2}))
    dir_salida = args{2};
else
    % Por defecto usar el directorio del archivo de parámetros
    [dir_base, ~, ~] = fileparts(ruta_parametros);
    if (isempty(dir_base))
        dir_salida = ".";
    else
        dir_salida = dir_base;
    end
end

fprintf("Leyendo parámetros desde: %s\n", ruta_parametros);
fprintf("Directorio de salida: %s\n", dir_salida);

% -------------------------------------------------------------------------
% 2. LECTURA Y PARSEO DE PARAMETROS.TXT
% -------------------------------------------------------------------------
fid = fopen(ruta_parametros, "r");
if (fid == -1)
    fprintf(stderr, "Error: No se pudo abrir el archivo %s para lectura.\n", ruta_parametros);
    exit(1);
end

params = struct();
while ~feof(fid)
    linea = fgetl(fid);
    if (~ischar(linea))
        continue;
    end
    linea = strtrim(linea);
    % Ignorar líneas vacías y comentarios
    if (isempty(linea) || linea(1) == '#' || linea(1) == '%')
        continue;
    end
    
    partes = strsplit(linea, "=");
    if (length(partes) == 2)
        clave = tolower(strtrim(partes{1}));
        valor_str = strtrim(partes{2});
        valor_num = str2double(valor_str);
        if (~isnan(valor_num))
            params.(clave) = valor_num;
        end
    end
end
fclose(fid);

% -------------------------------------------------------------------------
% 3. VALIDACIÓN DE PARÁMETROS LEÍDOS
% -------------------------------------------------------------------------
campos_requeridos = {
    "angulo", "velocidad", "viento_min", "viento_max", ...
    "direccion_min", "direccion_max", "objetivo_x", "objetivo_y", ...
    "tolerancia", "semilla"
};

for i = 1:length(campos_requeridos)
    campo = campos_requeridos{i};
    if (~isfield(params, campo))
        fprintf(stderr, "Error: Falta el parámetro obligatorio '%s' en %s.\n", campo, ruta_parametros);
        exit(1);
    end
end

angulo = params.angulo;
velocidad = params.velocidad;
viento_min = params.viento_min;
viento_max = params.viento_max;
direccion_min = params.direccion_min;
direccion_max = params.direccion_max;
objetivo_x = params.objetivo_x;
objetivo_y = params.objetivo_y;
tolerancia = params.tolerancia;
semilla = round(params.semilla);

if (velocidad <= 0)
    fprintf(stderr, "Error: La velocidad inicial debe ser mayor a 0.\n");
    exit(1);
end
if (viento_min < 0 || viento_max < viento_min)
    fprintf(stderr, "Error: Rango de viento inválido (viento_min >= 0 y viento_max >= viento_min).\n");
    exit(1);
end
if (direccion_min >= direccion_max)
    fprintf(stderr, "Error: direccion_min debe ser menor que direccion_max.\n");
    exit(1);
end
if (objetivo_x <= 0)
    fprintf(stderr, "Error: objetivo_x debe ser mayor a 0.\n");
    exit(1);
end
if (tolerancia <= 0)
    fprintf(stderr, "Error: tolerancia debe ser mayor a 0.\n");
    exit(1);
end

% -------------------------------------------------------------------------
% 4. CONFIGURACIÓN INICIAL Y MODELO MATEMÁTICO
% -------------------------------------------------------------------------
% Configurar semilla para reproducibilidad exacta
rng(semilla);

% Constantes físicas y de simulación
g = 9.81;       % Aceleración de la gravedad (m/s^2)
dt = 0.5;       % Paso de tiempo fijo (s)
t_max = 30.0;   % Tiempo máximo de seguridad (s)
epsilon = 1e-9; % Margen para evitar división por cero en cruce

% Conversión de ángulo inicial a radianes y componentes
theta = angulo * pi / 180.0;
vx0 = velocidad * cos(theta);
vy0 = velocidad * sin(theta);

% Vectores de trayectoria
tiempo = [0.0];
x = [0.0];
y = [0.0];
vx = [vx0];
vy = [vy0];
vel = [velocidad];

% Vectores de viento (sincronizados con cada frame)
w_tiempo = [0.0];
w_dir = [];
w_fuerza = [];
w_ax = [];
w_ay = [];

% Generar primer viento para t = 0.0
W_actual = viento_min + (viento_max - viento_min) * rand();
dir_actual = direccion_min + (direccion_max - direccion_min) * rand();
phi_rad = dir_actual * pi / 180.0;
ax_w_actual = W_actual * cos(phi_rad);
ay_w_actual = W_actual * sin(phi_rad);

w_dir = [dir_actual];
w_fuerza = [W_actual];
w_ax = [ax_w_actual];
w_ay = [ay_w_actual];

% Variables de estado final
estado = "CONTINUAR";
es_objetivo_alcanzado = false;
tiempo_objetivo = []; % Vacío si no se alcanza

% -------------------------------------------------------------------------
% 5. BUCLE DE INTEGRACIÓN TEMPORAL (dt = 0.5 s)
% -------------------------------------------------------------------------
i = 1;
while (true)
    % Aceleraciones totales con el viento del intervalo actual
    ax_total = w_ax(i);
    ay_total = -g + w_ay(i);
    
    % Actualización de posiciones según modelo parabólico
    t_sig = tiempo(i) + dt;
    x_sig = x(i) + vx(i) * dt + 0.5 * ax_total * (dt^2);
    y_sig = y(i) + vy(i) * dt + 0.5 * ay_total * (dt^2);
    
    % Actualización de velocidades
    vx_sig = vx(i) + ax_total * dt;
    vy_sig = vy(i) + ay_total * dt;
    vel_sig = sqrt(vx_sig^2 + vy_sig^2);
    
    % Almacenar nuevo punto de trayectoria
    tiempo = [tiempo, t_sig];
    x = [x, x_sig];
    y = [y, y_sig];
    vx = [vx, vx_sig];
    vy = [vy, vy_sig];
    vel = [vel, vel_sig];
    
    % Generar viento para el nuevo frame t_sig
    W_sig = viento_min + (viento_max - viento_min) * rand();
    dir_sig = direccion_min + (direccion_max - direccion_min) * rand();
    phi_sig_rad = dir_sig * pi / 180.0;
    ax_w_sig = W_sig * cos(phi_sig_rad);
    ay_w_sig = W_sig * sin(phi_sig_rad);
    
    w_tiempo = [w_tiempo, t_sig];
    w_dir = [w_dir, dir_sig];
    w_fuerza = [w_fuerza, W_sig];
    w_ax = [w_ax, ax_w_sig];
    w_ay = [w_ay, ay_w_sig];
    
    % ---------------------------------------------------------------------
    % COMPROBACIÓN DE OBJETIVO Y CASOS LÍMITE
    % ---------------------------------------------------------------------
    x_ant = x(i);
    y_ant = y(i);
    t_ant = tiempo(i);
    
    % 1. Comprobación de cruce entre frames (Sección 7)
    cruce_detectado = false;
    if (x_ant < objetivo_x && x_sig >= objetivo_x && abs(x_sig - x_ant) > epsilon)
        alpha = (objetivo_x - x_ant) / (x_sig - x_ant);
        y_cruce = y_ant + alpha * (y_sig - y_ant);
        
        if (abs(y_cruce - objetivo_y) <= tolerancia && y_cruce >= 0)
            cruce_detectado = true;
            es_objetivo_alcanzado = true;
            tiempo_objetivo = t_ant + alpha * (t_sig - t_ant);
            estado = "OBJETIVO_ALCANZADO";
            break;
        end
    end
    
    % 2. Comprobación directa en la zona objetivo (Sección 5 y 6: Casos 1 y 2)
    if (abs(x_sig - objetivo_x) <= tolerancia && abs(y_sig - objetivo_y) <= tolerancia && y_sig >= 0)
        es_objetivo_alcanzado = true;
        if (isempty(tiempo_objetivo))
            tiempo_objetivo = t_sig;
        end
        estado = "OBJETIVO_ALCANZADO";
        break;
    end
    
    % 3. Caso 4: Balón por debajo del suelo (y < 0)
    if (y_sig < 0)
        estado = "SIMULACION_FINALIZADA_POR_SUELO";
        break;
    end
    
    % 4. Caso 5: Supera la zona objetivo por delante (x > Xobjetivo + T)
    if (x_sig > objetivo_x + tolerancia)
        estado = "OBJETIVO_NO_ALCANZADO";
        break;
    end
    
    % 5. Caso 7: Límite de seguridad temporal (t >= t_max)
    if (t_sig >= t_max)
        estado = "SIMULACION_FINALIZADA_POR_LIMITE";
        break;
    end
    
    % Caso 6: Todavía no llega, continuar simulación
    i = i + 1;
end

% -------------------------------------------------------------------------
% 6. CÁLCULO DE ERRORES Y DISTANCIA AL OBJETIVO
% -------------------------------------------------------------------------
t_final = tiempo(end);
x_final = x(end);
y_final = y(end);

error_x = abs(x_final - objetivo_x);
error_y = abs(y_final - objetivo_y);
distancia_objetivo = sqrt(error_x^2 + error_y^2);

fprintf("\n--- RESULTADO DE SIMULACIÓN ---\n");
fprintf("Estado: %s\n", estado);
fprintf("Tiempo final: %.3f s\n", t_final);
fprintf("Posición final: (%.3f, %.3f) m\n", x_final, y_final);
fprintf("Error X: %.3f m | Error Y: %.3f m\n", error_x, error_y);
fprintf("Distancia euclidiana al objetivo: %.3f m\n", distancia_objetivo);
if (~isempty(tiempo_objetivo))
    fprintf("Tiempo estimado de cruce objetivo: %.3f s\n", tiempo_objetivo);
else
    fprintf("Tiempo objetivo: N/A\n");
end
fprintf("Total frames registrados: %d\n", length(tiempo));

% -------------------------------------------------------------------------
% 7. GENERACIÓN DE ARCHIVOS DE SALIDA
% -------------------------------------------------------------------------
% Asegurar que exista la carpeta de salida
if (exist(dir_salida, "dir") != 7)
    mkdir(dir_salida);
end

archivo_recorrido = fullfile(dir_salida, "recorrido.csv");
archivo_viento = fullfile(dir_salida, "viento.csv");
archivo_estado = fullfile(dir_salida, "estado.txt");

% 7.1. Escribir recorrido.csv
fid_rec = fopen(archivo_recorrido, "w");
if (fid_rec == -1)
    fprintf(stderr, "Error: No se pudo escribir %s\n", archivo_recorrido);
    exit(1);
end
fprintf(fid_rec, "tiempo,x,y,vx,vy,velocidad\n");
for k = 1:length(tiempo)
    fprintf(fid_rec, "%.3f,%.3f,%.3f,%.3f,%.3f,%.3f\n", ...
        tiempo(k), x(k), y(k), vx(k), vy(k), vel(k));
end
fclose(fid_rec);
fprintf("Generado: %s\n", archivo_recorrido);

% 7.2. Escribir viento.csv
fid_viento = fopen(archivo_viento, "w");
if (fid_viento == -1)
    fprintf(stderr, "Error: No se pudo escribir %s\n", archivo_viento);
    exit(1);
end
fprintf(fid_viento, "tiempo,direccion,fuerza,ax_viento,ay_viento\n");
for k = 1:length(w_tiempo)
    fprintf(fid_viento, "%.3f,%.2f,%.3f,%.3f,%.3f\n", ...
        w_tiempo(k), w_dir(k), w_fuerza(k), w_ax(k), w_ay(k));
end
fclose(fid_viento);
fprintf("Generado: %s\n", archivo_viento);

% 7.3. Escribir estado.txt
fid_est = fopen(archivo_estado, "w");
if (fid_est == -1)
    fprintf(stderr, "Error: No se pudo escribir %s\n", archivo_estado);
    exit(1);
end
fprintf(fid_est, "estado=%s\n", estado);
fprintf(fid_est, "tiempo_final=%.3f\n", t_final);
fprintf(fid_est, "x_final=%.3f\n", x_final);
fprintf(fid_est, "y_final=%.3f\n", y_final);
fprintf(fid_est, "error_x=%.3f\n", error_x);
fprintf(fid_est, "error_y=%.3f\n", error_y);
fprintf(fid_est, "distancia_objetivo=%.3f\n", distancia_objetivo);
if (~isempty(tiempo_objetivo))
    fprintf(fid_est, "tiempo_objetivo=%.3f\n", tiempo_objetivo);
else
    fprintf(fid_est, "tiempo_objetivo=\n");
end
fclose(fid_est);
fprintf("Generado: %s\n", archivo_estado);


fprintf("Simulación en GNU Octave completada exitosamente.\n");
exit(0);
