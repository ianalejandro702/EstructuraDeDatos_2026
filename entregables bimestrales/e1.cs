using System;

// Definimos la estructura para encapsular los datos del polígono
struct Poligono {
    public int NumeroLados;
    public double MedidaLado;
    public double Apotema;

}
class Program{
    static void Main(string[] args){
        int ladosSeleccionados = SeleccionarPoligono();
        Poligono poligonoUsuario = PedirDatos(ladosSeleccionados);
        double resultadoArea = CalcularArea(poligonoUsuario);

        Console.WriteLine("\n============================");
        Console.WriteLine("¡Calculo completado exitosamente!");
        Console.WriteLine($"Poligono: {poligonoUsuario.NumeroLados} lados.");
        Console.WriteLine($"El area total calculada es: {resultadoArea:F2} unidades cuadradas.");
        Console.WriteLine("=============================");

        Console.WriteLine("\nPresiona cualquier tecla para salir.");
        Console.ReadKey();
    }
        static int SeleccionarPoligono(){
            int lados = 0;
            while (lados < 3){
                Console.Clear();
                Console.WriteLine("=== CALCULADORA DE POLÍGONOS REGULARES ===");
                Console.WriteLine("3. Triángulo Equilátero");
                Console.WriteLine("4. Cuadrado");
                Console.WriteLine("5. Pentágono");
                Console.WriteLine("6. Hexágono");
                Console.WriteLine("7. Heptágono");
                Console.WriteLine("8. Octágono");
                Console.WriteLine("... o cualquier polígono con más lados.");
                Console.Write("\nSelecciona el número de lados de tu polígono (mínimo 3): "); 
                
                //validamos que sea un numero entero 
                if (!int.TryParse(Console.ReadLine(), out lados) || lados < 3){

                    Console.WriteLine("ERROR: Por favor introduce un numero valido ");
                    Console.ReadKey();
                    lados = 0;
                }    

            }
            return lados;

        }
    static Poligono PedirDatos(int NumeroLados){
        Poligono miFigura = new Poligono();
        miFigura.NumeroLados = NumeroLados;

        bool ladoValido = false;
        while (!ladoValido){
            Console.Write($"Introduce la medida del lado de tu poligono de {NumeroLados} lados: ");
            string entrada = Console.ReadLine();


            if (double.TryParse(entrada, out miFigura.MedidaLado) && miFigura.MedidaLado > 0){
                ladoValido = true;
            }
            else{
                Console.WriteLine("ERROR: La medida debe ser un numero decimal positivo. Intenta nuevamente.\n");  
            }

        }
        

        bool apotemaValido = false;
        while (!apotemaValido){
            Console.Write("Introduce la medida de la apotema: ");
            string entrada = Console.ReadLine();

            if (double.TryParse(entrada, out miFigura.Apotema) && miFigura.Apotema > 0){
                apotemaValido = true;
            }
            else{
                Console.WriteLine("ERROR: La apotema debe ser un numero decimal positivo. Intenta nuevamente.\n");

            }
        }
        return miFigura;
    }

    static double CalcularArea(Poligono figura){
        double perimetro = figura.NumeroLados * figura.MedidaLado;

        double area = (perimetro * figura.Apotema) / 2;
        return area;
    }
}