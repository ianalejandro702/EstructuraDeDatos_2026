using System;
using System.Diagnostics;

namespace ProyectoFase2
{
    // ==========================================
    // 1. ESTRUCTURA Y MODELO DE DATOS
    // ==========================================
    public struct RegistroDatos
    {
        public int Id { get; }
        public string HashValidacion { get; }
        public double PesoBytes { get; }

        public RegistroDatos(int id, string hashValidacion, double pesoBytes)
        {
            if (id <= 0)
                throw new ArgumentException("El Id debe ser un entero positivo mayor que cero.", nameof(id));

            if (string.IsNullOrEmpty(hashValidacion))
                throw new ArgumentNullException(nameof(hashValidacion), "HashValidacion no puede ser null ni una cadena vacía.");

            if (pesoBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(pesoBytes), "PesoBytes debe ser un valor numérico positivo mayor que cero.");

            Id = id;
            HashValidacion = hashValidacion;
            PesoBytes = pesoBytes;
        }

        public override string ToString() => $"Id: {Id} | Hash: {HashValidacion} | Peso: {PesoBytes:F2} B";
    }

    class Program
    {
        // Instrumentación y Contadores Globales
        public static long contadorLlamadas = 0;
        public static long contadorComparacionesSel = 0;
        public static long contadorIntercambiosSel = 0;

        // NUEVO: contadores de QuickSort, para poder reportar comparaciones e
        // intercambios igual que en Selección (lo pide el análisis comparativo).
        public static long contadorComparacionesQS = 0;
        public static long contadorIntercambiosQS = 0;

        // NUEVO: umbral de corte a InsertionSort para sublistas pequeñas.
        private const int UMBRAL_INSERCION = 10;

        static void Main(string[] args)
        {
            // NUEVO: pruebas de correctitud antes de correr el benchmark
            // (checklist de "Verificación de Correctitud Antes del Merge").
            EjecutarPruebasDeCorrectitud();

            // CAMBIO: se prueban los tres tamaños que pide el enunciado
            // (n = 100, 1,000 y 10,000) en lugar de un único tamaño fijo.
            int[] tamanos = { 100, 1000, 10000 };
            foreach (int tamano in tamanos)
            {
                EjecutarBenchmark(tamano);
            }
        }

        static void EjecutarBenchmark(int tamano)
        {
            RegistroDatos[] arregloOriginal = GenerarArregloAleatorio(tamano);

            // Clonar arreglos para garantizar igualdad de condiciones
            RegistroDatos[] copiaSeleccion = (RegistroDatos[])arregloOriginal.Clone();
            RegistroDatos[] copiaQuickSort = (RegistroDatos[])arregloOriginal.Clone();

            // ------------------------------------------
            // BENCHMARK 1: Selección Directa (Fase 1)
            // ------------------------------------------
            contadorComparacionesSel = 0;
            contadorIntercambiosSel = 0;
            Stopwatch swSelection = Stopwatch.StartNew();
            OrdenarPorSeleccion(copiaSeleccion);
            swSelection.Stop();
            double msSeleccion = swSelection.Elapsed.TotalMilliseconds; // CAMBIO: TotalMilliseconds (decimal) en vez de ElapsedMilliseconds (entero), para más precisión en n chicos
            long opSeleccion = contadorComparacionesSel + contadorIntercambiosSel;

            // ------------------------------------------
            // BENCHMARK 2: QuickSort (Fase 2)
            // ------------------------------------------
            contadorLlamadas = 0;
            contadorComparacionesQS = 0;
            contadorIntercambiosQS = 0;
            Stopwatch swQuickSort = Stopwatch.StartNew();
            QuickSort(copiaQuickSort, 0, copiaQuickSort.Length - 1);
            swQuickSort.Stop();
            double msQuickSort = swQuickSort.Elapsed.TotalMilliseconds;

            // Validación de Correctitud
            bool seleccionOk = EstaOrdenado(copiaSeleccion);
            bool quickSortOk = EstaOrdenado(copiaQuickSort);

            // ------------------------------------------
            // REPORTE COMPARATIVO DE SALIDA
            // ------------------------------------------
            Console.WriteLine("=========================================================");
            Console.WriteLine($"REPORTE COMPARATIVO DE ORDENAMIENTO (n = {tamano:N0})");
            Console.WriteLine("=========================================================");
            Console.WriteLine("Algoritmo: Selección Directa");
            Console.WriteLine($"  Registros procesados : {tamano:N0}");
            Console.WriteLine($"  Comparaciones        : {contadorComparacionesSel:N0}");
            Console.WriteLine($"  Intercambios         : {contadorIntercambiosSel:N0}");
            Console.WriteLine($"  Tiempo de ejecución  : {msSeleccion:F3} ms");
            Console.WriteLine($"  Estado final         : {(seleccionOk ? "OK: Correcto" : "ERROR: Desordenado")}");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Algoritmo: QuickSort (Pivote Mediana de Tres)"); // CAMBIO: se documenta la nueva estrategia de pivote
            Console.WriteLine($"  Registros procesados : {tamano:N0}");
            Console.WriteLine($"  Llamadas recursivas  : {contadorLlamadas:N0}");
            Console.WriteLine($"  Comparaciones        : {contadorComparacionesQS:N0}"); // NUEVO
            Console.WriteLine($"  Intercambios         : {contadorIntercambiosQS:N0}");   // NUEVO
            Console.WriteLine($"  Tiempo de ejecución  : {msQuickSort:F3} ms");
            Console.WriteLine($"  Estado final         : {(quickSortOk ? "OK: Correcto" : "ERROR: Desordenado")}");
            Console.WriteLine("---------------------------------------------------------");

            double ratio = msQuickSort > 0 ? msSeleccion / msQuickSort : msSeleccion;
            Console.WriteLine($"Ratio de velocidad: QuickSort fue {ratio:F1}x más rápido");
            Console.WriteLine("=========================================================\n");
        }

        // ==========================================
        // 2. IMPLEMENTACIÓN DE QUICKSORT RECURSIVO
        // ==========================================
        public static void QuickSort(RegistroDatos[] arr, int bajo, int alto)
        {
            contadorLlamadas++; // Incremento en el Call Stack

            if (bajo >= alto) return; // Caso base: si bajo >= alto, detiene la recursión

            // NUEVO: caso base extendido. Para sublistas pequeñas, InsertionSort
            // tiene menos overhead que seguir particionando recursivamente
            // (reduce la profundidad del Call Stack, ver Sustento Teórico - Pregunta 2).
            if (alto - bajo < UMBRAL_INSERCION)
            {
                InsertionSortParcial(arr, bajo, alto);
                return;
            }

            int indicePivote = Particionar(arr, bajo, alto);

            // Llamadas recursivas para sublistas izquierda y derecha
            QuickSort(arr, bajo, indicePivote - 1);
            QuickSort(arr, indicePivote + 1, alto);
        }

        private static int Particionar(RegistroDatos[] arr, int bajo, int alto)
        {
            // NUEVO: mediana de tres. Se elige el valor mediano entre arr[bajo],
            // arr[medio] y arr[alto] y se coloca en arr[alto] antes de particionar.
            // Esto evita el peor caso O(n²) con datos ya ordenados (ver Sustento
            // Teórico - Pregunta 1), con un costo de solo dos comparaciones extra.
            ColocarMedianaDeTresAlFinal(arr, bajo, alto);

            RegistroDatos pivote = arr[alto]; // Pivote = mediana de tres
            int i = bajo - 1;

            for (int j = bajo; j < alto; j++)
            {
                contadorComparacionesQS++; // NUEVO
                if (arr[j].Id <= pivote.Id)
                {
                    i++;
                    // Intercambio por tuplas (C# moderno / in-place)
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                    contadorIntercambiosQS++; // NUEVO
                }
            }

            // Colocar el pivote en su posición definitiva
            (arr[i + 1], arr[alto]) = (arr[alto], arr[i + 1]);
            contadorIntercambiosQS++; // NUEVO
            return i + 1;
        }

        // NUEVO: calcula la mediana entre arr[bajo], arr[medio] y arr[alto]
        // y la deja en arr[alto], lista para que Particionar() la use como pivote.
        private static void ColocarMedianaDeTresAlFinal(RegistroDatos[] arr, int bajo, int alto)
        {
            int medio = bajo + (alto - bajo) / 2;

            if (arr[medio].Id < arr[bajo].Id)
                (arr[medio], arr[bajo]) = (arr[bajo], arr[medio]);

            if (arr[alto].Id < arr[bajo].Id)
                (arr[alto], arr[bajo]) = (arr[bajo], arr[alto]);

            if (arr[alto].Id < arr[medio].Id)
                (arr[alto], arr[medio]) = (arr[medio], arr[alto]);

            if (medio != alto)
                (arr[medio], arr[alto]) = (arr[alto], arr[medio]);
        }

        // NUEVO: InsertionSort para el caso base de sublistas pequeñas.
        private static void InsertionSortParcial(RegistroDatos[] arr, int bajo, int alto)
        {
            for (int i = bajo + 1; i <= alto; i++)
            {
                RegistroDatos actual = arr[i];
                int j = i - 1;
                while (j >= bajo && arr[j].Id > actual.Id)
                {
                    contadorComparacionesQS++;
                    arr[j + 1] = arr[j];
                    j--;
                }
                if (j >= bajo) contadorComparacionesQS++; // comparación final que rompió el while
                arr[j + 1] = actual;
            }
        }

        // ==========================================
        // 3. SELECCIÓN DIRECTA Y MÉTODOS AUXILIARES
        // ==========================================
        public static void OrdenarPorSeleccion(RegistroDatos[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    contadorComparacionesSel++;
                    if (arr[j].Id < arr[minIdx].Id)
                    {
                        minIdx = j;
                    }
                }
                if (minIdx != i)
                {
                    contadorIntercambiosSel++;
                    (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
                }
            }
        }

        static RegistroDatos[] GenerarArregloAleatorio(int cantidad)
        {
            Random rnd = new Random(42); // Semilla fija para reproducibilidad
            RegistroDatos[] arreglo = new RegistroDatos[cantidad];

            for (int i = 0; i < cantidad; i++)
            {
                arreglo[i] = new RegistroDatos(
                    id: rnd.Next(1, 100001),
                    hashValidacion: Guid.NewGuid().ToString(),
                    pesoBytes: 1.0 + rnd.NextDouble() * 9999.0
                );
            }
            return arreglo;
        }

        static bool EstaOrdenado(RegistroDatos[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i].Id > arr[i + 1].Id)
                    return false;
            }
            return true;
        }

        // NUEVO: batería de pruebas de correctitud sobre casos extremos,
        // requerida por el checklist antes de hacer el merge.
        static void EjecutarPruebasDeCorrectitud()
        {
            Console.WriteLine("=== Verificación de correctitud (casos extremos) ===");

            void Probar(string nombre, RegistroDatos[] datos)
            {
                RegistroDatos[] copia = (RegistroDatos[])datos.Clone();
                QuickSort(copia, 0, copia.Length - 1);
                bool ok = EstaOrdenado(copia);
                Console.WriteLine($"  [{(ok ? "OK" : "FALLA")}] {nombre} (n={datos.Length})");
            }

            RegistroDatos R(int id) => new RegistroDatos(id, Guid.NewGuid().ToString(), 1.0);

            RegistroDatos[] Rango(int desde, int hasta)
            {
                var arr = new RegistroDatos[hasta - desde + 1];
                for (int i = 0; i < arr.Length; i++) arr[i] = R(desde + i);
                return arr;
            }

            RegistroDatos[] RangoInverso(int desde, int hasta)
            {
                var arr = new RegistroDatos[desde - hasta + 1];
                for (int i = 0; i < arr.Length; i++) arr[i] = R(desde - i);
                return arr;
            }

            Probar("Arreglo vacío", Array.Empty<RegistroDatos>());
            Probar("Un solo elemento", new[] { R(5) });
            Probar("Dos ya ordenados", new[] { R(1), R(2) });
            Probar("Dos invertidos", new[] { R(2), R(1) });
            Probar("Todos iguales", new[] { R(7), R(7), R(7), R(7), R(7) });
            Probar("Ya ordenado ascendente", Rango(1, 50));
            Probar("Ordenado descendente", RangoInverso(50, 1));
            Probar("10 elementos aleatorios", GenerarArregloAleatorio(10));

            Console.WriteLine();
        }
    }
}