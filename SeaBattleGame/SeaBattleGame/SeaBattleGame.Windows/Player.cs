using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SeaBattleGame
{
    class Player : Board
    {
        protected Ship[] _playerNavy;
        private int _shipCounter;

        // Constructor
        public Player(Canvas playerCanvas):base(playerCanvas)
        {
            
            // Place Navy (10 ships)
            _playerNavy = new Ship[10];
            _shipCounter = 0;
            for (int i = 1; i <=4; i++)
            {
                for (int j = 4; j >= i; j--)
                {
                    _playerNavy[_shipCounter] = new Ship(i, playerCanvas, this, _shipCounter);
                    _shipCounter++;
                }
            }
        }

        // Property get number of ships (alive) in the navy
        public int ShipsNum
        {
            get
            {
                return _shipCounter;
            }
        }

        #region Functions
        // Function check board and navy's condition after other player's Fire
        public string UnderFire(int indexY, int indexX)
        {
            if (CellsBoard[indexY, indexX].IsShot) // if already shooted cell
                return "again";
            else
            {
                if (CellsBoard[indexY, indexX].Fire()) // if hit to deck
                {
                    if (!_playerNavy[(CellsBoard[indexY, indexX].ShipIndex)].IsAlive)
                    {
                        DestroyShip(_playerNavy[(CellsBoard[indexY, indexX].ShipIndex)]);
                        if (_shipCounter == 0)
                            return "navy destroyed";
                        else return "ship destroyed";
                    }
                    else return "hit";
                }
                else return "miss"; // in the game-manager Move will be turn to THIS player
            }
        }

        // Function Destroy Ship 
        protected virtual void DestroyShip(Ship destroyingShip)
        {
            destroyingShip.SpaceToShot();
            _shipCounter--;
        }
        #endregion

    }
}
