using System;

namespace OptimizadorBitacoras
{
    // ==========================================
    // FASE 3 (Parte A): Estructura de Datos
    // ==========================================
    /// <summary>
    /// Representa una transacción financiera como un valor compacto (Value Type).
    /// </summary>
    public struct Transaccion
    {
        public int Id;           // Identificador único
        public double Monto;     // Importe en moneda local
        public long Timestamp;   // Marca de tiempo en milisegundos (Unix epoch)

        public Transaccion(int id, double monto, long timestamp)
        {
            Id = id;
            Monto = monto;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            return $"ID: {Id,4} | Monto: ${Monto,10:F2} | Timestamp: {Timestamp}";
        }
    }

    class Program
    {
        // ==========================================
        //   FASE 3 (Parte B): Módulo de Insertion Sort
        // ==========================================
        // Ordena un arreglo de Transacciones por ID utilizando Insertion Sort in-place.
        // Mide y retorna el total de desplazamientos realizados.
        static int OrdenarPorInsercion(Transaccion[] arr)
        {
            int contadorDesplazamientos = 0;
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                Transaccion clave = arr[i];
                int j = i - 1;

                // Desplaza los elementos de arr[0..i-1] que tengan un ID mayor que la clave
                while (j >= 0 && arr[j].Id > clave.Id)
                {
                    arr[j + 1] = arr[j]; // Desplazamiento a la derecha por valor (struct)
                    contadorDesplazamientos++;
                    j--;
                }

                // Inserción de la clave en la posición libre adecuada
                arr[j + 1] = clave;
            }

            return contadorDesplazamientos;
        }

        // ==========================================
        //  FASE 3 (Parte C): Módulo Main y Pruebas
        // ==========================================
        static void Main(string[] args)
        {
            try
            {
                // Bitácora de prueba con capacidad fija de 50 elementos
                Transaccion[] bitacora = new Transaccion[50];
                Random rng = new Random();

                // 1. Generación de los primeros 45 elementos en orden ascendente de ID
                for (int i = 0; i < 45; i++)
                {
                    bitacora[i] = new Transaccion(
                        id: i + 1,
                        monto: Math.Round(rng.NextDouble() * 9999.99 + 0.01, 2),
                        timestamp: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + (i * 100)
                    );
                }

                // 2. Generación de los últimos 5 elementos desordenados (Transacciones tardías/ajustes)
                int[] idsAleatorios = new int[] { 78, 3, 99, 12, 55 };
                for (int i = 0; i < 5; i++)
                {
                    bitacora[45 + i] = new Transaccion(
                        id: idsAleatorios[i],
                        monto: Math.Round(rng.NextDouble() * 9999.99 + 0.01, 2),
                        timestamp: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + ((45 + i) * 100)
                    );
                }

                Console.WriteLine("==================================================");
                Console.WriteLine("    OPTIMIZADOR DE BITÁCORAS DE TRANSACCIONES     ");
                Console.WriteLine("==================================================\n");

                // Ejecución e instrumentación
                int totalDesplazamientos = OrdenarPorInsercion(bitacora);

                // Despliegue de resultados
                Console.WriteLine("--- Transacciones Ordenadas por ID ---");
                foreach (var t in bitacora)
                {
                    Console.WriteLine(t.ToString());
                }

                Console.WriteLine("\n--------------------------------------------------");
                Console.WriteLine($"Total de desplazamientos realizados: {totalDesplazamientos}");

                // Cálculo del peor caso teórico: n * (n - 1) / 2 = 50 * 49 / 2 = 1225
                double peorCasoPeor = (50.0 * 49.0) / 2.0;
                double porcentajeEficiencia = (1.0 - ((double)totalDesplazamientos / peorCasoPeor)) * 100.0;

                Console.WriteLine($"Eficiencia: {porcentajeEficiencia:F1}% mejor que el peor caso teórico.");
                Console.WriteLine("--------------------------------------------------");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"[ERROR] Desbordamiento de datos: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"[ERROR] Formato de entrada inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Excepción inesperada: {ex.Message}");
            }
        }
    }
}