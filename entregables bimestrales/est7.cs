using System;

namespace SimuladorCallStack{
    // ==========================================
    //       CAPA DE FRONTERA / VALIDACIÓN
    // ==========================================
    public static class Validador{
        // Valida que la entrada del usuario sea un número entero y estrictamente mayor a cero.
        public static bool ValidarEnteroPositivo(string entrada, out int numeroValidado){
            // TryParse evita excepciones catastróficas si el usuario introduce letras.
            if (int.TryParse(entrada, out numeroValidado) && numeroValidado > 0){
                return true; 
            }
            return false;
        }
    }

    // ==========================================
    //   CAPA DE LÓGICA / ALGORITMOS RECURSIVOS
    // ==========================================
    public static class SimuladorStack{
        // Ejercicio A: Cuenta regresiva que visualiza las fases de Apilado y Retorno en el Call Stack.
        public static void ImprimirCuentaRegresiva(int numero){
            // 1. CASO BASE: Detiene la recursión para evitar un StackOverflowException
            if (numero < 1){
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("   [BASE ALCANZADA] -> ¡Iniciando fase de retorno!");
                Console.ResetColor();
                return;
            }

            // FASE DE APILADO (Push): Ocurre antes de la llamada recursiva
            Console.WriteLine($"[APILANDO] Creando marco para: ImprimirCuentaRegresiva({numero})");

            // 2. CASO RECURSIVO: La función se invoca a sí misma reduciendo el problema
            ImprimirCuentaRegresiva(numero - 1);

            // FASE DE RETORNO (Pop): Ocurre en orden inverso (LIFO) al liberar la memoria
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   [LIBERANDO] Destruyendo marco de: ImprimirCuentaRegresiva({numero})");
            Console.ResetColor();
        }

        // Ejercicio B: Sumatoria recursiva desde 1 hasta N.
        public static int SumarHasta(int n){
            // 1. CASO BASE: La suma del número 1 es simplemente 1
            if (n == 1) {
                return 1;
            }

            // 2. CASO RECURSIVO: Acumula el valor actual y delega el resto al siguiente marco en el Stack
            return n + SumarHasta(n - 1);
        }
    }

    // ==========================================
    //      CAPA DE CONTROL / PUNTO DE ENTRADA
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            // Configuración estética inicial de la consola corporativa
            Console.Title = "Simulador de Call Stack - UNITEC";
            
            Console.WriteLine("==================================================");
            Console.WriteLine("             SIMULADOR DE CALL STACK    ");
            Console.WriteLine("==================================================\n");

            // --------------------------------------------------
            //      EJECUCIÓN - EJERCICIO A (Cuenta Regresiva)
            // --------------------------------------------------
            Console.WriteLine("--- Ejecutando Ejercicio A: Cuenta Regresiva ---");
            Console.Write("Introduce el número para la cuenta regresiva (ej. 3): ");
            string entradaA = Console.ReadLine();

            if (Validador.ValidarEnteroPositivo(entradaA, out int numA)){
                SimuladorStack.ImprimirCuentaRegresiva(numA);
                Console.WriteLine("\n¡Despegue/Flujo completado con éxito!\n");
            }
            else{
                ImprimirError("Error: Entrada inválida. Debes ingresar un entero positivo mayor a 0.");
            }

            Console.WriteLine("--------------------------------------------------\n");

            // --------------------------------------------------
            //     EJECUCIÓN - EJERCICIO B (Sumatoria Dinámica)
            // --------------------------------------------------
            Console.WriteLine("--- Ejecutando Ejercicio B: Sumatoria Recursiva ---");
            Console.Write("Introduce el número límite para sumar (n): ");
            string entradaB = Console.ReadLine();

            if (Validador.ValidarEnteroPositivo(entradaB, out int numB)){
                int resultado = SimuladorStack.SumarHasta(numB);
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n✓ Resultado Exitoso: La suma de 1 hasta {numB} es: {resultado}\n");
                Console.ResetColor();
            }
            else{
                ImprimirError("Error: Entrada inválida. Solo se aceptan enteros positivos mayores a 0.");
            }

            // --------------------------------------------------
            //                  CIERRE DEL PROGRAMA
            // --------------------------------------------------
            Console.WriteLine("==================================================");
            Console.WriteLine("Presiona cualquier tecla para salir del simulador...");
            Console.ReadKey();
        }

        // Método auxiliar centralizado para desplegar errores visuales en color rojo.
        private static void ImprimirError(string mensaje){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[X] {mensaje}\n");
            Console.ResetColor();
        }
    }
}