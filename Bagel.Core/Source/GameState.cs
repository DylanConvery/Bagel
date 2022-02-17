using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bagel {
    public abstract class GameState {
        public abstract void LoadContent(ContentManager content_manager);
        public abstract void UnloadContent(ContentManager content_manager);
        public abstract void HandleInput();

        public event EventHandler<GameState> OnStateSwitched;

        protected void SwitchState(GameState game_state) => OnStateSwitched?.Invoke(this, game_state);
        protected void AddGameObject(GameObject game_object) { m_game_objects.Add(game_object); }

        private readonly List<GameObject> m_game_objects = new List<GameObject>();
    }
}