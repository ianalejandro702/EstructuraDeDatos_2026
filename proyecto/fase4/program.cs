using System;

namespace DataCore
{
    public static class Program
    {
        private static TablaDinamica dataCore = new TablaDinamica();
        private static RegistroDatos[]? indiceOrdenado = null;

        public static void Main(string[] args)
        {
            int opcion;
            bool salir = false;

            do
            {
                MostrarMenu();
                string? input = Console.ReadLine() ?? "";

                try
                {
                    if (!int.TryParse(input, out opcion))
                    {
                        Console.WriteLine("\nERROR: ingresa un número válido (0 al 5).\n");
                        continue;
                    }

                    switch (opcion)
                    {
                        case 1: EjecutarInsercion(); break;
                        case 2: EjecutarEliminacion(); break;
                        case 3: EjecutarMostrar(); break;
                        case 4: EjecutarOrdenar(); break;
                        case 5: EjecutarBusqueda(); break;
                        case 0: salir = ConfirmarSalida(); break;
                        default:
                            Console.WriteLine("\nOpción inválida. Elige un número del 0 al 5.\n");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nERROR: formato de entrada inválido. Intenta de nuevo.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError inesperado: {ex.Message}\n");
                }

            } while (!salir);

            Console.WriteLine("\nSesión finalizada. Hasta luego.");
        }

        private static void MostrarMenu()
        {
            Console.WriteLine("===========================================");
            Console.WriteLine(" DATACORE — MENÚ MAESTRO");
            Console.WriteLine("===========================================");
            Console.WriteLine($" Registros actuales en memoria: {dataCore.Cantidad}");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(" [1] Insertar nuevo registro");
            Console.WriteLine(" [2] Eliminar registro por Id");
            Console.WriteLine(" [3] Mostrar todos los registros");
            Console.WriteLine(" [4] Ordenar (QuickSort) y construir índice");
            Console.WriteLine(" [5] Búsqueda binaria indexada");
            Console.WriteLine(" [0] Salir del sistema");
            Console.WriteLine("===========================================");
            Console.Write("Seleccione una opción: ");
        }

        private static void EjecutarInsercion()
        {
            Console.Write("\nId del registro (entero positivo): ");
            string? idTexto = Console.ReadLine() ?? "";
            if (!int.TryParse(idTexto, out int id))
            {
                Console.WriteLine("ERROR: el Id debe ser un número entero.\n");
                return;
            }

            Console.Write("Nombre del registro: ");
            string nombre = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("ERROR: el nombre no puede estar vacío.\n");
                return;
            }

            Console.Write("Monto (número, ej. 150.50): ");
            string? montoTexto = Console.ReadLine() ?? "";
            if (!decimal.TryParse(montoTexto, out decimal monto))
            {
                Console.WriteLine("ERROR: el monto debe ser un número.\n");
                return;
            }

            var nuevoRegistro = new RegistroDatos(id, nombre, monto);
            dataCore.InsertarFinal(nuevoRegistro);
            indiceOrdenado = null;

            Console.WriteLine($"[OK] Registro con Id {id} insertado correctamente.\n");
        }

        private static void EjecutarEliminacion()
        {
            Console.Write("\nId del registro a eliminar: ");
            string? idTexto = Console.ReadLine() ?? "";
            if (!int.TryParse(idTexto, out int id))
            {
                Console.WriteLine("ERROR: el Id debe ser un número entero.\n");
                return;
            }

            int cantidadAntes = dataCore.Cantidad;
            dataCore.EliminarPorId(id);
            indiceOrdenado = null;

            if (dataCore.Cantidad < cantidadAntes)
                Console.WriteLine($"[OK] Registro con Id {id} eliminado correctamente.\n");
            else
                Console.WriteLine($"[AVISO] No existe ningún registro con Id {id}.\n");
        }

        private static void EjecutarMostrar()
        {
            var arreglo = dataCore.ObtenerComoArreglo();
            Console.WriteLine();

            if (arreglo.Length == 0)
            {
                Console.WriteLine("La tabla está vacía. No hay registros para mostrar.\n");
                return;
            }

            Console.WriteLine($"--- {arreglo.Length} registro(s) en memoria ---");
            foreach (var r in arreglo)
                Console.WriteLine($"  Id: {r.Id,-4} | Nombre: {r.Nombre,-15} | Monto: {r.Monto:C}");
            Console.WriteLine();
        }

        private static void EjecutarOrdenar()
        {
            var arreglo = dataCore.ObtenerComoArreglo();
            Console.WriteLine();

            if (arreglo.Length == 0)
            {
                Console.WriteLine("No hay registros para ordenar. Inserta datos primero.\n");
                return;
            }

            Ordenamiento.QuickSort(arreglo, 0, arreglo.Length - 1);
            indiceOrdenado = arreglo;

            Console.WriteLine($"[OK] Índice construido y ordenado por Id ({arreglo.Length} registros).");
            Console.WriteLine("Ya puedes usar la opción [5] Búsqueda binaria indexada.\n");
        }

        private static void EjecutarBusqueda()
        {
            Console.WriteLine();

            if (indiceOrdenado == null)
            {
                Console.WriteLine("Aún no existe un índice ordenado. Ejecuta primero la opción [4].\n");
                return;
            }

            Console.Write("Id a buscar: ");
            string? idTexto = Console.ReadLine() ?? "";
            if (!int.TryParse(idTexto, out int idBuscado))
            {
                Console.WriteLine("ERROR: el Id debe ser un número entero.\n");
                return;
            }

            var (registro, comparaciones) = BusquedaBinaria.BuscarRegistroIndexado(indiceOrdenado, idBuscado);

            if (registro != null)
            {
                var r = registro.Value;
                Console.WriteLine("[OK] Registro encontrado:");
                Console.WriteLine($"  Id: {r.Id} | Nombre: {r.Nombre} | Monto: {r.Monto:C}");
                Console.WriteLine($"  Comparaciones realizadas: {comparaciones}\n");
            }
            else
            {
                Console.WriteLine($"[AVISO] Id {idBuscado} no encontrado.");
                Console.WriteLine($"  Comparaciones realizadas: {comparaciones}\n");
            }
        }

        private static bool ConfirmarSalida()
        {
            Console.Write("\n¿Seguro que deseas salir? (s/n): ");
            string? resp = Console.ReadLine() ?? "";
            return resp.Trim().ToLower() == "s";
        }
    }
}