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
        enum heuristic { nMisplaced, manhattan };

        private int[,] currentBoard;
        heuristic currentHeuristic;
        private int[,] solvedBoard = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };

        HashSet<string> boardsSeen;
        Dictionary<string, int> boardHeuristicTracker;
        Dictionary<string, string> parentMap;

        public boardSolver(int[,] board)
        {
            this.currentBoard = board;

            boardsSeen = new HashSet<string>();
            boardHeuristicTracker = new Dictionary<string, int>();
            parentMap = new Dictionary<string, string>();

            boardsSeen.Add(boardToString(board));
            parentMap.Add(boardToString(solvedBoard), boardToString(solvedBoard));

            currentHeuristic = heuristic.nMisplaced;
        }

        public List<string> depthFirstSearch()
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
            string childBoard = boardToString(solvedBoard);
            List<string> stepsToSolve = new List<string>();

            while (childBoard != originalBoard)
            {
                stepsToSolve.Add(getBoardDifference(childBoard, parentMap[childBoard]));
                childBoard = parentMap[childBoard];
            }

            stepsToSolve.Reverse();
            return stepsToSolve;
        }

        public List<string> breadthFirstSearch()
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
                boardCheck = boardQueue.Dequeue();
            }
            nSteps++;


            solved:
            string originalBoard = boardToString(this.currentBoard);
            string childBoard = boardToString(solvedBoard);
            List<string> stepsToSolve = new List<string>();

            while (childBoard != originalBoard)
            {
                stepsToSolve.Add(getBoardDifference(childBoard, parentMap[childBoard]));
                childBoard = parentMap[childBoard];
            }

            stepsToSolve.Reverse();
            return stepsToSolve;
        }

        public List<string> greedyBestFirstSearch()
        {
            int nSteps = 0;
            boardHeuristicTracker[boardToString(currentBoard)] = -1;
            int[,] boardCheck = currentBoard;
            string boardCheckString = boardToString(boardCheck);

            if (isSolved(boardCheck))
            {
                return new List<string>();
            }

            while (!isSolved(boardCheck))
            {
                foreach (neighbors neighbor in Enum.GetValues(typeof(neighbors)))
                {
                    int[,] neighborBoard = getNeighbor(boardCheck, neighbor);
                    
                    if (neighborBoard != null)
                    {
                        string neighborBoardString = boardToString(neighborBoard);
                        int neighborHeuristicValue = getHeuristicValue(neighborBoard);

                        if (!boardHeuristicTracker.ContainsKey(neighborBoardString))
                        {
                            parentMap[neighborBoardString] = boardCheckString;
                            boardHeuristicTracker[neighborBoardString] = neighborHeuristicValue;
                        }
                    }
                }

                nSteps++;
                boardCheck = getCurrentBestBoard();
                boardCheckString = boardToString(boardCheck);
            }

            string originalBoard = boardToString(this.currentBoard);
            string childBoard = boardToString(solvedBoard);
            List<string> stepsToSolve = new List<string>();

            while (childBoard != originalBoard)
            {
                stepsToSolve.Add(getBoardDifference(childBoard, parentMap[childBoard]));
                childBoard = parentMap[childBoard];
            }

            stepsToSolve.Reverse();
            return stepsToSolve;
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

        private int getHeuristicValue(int[,] board)
        {
            switch(currentHeuristic)
            {
                case heuristic.nMisplaced:
                    int nMisplacedTiles = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (board[i,j] != solvedBoard[i,j])
                            {
                                nMisplacedTiles++;
                            }
                        }
                    }
                    return nMisplacedTiles;

                case heuristic.manhattan:
                    break;
            }
            return -1;
        }

        private int[,] getCurrentBestBoard()
        {
            int currentMin = -1;
            string currentMinBoardString = "00000000";
            int[,] currentMinBoard = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

            foreach (KeyValuePair<string, int> board in boardHeuristicTracker)
            {
                if ((currentMin == -1 || board.Value < currentMin) && board.Value != -1)
                {
                    currentMin = board.Value;
                    currentMinBoardString = board.Key;
                }
            }

            boardHeuristicTracker[currentMinBoardString] = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    currentMinBoard[i, j] = currentMinBoardString[j + 3*i] - 48;
                }
            }

            return currentMinBoard;
        }

        private string getBoardDifference(string board1, string board2)
        {
            int zero1 = -1, zero2 = -1, difference;

            for (int i = 0; i < 9; i++)
            {
                if (board1[i] == '0')
                {
                    zero1 = i;
                }
                if (board2[i] == '0')
                {
                    zero2 = i;
                }
            }

            difference = zero1 - zero2;

            switch(difference)
            {
                case (3): return "Up";
                case (-3): return "Down";
                case (1): return "Left";
                case (-1): return "Right";
                default: return "Multiple Steps required";
            }
        }
    }
}
