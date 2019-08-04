using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace SeaBattleGame
{
    // Gameboard 10X10 cells. Contains ships & empty cells of player or enemy. Total: 2 gameboards in the game.
    class Board
    {
        private const int Side = 10;  // num of cells in weight or height
        private int _cellSize;
        public Cell[,] CellsBoard { get; set; } // its work like property

        // Constructor
        public Board(Canvas playerCanvas)
        {
            // Ensure canvas is squere & init deck size value
            if (playerCanvas.Width == playerCanvas.Height)
            {
                _cellSize = (int)playerCanvas.Width / 10;
            }
            else
            {
                playerCanvas.Width = Math.Min(playerCanvas.Width, playerCanvas.Height);
                playerCanvas.Height = Math.Min(playerCanvas.Width, playerCanvas.Height);
                _cellSize = (int)playerCanvas.Width / 10;
            }

            // Build array
            CellsBoard = new Cell[Side, Side];
            double positionY = 0;
            for (int i = 0; i < Side; i++)
            {
                double positionX = 0;
                for (int j = 0; j < Side; j++)
                {
                    CellsBoard[i, j] = new Cell(_cellSize, positionX, positionY, i, j, playerCanvas);
                    positionX += _cellSize;
                }
                positionY += _cellSize;
            }
        }

        #region Property

        // Get Property Num of cells per side
        public int CellsInSide
        {
            get
            {
                return Side;
            }
        }

        // Get Property Cell's Size
        public int SizeOfCells
        {
            get
            {
                return _cellSize;
            }
        }

        #endregion

        #region Functions for build Ships
        // Function Try Place Ship, return is it posible, 1 option to deck positions, but NOT PLACE SHIP
        public bool IsCanPlaceShip (int numOfDecks, int firstDeckIndexY, int firstDeckIndexX, out Cell[] decks, out char direction)
        {
            decks = new Cell[numOfDecks];
            direction = ' ';

            Random rnd = new Random();
            bool succesfulHorisont = true;
            bool successfulVertical = true;

            Cell[] tempCellHorisontPositions = new Cell[numOfDecks];
            Cell[] tempCellVerticalPositions = new Cell[numOfDecks];

            int indexY = firstDeckIndexY;
            int indexX = firstDeckIndexX;

            for (int i = 0; i < numOfDecks; i++)
            {
                // try horisontal
                if (succesfulHorisont && indexX < Side && !CellsBoard[firstDeckIndexY, indexX].IsDeck && !CellsBoard[firstDeckIndexY, indexX].IsSpace)
                {
                    tempCellHorisontPositions[i] = CellsBoard[firstDeckIndexY, indexX];
                    indexX++;
                }
                else
                    succesfulHorisont = false;

                // try vertical
                if (successfulVertical && indexY < Side && !CellsBoard[indexY, firstDeckIndexX].IsDeck && !CellsBoard[indexY, firstDeckIndexX].IsSpace)
                {
                    tempCellVerticalPositions[i] = CellsBoard[indexY, firstDeckIndexX];
                    indexY++;
                }
                else
                    successfulVertical = false;
            }

            // choise between 2 succesful place-direction & return it
            if (succesfulHorisont && successfulVertical)
            {
                int choise = rnd.Next(2);
                switch (choise)
                {
                    case 0:
                        decks = tempCellHorisontPositions;
                        direction = 'H';
                        break;
                    case 1:
                        decks = tempCellVerticalPositions;
                        direction = 'V';
                        break;
                }
                return true;
            }
            // check if can place horisontal or vertical ship & return it's reference & direction
            else if (succesfulHorisont)
            {
                decks = tempCellHorisontPositions;
                direction = 'H';
                return true;
            }
            else if (successfulVertical)
            {
                decks = tempCellVerticalPositions;
                direction = 'V';
                return true;
            }
            else
                return false;   // ship can NOT be placed here
        }

        // Function Build Space around Ship
        public void MakeSpace(Cell[] ship, out Cell[] space)
        {
            Cell[] tempSpace = new Cell[((ship.Length * 2) + 6)];
            int spaceCounter = 0;
            // this loop buid space around each deck
            for (int i = 0; i < ship.Length; i++)
            {
                int startIndexY = ship[i].IndexY-1;     // 1 step left for start build space around deck
                int startIndexX = ship[i].IndexX-1;     // 1 step up
                //build space around 1 ship's deck
                for (int v = startIndexY; v < startIndexY+3; v++)
                {
                    if (v >= 0 && v < this.CellsInSide)     //check if it not out of board
                    {
                        for (int h = startIndexX; h < startIndexX + 3; h++)
                        {
                            if (h >= 0 && h < this.CellsInSide && !CellsBoard[v, h].IsDeck) //check if it not out of board & if it not Deck
                            {
                                // check if cell already exist as space for this ship (for prevent duplicates)
                                bool isExist = false;
                                for (int j = 0; j < spaceCounter; j++)
                                {
                                    if(CellsBoard[v, h].IndexX == tempSpace[j].IndexX && CellsBoard[v, h].IndexY == tempSpace[j].IndexY)
                                    {
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    tempSpace[spaceCounter] = CellsBoard[v, h];
                                    tempSpace[spaceCounter].IsSpace = true;
                                    spaceCounter++;
                                }
                            }
                        }

                    }
                }
            } 
            space = new Cell[spaceCounter];
            for (int i = 0; i < space.Length; i++)
            {
                space[i] = tempSpace[i];
            }
        }

        #endregion





    }
}
