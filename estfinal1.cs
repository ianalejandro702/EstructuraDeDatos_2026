using System;

namespace AlgoritmosBusqueda
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Generador de Datos
            int[] matriculas = new int[10000];
            for (int i = 0; i < matriculas.Length; i++)
            {
                matriculas[i] = i + 1;
            }

            // 2. Interacción con el usuario
            Console.Write("Ingresa la matrícula a buscar: ");
            if (!int.TryParse(Console.ReadLine(), out int objetivo))
            {
                Console.WriteLine("Entrada inválida.");
                return;
            }

            // 3. Ejecución e Instrumentación
            int iterLineal, iterBinaria;
            int idxLineal = BusquedaLineal(matriculas, objetivo, out iterLineal);
            int idxBinaria = BusquedaBinaria(matriculas, objetivo, out iterBinaria);

            // 4. Reporte en consola
            Console.WriteLine("\n=== REPORTE DE BÚSQUEDA ===");
            Console.WriteLine($"Tamaño del arreglo:  {matriculas.Length}");
            Console.WriteLine($"Matrícula objetivo: {objetivo}\n");

            if (idxLineal != -1)
                Console.WriteLine($"[Lineal]  Encontrado en índice: {idxLineal} | Iteraciones: {iterLineal}");
            else
                Console.WriteLine($"[Lineal]  No encontrado | Iteraciones: {iterLineal}");

            if (idxBinaria != -1)
                Console.WriteLine($"[Binaria] Encontrado en índice: {idxBinaria} | Iteraciones: {iterBinaria}");
            else
                Console.WriteLine($"[Binaria] No encontrado | Iteraciones: {iterBinaria}");

            Console.WriteLine("\nObservación:");
            Console.WriteLine("La búsqueda lineal incrementa sus iteraciones de forma proporcional al tamaño del problema.");
            Console.WriteLine("La búsqueda binaria mantiene un crecimiento logarítmico sumamente eficiente.");
        }

        static int BusquedaLineal(int[] arr, int objetivo, out int iteraciones)
        {
            iteraciones = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                iteraciones++;
                if (arr[i] == objetivo)
                    return i;
            }
            return -1;
        }

        static int BusquedaBinaria(int[] arr, int objetivo, out int iteraciones)
        {
            iteraciones = 0;
            int izquierda = 0;
            int derecha = arr.Length - 1;

            while (izquierda <= derecha)
            {
                iteraciones++;
                int centro = izquierda + (derecha - izquierda) / 2;

                if (arr[centro] == objetivo)
                    return centro;

                if (arr[centro] < objetivo)
                    izquierda = centro + 1;
                else
                    derecha = centro - 1;
            }
            return -1;
        }
    }
}