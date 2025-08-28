# tunel_parque_grafos

<img width="2552" height="1079" alt="image" src="https://github.com/user-attachments/assets/dfcec0bb-7f75-485d-bd4d-b62b89aad63a" />

# 🏞️ Simulación de un Túnel en el Parque usando Grafos (C# - Visual Studio 2022)

Este proyecto simula un túnel dentro del parque principal de una ciudad utilizando grafos. Se desarrollará una **aplicación de consola en C#** que permitirá:

---

## ✅ Objetivos del Programa

- Representar el parque como un grafo (nodos = puntos del parque, aristas = caminos).
- Permitir al usuario agregar nodos (ubicaciones del parque).
- Permitir al usuario agregar aristas (caminos entre nodos con peso, que representa la distancia).
- Calcular el túnel óptimo: el camino más corto entre dos puntos (utilizando Dijkstra).
- Visualizar la estructura del grafo y el resultado del túnel.

---

## 📌 ¿Qué es un "Túnel"?

Para efectos prácticos, un **túnel** será el **camino más corto entre dos ubicaciones del parque**, calculado con un algoritmo de rutas mínimas.

---

## 💻 Paso a Paso

### 1. Crear el Proyecto

1. Abrir **Visual Studio 2022**
2. Crear un nuevo proyecto → **Aplicación de Consola** (.NET Core o .NET 6/7)
3. Nombrar el proyecto: `TúnelParqueGrafo`

---

## 📄 Código Base en C#

```csharp
using System;
using System.Collections.Generic;

class Nodo
{
    public string Nombre { get; set; }

    public Nodo(string nombre)
    {
        Nombre = nombre;
    }
}

class Grafo
{
    private Dictionary<string, List<(string destino, int peso)>> adyacencia;

    public Grafo()
    {
        adyacencia = new Dictionary<string, List<(string, int)>>();
    }

    public void AgregarNodo(string nombre)
    {
        if (!adyacencia.ContainsKey(nombre))
            adyacencia[nombre] = new List<(string, int)>();
    }

    public void AgregarArista(string origen, string destino, int peso)
    {
        if (!adyacencia.ContainsKey(origen) || !adyacencia.ContainsKey(destino))
        {
            Console.WriteLine("Uno de los nodos no existe.");
            return;
        }

        adyacencia[origen].Add((destino, peso));
        adyacencia[destino].Add((origen, peso)); // Grafo no dirigido
    }

    public void MostrarGrafo()
    {
        foreach (var nodo in adyacencia)
        {
            Console.Write($"{nodo.Key} -> ");
            foreach (var arista in nodo.Value)
            {
                Console.Write($"{arista.destino} ({arista.peso}), ");
            }
            Console.WriteLine();
        }
    }

    public void Dijkstra(string inicio, string fin)
    {
        var distancias = new Dictionary<string, int>();
        var anterior = new Dictionary<string, string>();
        var visitados = new HashSet<string>();
        var cola = new PriorityQueue<string, int>();

        foreach (var nodo in adyacencia.Keys)
        {
            distancias[nodo] = int.MaxValue;
            anterior[nodo] = null;
        }

        distancias[inicio] = 0;
        cola.Enqueue(inicio, 0);

        while (cola.Count > 0)
        {
            var actual = cola.Dequeue();

            if (visitados.Contains(actual)) continue;
            visitados.Add(actual);

            foreach (var vecino in adyacencia[actual])
            {
                int nuevaDistancia = distancias[actual] + vecino.peso;
                if (nuevaDistancia < distancias[vecino.destino])
                {
                    distancias[vecino.destino] = nuevaDistancia;
                    anterior[vecino.destino] = actual;
                    cola.Enqueue(vecino.destino, nuevaDistancia);
                }
            }
        }

        // Mostrar camino
        if (distancias[fin] == int.MaxValue)
        {
            Console.WriteLine("No hay camino disponible.");
            return;
        }

        var camino = new Stack<string>();
        string nodoActual = fin;
        while (nodoActual != null)
        {
            camino.Push(nodoActual);
            nodoActual = anterior[nodoActual];
        }

        Console.WriteLine($"\nTúnel más corto de {inicio} a {fin}:");
        Console.WriteLine(string.Join(" -> ", camino));
        Console.WriteLine($"Distancia total: {distancias[fin]}");
    }
}

🎮 Menú Principal
Agrega este código dentro del método Main():

class Program
{
    static void Main(string[] args)
    {
        Grafo parque = new Grafo();

        while (true)
        {
            Console.WriteLine("\n--- Menú del Parque ---");
            Console.WriteLine("1. Agregar ubicación");
            Console.WriteLine("2. Agregar camino");
            Console.WriteLine("3. Ver grafo");
            Console.WriteLine("4. Encontrar túnel más corto");
            Console.WriteLine("5. Salir");
            Console.Write("Elige una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Nombre de la ubicación: ");
                    parque.AgregarNodo(Console.ReadLine());
                    break;

                case "2":
                    Console.Write("Origen: ");
                    string origen = Console.ReadLine();
                    Console.Write("Destino: ");
                    string destino = Console.ReadLine();
                    Console.Write("Distancia: ");
                    int peso = int.Parse(Console.ReadLine());
                    parque.AgregarArista(origen, destino, peso);
                    break;

                case "3":
                    parque.MostrarGrafo();
                    break;

                case "4":
                    Console.Write("Inicio: ");
                    string inicio = Console.ReadLine();
                    Console.Write("Fin: ");
                    string fin = Console.ReadLine();
                    parque.Dijkstra(inicio, fin);
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }
}

📦 Requisitos
.NET 6.0 o superior

Visual Studio 2022

🧪 Ejemplo de Uso
Agregar ubicación → A

Agregar ubicación → B

Agregar ubicación → C

Agregar camino: A-B (5)

Agregar camino: B-C (3)

Agregar camino: A-C (10)

Buscar túnel: A → C

Resultado esperado:
Túnel más corto de A a C:
A -> B -> C
Distancia total: 8
