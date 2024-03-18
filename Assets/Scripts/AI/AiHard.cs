using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;


public class AiHard : MonoBehaviour
{


    public GameObject[,] Board;
    public GameObject game;
    public bool didOpponentWin;
    public GameObject[,] tempGame = new GameObject[5, 5];



    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameBoard");
    }

    public void movePiece(GamePiece piece, GamePiece move)
    {
        GameObject temp;
        //Shift is row based
        if (piece.row != move.row)
        {
            if (move.row < piece.row)
            {
                for (int i = piece.row; i > move.row; i--)
                {
                    temp = tempGame[piece.col, i];
                    tempGame[piece.col, i] = tempGame[piece.col, i - 1];
                    tempGame[piece.col, i - 1] = temp;
                }
            }
            else
            {
                for (int i = piece.row; i < move.row; i++)
                {
                    temp = tempGame[piece.col, i];
                    tempGame[piece.col, i] = tempGame[piece.col, i + 1];
                    tempGame[piece.col, i + 1] = temp;
                }
            }
        }
        //Shift is column based
        else
        {

            if (move.col < piece.col)
            {
                for (int i = piece.col; i > move.col; i--)
                {
                    temp = tempGame[i, piece.row];
                    tempGame[i, piece.row] = tempGame[i - 1, piece.row];
                    tempGame[i - 1, piece.row] = temp;
                }
            }
            else
            {
                for (int i = piece.col; i < move.col; i++)
                {
                    temp = tempGame[i, piece.row];
                    tempGame[i, piece.row] = tempGame[i + 1, piece.row];
                    tempGame[i + 1, piece.row] = temp;
                }
            }
        }
    }

    public bool checkWin(bool isPlayerOnesTurn)
    {
        int player1WinTracker = 0;
        int player2WinTracker = 0;
        int player1RowTracker = 0;
        int player2RowTracker = 0;
        int player1ColTracker = 0;
        int player2ColTracker = 0;

        //Checks diagnals for a win
        for (int i = 0; i < 5; i++)
        {
            if (tempGame[i, i].CompareTag("Player1"))
            {
                player1RowTracker++;
            }
            else if (tempGame[i, i].CompareTag("Player2"))
            {
                player2RowTracker++;
            }

            if (tempGame[(4 - i), i].CompareTag("Player1"))
            {
                player1ColTracker++;
            }
            else if (tempGame[(4 - i), i].CompareTag("Player2"))
            {
                player2ColTracker++;
            }
        }

        if (player1RowTracker == 5 || player1ColTracker == 5)
        {
            if (!isPlayerOnesTurn)
            {
                didOpponentWin = true;
            }
            return true;
        }
        else if (player2RowTracker == 5 || player2ColTracker == 5)
        {
            if (isPlayerOnesTurn)
            {
                didOpponentWin = true;
            }
            return true;
        }

        player1RowTracker = 0;
        player2RowTracker = 0;
        player1ColTracker = 0;
        player2ColTracker = 0;

        //checks rows and columns for a win
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (tempGame[j, i].CompareTag("Player1"))
                {
                    player1RowTracker++;
                }
                else if (tempGame[j, i].CompareTag("Player2"))
                {
                    player2RowTracker++;
                }

                if (tempGame[i, j].CompareTag("Player1"))
                {
                    player1ColTracker++;
                }
                else if (tempGame[i, j].CompareTag("Player2"))
                {
                    player2ColTracker++;
                }
            }
            if (player1RowTracker == 5 || player1ColTracker == 5)
            {
                player1WinTracker++;
            }
            else if (player2RowTracker == 5 || player2ColTracker == 5)
            {
                player2WinTracker++;
            }
            player1RowTracker = 0;
            player2RowTracker = 0;
            player1ColTracker = 0;
            player2ColTracker = 0;
        }

        if (isPlayerOnesTurn && player1WinTracker > 0 && player2WinTracker == 0)
        {
            return true;
        }
        else if (!isPlayerOnesTurn && player1WinTracker == 0 && player2WinTracker > 0)
        {
            return true;
        }
        else if (player1WinTracker > 0 && player2WinTracker > 0)
        {
            didOpponentWin = true;
            return true;
        }
        return false;
    }



    GamePiece[] AvailablePieces()
    {
        //I Initialize the Array size to 16 because that's the max amount of moves we can ever have
        //Im still figuiring out how to make the size of the array change depending on the available moves, and not be just a predetermined size
        List<GamePiece> avaMoves = new List<GamePiece>();
        //Coordinate[] avaMoves = new Coordinate[16];
        //GamePiece temp = new GamePiece();
        //temp.row = 0;
        //temp.col = 0;

        int counter = 0;

        //System.Collections.IEnumerator myEnumerator = Board.GetEnumerator();
        //int rows = 0;
        //int cols = Board.GetLength(Board.Rank - 1);
        //int row = 0;
        //int col = -1;
        //Iterate through the entire board array obtaining the coords of each piece
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                if (c == 0 || r == 0 || c == 4 || r == 4)
                {
                    if (gameObject.GetComponent<Click>().isPlayerOneTurn)
                    {
                        if (Board[c, r].tag == "Player1" || Board[c, r].tag == "Blank")
                        {
                            //Console.WriteLine("X(" + row + "," + col + ")");
                            //temp.row = r;
                            //temp.col = c;
                            avaMoves.Add(Board[c, r].GetComponent<GamePiece>());
                        }
                    }
                    else
                    {
                        if (Board[c, r].tag == "Player2" || Board[c, r].tag == "Blank")
                        {
                            //Console.WriteLine("O(" + row + "," + col + ")");
                            //temp.row = r;
                            //temp.col = c;
                            avaMoves.Add(Board[c, r].GetComponent<GamePiece>());
                        }
                    }
                    //avaMoves.Add(temp);
                    counter++;
                }
            }
        }

        return avaMoves.ToArray();
    }

    int CheckBoardValue(GamePiece piece, GamePiece move)
    {

        string tag = piece.tag;
        int value = 0;
        KeyValuePair<int, int> tempPiece = new KeyValuePair<int, int>(piece.row, piece.col);
        KeyValuePair<int, int> tempMove = new KeyValuePair<int, int>(move.row, move.col);
        List<GamePiece> tg = new List<GamePiece>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                tempGame[i, j] = Board[i, j];
                tg.Add(tempGame[i, j].GetComponent<GamePiece>());
            }
        }
        List<GamePiece> X = new List<GamePiece>();
        List<GamePiece> O = new List<GamePiece>();
        //tempGame.FlipBlock(piece);
        //Debug.Log("-------------------------------------------------------------------");
        //for (int r = 0; r < tempGame.GetLength(0); r++)
        //{
        //    for (int c = 0; c < tempGame.GetLength(0); c++)
        //    {
        //        Debug.Log(r + ", " + c + " (" + tempGame[r, c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
        //    }
        //}
        //Debug.Log("-------------------------------------------------------------------");
        piece.SetPlayer(gameObject.GetComponent<Click>().isPlayerOneTurn);
        movePiece(piece, move);
        Debug.Log("Moving Piece: 1(" + piece.col + "," + piece.row + ")");
        Debug.Log("Here: 1(" + move.col + "," + move.row + ")");
        Debug.Log("-------------------------------------------------------------------");
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(tempGame[i, 0].GetComponent<GamePiece>().tag + " " + tempGame[i, 1].GetComponent<GamePiece>().tag + " " + tempGame[i, 2].GetComponent<GamePiece>().tag + " " + tempGame[i, 3].GetComponent<GamePiece>().tag + " " + tempGame[i, 4].GetComponent<GamePiece>().tag);
        }
        Debug.Log("-------------------------------------------------------------------");


        for (int r = 0; r < tempGame.GetLength(0); r++)
        {
            for (int c = 0; c < tempGame.GetLength(0); c++)
            {
                tempGame[r, c].GetComponent<GamePiece>().row = c;
                tempGame[r, c].GetComponent<GamePiece>().col = r;
                //Debug.Log(r + ", " + c + " (" + tempGame[r,c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
                if ((tempGame[c, r].CompareTag("Player1")))
                {
                    X.Add(tempGame[c, r].GetComponent<GamePiece>());
                }
                else if ((tempGame[c, r].CompareTag("Player2")))
                {
                    O.Add(tempGame[c, r].GetComponent<GamePiece>());
                }
            }
        }
        //foreach (var item in tempGame)
        //{
        //    Debug.Log("(" + item.GetComponent<GamePiece>().col + ", " + item.GetComponent<GamePiece>().row + "): " + item.GetComponent<GamePiece>().tag);
        //}
        //X = tg.FindAll(t => t.GetComponent<GamePiece>().tag == "Player1");
        //O = tg.FindAll(t => t.GetComponent<GamePiece>().tag == "Player2");

        //Debug.Log("X");
        //foreach (var cube in X)
        //{
        //    Debug.Log("X Piece: (" + cube.col + "," + cube.row + ")");
        //}
        //Debug.Log("O");
        //foreach (var cube in O)
        //{
        //    Debug.Log("O Piece: (" + cube.col + "," + cube.row + ")");
        //}
        //Debug.Log("-------------------------------------------------------------------");
        if (gameObject.GetComponent<Click>().isPlayerOneTurn)
        {
            foreach (var xPiece in X)
            {
                switch (xPiece.row)
                {
                    case 0:
                        switch (xPiece.col)
                        {
                            case 0: value += 1; break;
                            case 2: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                    case 1:
                        switch (xPiece.col)
                        {
                            case 1: value += 1; break;
                            case 2: value += 5; break;
                            case 3: value += 1; break;
                        }
                        break;
                    case 2:
                        switch (xPiece.col)
                        {
                            case 0: value += 2; break;
                            case 1: value += 3; break;
                            case 2: value += 6; break;
                            case 3: value += 3; break;
                            case 4: value += 2; break;
                        }
                        break;
                    case 3:
                        switch (xPiece.col)
                        {
                            case 1: value += 1; break;
                            case 2: value += 5; break;
                            case 3: value += 1; break;
                        }
                        break;
                    case 4:
                        switch (xPiece.col)
                        {
                            case 0: value += 1; break;
                            case 2: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                }

            }
            foreach (var oPiece in O)
            {
                switch (oPiece.row)
                {

                    case 0:
                        switch (oPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 2: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                    case 1:
                        switch (oPiece.col)
                        {
                            case 1: value -= 1; break;
                            case 2: value -= 5; break;
                            case 3: value -= 1; break;
                        }
                        break;
                    case 2:
                        switch (oPiece.col)
                        {
                            case 0: value -= 2; break;
                            case 1: value -= 3; break;
                            case 2: value -= 6; break;
                            case 3: value -= 3; break;
                            case 4: value -= 2; break;
                        }
                        break;
                    case 3:
                        switch (oPiece.col)
                        {
                            case 1: value -= 1; break;
                            case 2: value -= 5; break;
                            case 3: value -= 1; break;
                        }
                        break;
                    case 4:
                        switch (oPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 2: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                }
            }
        }
        else
        {
            foreach (var oPiece in O)
            {
                switch (oPiece.row)
                {
                    case 0:
                        switch (oPiece.col)
                        {
                            case 0: value += 1; break;
                            case 2: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                    case 1:
                        switch (oPiece.col)
                        {
                            case 1: value += 1; break;
                            case 2: value += 5; break;
                            case 3: value += 1; break;
                        }
                        break;
                    case 2:
                        switch (oPiece.col)
                        {
                            case 0: value += 2; break;
                            case 1: value += 3; break;
                            case 2: value += 6; break;
                            case 3: value += 3; break;
                            case 4: value += 2; break;
                        }
                        break;
                    case 3:
                        switch (oPiece.col)
                        {
                            case 1: value += 1; break;
                            case 2: value += 5; break;
                            case 3: value += 1; break;
                        }
                        break;
                    case 4:
                        switch (oPiece.col)
                        {
                            case 0: value += 1; break;
                            case 2: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                }

            }
            foreach (var xPiece in X)
            {
                switch (xPiece.row)
                {

                    case 0:
                        switch (xPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 2: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                    case 1:
                        switch (xPiece.col)
                        {
                            case 1: value -= 1; break;
                            case 2: value -= 5; break;
                            case 3: value -= 1; break;
                        }
                        break;
                    case 2:
                        switch (xPiece.col)
                        {
                            case 0: value -= 2; break;
                            case 1: value -= 3; break;
                            case 2: value -= 6; break;
                            case 3: value -= 3; break;
                            case 4: value -= 2; break;
                        }
                        break;
                    case 3:
                        switch (xPiece.col)
                        {
                            case 1: value -= 1; break;
                            case 2: value -= 5; break;
                            case 3: value -= 1; break;
                        }
                        break;
                    case 4:
                        switch (xPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 2: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                }
            }
        }

        if (gameObject.GetComponent<Click>().isPlayerOneTurn)
        {
            var sameRow = X.FindAll(t => t.row == move.row);
            var sameCol = X.FindAll(t => t.col == move.col);
            var diag1 = X.FindAll(t => t.col == t.row && t.col == move.col);
            var diag2 = X.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            value += sameRow.Count;
            value += sameCol.Count;
            value += diag1.Count;
            value += diag2.Count;

            if (sameRow.Count > 3 || sameCol.Count > 3 || diag1.Count > 3 || diag2.Count > 3)
            {
                value += 1000;
            }

            //var enemyRow = O.FindAll(t => t.row == move.row);
            //var enemyCol = O.FindAll(t => t.col == move.col);
            //value += enemyRow.Count;
            //value += enemyCol.Count;

            var eRow0 = O.FindAll(t => t.row == 0);
            var eRow1 = O.FindAll(t => t.row == 1);
            var eRow2 = O.FindAll(t => t.row == 2);
            var eRow3 = O.FindAll(t => t.row == 3);
            var eRow4 = O.FindAll(t => t.row == 4);

            var eCol0 = O.FindAll(t => t.col == 0);
            var eCol1 = O.FindAll(t => t.col == 1);
            var eCol2 = O.FindAll(t => t.col == 2);
            var eCol3 = O.FindAll(t => t.col == 3);
            var eCol4 = O.FindAll(t => t.col == 4);

            var ediag1 = O.FindAll(t => t.col == t.row && t.col == move.col);
            var ediag2 = O.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            if (ediag1.Count >= 4 || ediag2.Count >= 4 || eRow0.Count >= 4 || eRow1.Count >= 4 || eRow2.Count >= 4 || eRow3.Count >= 4 || eRow4.Count >= 4 || eCol0.Count >= 4 || eCol1.Count >= 4 || eCol2.Count >= 4 || eCol3.Count >= 4 || eCol4.Count >= 4)
            {
                value -= 10000;
            }
            else
            {
                value += 100;
            }

        }
        else
        {
            var sameRow = O.FindAll(t => t.row == move.row);
            var sameCol = O.FindAll(t => t.col == move.col);
            var diag1 = O.FindAll(t => t.col == t.row && t.col == move.col);
            var diag2 = O.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            value += sameRow.Count;
            value += sameCol.Count;
            value += diag1.Count;
            value += diag2.Count;

            if (sameRow.Count > 3 || sameCol.Count > 3 || diag1.Count > 3 || diag2.Count > 3)
            {
                value += 1000;
            }

            //var enemyRow = X.FindAll(t => t.row == move.row);
            //var enemyCol = X.FindAll(t => t.col == move.col);
            //value += enemyRow.Count;
            //value += enemyCol.Count;
            var eRow0 = X.FindAll(t => t.row == 0);
            var eRow1 = X.FindAll(t => t.row == 1);
            var eRow2 = X.FindAll(t => t.row == 2);
            var eRow3 = X.FindAll(t => t.row == 3);
            var eRow4 = X.FindAll(t => t.row == 4);

            var eCol0 = X.FindAll(t => t.col == 0);
            var eCol1 = X.FindAll(t => t.col == 1);
            var eCol2 = X.FindAll(t => t.col == 2);
            var eCol3 = X.FindAll(t => t.col == 3);
            var eCol4 = X.FindAll(t => t.col == 4);

            var ediag1 = X.FindAll(t => t.col == t.row && t.col == move.col);
            var ediag2 = X.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            if (ediag1.Count >= 4 || ediag2.Count >= 4 || eRow0.Count >= 4 || eRow1.Count >= 4 || eRow2.Count >= 4 || eRow3.Count >= 4 || eRow4.Count >= 4 || eCol0.Count >= 4 || eCol1.Count >= 4 || eCol2.Count >= 4 || eCol3.Count >= 4 || eCol4.Count >= 4)
            {
                value -= 10000;
            }
            else
            {
                value += 100;
            }
        }

        //if (checkWin(gameObject.GetComponent<Click>().isPlayerOneTurn) && !didOpponentWin)
        //{
        //    value += 100000;
        //}

        //tempGame.FlipBlock(piece);
        //tempGame.MakeMove(piece, move);
        //Debug.Log("-------------------------------------------------------------------");
        int mr = move.row;
        move.row = tempMove.Key;
        int mc = move.col;
        move.col = tempMove.Value;
        int pr = piece.row;
        piece.row = tempPiece.Key;
        int pc = piece.col;
        piece.col = tempPiece.Value;
        if (checkWin(gameObject.GetComponent<Click>().isPlayerOneTurn) && !didOpponentWin)
        {
            value += 1000000;
        }
        movePiece(move, piece);
        tempGame[piece.col, piece.row].GetComponent<GamePiece>().isBlank = true;
        tempGame[piece.col, piece.row].GetComponent<GamePiece>().transform.tag = tag;
        Debug.Log("Moving Piece: 2(" + move.col + "," + move.row + ")");
        Debug.Log("Here: 2(" + piece.col + "," + piece.row + ")");
        Debug.Log("-------------------------------------------------------------------");

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(tempGame[i, 0].tag + " " + tempGame[i, 1].tag + " " + tempGame[i, 2].tag + " " + tempGame[i, 3].tag + " " + tempGame[i, 4].tag);
        }
        Debug.Log("-------------------------------------------------------------------");

        //Debug.Log("-------------------------------------------------------------------");
        for (int r = 0; r < tempGame.GetLength(0); r++)
        {
            for (int c = 0; c < tempGame.GetLength(0); c++)
            {
                tempGame[r, c].GetComponent<GamePiece>().row = c;
                tempGame[r, c].GetComponent<GamePiece>().col = r;
                //Debug.Log(r + ", " + c + " (" + tempGame[r, c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
            }
        }
        //Debug.Log("-------------------------------------------------------------------");


        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                //Board[i, j] = tempGame[i, j];
            }
        }


        //move.row = mr;
        //move.col = mc;
        //piece.row = pr;
        //piece.col = pc;
        return value;
    }


    public KeyValuePair<GamePiece, GamePiece> AITurn()
    {
        Board = game.GetComponent<GameBoard>().Board;

        int moveCounter = 0;
        List<Tuple<GamePiece, GamePiece, int>> values = new List<Tuple<GamePiece, GamePiece, int>>();
        GamePiece[] moves = AvailablePieces();
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i].row >= 0 || moves[i].col >= 0)
            {
                if (moves[i].CheckPickedPiece((gameObject.GetComponent<Click>().isPlayerOneTurn)))
                {
                    GameObject[] posMoves = moves[i].PossibleMoves();
                    for (int j = 0; j < posMoves.Length; j++)
                    {
                        moveCounter++;
                        int value = 1;
                        GamePiece curMove = posMoves[j].GetComponent<GamePiece>();
                        if (Board[moves[i].col, moves[i].row].tag == "Blank")
                        {
                            value += 100;
                        }
                        value += CheckBoardValue(moves[i], posMoves[j].GetComponent<GamePiece>());
                        values.Add(new Tuple<GamePiece, GamePiece, int>(moves[i], posMoves[j].GetComponent<GamePiece>(), value));
                    }
                }
            }
        }
        List<Tuple<GamePiece, GamePiece, int>> NewBestValues = new List<Tuple<GamePiece, GamePiece, int>>();
        foreach (var move in values)
        {
            //Board = game.GetComponent<GameBoard>().Board;

            Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            for (int i = 0; i < 5; i++)
            {
                Debug.Log(tempGame[i, 0].tag + " " + tempGame[i, 1].tag + " " + tempGame[i, 2].tag + " " + tempGame[i, 3].tag + " " + tempGame[i, 4].tag);
            }
            Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            KeyValuePair<int, int> tempPiece = new KeyValuePair<int, int>(move.Item1.row, move.Item1.col);
            KeyValuePair<int, int> tempMove = new KeyValuePair<int, int>(move.Item2.row, move.Item2.col);
            List<GamePiece> tg = new List<GamePiece>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    tempGame[i, j] = Board[i, j];
                    tg.Add(tempGame[i, j].GetComponent<GamePiece>());
                }
            }
            string tag = move.Item1.tag;

            move.Item1.SetPlayer(gameObject.GetComponent<Click>().isPlayerOneTurn);
            gameObject.GetComponent<Click>().isPlayerOneTurn = !gameObject.GetComponent<Click>().isPlayerOneTurn;
            movePiece(move.Item1, move.Item2);
            Debug.Log("Moving Piece: p1(" + move.Item1.col + "," + move.Item1.row + ")");
            Debug.Log("Here: p1(" + move.Item2.col + "," + move.Item2.row + ")");
            Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            for (int i = 0; i < 5; i++)
            {
                Debug.Log(tempGame[i, 0].tag + " " + tempGame[i, 1].tag + " " + tempGame[i, 2].tag + " " + tempGame[i, 3].tag + " " + tempGame[i, 4].tag);
            }
            Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");


            for (int r = 0; r < tempGame.GetLength(0); r++)
            {
                for (int c = 0; c < tempGame.GetLength(0); c++)
                {
                    //tempGame[r, c].GetComponent<GamePiece>().row = c;
                    //tempGame[r, c].GetComponent<GamePiece>().col = r;
                    //Debug.Log(r + ", " + c + " (" + tempGame[r,c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
                }
            }


            List<Tuple<GamePiece, GamePiece, int>> eValues = new List<Tuple<GamePiece, GamePiece, int>>();
            GamePiece[] eMoves = AvailablePieces();
            for (int i = 0; i < eMoves.Length; i++)
            {
                if (eMoves[i].row >= 0 || eMoves[i].col >= 0)
                {
                    if (eMoves[i].CheckPickedPiece((gameObject.GetComponent<Click>().isPlayerOneTurn)))
                    {
                        //Console.WriteLine("Piece:");
                        //Console.Write(moves[i].row + ", ");
                        //Console.WriteLine(moves[i].col);
                        GameObject[] posMoves = eMoves[i].PossibleMoves();
                        //Console.WriteLine("Moves:");
                        for (int j = 0; j < posMoves.Length; j++)
                        {
                            //Console.Write(posMoves[j].row + ", ");
                            //Console.WriteLine(posMoves[j].col);
                            moveCounter++;
                            int value = 1;
                            GamePiece curMove = posMoves[j].GetComponent<GamePiece>();
                            if (Board[eMoves[i].col, eMoves[i].row].tag == "Blank")
                            {
                                value += 100;
                            }
                            value += CheckBoardValue(eMoves[i], posMoves[j].GetComponent<GamePiece>());
                            eValues.Add(new Tuple<GamePiece, GamePiece, int>(eMoves[i], posMoves[j].GetComponent<GamePiece>(), value));
                        }
                    }
                }
            }
            //Debug.Log(eValues.Max(t => t.Item3));
            int newValue = move.Item3 - eValues.Max(t => t.Item3);
            NewBestValues.Add(new Tuple<GamePiece, GamePiece, int>(move.Item1, move.Item2.GetComponent<GamePiece>(), newValue));


            gameObject.GetComponent<Click>().isPlayerOneTurn = !gameObject.GetComponent<Click>().isPlayerOneTurn;


            int mr = move.Item2.row;
            move.Item2.row = tempMove.Key;
            int mc = move.Item2.col;
            move.Item2.col = tempMove.Value;
            int pr = move.Item1.row;
            move.Item1.row = tempPiece.Key;
            int pc = move.Item1.col;
            move.Item1.col = tempPiece.Value;



            movePiece(move.Item2, move.Item1);
            tempGame[move.Item1.col, move.Item1.row].GetComponent<GamePiece>().isBlank = true;
            tempGame[move.Item1.col, move.Item1.row].GetComponent<GamePiece>().transform.tag = tag;
            Debug.Log("Moving Piece: pr2(" + move.Item2.col + "," + move.Item2.row + ")");
            Debug.Log("Here: pr2(" + move.Item1.col + "," + move.Item1.row + ")");
            Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            for (int i = 0; i < 5; i++)
            {
                Debug.Log(tempGame[i, 0].tag + " " + tempGame[i, 1].tag + " " + tempGame[i, 2].tag + " " + tempGame[i, 3].tag + " " + tempGame[i, 4].tag);
            }
            Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            for (int r = 0; r < tempGame.GetLength(0); r++)
            {
                for (int c = 0; c < tempGame.GetLength(0); c++)
                {
                    tempGame[r, c].GetComponent<GamePiece>().row = c;
                    tempGame[r, c].GetComponent<GamePiece>().col = r;
                    //Debug.Log(r + ", " + c + " (" + tempGame[r, c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Board[i, j] = tempGame[i, j];
                }
            }

        }

        NewBestValues.Sort(delegate (Tuple<GamePiece, GamePiece, int> x, Tuple<GamePiece, GamePiece, int> y)
        {
            //if (x.Item3 == y.Item3) return 0;
            //else if (x.Item3 > y.Item3) return -1;
            //else if (y.Item3 > x.Item3) return 1;
            //else 
            return y.Item3.CompareTo(x.Item3);
        });
        int biggestVal = values[0].Item3;
        foreach (var move in values)
        {
            if (move.Item3 > biggestVal)
            {
                biggestVal = move.Item3;
            }
        }
        //Debug.Log("-------------------------------------------------------------------");
        //foreach (var move in values)
        //{
        //    Debug.Log("Moving Piece: (" + move.Item1.row + "," + move.Item1.col + ")");
        //    Debug.Log("Here: (" + move.Item2.row + "," + move.Item2.col + ")");
        //    Debug.Log("Value: " + move.Item3);
        //}
        //Debug.Log("-------------------------------------------------------------------");
        System.Random rnd = new System.Random();

        return new KeyValuePair<GamePiece, GamePiece>(NewBestValues[0].Item1, NewBestValues[0].Item2);
    }



}
