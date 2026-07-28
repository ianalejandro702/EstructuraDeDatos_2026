using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===== CICLO DE DIAGNÓSTICO (int) =====");
        for (int i = 1; i <= 20; i++)
        {
            Console.WriteLine($"n={i:D2} | Recursivo: {FactorialInt(i),25} | Iterativo: {FactorialIterativo(i),25}");
        }
        
        // NOTA DE DIAGNÓSTICO EVALUADA:
        // El punto de quiebre exacto ocurre en n = 13. El valor real de 13! debería ser: 6,227,020,800
        // El límite máximo de un int de 32 bits es: 2,147,483,647
        // Resultado erróneo producido por wraparound: 1,932,053,504 (en modo unchecked por defecto)
    }

    // Cálculo del factorial mediante enfoque recursivo tradicional utilizando Call Stack.

    static int FactorialInt(int n)
    {
        if (n == 0 || n == 1)
            return 1;
            
        return n * FactorialInt(n - 1);
    }

    // Cálculo del factorial mediante enfoque iterativo sin uso extra del Call Stack.
    static int FactorialIterativo(int n)
    {
        int resultado = 1;
        for (int i = 2; i <= n; i++)
        {
            resultado *= i;
        }
        return resultado;
    }
}