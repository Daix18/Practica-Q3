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
            //Maze env = Maze.BuildDefault5x5(); --> laberinto básico sin pozos ni monedas
            //Maze env = Maze.BuildDefault5x5WithHazards(); //--> laberinto con un pozo en el centro (50% de morir al caer) y monedas con bonificaciones en dos celdas
            string rutaMapa = "../../../mapa_ejemplo.txt";
            Maze env = Maze.LoadFromFile(rutaMapa);
            Console.WriteLine("Mapa:");
            Visualizer.PrintMaze(env);

            //DONE
            // Configuración del experimento:
            ExperimentConfig config = new ExperimentConfig();

            //  1. Leer del usuario: algoritmo (SARSA/Q-Learning), α, γ, recompensas,
            //     nº de episodios, semilla.
            bool isPersonalizing = true;

            while (isPersonalizing)
            {
                Console.WriteLine("\n¿Desea personalizar los valores? (algoritmo, α, γ, recompensas, episodios, semilla)");
                Console.Write("Introduzca 'sí' o 'no': ");

                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "sí" || input == "si")
                {
                    isPersonalizing = false;
                    Personalizing(config,env);
                }
                else if (input == "no")
                {
                    Console.WriteLine("Se usarán los valores por defecto.");
                    isPersonalizing = false;
                }
                else
                {
                    Console.WriteLine(" Entrada no válida, por favor introduzca de nuevo su elección.");
                }
            }

            //Tras personalizar, imprimir la configuración del experimento antes de ejecutarlo
            Visualizer.PrintExperimentConfig(config, env);
            //  2. Llamar Experiment.Run.
            ExperimentResult result = Experiment.Run(env, config);
                //Experiment.PrintSummary(result);
               
            //  3. Mostrar la política la Q-Table final y el HeatMap
                Visualizer.PrintPolicy(result.Agent!, env);
                Visualizer.PrintQTable(result.Agent!, env);
                Visualizer.PrintHeatmap(result.Agent!, env);


            //Imprimir resumen del experimento (configuración, métricas finales, tiempo de entrenamiento, etc.)
            Experiment.PrintSummary(result);

            //  4. (Opcional) Exportar curva de aprendizaje a CSV para graficar.
            //string csvPath = "learning_curve.csv";
            //Experiment.ExportCsv(result, csvPath);
            Visualizer.PrintExecutionPath(result.Agent!, env);
            Console.WriteLine("Implementa las clases LearningAgent, Visualizer y Experiment.");

            // 2. Definir el nombre del archivo (puedes diferenciar por algoritmo)
            string nombreArchivo = $"resultados_{config.Algo}_{DateTime.Now:HHmmss}.csv";

            // 3. Exportar a CSV
            Experiment.ExportCsv(result, nombreArchivo);
        }

       
        private static void Personalizing(ExperimentConfig config, Maze env )
        {
            //algoritmo
            Console.WriteLine(" ¿Qué algoritmo quieres usar?  - Algoritmo: QLearning o Sarsa, pulsa 0 para usar Q-learning o 1 para usar sarsa");
            string algoInput = Console.ReadLine();
            if (algoInput == "0") config.Algo = Algorithm.QLearning;
            else if (algoInput == "1") config.Algo = Algorithm.Sarsa;
            else Console.WriteLine("Entrada no válida, se usará Q-learning por defecto.");

            //alpha -> cuanto priorizamos la nueva información frente a la antigua (entre 0 y 1)
            Console.WriteLine("Introduce el valor de α (tasa de aprendizaje, entre 0 y 1):");
            if (double.TryParse(Console.ReadLine(), out double alpha) && alpha >= 0 && alpha <= 1)
            {
                config.Alpha = alpha;
            }
            else Console.WriteLine("Entrada no válida, se usará 0.1 por defecto.");

            //gamma -> cuánto valoramos las recompensas futuras frente a las inmediatas (entre 0 y 1)
            Console.WriteLine("Introduce el valor de γ (factor de descuento, entre 0 y 1):");
            if (double.TryParse(Console.ReadLine(), out double gamma) && gamma >= 0 && gamma <= 1)
            {
                config.Gamma = gamma;
            }
            else Console.WriteLine("Entrada no válida, se usará 0.95 por defecto.");


            //epsilon -> probabilidad de explorar (elegir acción aleatoria) en lugar de explotar (elegir la mejor acción según Q). 1 es explorar siempre, 0 explotar siempre. 
            Console.WriteLine("Introduce el valor de ε (probabilidad de explorar, entre 0 y 1):");
            if (double.TryParse(Console.ReadLine(), out double epsilon) && epsilon >= 0.5 && epsilon <= 1)
            {
                config.EpsilonStart = epsilon;
            }
            else Console.WriteLine("Entrada no válida, se usará 1 por defecto.");



            ////Recompesa por moverse, por alcanzar la meta.
            //Console.WriteLine("Introduce la recompensa por moverse (valor negativo recomendado o 0):");
            //if (double.TryParse(Console.ReadLine(), out double rewardstep) && rewardstep < 0)
            //{
            //    env.RewardStep = rewardstep;
            //}
            //else Console.WriteLine("Entrada no válida, se usará -1 por defecto.");

            ////Recompensa por alcanzar la meta
            //Console.WriteLine("Introduce la recompensa por alcanzar la meta (valor positivo recomendado entre 100 y 1000):");
            //if (double.TryParse(Console.ReadLine(), out double rewardgoal) && rewardgoal > 10 && rewardgoal > env.RewardStep)
            //{
            //    env.RewardGoal = rewardgoal;
            //}
            //else Console.WriteLine("Entrada no válida, se usará 100 por defecto.");

            ////Número de episodios para entrenar al agente
            //Console.WriteLine("Introduce el número de episodios para entrenar al agente (entero positivo):");
            //if (int.TryParse(Console.ReadLine(), out int episodes) && episodes > 0)
            //{
            //    config.Episodes = episodes;
            //}
            //else Console.WriteLine("Entrada no válida, se usarán 500 episodios por defecto.");

            //Semilla para la generación de números aleatorios (para reproducir episodios). Buenas semillas facilitan el aprendizaje (ej. 42), malas semillas lo dificultan. 
            Console.WriteLine("Introduce la semilla para la generación de números aleatorios (entero):");
            if (int.TryParse(Console.ReadLine(), out int seed))
            {
                config.Seed = seed;
            }
            else Console.WriteLine("Entrada no válida, se usará 42 por defecto.");
        }
    }
}
