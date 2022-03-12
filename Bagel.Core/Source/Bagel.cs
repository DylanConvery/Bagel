using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DefaultEcs;
using DefaultEcs.System;

namespace Bagel {
    public class Bagel : Game {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_sprite_batch;
        private World m_world;
        private ISystem<SpriteBatch> m_drawSystem;

        public Bagel() {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //removes fixed timestep
            m_graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            m_graphics.GraphicsProfile = GraphicsProfile.HiDef;
            m_graphics.ApplyChanges();
        }

        protected   override void Initialize() {
            m_world = new World();


            base.Initialize();
        }

        protected override void LoadContent() {
            m_sprite_batch = new SpriteBatch(GraphicsDevice);
        }

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