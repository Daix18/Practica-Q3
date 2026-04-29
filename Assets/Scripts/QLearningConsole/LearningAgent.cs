using System;

namespace QLearningConsole
{
    public enum Algorithm { QLearning, Sarsa }

    /// Agente de aprendizaje por refuerzo tabular.
    /// El alumno debe completar los métodos marcados con TODO.
    public class LearningAgent
    {
        public double[,] Q { get; }
        public Algorithm Algo { get; }
        public double Alpha { get; set; }
        public double Gamma { get; set; }
        public double Epsilon { get; set; }

        private readonly int _numStates;
        private readonly int _numActions;
        private readonly Random _rng;

        public LearningAgent(int numStates, int numActions, Algorithm algo,
                             double alpha, double gamma, double epsilon, int seed = 0)
        {
            _numStates = numStates;
            _numActions = numActions;
            Algo = algo;
            Alpha = alpha;
            Gamma = gamma;
            Epsilon = epsilon;
            _rng = new Random(seed);
            Q = new double[numStates, numActions];
        }

        /// Selección ε-greedy: con probabilidad ε elige acción aleatoria,
        /// en caso contrario la de mayor valor Q.
        public int SelectAction(int state)
        {
            // TODO: implementar ε-greedy
            throw new NotImplementedException();
        }

        public int ArgMaxAction(int state)
        {
            // TODO: devolver el índice de acción con mayor Q[state, a]
            throw new NotImplementedException();
        }

        public double MaxQ(int state)
        {
            // TODO: devolver max_a Q[state, a]
            throw new NotImplementedException();
        }

        /// Regla Q-Learning (off-policy):
        ///   Q(s,a) ← Q(s,a) + α · [ r + γ · max_a' Q(s',a') − Q(s,a) ]
        public void UpdateQLearning(int s, int a, double r, int sNext, bool done)
        {
            // TODO: implementar la actualización (cuidado con el caso terminal)
            throw new NotImplementedException();
        }

        /// Regla SARSA (on-policy):
        ///   Q(s,a) ← Q(s,a) + α · [ r + γ · Q(s',a') − Q(s,a) ]
        /// donde a' es la acción realmente elegida en s' por la política ε-greedy.
        public void UpdateSarsa(int s, int a, double r, int sNext, int aNext, bool done)
        {
            // TODO: implementar la actualización (cuidado con el caso terminal)
            throw new NotImplementedException();
        }

        /// Ejecuta un episodio completo desde env.StartState hasta el final (terminal o maxSteps).
        /// Devuelve pasos, recompensa total y si se alcanzó la meta.
        public EpisodeResult RunEpisode(Maze env, int maxSteps = 500)
        {
            // TODO: bucle de episodio
            //  1) resetear state = env.StartState
            //  2) elegir acción inicial con SelectAction
            //  3) repetir:
            //       - llamar env.Step para obtener (next, r, done)
            //       - actualizar Q con UpdateQLearning o UpdateSarsa según Algo
            //       - si done: salir
            //       - sincronizar state / action para la siguiente iteración
            throw new NotImplementedException();
        }
    }

    public readonly record struct EpisodeResult(int Steps, double Reward, bool Reached);
}
