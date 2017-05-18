using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightPuzzleSolver
{
    class boardSolver
    {
        enum neighbors { right, bottom, left, top };

        private int[,] currentBoard;
        private int[,] solvedBoard = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };

        HashSet<string> boardsSeen;
        Dictionary<string, string> parentMap;

        public boardSolver(int[,] board)
        {
            this.currentBoard = board;

            boardsSeen = new HashSet<string>();
            parentMap = new Dictionary<string, string>();

            boardsSeen.Add(boardToString(board));
            parentMap.Add(boardToString(solvedBoard), boardToString(solvedBoard));
        }

        public int depthFirstSearch()
        {
            Stack<int[,]> boardStack = new Stack<int[,]>();
            boardStack.Push(currentBoard);
            int[,] boardCheck = boardStack.Pop();
            int nSteps = 0;

            while (!isSolved(boardCheck))
            {
                foreach (neighbors neighbor in Enum.GetValues(typeof(neighbors)))
                {
                    int[,] neighborBoard = getNeighbor(boardCheck, neighbor);
                    if (neighborBoard != null && !boardsSeen.Contains(boardToString(neighborBoard)))
                    {
                        boardStack.Push(neighborBoard);
                        boardsSeen.Add(boardToString(neighborBoard));
                        parentMap[boardToString(neighborBoard)] = boardToString(boardCheck);
                        if (isSolved(neighborBoard))
                        {
                            nSteps++;
                            goto solved;
                        }
                    }
                }
                nSteps++;

                boardCheck = boardStack.Pop();
            }

            solved:
            string originalBoard = boardToString(this.currentBoard);
            string childBoard = parentMap[boardToString(solvedBoard)];
            int nRealSteps = 0;
            while (childBoard != originalBoard)
            {
                childBoard = parentMap[childBoard];
                nRealSteps++;
            }

            return nRealSteps;
        }

        public int breadthFirstSearch()
        {
            Queue<int[,]> boardQueue = new Queue<int[,]>();
            boardQueue.Enqueue(currentBoard);
            int[,] boardCheck = boardQueue.Dequeue();
            int nSteps = 0;

            while (!isSolved(boardCheck))
            {
                foreach (neighbors neighbor in Enum.GetValues(typeof(neighbors)))
                {
                    int[,] neighborBoard = getNeighbor(boardCheck, neighbor);
                    if (neighborBoard != null && !boardsSeen.Contains(boardToString(neighborBoard)))
                    {
                        boardQueue.Enqueue(neighborBoard);
                        boardsSeen.Add(boardToString(neighborBoard));
                        parentMap[boardToString(neighborBoard)] = boardToString(boardCheck);
                        if (isSolved(neighborBoard))
                        {
                            nSteps++;
                            goto solved;
                        }
                    }
                }
                nSteps++;

                boardCheck = boardQueue.Dequeue();
            }

            solved:
            string originalBoard = boardToString(this.currentBoard);
            string childBoard = parentMap[boardToString(solvedBoard)];
            int nRealSteps = 0;
            while (childBoard != originalBoard)
            {
                childBoard = parentMap[childBoard];
                nRealSteps++;
            }

            return nRealSteps;
        }

        private bool isSolved(int[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] != solvedBoard[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int[,] getNeighbor(int[,] board, neighbors neighbor)
        {
            int[,] returnBoard = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            int emptyX = -1, emptyY = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    returnBoard[i, j] = board[i, j];
                    if (board[i, j] == 0)
                    {
                        emptyX = i;
                        emptyY = j;
                    }
                }
            }

            switch (neighbor)
            {
                case (neighbors.top):
                    if (emptyX > 0)
                    {
                        returnBoard[emptyX, emptyY] = board[emptyX - 1, emptyY];
                        returnBoard[emptyX - 1, emptyY] = board[emptyX, emptyY];
                    }
                    else
                    {
                        return null;
                    }
                    break;

                case (neighbors.left):
                    if (emptyY > 0)
                    {
                        returnBoard[emptyX, emptyY] = board[emptyX, emptyY - 1];
                        returnBoard[emptyX, emptyY - 1] = board[emptyX, emptyY];
                    }
                    else
                    {
                        return null;
                    }
                    break;

                case (neighbors.bottom):
                    if (emptyX < 2)
                    {
                        returnBoard[emptyX, emptyY] = board[emptyX + 1, emptyY];
                        returnBoard[emptyX + 1, emptyY] = board[emptyX, emptyY];
                    }
                    else
                    {
                        return null;
                    }
                    break;

                case (neighbors.right):
                    if (emptyY < 2)
                    {
                        returnBoard[emptyX, emptyY] = board[emptyX, emptyY + 1];
                        returnBoard[emptyX, emptyY + 1] = board[emptyX, emptyY];
                    }
                    else
                    {
                        return null;
                    }
                    break;
            }

            return returnBoard;
        }

        private string boardToString(int[,] board)
        {
            string returnString = "";

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    returnString += board[i, j].ToString();
                }
            }

            return returnString;
        }
    }
}
