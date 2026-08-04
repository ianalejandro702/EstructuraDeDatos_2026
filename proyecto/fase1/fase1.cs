using System;
using System.Diagnostics;

namespace DataCore
{
    public readonly struct RegistroDatos
    {
        public int Id { get; }
        public long HashValidacion { get; }
        public int PesoBytes { get; }

        public RegistroDatos(int id, long hashValidacion, int pesoBytes)
        {
            if (pesoBytes <= 0)
            {
                throw new ArgumentException(
                    "PesoBytes debe ser mayor a 0. Un registro no puede tener tamaño nulo o negativo.", 
                    nameof(pesoBytes)
                );
            }

            Id = id;
            HashValidacion = hashValidacion;
            PesoBytes = pesoBytes;
        }
    }

    public readonly struct SortReport
    {
        public int TotalComparaciones { get; }
        public int TotalIntercambios { get; }
        public double TiempoEjecucionMs { get; }

        public SortReport(int comparaciones, int intercambios, double tiempoEjecucionMs)
        {
            TotalComparaciones = comparaciones;
            TotalIntercambios = intercambios;
            TiempoEjecucionMs = tiempoEjecucionMs;
        }
    }

    public class Program
    {

        public static SortReport OrdenarPorSeleccion(RegistroDatos[] arr)
        {
            int comparaciones = 0;
            int intercambios = 0;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < arr.Length - 1; i++)
            {
                int indiceMinimo = i;
                for (int j = i + 1; j < arr.Length; j++)
                {
                    comparaciones++;
                    if (arr[j].Id < arr[indiceMinimo].Id)
                    {
                        indiceMinimo = j;
                    }
                }

                if (indiceMinimo != i)
                {
                    // Intercambio elegante mediante tuplas modernas de C# (C# 7.0+)
                    (arr[i], arr[indiceMinimo]) = (arr[indiceMinimo], arr[i]);
                    intercambios++;
                }
            }

            sw.Stop();
            double tiempoMs = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1000.0;

            return new SortReport(comparaciones, intercambios, tiempoMs);
        }

        public static void Main()
        {
            var rng = new Random();
            var arreglo = new RegistroDatos[40];

            try
            {
                for (int i = 0; i < arreglo.Length; i++)
                {
                    arreglo[i] = new RegistroDatos(
                        id: rng.Next(1, 1001),
                        hashValidacion: ((long)rng.Next() << 32) | (long)rng.Next(),
                        pesoBytes: rng.Next(10, 5001) // 5001 exclusivo para generar hasta 5000
                    );
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[ERROR DE INTEGRIDAD] {ex.Message}");
                return;
            }

            Console.WriteLine("=== ESTADO INICIAL ===");
            ImprimirArreglo(arreglo);

            // Ejecución e instrumentación
            SortReport reporte = OrdenarPorSeleccion(arreglo);

            Console.WriteLine("\n=== ESTADO FINAL ORDENADO ===");
            ImprimirArreglo(arreglo);

            Console.WriteLine("\n=== REPORTE DE MÉTRICAS ===");
            Console.WriteLine($"Total Comparaciones : {reporte.TotalComparaciones}");
            Console.WriteLine($"Total Intercambios  : {reporte.TotalIntercambios}");
            Console.WriteLine($"Tiempo de Ejecución : {reporte.TiempoEjecucionMs:F4} ms");
        }

        private static void ImprimirArreglo(RegistroDatos[] arr)
        {
            foreach (var r in arr)
            {
                Console.WriteLine($"Id: {r.Id,4} | Hash: {r.HashValidacion,20} | Peso: {r.PesoBytes,4} bytes");
            }
        }
    }
}