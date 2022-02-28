using System;
using System.Linq;
using System.Collections.Generic;
using Bagel.Core.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bagel
{
    public abstract class Scene
    {
        protected Scene(ContentManager content_manager)
        {
            this.content_manager = content_manager;
            gameObjectManager = new GameObjectManager(this);
        }

        public virtual void Start() { }

        public virtual void Update(GameTime game_time)
        {
            gameObjectManager.Update(game_time);
        }

        public virtual void Draw(SpriteBatch sprite_batch)
        {
            gameObjectManager.Draw(sprite_batch);
        }

        public virtual void Destroy()
        {
            gameObjectManager.Clear();
        }

        protected ContentManager content_manager;
        protected GameObjectManager gameObjectManager;
    }
}