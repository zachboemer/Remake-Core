﻿using System;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace SubterfugeCore.Shared.Content.Game.Objects.Base
{
    abstract class GameObject
    {
        protected Vector2 position;
        public GameObject()
        {

        }

        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public void setPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }

    }
}
