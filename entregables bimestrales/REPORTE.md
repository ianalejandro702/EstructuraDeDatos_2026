# Reporte de Simulacro: Estructura de Datos (UNITEC)
Alumno: Ian Alejandro Vargas Arias 
Matrícula: 333009736 
Materia: Estructura de Datos  
Fecha: 22 Junio 2026  


## 1. Introducción y Objetivo
El objetivo de esta práctica es integrar estructuras de datos compuestas e implementar el paso de parámetros por referencia (ref) y por salida (out) en el lenguaje C#. Se busca comprender la diferencia entre la administración de memoria en el **Stack** y el **Heap** mediante la interacción de Clases y Structs.


## 2. Código de la Solución
A continuación se presenta la implementación limpia en C#, aplicando encapsulamiento, inmutabilidad en el struct y validación defensiva:

using System;
using System.Collections.Generic;
using System.Linq;

// ==========================================
// COMPONENTE 1: STRUCT
// ==========================================
public struct PuntoDeRed
{
    public double Latitud { get; }
    public double Longitud { get; }

    public PuntoDeRed(double latitud, double longitud)
    {
        if (latitud < -90.0 || latitud > 90.0)
            throw new ArgumentOutOfRangeException(nameof(latitud), "La latitud debe estar entre -90 y 90 grados.");
        
        if (longitud < -180.0 || longitud > 180.0)
            throw new ArgumentOutOfRangeException(nameof(longitud), "La longitud debe estar entre -180 y 180 grados.");

        Latitud = latitud;
        Longitud = longitud;
    }

    public override string ToString() => $"({Latitud}°, {Longitud}°)";
}

// ==========================================
// COMPONENTES 2 Y 3: CLASE E INTEGRACIÓN DEL MÉTODO
// ==========================================
public class ServidorConexion
{
    public int ID { get; set; }
    public string Nombre { get; set; }
    public PuntoDeRed Ubicacion { get; set; }
    public List<int> CodigosRespuesta { get; set; }

    // Caché para Memoization
    private readonly long[] _cache = new long[100];

    public ServidorConexion(int id, string nombre, PuntoDeRed ubicacion, List<int> codigos)
    {
        ID = id;
        Nombre = nombre;
        Ubicacion = ubicacion;
        CodigosRespuesta = codigos ?? new List<int>();
    }

    // El Componente 3 vive AQUÍ ADENTRO obligatoriamente
    public long DiagnosticarLatencia(int n, out string alerta)
    {
        if (n < 0 || n >= 100)
            throw new ArgumentOutOfRangeException(nameof(n), "El valor de n debe estar entre 0 y 99.");

        if (n <= 1)
        {
            alerta = string.Empty;
            return n;
        }

        if (_cache[n] != 0)
        {
            alerta = string.Empty;
            return _cache[n];
        }

        // Se usa 'out _' simplificado para evitar errores de duplicidad en la misma línea
        _cache[n] = DiagnosticarLatencia(n - 1, out _) + DiagnosticarLatencia(n - 2, out _);

        if (_cache[n] > 10000)
        {
            alerta = $"ALERTA: Índice de estrés crítico en {Nombre}";
        }
        else
        {
            alerta = string.Empty;
        }

        return _cache[n];
    }

    public override string ToString() => $"[{ID}] {Nombre} @ {Ubicacion}";
}

// ==========================================
// COMPONENTE 4: ORQUESTADOR PRINCIPAL
// ==========================================
public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=========================================================");
        Console.WriteLine("  SISTEMA DE MONITOREO DE CONEXIONES DE RED (NETWORK LOGS) ");
        Console.WriteLine("=========================================================");

        // 1. Inicialización de la Base de Datos In-Memory de Servidores
        var servidores = new List<ServidorConexion>
        {
            new ServidorConexion(1, "Servidor-CDMX", new PuntoDeRed(19.43, -99.13), new List<int> { 200, 200, 500 }),
            new ServidorConexion(2, "Servidor-NYC", new PuntoDeRed(40.71, -74.01), new List<int> { 200, 404 }),
            new ServidorConexion(3, "Servidor-Sydney", new PuntoDeRed(-33.87, 151.21), new List<int> { 500, 500 }),
            new ServidorConexion(4, "Servidor-Londres", new PuntoDeRed(51.51, -0.13), new List<int> { 200, 200, 200 })
        };

        // 2. Ejecución y muestra de la consulta LINQ Avanzado
        Console.WriteLine("\n[EJECUTANDO FILTRO LINQ]: Nodos Hemisferio Norte con Errores Críticos (HTTP 500)");
        Console.WriteLine("---------------------------------------------------------------------");

        var servidoresCriticos = servidores
            .Where(s => s.Ubicacion.Latitud > 0 && s.CodigosRespuesta.Contains(500))
            .ToList();

        if (servidoresCriticos.Count > 0)
        {
            foreach (var srv in servidoresCriticos)
            {
                Console.WriteLine($"  [CRÍTICO] {srv}");
            }
        }
        else
        {
            Console.WriteLine("  No se detectaron servidores bajo este criterio.");
        }
        Console.WriteLine("---------------------------------------------------------------------");

        // 3. Capa de Robustez, Entrada Defensiva e Interacción
        try
        {
            Console.WriteLine("\n[DIAGNÓSTICO INTERACTIVO]: Alta y Análisis de un Nuevo Nodo");
            Console.ResetColor();
            
            Console.Write(" -> Ingresa la latitud decimal para el nuevo servidor: ");
            string input = Console.ReadLine();

            // Validación perimetral con TryParse
            if (!double.TryParse(input, out double latitud))
                throw new FormatException($"'{input}' no representa un valor decimal válido para el sistema.");

            // Creación de las estructuras de datos pasadas al constructor
            var puntoValido = new PuntoDeRed(latitud, -102.35);
            var nuevoServidor = new ServidorConexion(99, "Servidor-Evaluacion-Dinamica", puntoValido, new List<int> { 200 });

            Console.WriteLine($"\n  Instanciando: {nuevoServidor}");
            Console.WriteLine("  Calculando índice de estrés mediante algoritmo optimizado (Memoization)...");

            // Evaluación del algoritmo con parámetro out para alertas de estrés
            long indiceEstres = nuevoServidor.DiagnosticarLatencia(20, out string mensajeAlerta);

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine($"  Métrica de latencia/estrés calculada: {indiceEstres}");
            
            if (!string.IsNullOrEmpty(mensajeAlerta))
            {
                Console.WriteLine($"  {mensajeAlerta}");
            }
            else
            {

                Console.WriteLine("  ESTADO DEL SISTEMA: Operando bajo parámetros normales (Óptimo).");
            }
            Console.WriteLine("---------------------------------------------------------------------");
        }
        catch (FormatException fe)
        {
            Console.WriteLine($"\n[ERROR DE FORMATO]: {fe.Message}");
            
        }
        catch (ArgumentOutOfRangeException aore)
        {
            Console.WriteLine($"\n[ERROR DE RANGO GEOGRÁFICO]: {aore.Message}");
            
        }
        catch (OverflowException oe)
        {
            Console.WriteLine($"\n[DESBORDAMIENTO]: {oe.Message}");

        }
        
        Console.WriteLine("\n=== Presiona cualquier tecla para finalizar el monitoreo ===");
        Console.ReadKey();
    }
}

¿Por qué usar ref con un Struct?
Los structs son tipos de valor que por defecto se copian por completo en el Stack al ser pasados a un método. Al utilizar el modificador ref, forzamos al compilador a pasar únicamente la dirección de memoria (un puntero de 4 u 8 bytes), optimizando el rendimiento y evitando la duplicación de datos innecesaria.

¿Por qué usar out para el diagnóstico?
El modificador out permite que el método retorne más de un valor el return tradicional devuelve el índice de estrés, mientras que out devuelve la cadena de texto con el diagnóstico.

Durante la ejecución del programa, la memoria se distribuye de la siguiente manera:

Stack: Almacena las variables locales, las referencias a los objetos y el struct PuntoDeRed.
Heap: Almacena las instancias de las clases como el objeto ServidorConexion y las cadenas de texto dinámicas, las cuales son gestionadas por el Garbage Collector.

Registro de Fallos de Diseño y Principios Violados

Clase Afectada: ServidorConexion
Principio Violado:Principio de Responsabilidad Única
Severidad: Alta
Descripción del Fallo Detectado:La clase ServidorConexion está asumiendo la responsabilidad de evaluar reglas de negocio geográficas (saber si una latitud está en el norte o sur) e interpretar si eso es un peligro, cuando su único rol debería ser gestionar la conectividad del nodo.

Clase Afectada: PuntoDeRed (Struct)
Principio Violado: Encapsulamiento Defensivo / Cohesión
Severidad: Media
Descripción del Fallo Detectado: El struct expone los datos de Latitud y Longitud, pero delega la lógica de su estado a un método externo en otra clase. El struct debería ser capaz de diagnosticar su propia validez posicional de manera autónoma.

Clase Afectada: ServidorConexion
Principio Violado: Acoplamiento Fuerte
Severidad: Alta
Descripción del Fallo Detectado: El método de diagnóstico depende directamente de la implementación concreta del struct PuntoDeRed. Si en el futuro se requiere cambiar a coordenadas en formato UTM o usar otra estructura, el método de la clase se romperá por completo.

Clase Afectada:Program (Main)
Principio Violado: Principio de Abierto/Cerrado
Severidad: Media
Descripción del Fallo Detectado: El flujo principal maneja la lógica de control de errores de forma muy rígida. Si se añaden nuevos tipos de servidores o nuevas estructuras de red, el código del cliente (Main) tendrá que modificarse extensamente en lugar de extenderse mediante abstracciones.

Conclusiones
La combinación de clases y estructuras optimiza el uso de recursos en C#. El uso correcto de ref y out previene la sobrecarga de la memoria Stack y permite diseñar métodos con múltiples salidas de datos sin necesidad de crear clases contenedoras adicionales.