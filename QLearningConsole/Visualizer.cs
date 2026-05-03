using System;
using System.Text;

namespace QLearningConsole
{
    /// Utilidades de impresión. Se proporcionan PrintMaze y PrintPolicy
    /// ya funcionando; el alumno debe implementar PrintQTable y PrintHeatmap
    /// para facilitar el análisis.
    public static class Visualizer
    {
        public static void PrintMaze(Maze m)
        {
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
            var arrows = new[] { "^", ">", "v", "<" };

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
                        int bestA = agent.ArgMaxAction(s);
                        bool allZero = agent.Q[s, 0] == 0 && agent.Q[s, 1] == 0 &&
                                       agent.Q[s, 2] == 0 && agent.Q[s, 3] == 0;
                        cellStr = allZero ? " . " : $" {arrows[bestA]} ";
                    }
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
        }

        public static void PrintHeatmap(LearningAgent agent, Maze m)
        {
            // TODO: imprimir V(s) = max_a Q[s,a] en forma de cuadrícula 2D
            Console.WriteLine("(pendiente) PrintHeatmap");
        }
    }
}
