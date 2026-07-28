using System;

class Program{
    // Módulo 3: Estructura de tipo referencia (Vive en el Heap)
    class Alumno{
        public string Nombre { get; set; }
    }

    static void Main(){
        Console.WriteLine("=== MÓDULO 1: Intercambiar con 'ref' ===");
        int x = 10;
        int y = 25;
        Console.WriteLine($"Antes: x = {x}, y = {y}");
        
        // Pasamos las variables por referencia (dirección de memoria real)
        Intercambiar(ref x, ref y);
        Console.WriteLine($"Después: x = {x}, y = {y}\n");


        Console.WriteLine("=== MÓDULO 2: Calcular y Validar con 'out' ===");
        // 'resto' no requiere inicialización previa gracias a 'out'
        int cociente = CalcularYValidar(17, 5, out int resto);
        Console.WriteLine($"Cociente (Retorno): {cociente}");
        Console.WriteLine($"Residuo (Parámetro out): {resto}\n");


        Console.WriteLine("=== MÓDULO 3: Comportamiento de Referencias ===");
        Alumno alumno1 = new Alumno { Nombre = "Dany" };
        Alumno alumno2 = alumno1; // Copia la DIRECCIÓN en el Heap, no el objeto

        Console.WriteLine($"Nombre en alumno1: {alumno1.Nombre}");
        
        alumno2.Nombre = "3Treum"; // Modifica el único objeto compartido
        Console.WriteLine($"Nombre modificado mediante alumno2: {alumno2.Nombre}");
        Console.WriteLine($"Nombre final en alumno1: {alumno1.Nombre} (¡Sufrió el cambio!)");
    }

    // Módulo 1: Modifica directamente el Stack del llamador
    static void Intercambiar(ref int a, ref int b){
        int temp = a;
        a = b;
        b = temp;
    }

    // Módulo 2: Contrato estricto de asignación antes de retornar
    static int CalcularYValidar(int dividendo, int divisor, out int residuo){
        residuo = dividendo % divisor; // Obligatorio asignar
        return dividendo / divisor;
    }
}