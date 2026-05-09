using System;
using System.Text;

namespace QLearningConsole
{
    /// Utilidades de impresión. Se proporcionan PrintMaze y PrintPolicy
    /// ya funcionando; el alumno debe implementar PrintQTable y PrintHeatmap
    /// para facilitar el análisis.
    /// Class that prints the maze, the learned policy, the Q-table and a heatmap of V(s) = max_a Q[s,a].
    public static class Visualizer
    {
        public static void PrintMaze(Maze m) //dibuja el laberinto, paredes y tipo de celda.
        {
            //pieza de código para imprimir.
            var sb = new StringBuilder();
            sb.Append("+");
            for (int c = 0; c < m.Cols; c++) sb.Append("---+");
            sb.AppendLine();

            for (int r = 0; r < m.Rows; r++)
            {
                sb.Append("|");
                for (int c = 0; c < m.Cols; c++)
                {
                    int s = m.ToState(r, c); //convierte la coordenada en un estado único
                    string label = " . ";
                    if (s == m.StartState) label = " S ";
                    else if (m.KindOf(s) == CellKind.Goal) label = " G ";
                    else if (m.KindOf(s) == CellKind.Pit) label = " X ";
                    else if (m.KindOf(s) == CellKind.Coin) label = " $ ";
                    sb.Append(label);
                    sb.Append(m.IsWall(s, Action.Right) ? "|" : " ");
                }
                sb.AppendLine();

                sb.Append("+");
                for (int c = 0; c < m.Cols; c++)
                {
                    int s = m.ToState(r, c);
                    sb.Append(m.IsWall(s, Action.Down) ? "---+" : "   +");
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        public static void PrintPolicy(LearningAgent agent, Maze m)
        {
            Console.WriteLine("--- Política derivada de Q (flecha = argmax_a Q[s,a]) ---");
            var arrows = new[] { "^", ">", "v", "<" }; // Up, Right, Down, Left array de simbolos

            var sb = new StringBuilder();
            sb.Append("+");
            for (int c = 0; c < m.Cols; c++) sb.Append("---+");
            sb.AppendLine();

            for (int r = 0; r < m.Rows; r++)
            {
                sb.Append("|");
                for (int c = 0; c < m.Cols; c++)
                {
                    int s = m.ToState(r, c);
                    string cellStr;
                    if (s == m.GoalState) cellStr = " G ";
                    else if (m.KindOf(s) == CellKind.Pit) cellStr = " X ";
                    else
                    {
                        int bestA = agent.ArgMaxAction(s);// acción con mayor Q[s,a]?. deuvuelve el índice (0, 1, 2 o 3) de la mejor dirección. > v< arriba
                        bool allZero = agent.Q[s, 0] == 0 && agent.Q[s, 1] == 0 &&
                                       agent.Q[s, 2] == 0 && agent.Q[s, 3] == 0;
                        cellStr = allZero ? " . " : $" {arrows[bestA]} ";
                    } //si el agente no ha aprendido nada (todos los Q[s,a] son 0) se muestra un punto, sino la flecha de la mejor acción.
                    sb.Append(cellStr);
                    sb.Append(m.IsWall(s, Action.Right) ? "|" : " ");
                }
                sb.AppendLine();

                sb.Append("+");
                for (int c = 0; c < m.Cols; c++)
                {
                    int s = m.ToState(r, c);
                    sb.Append(m.IsWall(s, Action.Down) ? "---+" : "   +");
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        public static void PrintQTable(LearningAgent agent, Maze m)
        {
            // TODO: imprimir la tabla Q con columnas: Estado (r,c) | Up | Right | Down | Left
            Console.WriteLine("(pendiente) PrintQTable");
            Console.WriteLine("\n--- Tabla Q completa (Estado (r,c) | Up | Right | Down | Left) ---");
            Console.WriteLine(new string('-', 65));
            Console.WriteLine(" Estado (r,c) |    Up    |   Right  |   Down   |   Left   ");
            Console.WriteLine(new string('-', 65));

            for (int s = 0; s < m.NumStates; s++) //  NumStates de la clase Maze
            {
                // Obtenemos fila y columna 
                (int r, int c) = m.ToRowCol(s);

                Console.Write($"  ({r},{c})      |");
                // Acceso a la matriz Q del agente
                Console.Write($"{agent.Q[s, 0],9:F2} |");
                Console.Write($"{agent.Q[s, 1],9:F2} |");
                Console.Write($"{agent.Q[s, 2],9:F2} |");
                Console.Write($"{agent.Q[s, 3],9:F2} |");
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', 65));
        }

        public static void PrintHeatmap(LearningAgent agent, Maze m)
        {
            // TODO: imprimir V(s) = max_a Q[s,a] en forma de cuadrícula 2D
            Console.WriteLine("(pendiente) PrintHeatmap");
            PrintHorizontalLine(m.Cols);

            for (int r = 0; r < m.Rows; r++)
            {
                Console.Write("|");
                for (int c = 0; c < m.Cols; c++)
                {
                    int s = m.ToState(r, c);
                    double vValor = agent.MaxQ(s);//máximo valor Q(premio q se puede optener) para el estado s, es decir, V(s) = max_a Q[s,a]
                    Console.Write($"{vValor,8:F1} | ");
                }
                Console.WriteLine();
                PrintHorizontalLine(m.Cols);
            }
        }

        private static void PrintHorizontalLine(int cols)
        {
            Console.WriteLine(new string('-', cols * 10 + 1));
        }
    }
}
