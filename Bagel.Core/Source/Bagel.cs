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

        protected override void LoadContent() { m_sprite_batch = new SpriteBatch(GraphicsDevice); }

        protected override void Update(GameTime game_time) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time) {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(game_time);
        }
    }
}