using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

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
            }
            // al terminar los episodios parar el cronómetro y guardar el tiempo transcurrido del experimento
            cronometro.Stop();
            finalresult.ElapsedMs = cronometro.ElapsedMilliseconds;

            return finalresult;//devolver el resultado del experimento (con la configuración, los resultados de cada episodio, el tiempo total y el agente entrenado)
        }

        /// Volcar curva de aprendizaje a CSV (episode,steps,reward,reached) para graficar en Excel.
        public static void ExportCsv(ExperimentResult r, string path)
        {
            // TODO: escribir cabecera y una fila por episodio
            throw new NotImplementedException();
        }

        /// Resumen por consola: algoritmo, episodios, tiempo, pasos/reward/éxito en ventana final.
        public static void PrintSummary(ExperimentResult r)
        {
            // TODO
        }
    }
}
