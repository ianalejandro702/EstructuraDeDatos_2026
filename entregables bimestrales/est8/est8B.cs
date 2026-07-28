using System;
using System.Numerics; // Requerido para BigInteger

class Program
{
    static void Main(string[] args)
    {
       

        Console.WriteLine("\n=== REFACTORIZACIÓN PROFESIONAL (BigInteger) ===");
        
        // Prueba solicitada: n = 100
        BigInteger resultadoMasivo = FactorialProfesional(100);
        
        Console.WriteLine($"¡Éxito! 100! calculado con precisión absoluta (158 dígitos):");
        Console.WriteLine(resultadoMasivo);
    }
    
    static BigInteger FactorialProfesional(BigInteger n)
    {
        // Caso Base usando BigInteger.One para evitar conversiones implícitas costosas
        if (n == 0 || n == 1)
            return BigInteger.One;

        // Caso Recursivo
        return n * FactorialProfesional(n - 1);
    }
}