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
            Console.WriteLine("Uno de los nodos no existe. ");
            return;
        }

        adyacencia[origen].Add((destino, peso));
        adyacencia[destino].Add((origen, peso)); // grafo no dirigido
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

        Console.WriteLine($"\ntunel mas corto de {inicio} a {fin}:");
        Console.WriteLine(string.Join(" -> ", camino));
        Console.WriteLine($"Distancia total: {distancias[fin]}");
    }
}

//menu principal
class Program
{
    static void Main(string[] args)
    {
        Grafo parque = new Grafo();

        while (true)
        {
            Console.WriteLine("\n--- Menu del parque ---");
            Console.WriteLine("1. agregar ubicacion");
            Console.WriteLine("2. agregar camino");
            Console.WriteLine("3. ver grafo");
            Console.WriteLine("4. encontrar tunel mas corto");
            Console.WriteLine("5. salir");
            Console.WriteLine("elige una opcion: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("nombre de la ubicacion: ");
                    parque.AgregarNodo(Console.ReadLine());
                    break;
                
                case "2":
                    Console.Write("origen: ");
                    string origen = Console.ReadLine();
                    Console.Write("destino: ");
                    string destino = Console.ReadLine();
                    Console.Write("distancia: ");
                    int peso = int.Parse(Console.ReadLine());
                    parque.AgregarArista(origen, destino, peso);
                    break;

                case "3":
                    parque.MostrarGrafo();
                    break;

                case "4":
                    Console.Write("Inicio: ");
                    string inicio = Console.ReadLine();
                    Console.Write("fin: ");
                    string fin = Console.ReadLine();
                    parque.Dijkstra(inicio, fin);
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("opcion invalidad.");
                    break;
            }
        }
    }
}