using System;

namespace DataCore
{
    /// <summary>
    /// Búsqueda binaria indexada sobre un arreglo previamente ordenado.
    /// Complejidad temporal: O(log n) | Complejidad espacial: O(1).
    /// Precondición: arrOrdenado debe estar ordenado ascendentemente por Id.
    /// </summary>
    public static class BusquedaBinaria
    {
        public static (RegistroDatos? registro, int comparaciones) BuscarRegistroIndexado(
            RegistroDatos[] arrOrdenado, int idBuscado)
        {
            if (arrOrdenado == null || arrOrdenado.Length == 0)
                return (null, 0);

            int izq = 0;
            int der = arrOrdenado.Length - 1;
            int comparaciones = 0;

            while (izq <= der)
            {
                int medio = izq + (der - izq) / 2;
                comparaciones++;

                if (arrOrdenado[medio].Id == idBuscado)
                    return (arrOrdenado[medio], comparaciones);
                else if (arrOrdenado[medio].Id < idBuscado)
                    izq = medio + 1;
                else
                    der = medio - 1;
            }

            return (null, comparaciones);
        }
    }
}