using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Monogame_project
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Stopped
    }
    internal class Player
    {
        Texture2D _texture;
        List<Texture2D> _textures;
        private Rectangle _location;
        private int _textureState;
        private int _count;
        private Vector2 _speed;
        private Vector2 _locationVector;
        private Direction _direction;
        private float _speedMultiplier;
        private Texture2D _rectangleTexture;
        private int _xCurrent, _yCurrent;
        private int _xChange, _yChange;
        private Vector2 _target;
        private Random gen = new Random();
        public Box[,] _game { get; set; }
        public Player(List<Texture2D> textures, Vector2 locationVector, Rectangle location,float speed, Texture2D rectangleTexture,int x, int y)
        {
            _textures = textures;
            _texture = _textures[0];
            _locationVector = locationVector;
            _target = locationVector;
            _location = location;
            _location.X = (int)Math.Round(_locationVector.X);
            _location.Y = (int)Math.Round(_locationVector.Y);
            _textureState = 0;
            _speed = Vector2.Zero;
            _count = 1;
            _direction = Direction.Stopped;
            _speedMultiplier = speed;
            _rectangleTexture = rectangleTexture;
            _xCurrent = x; _yCurrent = y;
        }
        private void ShiftRight()
        {
            _target = _game[_xCurrent + 1, _yCurrent].Position;
            _speed = _game[_xCurrent + 1, _yCurrent].Position - _game[_xCurrent, _yCurrent].Position;
            _speed.Normalize();
            _speed *= _speedMultiplier;
            _xChange -= 1;
            _xCurrent += 1;
        }
        private void ShiftLeft()
        {
            _target = _game[_xCurrent - 1, _yCurrent].Position;
            _speed = _game[_xCurrent - 1, _yCurrent].Position - _game[_xCurrent, _yCurrent].Position;
            _speed.Normalize();
            _speed *= _speedMultiplier;
            _xChange += 1;
            _xCurrent -= 1;
        }
        private void ShiftDown()
        {
            _target = _game[_xCurrent , _yCurrent+1].Position;
            _speed = _game[_xCurrent , _yCurrent+1].Position - _game[_xCurrent, _yCurrent].Position;
            _speed.Normalize();
            _speed *= _speedMultiplier;
            _yChange -= 1;
            _yCurrent += 1;
        }
        private void ShiftUp()
        {
            _target = _game[_xCurrent , _yCurrent - 1].Position;
            _speed = _game[_xCurrent , _yCurrent-1].Position - _game[_xCurrent, _yCurrent].Position;
            _speed.Normalize();
            _speed *= _speedMultiplier;
            _yChange += 1;
            _yCurrent -= 1;
        }
        public void UpdateLocation(int x, int y)
        {
            _xChange = x - _xCurrent;
            _yChange = y - _yCurrent;
        }
        public void Move()
        {
            if (_xChange == 0 && _yChange == 0)
            {
                _direction = Direction.Stopped;
            }
            else if (Math.Abs(_xChange) >= Math.Abs(_yChange))
            {
                MoveH();
            }
            else
            {
                MoveV();
            }
        }
        private void MoveH()
        {
            if (_xChange > 0)
            {
                if (_game[_xCurrent + 1, _yCurrent].AbleToPass)
                {
                    _direction = Direction.Right;
                    ShiftRight();
                }
                else if (_yChange == 0)
                {
                    _direction = Direction.Down;
                    ShiftDown();
                }
                else 
                {
                    MoveV() ;
                }
            }
            else if (_xChange < 0)
            {
                if (_game[_xCurrent - 1, _yCurrent].AbleToPass)
                {
                    _direction = Direction.Left;
                    ShiftLeft();
                }
                else if (_yChange == 0)
                {
                        _direction = Direction.Up;
                        ShiftUp();
                }
                else
                {
                    MoveV();
                }
            }
        }
        private void MoveV()
        {
            if (_yChange > 0)
            {
                if (_game[_xCurrent, _yCurrent+1].AbleToPass)
                {
                    _direction = Direction.Down;
                    ShiftDown();
                }
                else if (_xChange == 0)
                {
                    /*if (gen.Next(0, 2) == 0)
                    {
                        _direction = Direction.Right;
                        ShiftRight();
                    }
                    else
                    {
                        _direction = Direction.Left;
                        ShiftLeft();
                    }*/
                    _direction = Direction.Right;
                    ShiftRight();
                }
                else
                {
                    MoveH();
                }
            }
            else if (_yChange < 0)
            {
                if (_game[_xCurrent, _yCurrent - 1].AbleToPass)
                {
                    _direction = Direction.Up;
                    ShiftUp();
                }
                else if (_xChange == 0)
                {
                        _direction = Direction.Left;
                        ShiftLeft();
                }
                else
                {
                    MoveH();
                }
            }
        }
        public void Update()
        {
            if (_locationVector == _target)
            {
                _speed = Vector2.Zero;
                _direction = Direction.Stopped;
                Move();
            }

            _locationVector += _speed;
            _location.X = (int)Math.Round(_locationVector.X);
            _location.Y = (int)Math.Round(_locationVector.Y);

            _count++;
            if (_count == 8)
            {
                _count = 1;
                _textureState++;
                if (_direction == Direction.Left)
                {
                    if (!(_textureState >= 8 && _textureState <= 11))
                    {
                        _textureState = 8;
                    }
                }
                else if (_direction == Direction.Right)
                {
                    if (!(_textureState >= 12 && _textureState <= 15))
                    {
                        _textureState = 12;
                    }
                }
                else if (_direction == Direction.Up)
                {
                    if (!(_textureState >= 4 && _textureState <= 7))
                    {
                        _textureState = 4;
                    }
                }
                else if (_direction == Direction.Down)
                {
                    if (!(_textureState >= 0 && _textureState <= 3))
                    {
                        _textureState = 0;
                    }
                }
                else if (_direction == Direction.Stopped)
                {
                    _textureState = 0;
                }

            }

            _texture = _textures[_textureState];
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_rectangleTexture, Hitbox, Color.Red);
            Rectangle drawRect = new Rectangle(_location.Center.X-30, _location.Center.Y-45, 60, 90);
            //spriteBatch.Draw(_rectangleTexture, drawRect, Color.White);
            spriteBatch.Draw(_texture, drawRect, Color.White);

        }
    }
}
