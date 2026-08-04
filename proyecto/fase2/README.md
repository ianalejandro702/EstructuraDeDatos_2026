## Fase 2 — QuickSort vs Selección Directa

Implementación de QuickSort recursivo en C# sobre `RegistroDatos[]`, contrastado
analítica y empíricamente contra Selección Directa (Fase 1).

### Cómo compilar y ejecutar

```bash
dotnet build
dotnet run -c Release
```

## Decisiones de diseño

- **Pivote por mediana de tres** (`arr[bajo]`, `arr[medio]`, `arr[alto]`) en vez
  de un pivote fijo, para evitar la degradación a O(n²) en arreglos ya ordenados.
- **Caso base extendido con InsertionSort** para sublistas con menos de 10
  elementos, reduciendo la profundidad del Call Stack.
- **Esquema de partición de Lomuto** (un solo puntero), elegido por simplicidad
  de verificación frente a Hoare.
- El struct `RegistroDatos` se mantiene sin modificaciones respecto a la Fase 1.

### Verificación de correctitud

Se ejecutó una batería de 8 casos extremos antes del benchmark (arreglo vacío,
un elemento, elementos repetidos, ya ordenado, ordenado inversamente, etc.).
Todos los casos resultaron `OK`.

### Resultados del benchmark

Ejecutado con semilla fija `Random(42)`, .NET [COMPLETAR: versión de tu SDK, ej. 8.0].

| n | Comparaciones Selección | Intercambios Selección | Tiempo Selección | Llamadas QuickSort | Comparaciones QuickSort | Intercambios QuickSort | Tiempo QuickSort | Ratio |
|---|---|---|---|---|---|---|---|---|
| 100 | 4,950 | 95 | 1.048 ms | 33 | 620 | 292 | 0.045 ms | 23.1x |
| 1,000 | 499,500 | 993 | 14.389 ms | 283 | 9,632 | 4,197 | 0.524 ms | 27.4x |
| 10,000 | 49,995,000 | 9,990 | 1517.691 ms | 2,871 | 135,532 | 59,612 | 7.089 ms | 214.1x |

**Interpretación:** el ratio de velocidad crece de forma no lineal (23x → 27x →
214x), confirmando empíricamente que la brecha entre O(n²) y O(n log n) se
amplifica conforme crece n. Las comparaciones de Selección Directa siguen
exactamente la fórmula n(n−1)/2, y las llamadas recursivas de QuickSort (33 →
283 → 2,871) crecen de forma aproximadamente logarítmica, muy por debajo del
peor caso, evidencia de que la estrategia de mediana de tres evita la
degradación a O(n²).

### Limitaciones conocidas

- No se implementó partición de tres vías (Dutch National Flag); un arreglo
  con valores de `Id` masivamente repetidos seguiría generando particiones
  desbalanceadas.
- No se implementó recursión de cola ni límite de profundidad explícito
  (estilo Introsort); un adversario construido específicamente contra la
  mediana de tres podría todavía provocar el peor caso O(n²).

