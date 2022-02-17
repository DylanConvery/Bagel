using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bagel {
    public class Bagel : Game {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_sprite_batch;
        private GameObject cube;
        private Texture2D texture;
        private Color[] colorData;

        public Bagel() {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            texture = new Texture2D(GraphicsDevice, 100, 100);
            colorData = new Color[100 * 100];
            cube = new GameObject("cube");
            base.Initialize();
        }

        protected override void LoadContent() {
            m_sprite_batch = new SpriteBatch(GraphicsDevice);
            for (int i = 0; i < colorData.Length; i++) {
                colorData[i] = Color.Red;
            }
            texture.SetData(colorData);
            cube.AddComponent<Sprite>(texture);
        }

        protected override void Update(GameTime game_time) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            cube.Update(game_time);
            cube.Position += new Vector2(0.2f * (float)game_time.ElapsedGameTime.TotalMilliseconds, 0);
            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time) {
            GraphicsDevice.Clear(Color.Black);
            m_sprite_batch.Begin();
            cube.Draw(m_sprite_batch);
            m_sprite_batch.End();
            base.Draw(game_time);
        }
    }
}