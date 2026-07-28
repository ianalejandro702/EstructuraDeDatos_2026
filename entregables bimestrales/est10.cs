using System;

namespace TelemetriaGPS
{
    // ==========================================
    //      MÓDULO A Y C: EL STRUCT INMUTABLE
    // ==========================================
    public readonly struct CoordenadaGPS
    {
        // Propiedades de solo lectura para garantizar la inmutabilidad
        public double Latitud { get; }
        public double Longitud { get; }

        // Constructor con validación defensiva de rangos
        public CoordenadaGPS(double lat, double lon)
        {
            // Validación de Latitud [-90, 90]
            if (lat < -90 || lat > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(lat), "La latitud debe estar entre -90 y 90 grados.");
            }

            // Validación de Longitud [-180, 180]
            if (lon < -180 || lon > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(lon), "La longitud debe estar entre -180 y 180 grados.");
            }

            Latitud = lat;
            Longitud = lon;
        }

        // Método para imprimir la ubicación en consola
        public void ImprimirUbicacion()
        {
            Console.WriteLine($"[GPS] -> Latitud: {Latitud} | Longitud: {Longitud}");
        }
    }

    // ==========================================
    //        PUNTO DE ENTRADA PRINCIPAL
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            //====================================================
            //   MÓDULO B: EXPERIMENTO DE COPIA POR VALOR (STACK)
            //===================================================
            Console.WriteLine("=== MÓDULO B: EXPERIMENTO EN EL STACK ===");
            
            // Instanciamos Ciudad de México
            CoordenadaGPS c1 = new CoordenadaGPS(19.4326, -99.1332);
            
            // Se genera una copia independiente en la memoria Stack
            CoordenadaGPS c2 = c1;
            
            // Reasignamos c2 a Berlín para probar la independencia de datos
            c2 = new CoordenadaGPS(52.5200, 13.4050);

            // Comprobación de resultados en consola
            Console.WriteLine("--- Coordenada c1 (CDMX) ---");
            c1.ImprimirUbicacion();

            Console.WriteLine("--- Coordenada c2 (Berlín) ---");
            c2.ImprimirUbicacion();
            
            // ====================================================
            //     MÓDULO C: CAPTURA DE EXCEPCIONES E INTERACCIÓN
            // ====================================================
            Console.WriteLine("\n=== MÓDULO C: CONTROL DE EXCEPCIONES ===");
            
            try
            {
                Console.Write("Ingrese la Latitud: ");
                double lat = double.Parse(Console.ReadLine());

                Console.Write("Ingrese la Longitud: ");
                double lon = double.Parse(Console.ReadLine());

                // Intentamos crear el objeto con las entradas del usuario
                var coordUsuario = new CoordenadaGPS(lat, lon);
                
                Console.WriteLine("\n[ÉXITO] Coordenada creada correctamente:");
                coordUsuario.ImprimirUbicacion();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Captura el error específico de los rangos geográficos
                Console.WriteLine($"\n[EXCEPCIÓN CAPTURADA] Error: {ex.Message}");
            }
            catch (FormatException)
            {
                // Manejo de error si el usuario no ingresa números válidos
                Console.WriteLine("\n[ERROR] El formato de texto ingresado no es un número válido.");
            }
            
            Console.WriteLine("\nPresione cualquier tecla para finalizar...");
            Console.ReadKey();
        }
    }
}