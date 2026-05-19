using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace QLearningConsole
{
    /// Configuración de un experimento de entrenamiento.
    public class ExperimentConfig
    {
        public Algorithm Algo { get; set; } = Algorithm.QLearning;
        public double Alpha { get; set; } = 0.1;
        public double Gamma { get; set; } = 0.95;
        public double EpsilonStart { get; set; } = 1.0;
        public double EpsilonEnd { get; set; } = 0.05;
        public int Episodes { get; set; } = 500;
        public int MaxStepsPerEpisode { get; set; } = 500;
        public int Seed { get; set; } = 42;
    }

    /// Resultados agregados de un experimento.
    public class ExperimentResult
    {
        public ExperimentConfig Config { get; set; } = new();
        public List<EpisodeResult> Episodes { get; } = new();
        public long ElapsedMs { get; set; }
        public int TotalUpdates { get; set; }
        public LearningAgent? Agent { get; set; }

        // TODO: añadir métricas útiles (SuccessRateLast, AverageStepsLast, AverageRewardLast)
        public double SuccessRateLast { get; set; }
        public double AverageStepsLast { get; set; }
        public double AverageRewardLast { get; set; }

    }

    public static class Experiment
    {
        /// Entrena un agente durante N episodios. Opcional: decaer ε linealmente de EpsilonStart a EpsilonEnd.
        public static ExperimentResult Run(Maze env, ExperimentConfig cfg)
        {
            //Crear un cronómetro para medir el tiempo de entrenamiento
            Stopwatch cronometro = System.Diagnostics.Stopwatch.StartNew();
            //Variable para almacenar los resultados del experimento
            ExperimentResult finalresult = new ExperimentResult { Config = cfg };
            // TODO: crear LearningAgent con (env.NumStates, env.NumActions, algo, alpha, gamma, epsilon, seed)
            LearningAgent agent = new LearningAgent(env.NumStates, env.NumActions, cfg.Algo, cfg.Alpha, cfg.Gamma, cfg.EpsilonStart, cfg.Seed);

            //decirle al resultado que agente ha sido creado pra q guarde resultado del entrenamiento de ese agente concreto
            finalresult.Agent = agent;

            // TODO: bucle de episodios, decaer epsilon, acumular EpisodeResult, cronometrar con Stopwatch
            for (int i = 0; i < cfg.Episodes; i++)
            {
                //decaer epsilon linealmente de EpsilonStart a EpsilonEnd a lo largo de los episodios
                double progresoEpisodio = (cfg.Episodes > 1) ? (double)i / (cfg.Episodes - 1) : 1.0;
                agent.Epsilon = cfg.EpsilonStart + progresoEpisodio * (cfg.EpsilonEnd - cfg.EpsilonStart);// empieza en 1 y decae a 0.05 a lo largo de los episodios

                //ejecutar episodio y obtener su resultado
                EpisodeResult episodeResult = agent.RunEpisode(env, cfg.MaxStepsPerEpisode);

                //guardar su resultado en la lista de resultados del experimento
                finalresult.Episodes.Add(episodeResult);

                //acumular el número total de actualizaciones de la tabla Q (TotalUpdates)
                finalresult.TotalUpdates += episodeResult.Steps;//el número total de veces que el agente ha "aprendido" (Updates) es igual a la suma de todos los pasos que ha dado en todos los episodios.
            }
            // al terminar los episodios parar el cronómetro y guardar el tiempo transcurrido del experimento
            cronometro.Stop();
            finalresult.ElapsedMs = cronometro.ElapsedMilliseconds;

            //calcular métricas útiles para el resumen final (SuccessRateLast, AverageStepsLast, AverageRewardLast) a partir de los resultados de los episodios
            var lastEpisodes = finalresult.Episodes.Skip(Math.Max(0, finalresult.Episodes.Count - 100)).ToList(); // últimos 100 episodios o todos si hay menos de 100
            finalresult.SuccessRateLast = lastEpisodes.Count > 0 ? lastEpisodes.Count(e => e.Reached) / (double)lastEpisodes.Count : 0.0; //% de episodios exitosos en los últimos 100
            finalresult.AverageStepsLast = lastEpisodes.Count > 0 ? lastEpisodes.Average(e => e.Steps) : 0.0; //  promedio de pasos en los últimos 100 episodios
            finalresult.AverageRewardLast = lastEpisodes.Count > 0 ? lastEpisodes.Average(e => e.Reward) : 0.0; // promedio de recompensa total en los últimos 100 episodios                                                                                                 

            return finalresult;//devolver el resultado del experimento (con la configuración, los resultados de cada episodio, el tiempo total y el agente entrenado)
        }

        /// Volcar curva de aprendizaje a CSV (episode,steps,reward,reached) para graficar en Excel.
        public static void ExportCsv(ExperimentResult r, string path)
        {
            // TODO: escribir cabecera y una fila por episodio
            //cabecera
            var EpisodeInfo =  new StringBuilder();
            EpisodeInfo.AppendLine("Episode;Steps;Reward;Reached");
            for (int i = 0; i < r.Episodes.Count; i++)
            {
                var e = r.Episodes[i];
              
                EpisodeInfo.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0};{1};{2:F2};{3}", i, e.Steps, e.Reward, e.Reached ? 1 : 0)) ;

            }
            File.WriteAllText(path, EpisodeInfo.ToString());
            Console.WriteLine($"Curva de aprendizaje exportada a {path}");
            
        }

        /// Resumen por consola: algoritmo, episodios, tiempo, pasos/reward/éxito en ventana final.
        public static void PrintSummary(ExperimentResult r)
        {
            // TODO
            Console.WriteLine("\n--- RESUMEN DEL EXPERIMENTO ---");
            Console.WriteLine($"Algoritmo: {r.Config.Algo} | Episodios: {r.Config.Episodes}");
            Console.WriteLine($"Tiempo total: {r.ElapsedMs} ms | Total Updates: {r.TotalUpdates}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Resultados últimos 100 episodios:");
            Console.WriteLine($"  - Tasa de Éxito: {r.SuccessRateLast:P2}"); // P2 lo pone como %
            Console.WriteLine($"  - Media de Pasos: {r.AverageStepsLast:F2}");
            Console.WriteLine($"  - Media de Recompensa: {r.AverageRewardLast:F2}");
        }
    }
}
