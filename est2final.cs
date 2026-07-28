using System;

class Program
{
    static void Main(string[] args)
    {
        // MÓDULO 3: Banco de pruebas y manejo de errores
        try
        {
            // MÓDULO 1: Inicialización aleatoria (100 elementos de 0 a 100)
            int[] calificaciones = new int[100];
            Random rng = new Random();

            for (int i = 0; i < calificaciones.Length; i++)
            {
                calificaciones[i] = rng.Next(0, 101); // 101 para incluir el 100
            }

            Console.WriteLine("=== Estado inicial: calificaciones desordenadas ===");
            ImprimirArreglo(calificaciones);

            // MÓDULO 2: Llamada al algoritmo Bubble Sort
            OrdenarPorBurbuja(calificaciones);

            Console.WriteLine("\n=== Estado final: calificaciones ordenadas (menor a mayor) ===");
            ImprimirArreglo(calificaciones);
        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine($"\n[ERROR]: Índice fuera de rango detectado: {ex.Message}");
            Console.WriteLine("Revisa los límites de los ciclos for anidados.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERROR inesperado]: {ex.Message}");
        }
    }

    // MÓDULO 2: Bubble Sort con contador e intercambio con tuplas
    static void OrdenarPorBurbuja(int[] arr)
    {
        int n = arr.Length;
        int contadorIntercambios = 0;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // Intercambio con la sintaxis moderna de tuplas de C#
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    contadorIntercambios++;
                }
            }
        }

        Console.WriteLine($"\nTotal de intercambios realizados: {contadorIntercambios}");
    }

    // Método helper para imprimir
    static void ImprimirArreglo(int[] arr)
    {
        Console.WriteLine(string.Join(", ", arr));
    }
}