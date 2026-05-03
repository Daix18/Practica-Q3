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
            // TODO: crear LearningAgent con (env.NumStates, env.NumActions, algo, alpha, gamma, epsilon, seed)
            // TODO: bucle de episodios, decaer epsilon, acumular EpisodeResult, cronometrar con Stopwatch
            throw new NotImplementedException();
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
