using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bagel {
    public class Bagel : Game {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_sprite_batch;

        public Bagel() {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true; 
        }

        protected override void Initialize() { base.Initialize(); }

        protected override void LoadContent() {
            m_sprite_batch = new SpriteBatch(GraphicsDevice);
            testECS();
        }

        private void testECS() {
            GameObject game = new GameObject("game");
            game.AddComponent<Sprite>();
            Component sprite = new Sprite(game);
            game.AddComponent(sprite);
            game.HasComponent<Sprite>();
            game.RemoveComponent<Sprite>();
            game.RemoveComponent<Sprite>();
            game.RemoveComponent<Sprite>();
            game.HasComponent<Sprite>();
            game.enabled = false;
            game.RemoveAllComponents();
            game.AddComponent<Sprite>();
            game.layer_index = 5;
            game.AddComponent(sprite);
            game.HasComponent<Sprite>();
            game.RemoveComponent<Sprite>();
            game.enabled = false;

        }

        protected override void Update(GameTime game_time) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(game_time);
        }
    }
}