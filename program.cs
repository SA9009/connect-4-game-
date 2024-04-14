using System;

// Enum representing the different states of a cell on the game board
public enum CellState
{
    Empty,
    Player1,
    Player2
}

// Class representing a player
public class Player
{
    public string Name { get; private set; }
    public CellState Disc { get; private set; }

    public Player(string name, CellState disc)
    {
        Name = name;
        Disc = disc;
    }
}

// Class representing the game board
public class Board
{
    private const int Rows = 6;
    private const int Columns = 7;
    private CellState[,] grid;

    public Board()
    {
        grid = new CellState[Rows, Columns];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                grid[row, col] = CellState.Empty;
            }
        }
    }

    public void DisplayBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                switch (grid[row, col])
                {
                    case CellState.Empty:
                        Console.Write(". ");
                        break;
                    case CellState.Player1:
                        Console.Write("X ");
                        break;
                    case CellState.Player2:
                        Console.Write("O ");
                        break;
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine("1 2 3 4 5 6 7");
    }

    public bool IsColumnFull(int column)
    {
        return grid[0, column] != CellState.Empty;
    }

    public bool IsValidMove(int column)
    {
        return column >= 0 && column < Columns && !IsColumnFull(column);
    }

    public bool DropDisc(int column, CellState playerDisc)
    {
        if (!IsValidMove(column))
            return false;

        for (int row = Rows - 1; row >= 0; row--)
        {
            if (grid[row, column] == CellState.Empty)
            {
                grid[row, column] = playerDisc;
                return true;
            }
        }
        return false;
    }

    public bool CheckForWin(CellState playerDisc)
    {
        // Check horizontally
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col <= Columns - 4; col++)
            {
                if (grid[row, col] == playerDisc &&
                    grid[row, col + 1] == playerDisc &&
                    grid[row, col + 2] == playerDisc &&
                    grid[row, col + 3] == playerDisc)
                {
                    return true;
                }
            }
        }

        // Check vertically
        for (int col = 0; col < Columns; col++)
        {
            for (int row = 0; row <= Rows - 4; row++)
            {
                if (grid[row, col] == playerDisc &&
                    grid[row + 1, col] == playerDisc &&
                    grid[row + 2, col] == playerDisc &&
                    grid[row + 3, col] == playerDisc)
                {
                    return true;
                }
            }
        }

        // Check diagonally (down-right)
        for (int row = 0; row <= Rows - 4; row++)
        {
            for (int col = 0; col <= Columns - 4; col++)
            {
                if (grid[row, col] == playerDisc &&
                    grid[row + 1, col + 1] == playerDisc &&
                    grid[row + 2, col + 2] == playerDisc &&
                    grid[row + 3, col + 3] == playerDisc)
                {
                    return true;
                }
            }
        }

        // Check diagonally (down-left)
        for (int row = 0; row <= Rows - 4; row++)
        {
            for (int col = 3; col < Columns; col++)
            {
                if (grid[row, col] == playerDisc &&
                    grid[row + 1, col - 1] == playerDisc &&
                    grid[row + 2, col - 2] == playerDisc &&
                    grid[row + 3, col - 3] == playerDisc)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsBoardFull()
    {
        for (int col = 0; col < Columns; col++)
        {
            if (!IsColumnFull(col))
                return false;
        }
        return true;
    }
}

// Class representing the Connect Four game
public class ConnectFour
{
    private Player player1;
    private Player player2;
    private Board board;
    private Player currentPlayer;

    public ConnectFour(string player1Name, string player2Name)
    {
        player1 = new Player(player1Name, CellState.Player1);
        player2 = new Player(player2Name, CellState.Player2);
        board = new Board();
        currentPlayer = player1;
    }

    public void StartGame()
    {
        Console.WriteLine($"Welcome to Connect Four!\n{player1.Name} (X) vs. {player2.Name} (O)\n");

        while (true)
        {
            board.DisplayBoard();
            Console.WriteLine($"{currentPlayer.Name}'s turn ({(currentPlayer.Disc == CellState.Player1 ? "X" : "O")})");

            int column;
            do
            {
                Console.Write("Enter column number (1-7): ");
            } while (!int.TryParse(Console.ReadLine(), out column) || column < 1 || column > 7 || !board.IsValidMove(column - 1));

            board.DropDisc(column - 1, currentPlayer.Disc);

            if (board.CheckForWin(currentPlayer.Disc))
            {
                board.DisplayBoard();
                Console.WriteLine($"{currentPlayer.Name} wins!");
                break;
            }

            if (board.IsBoardFull())
            {
                board.DisplayBoard();
                Console.WriteLine("It's a draw!");
                break;
            }

            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        ConnectFour game = new ConnectFour("Player 1", "Player 2");
        game.StartGame();
    }
}

