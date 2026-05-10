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
            if (_rng.NextDouble() < Epsilon)
            {
                return _rng.Next(0, _numActions);
            }
            // En caso contrario: Explotación (mejor acción conocida)
            return ArgMaxAction(state);
        }

        public int ArgMaxAction(int state)
        {
            int bestAction = 0;
            double maxVal = Q[state, 0];
            for (int a = 1; a < _numActions; a++)
            {
                if (Q[state, a] > maxVal)
                {
                    maxVal = Q[state, a];
                    bestAction = a;
                }
            }
            return bestAction;
        }

        public double MaxQ(int state)
        {
            // TODO: devolver max_a Q[state, a]
            double maxVal = Q[state, 0];
            for (int a = 1; a < _numActions; a++)
            {
                if (Q[state, a] > maxVal) maxVal = Q[state, a];
            }
            return maxVal;
        }

        /// Regla Q-Learning (off-policy):
        ///   Q(s,a) ← Q(s,a) + α · [ r + γ · max_a' Q(s',a') − Q(s,a) ]
        public void UpdateQLearning(int s, int a, double r, int sNext, bool done)
        {
            // TODO: implementar la actualización (cuidado con el caso terminal)
            double target = done ? r : r + Gamma * MaxQ(sNext);
            Q[s, a] += Alpha * (target - Q[s, a]);
        }

        /// Regla SARSA (on-policy):
        ///   Q(s,a) ← Q(s,a) + α · [ r + γ · Q(s',a') − Q(s,a) ]
        /// donde a' es la acción realmente elegida en s' por la política ε-greedy.
        public void UpdateSarsa(int s, int a, double r, int sNext, int aNext, bool done)
        {
            // TODO: implementar la actualización (cuidado con el caso terminal)
            double target = done ? r : r + Gamma * Q[sNext, aNext];
            Q[s, a] += Alpha * (target - Q[s, a]);
        }

        /// Ejecuta un episodio completo desde env.StartState hasta el final (terminal o maxSteps).
        /// Devuelve pasos, recompensa total y si se alcanzó la meta.
        public EpisodeResult RunEpisode(Maze env, int maxSteps)
        {
            // TODO: bucle de episodio
            //  1) resetear state = env.StartState
            //  2) elegir acción inicial con SelectAction
            //  3) repetir:
            //       - llamar env.Step para obtener (next, r, done)
            //       - actualizar Q con UpdateQLearning o UpdateSarsa según Algo
            //       - si done: salir
            //       - sincronizar state / action para la siguiente iteración
            int state = env.StartState;
            double totalReward = 0;
            int steps = 0;
            bool reached = false;

            int action = SelectAction(state); // Acción inicial

            while (steps < maxSteps)
            {
                // Realizar paso en el entorno
                var (nextState, r, done) = env.Step(state, (Action)action, _rng);
                totalReward += r;
                steps++;

                if (Algo == Algorithm.QLearning)
                {
                    UpdateQLearning(state, action, r, nextState, done);
                    state = nextState;
                    action = SelectAction(state); // Nueva acción para el siguiente paso
                }
                else // SARSA
                {
                    int nextAction = SelectAction(nextState); // Se elige a' antes de actualizar
                    UpdateSarsa(state, action, r, nextState, nextAction, done);
                    state = nextState;
                    action = nextAction; // La acción elegida se mantiene para el siguiente paso
                }

                if (done)
                {
                    reached = (env.KindOf(nextState) == CellKind.Goal);
                    break;
                }
            }
            return new EpisodeResult(steps, totalReward, reached);
        }
    }

    public readonly record struct EpisodeResult(int Steps, double Reward, bool Reached);
}
