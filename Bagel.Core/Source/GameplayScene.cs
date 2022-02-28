using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bagel.Core.Source
{
    public class GameplayScene : Scene
    {
        public GameplayScene(ContentManager content_manager) : base(content_manager) { }

        GameObject player;
        Texture2D player_texture;

        public override void Start()
        {
            player_texture = content_manager.Load<Texture2D>("bagel");
            player = gameObjectManager.Add("player");
            player.AddComponent<Sprite>(player_texture);
        }

        public override void Update(GameTime game_time)
        {
            player.Position += new Vector2(0.2f * (float)game_time.ElapsedGameTime.TotalMilliseconds, 0);
            base.Update(game_time);
        }
    }
}
