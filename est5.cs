using System;

namespace ArbolBinarioBusqueda;

// ==============================================
//       1. EL MODELO DE DATOS: CLASE NODO
// ==============================================
public class Nodo
{
    // Identificador único para ordenar el árbol (Clave del BST)
    public int ID { get; set; }

    // Información o carga útil que almacena el nodo
    public string Dato { get; set; } = string.Empty;

    // Referencias recursivas a los hijos (pueden ser null si es un nodo hoja)
    public Nodo? HijoIzquierdo { get; set; }
    public Nodo? HijoDerecho { get; set; }

    // Constructor compacto para instanciar nodos fácilmente
    public Nodo(int id, string dato)
    {
        ID = id;
        Dato = dato;
    }
}

// ================================================
//       2. LÓGICA DEL PROGRAMA Y PRUEBAS
// ================================================
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Implementación de Árbol Binario de Búsqueda (BST) ===");

        // 1. Inicializamos la raíz del árbol
        Nodo? raiz = null;

        // 2. Insertamos los datos de prueba
        // Insertamos primero el 5 para que actúe como raíz inicial y balancee el ejemplo
        raiz = InsertarNodo(raiz, new Nodo(5, "Nodo Raíz (5)"));
        raiz = InsertarNodo(raiz, new Nodo(3, "Nodo Izquierdo (3)"));
        raiz = InsertarNodo(raiz, new Nodo(7, "Nodo Derecho (7)"));
        raiz = InsertarNodo(raiz, new Nodo(2, "Nodo Hijo de 3 (2)"));
        raiz = InsertarNodo(raiz, new Nodo(4, "Nodo Hijo de 3 (4)"));

        Console.WriteLine("\n[✔] Árbol construido exitosamente en memoria.");

        // 3. Pruebas de Búsqueda Exitosas (Complejidad Promedio O(log n))
        Console.WriteLine("\n--- Realizando Búsquedas ---");
        ProbarBusqueda(raiz, 3);
        ProbarBusqueda(raiz, 7);
        ProbarBusqueda(raiz, 4);

        // 4. Prueba de Búsqueda de un ID inexistente
        ProbarBusqueda(raiz, 99); 

        Console.WriteLine("\n=======================================================");
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    /// <summary>
    /// Inserta un nuevo nodo de forma recursiva respetando las propiedades de un BST.
    /// </summary>
    public static Nodo InsertarNodo(Nodo? raiz, Nodo nuevoNodo)
    {
        // CASO BASE: Si la posición actual es null, encontramos el lugar del nuevo nodo
        if (raiz == null)
        {
            return nuevoNodo;
        }

        // CASO RECURSIVO: Decidir si el flujo va hacia la izquierda o hacia la derecha
        if (nuevoNodo.ID < raiz.ID)
        {
            // El ID es menor -> Va al subárbol izquierdo
            raiz.HijoIzquierdo = InsertarNodo(raiz.HijoIzquierdo, nuevoNodo);
        }
        else if (nuevoNodo.ID > raiz.ID)
        {
            // El ID es mayor -> Va al subárbol derecho
            raiz.HijoDerecho = InsertarNodo(raiz.HijoDerecho, nuevoNodo);
        }
        // Si nuevoNodo.ID == raiz.ID, el valor ya existe en el BST. Por definición, lo ignoramos.

        return raiz; // Retorna la raíz modificada/actualizada en la cadena de retornos
    }

    /// <summary>
    /// Busca un nodo por su ID aprovechando el descarte binario (O(log n)).
    /// </summary>
    public static string? BuscarNodo(Nodo? raiz, int idTarget)
    {
        // CASO BASE 1: Llegamos a una referencia nula (El elemento no existe en el árbol)
        if (raiz == null)
        {
            return null;
        }

        // CASO BASE 2: ¡Éxito! El ID actual coincide con el que estamos buscando
        if (idTarget == raiz.ID)
        {
            return raiz.Dato;
        }

        // CASO RECURSIVO: Decidir qué mitad del árbol descartar por completo
        if (idTarget < raiz.ID)
        {
            // El target es menor, buscamos a la izquierda descartando la derecha por completo
            return BuscarNodo(raiz.HijoIzquierdo, idTarget);
        }
        else
        {
            // El target es mayor, buscamos a la derecha descartando la izquierda por completo
            return BuscarNodo(raiz.HijoDerecho, idTarget);
        }
    }

    /// <summary>
    /// Método auxiliar para imprimir los resultados con un formato limpio en consola.
    /// </summary>
    private static void ProbarBusqueda(Nodo? raiz, int id)
    {
        Console.Write($"Buscando ID [{id}]... ");
        string? resultado = BuscarNodo(raiz, id);

        if (resultado != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Encontrado -> Dato: \"{resultado}\"");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Resultado -> [No encontrado]");
        }
        Console.ResetColor();
    }
}