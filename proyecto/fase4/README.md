# DataCore v4.0

**Institución:** UNITEC
**Estudiante:** Ian Alejandro Vargas Arias
**Número de cuenta:** 333009736
**Docente:** Paula Daniela Muñoz Zarate

## Descripción General

DataCore es un motor de base de datos en memoria desarrollado en C# a lo
largo de cuatro fases progresivas. El sistema almacena registros
(`RegistroDatos`) en una lista simplemente enlazada (`TablaDinamica`),
permite ordenarlos con QuickSort y realizar búsquedas indexadas con
complejidad O(log n) mediante búsqueda binaria. Todas las operaciones se
exponen a través de un Menú Maestro interactivo en consola (CLI).

## Requisitos y Ejecución

- .NET SDK 10.0

Para compilar y ejecutar:

```bash
git clone https://github.com/ianalejandro702/EstructuraDeDatos_2026.git
cd EstructuraDeDatos_2026/proyecto/fase4
dotnet build
dotnet run
```

## Estructura del Proyecto

| Archivo | Propósito |
|---|---|
| `NucleoDataCore.cs` | Define `RegistroDatos` (struct), `NodoRegistro` y `TablaDinamica` (lista simplemente enlazada), y `Ordenamiento` (QuickSort). |
| `AlgoritmoyBusquedaBinaria.cs` | Implementa `BuscarRegistroIndexado`, búsqueda binaria O(log n) sobre un arreglo ordenado. |
| `program.cs` | Menú Maestro (CLI): bucle `do-while` que integra inserción, eliminación, visualización, ordenamiento y búsqueda. |
| `fase4.csproj` | Archivo de proyecto de .NET. |

## Funcionalidades Implementadas

- [x] Struct `RegistroDatos` con campos `Id`, `Nombre`, `Monto`
- [x] Lista simplemente enlazada (`TablaDinamica`) con `InsertarInicio`, `InsertarFinal`, `EliminarPorId`, `ObtenerComoArreglo`
- [x] Ordenamiento con QuickSort (`Ordenamiento.QuickSort`)
- [x] Búsqueda binaria indexada con contador de comparaciones (`BusquedaBinaria.BuscarRegistroIndexado`)
- [x] Menú Maestro interactivo con 6 opciones (insertar, eliminar, mostrar, ordenar, buscar, salir)
- [x] Validación de entradas con `int.TryParse` / `decimal.TryParse` (sin excepciones no controladas)
- [x] Manejo de casos borde: tabla vacía, Id inexistente, búsqueda sin índice construido

## Complejidad de cada módulo

| Operación | Estructura | Complejidad |
|---|---|---|
| Insertar al inicio | Lista enlazada | O(1) |
| Insertar al final | Lista enlazada | O(n) |
| Eliminar por Id | Lista enlazada | O(n) |
| Convertir a arreglo | Lista → Arreglo | O(n) |
| Ordenar (QuickSort) | Arreglo | O(n log n) promedio, O(n²) peor caso |
| Búsqueda binaria indexada | Arreglo ordenado | O(log n) |

## Limitaciones Conocidas

1. El índice construido con la opción `[4]` se invalida automáticamente
   tras cualquier inserción o eliminación (`indiceOrdenado = null`), por
   lo que debe reconstruirse con `[4]` antes de volver a usar la opción
   `[5]` de búsqueda.
2. `RegistroDatos` no valida duplicados: es posible insertar dos
   registros con el mismo `Id`. En ese caso, `BuscarRegistroIndexado`
   devuelve solo uno de los dos de forma no determinística, dependiendo
   del punto medio calculado en cada iteración.
3. El pivote de `QuickSort` es siempre el último elemento del subarreglo
   (sin mediana de tres ni aleatorización), por lo que un conjunto de
   datos ya ordenado por `Id` degrada el algoritmo a su peor caso O(n²).
4. La eliminación (`EliminarPorId`) no retorna un valor booleano de
   éxito; el Menú Maestro determina si la eliminación ocurrió comparando
   `Cantidad` antes y después de la operación.

## Uso de IA

- **Herramienta utilizada:** Claude (Anthropic)
- **Problema consultado:** Diseño e implementación del método
  `BuscarRegistroIndexado` (búsqueda binaria con contador de
  comparaciones) y del Menú Maestro con bucle `do-while`, validación de
  entradas y manejo de excepciones por opción individual. También se
  consultó cómo resolver errores de compilación relacionados con
  namespaces inconsistentes entre archivos y con el comando
  `dotnet build` al ejecutarse sobre un archivo `.cs` individual en
  lugar del proyecto completo.
- **Qué sugirió la IA:** Implementar la búsqueda binaria con los
  punteros `izquierda`, `derecha` y `medio`, incrementando un contador
  en cada iteración, y devolver una tupla `(RegistroDatos? registro,
  int comparaciones)`. Para el menú, propuso una estructura de métodos
  privados por opción (`EjecutarInsercion`, `EjecutarEliminacion`, etc.)
  en vez de escribir toda la lógica dentro de `Main`, junto con
  `int.TryParse`/`decimal.TryParse` para evitar excepciones de formato
  como flujo de control.
- **Qué decidí y por qué:** Adopté la estructura de métodos separados
  por opción porque hace el código más legible y más fácil de depurar
  cuando un error ocurre en una sola operación. Adapté el código
  genérico propuesto para que usara los campos reales de mi
  `RegistroDatos` (`Id`, `Nombre`, `Monto`) en lugar de los campos de
  ejemplo iniciales, y verifiqué cada compilación y ejecución en
  consola antes de integrarlo a mi proyecto, confirmando que las
  pruebas manuales (insertar, ordenar, buscar existente e inexistente,
  eliminar, entradas inválidas) funcionaran correctamente.