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

        public int Cantidad => contadorRegistros;

        public TablaDinamica()
        {
            cabeza = null;
            contadorRegistros = 0;
        }

        public void InsertarInicio(RegistroDatos nuevoRegistro)
        {
            NodoRegistro nuevoNodo = new NodoRegistro(nuevoRegistro);
            nuevoNodo.Siguiente = cabeza;
            cabeza = nuevoNodo;
            contadorRegistros++;
        }

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

        public void EliminarPorId(int idTarget)
        {
            if (cabeza == null) return;

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
    // 4. FASE 2: MOTOR DE ORDENAMIENTO (QuickSort)
    // ==========================================
    public static class Ordenamiento
    {
        public static void QuickSort(RegistroDatos[] arr, int izquierda, int derecha)
        {
            if (izquierda < derecha)
            {
                int indicePivote = Particion(arr, izquierda, derecha);
                QuickSort(arr, izquierda, indicePivote - 1);
                QuickSort(arr, indicePivote + 1, derecha);
            }
        }

        private static int Particion(RegistroDatos[] arr, int izquierda, int derecha)
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

        private static void Intercambiar(RegistroDatos[] arr, int i, int j)
        {
            RegistroDatos temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}