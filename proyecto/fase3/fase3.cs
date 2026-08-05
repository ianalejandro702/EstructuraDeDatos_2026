using System;

namespace DataCore
{
    // ==========================================
    // 1. DATO BASE (Fases anteriores)
    // ==========================================
    public struct RegistroDatos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Monto { get; set; }

        public RegistroDatos(int id, string nombre, decimal monto)
        {
            Id = id;
            Nombre = nombre;
            Monto = monto;
        }
    }

    // ==========================================
    // 2. FASE 3: NODO EN EL HEAP
    // ==========================================
    public class NodoRegistro
    {
        public RegistroDatos Dato { get; set; }
        public NodoRegistro? Siguiente { get; set; }

        public NodoRegistro(RegistroDatos dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

    // ==========================================
    // 3. FASE 3: ESTRUCTURA LISTA ENLAZADA
    // ==========================================
    public class TablaDinamica
    {
        private NodoRegistro? cabeza;
        private int contadorRegistros;

        public TablaDinamica()
        {
            cabeza = null;
            contadorRegistros = 0;
        }

        // Inserción al Inicio - O(1)
        public void InsertarInicio(RegistroDatos nuevoRegistro)
        {
            if (nuevoRegistro.Equals(default(RegistroDatos)) && nuevoRegistro.Nombre == null) 
                throw new ArgumentNullException(nameof(nuevoRegistro));

            NodoRegistro nuevoNodo = new NodoRegistro(nuevoRegistro);
            nuevoNodo.Siguiente = cabeza;
            cabeza = nuevoNodo;
            contadorRegistros++;
        }

        // Inserción al Final - O(n)
        public void InsertarFinal(RegistroDatos nuevoRegistro)
        {
            NodoRegistro nuevoNodo = new NodoRegistro(nuevoRegistro);

            if (cabeza == null)
            {
                cabeza = nuevoNodo;
            }
            else
            {
                NodoRegistro actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
            contadorRegistros++;
        }

        // Eliminación por ID - O(n)
        public void EliminarPorId(int idTarget)
        {
            if (cabeza == null) return;

            // Caso especial: eliminar la cabeza
            if (cabeza.Dato.Id == idTarget)
            {
                cabeza = cabeza.Siguiente;
                contadorRegistros--;
                return;
            }

            NodoRegistro anterior = cabeza;
            NodoRegistro? actual = cabeza.Siguiente;

            while (actual != null)
            {
                if (actual.Dato.Id == idTarget)
                {
                    anterior.Siguiente = actual.Siguiente;
                    contadorRegistros--;
                    return;
                }
                anterior = actual;
                actual = actual.Siguiente;
            }
        }

        // Puente de Interoperabilidad: Conversión a Arreglo - O(n)
        public RegistroDatos[] ObtenerComoArreglo()
        {
            RegistroDatos[] resultado = new RegistroDatos[contadorRegistros];
            NodoRegistro? actual = cabeza;
            int i = 0;

            while (actual != null)
            {
                resultado[i] = actual.Dato;
                actual = actual.Siguiente;
                i++;
            }

            return resultado;
        }
    }

    // ==========================================
    // 4. PROGRAMA PRINCIPAL (ORQUESTADOR)
    // ==========================================
    internal class Program
    {
        static void Main(string[] args)
        {
            TablaDinamica dataCore = new TablaDinamica();

            // Paso 1: Insertar 15 registros dinámicos
            for (int i = 1; i <= 15; i++)
            {
                RegistroDatos reg = new RegistroDatos(i, $"Transacción-{i}", i * 100.0m);
                dataCore.InsertarFinal(reg);
                Console.WriteLine($"[INSERT] Registro {i} añadido a la cadena.");
            }

            // Paso 2: Eliminar 2 registros específicos (5 y 11)
            Console.WriteLine("\n--- Eliminando registros con Id 5 y Id 11 ---");
            dataCore.EliminarPorId(5);
            dataCore.EliminarPorId(11);
            Console.WriteLine("Cadena reestructurada exitosamente. Sin NullReferenceException.");

            // Paso 3: Convertir a arreglo y ordenar con QuickSort (Fase 2)
            RegistroDatos[] arreglo = dataCore.ObtenerComoArreglo();
            Console.WriteLine($"\nRegistros en arreglo: {arreglo.Length} (esperado: 13)");

            QuickSort(arreglo, 0, arreglo.Length - 1);

            Console.WriteLine("\n--- Arreglo ordenado por Id (QuickSort) ---");
            foreach (var r in arreglo)
            {
                Console.WriteLine($"Id: {r.Id,-2} | Nombre: {r.Nombre,-15} | Monto: {r.Monto:C}");
            }

            // Opcional para evitar que la consola se cierre de golpe en VS Code
            // Console.ReadKey();
        }

        // ==========================================
        // 5. MOTOR DE ORDENAMIENTO (Fase 2)
        // ==========================================
        static void QuickSort(RegistroDatos[] arr, int izquierda, int derecha)
        {
            if (izquierda < derecha)
            {
                int indicePivote = Particion(arr, izquierda, derecha);
                QuickSort(arr, izquierda, indicePivote - 1);
                QuickSort(arr, indicePivote + 1, derecha);
            }
        }

        static int Particion(RegistroDatos[] arr, int izquierda, int derecha)
        {
            int pivote = arr[derecha].Id;
            int i = izquierda - 1;

            for (int j = izquierda; j < derecha; j++)
            {
                if (arr[j].Id <= pivote)
                {
                    i++;
                    Intercambiar(arr, i, j);
                }
            }
            Intercambiar(arr, i + 1, derecha);
            return i + 1;
        }

        static void Intercambiar(RegistroDatos[] arr, int i, int j)
        {
            RegistroDatos temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}