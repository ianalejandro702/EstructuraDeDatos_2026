using System;
using System.Collections.Generic;
using System.Linq;

namespace InventarioEstructuraDatos{
    public class Producto{
        public int ID { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }

        public Producto(int id, string nombre, double precio, int cantidad){
            ID = id;
            Nombre = nombre;
            Precio = precio;
            Cantidad = cantidad;
        }

        // Override de ToString() para impresión directa en consola
        public override string ToString(){
            return $"[{ID}] {Nombre} - ${Precio:F2} | Stock: {Cantidad}";
        }
    }

    class Program{
        static void Main(string[] args){
            Console.WriteLine("=== SISTEMA DE GESTIÓN DE INVENTARIO ===\n");


            // PASO 3: CONSTRUYENDO EL INVENTARIO CON List<T>
            
            // Sintaxis 1: Inicializador de colección (compacta y legible)
            List<Producto> inventario = new List<Producto>{
                new Producto(1, "Laptop Lenovo", 15999.00, 10),
                new Producto(2, "Mouse Inalámbrico", 349.00, 25),
                new Producto(3, "Teclado Mecánico", 899.00, 0),
                new Producto(4, "Monitor 24\"", 4500.00, 5),
                new Producto(5, "Audífonos Sony", 1200.00, 0)
            };

            // Sintaxis 2: Agregar elementos usando .Add() después de la creación
            inventario.Add(new Producto(6, "Webcam HD", 750.00, 12));

            // Sintaxis 3: Uso de 'var' para inferencia de tipos en C# moderno
            var otroProducto = new Producto(7, "Hub USB-C", 450.00, 8);
            inventario.Add(otroProducto);

            // Mostrar el total actual usando la propiedad .Count
            Console.WriteLine($"Total de productos registrados en lista: {inventario.Count}\n");


            // PASO 4: CONSULTAS LINQ - FILTRANDO Y ORDENANDO
         
            
            // Consulta 1: Ordenar por precio descendente (Mayor a Menor)
            Console.WriteLine("=== Productos por Precio (Descendente) ===");
            var porPrecio = inventario.OrderByDescending(p => p.Precio).ToList();
            foreach (var p in porPrecio){
                Console.WriteLine(p); // Llama automáticamente a ToString()
            }
            Console.WriteLine();

            // Consulta 2: Filtrar productos agotados (Cantidad == 0)
            Console.WriteLine("=== Productos Agotados ===");
            var agotados = inventario.Where(p => p.Cantidad == 0).ToList();
            if (agotados.Count == 0){
                Console.WriteLine("Sin productos agotados.");
            }
            else{
                // Uso de ForEach directo de List<T> con una expresión lambda
                agotados.ForEach(p => Console.WriteLine(p));
            }
            Console.WriteLine();


            // PASO 5: BÚSQUEDA INSTANTÁNEA CON Dictionary<K,V>
   
            
            // Conversión eficiente de List a Dictionary usando LINQ O(n)
            // Llave: p.ID, Valor: p (el objeto completo)
            Dictionary<int, Producto> catalogo = inventario.ToDictionary(p => p.ID, p => p);

            // Ejecutamos el método de búsqueda rápida
            BuscarPorID(catalogo);
        }

        /// <summary>
        /// Realiza búsquedas con complejidad O(1) en el diccionario.
        /// Incluye una validación robusta de entrada de datos (Defensa del canal de entrada).
        /// </summary>
        static void BuscarPorID(Dictionary<int, Producto> catalogo){
            Console.Write("Ingresa el ID del producto a buscar: ");
            string input = Console.ReadLine();

            // Validación estricta usando Int32.TryParse para evitar excepciones por formato inválido
            if (int.TryParse(input, out int idBuscado)){
                // TryGetValue busca directamente en la Tabla Hash en tiempo constante O(1)
                if (catalogo.TryGetValue(idBuscado, out Producto encontrado)){
                    Console.WriteLine($"\n[ÉXITO] Producto encontrado:\n{encontrado}");
                }
                else{
                    Console.WriteLine($"\n[ERROR] El ID {idBuscado} no existe en el catálogo.");
                }
            }
            else{
                Console.WriteLine("\n[ERROR] Entrada inválida. Debes ingresar un número entero para el ID.");
            }
        }
    }
}