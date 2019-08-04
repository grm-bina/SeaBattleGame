using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleGame
{
    class AutoShooter
    {
        private GameManager _manager;
        private Player _aimboard;
        private Random rnd = new Random();
        private Cell _firstHit;
        private Cell _lastHit;
        private Cell _lastMove;
        private char _direction; // 'v' - vertical, 'h' - horisontal, 'u' - unknown

        // Constructor
        public AutoShooter(GameManager manager, Player aimBoard)
        {
            _manager = manager;
            _aimboard = aimBoard;
            _direction = 'u';
        }

        // Function Search Ship (if deck was shooted)
        private void SearchShip(out int indexY, out int indexX)
        {
            indexY = _lastHit.IndexY;
            indexX = _lastHit.IndexX;

            for (int i = 0; i < 2; i++)
            {
                // search around last hit
                if (indexY - 1 >= 0 && !_aimboard.CellsBoard[indexY - 1, indexX].IsShot && (_direction=='u' || _direction=='v')) // search Up
                {
                    indexY--;
                    break;
                }
                else if (indexY + 1 < _aimboard.CellsInSide && !_aimboard.CellsBoard[indexY + 1, indexX].IsShot && (_direction == 'u' || _direction == 'v')) // search Down
                {
                    indexY++;
                    break;
                }
                else if (indexX + 1 < _aimboard.CellsInSide && !_aimboard.CellsBoard[indexY, indexX + 1].IsShot && (_direction == 'u' || _direction == 'h')) // search Right
                {
                    indexX++;
                    break;
                }
                else if (indexX - 1 >= 0 && !_aimboard.CellsBoard[indexY, indexX - 1].IsShot && (_direction == 'u' || _direction == 'h')) // search Left
                {
                    indexX--;
                    break;
                }
                else
                {
                    // search around first hit
                    indexY = _firstHit.IndexY;
                    indexX = _firstHit.IndexX;
                }
            }
        }

        // Function get feedback from game-manager, update first&last-hit cells & initiate enemy's move
        public void EnemyMove(string feedback)
        {
            // update data about first&last-hit
            if (feedback == "hit")
            {
                if (_firstHit == null)
                {
                    _firstHit = _lastMove;
                    _lastHit = _lastMove;
                }
                else
                {
                    _lastHit = _lastMove;
                    if (_firstHit.IndexY == _lastHit.IndexY)
                        _direction = 'h';
                    else if (_firstHit.IndexX == _lastHit.IndexX)
                        _direction = 'v';
                }

            }
            else if (feedback == "ship destroyed")
            {
                _firstHit = null;
                _lastHit = null;
                _lastMove = null;
                _direction = 'u';
            }

            // choice aim & save it
            int aimIndexY;
            int aimIndexX;
            if (_firstHit != null && _lastHit != null)
                SearchShip(out aimIndexY, out aimIndexX);
            else
            {
                aimIndexY = rnd.Next(10);
                aimIndexX = rnd.Next(10);
            }
            _lastMove = _aimboard.CellsBoard[aimIndexY, aimIndexX];

            // shoot!
            _manager.Move(aimIndexY, aimIndexX, _aimboard);
        }

    }
}
