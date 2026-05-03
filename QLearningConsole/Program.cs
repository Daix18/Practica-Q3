using System;
using System.Globalization;

namespace QLearningConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("======================================================");
            Console.WriteLine("  Práctica 3 - Aprendizaje Reforzado (SARSA/Q-Learning)");
            Console.WriteLine("======================================================");
            Console.WriteLine();

            // Entorno (Maze) ya implementado en Maze.cs
            Maze env = Maze.BuildDefault5x5();
            Console.WriteLine("Mapa:");
            Visualizer.PrintMaze(env);

            // TODO:
            //  1. Leer del usuario: algoritmo (SARSA/Q-Learning), α, γ, recompensas,
            //     nº de episodios, semilla.
            //  2. Llamar Experiment.Run.
            //  3. Mostrar la política y la Q-Table final.
            //  4. (Opcional) Exportar curva de aprendizaje a CSV para graficar.

            Console.WriteLine("Implementa las clases LearningAgent, Visualizer y Experiment.");
        }
    }
}
