using System;

namespace RetoMemoria{
    class Program{
        static void Main(string[] args){
            Console.WriteLine("=== DEMOSTRACIÓN DE MEMORIA: STACK VS HEAP ===\n");

            // --- CASO 1: VALUE TYPE (int) ---
            int numeroOriginal = 5;
            Console.WriteLine("--- Pruebas con Tipo de Valor (int) ---");
            Console.WriteLine($"Valor inicial antes de la función: {numeroOriginal}");
            
            CambiarValor(numeroOriginal);
            
            Console.WriteLine($"Valor final después de la función: {numeroOriginal}");
            Console.WriteLine("Nota: El valor NO cambió porque se pasó una copia.\n");


            // --- CASO 2: REFERENCE TYPE (int[]) ---
            int[] arregloOriginal = { 1, 2, 3 };
            Console.WriteLine("--- Pruebas con Tipo de Referencia (int[]) ---");
            Console.WriteLine($"Primer elemento antes de la función: {arregloOriginal[0]}");
            
            CambiarReferencia(arregloOriginal);
            
            Console.WriteLine($"Primer elemento después de la función: {arregloOriginal[0]}");
            Console.WriteLine("Nota: El valor SÍ cambió porque se pasó la dirección de memoria.\n");
        }

        // 1. Intenta cambiar el valor de un entero a 100 (Value Type)
        static void CambiarValor(int x){
            x = 100;
            Console.WriteLine($"[Dentro de CambiarValor]: Modificado localmente a {x}");
        }

        // 2. Intenta cambiar el primer elemento de un arreglo a 100 (Reference Type)
        static void CambiarReferencia(int[] arr){
            if (arr != null && arr.Length > 0){
                arr[0] = 100;
                Console.WriteLine($"[Dentro de CambiarReferencia]: Elemento[0] modificado a {arr[0]}");
            }
        }
    }
}