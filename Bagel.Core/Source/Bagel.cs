using System;
using Bagel.Core.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bagel {
    public class Bagel : Game {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_sprite_batch;

        private RenderTarget2D m_render_target;
        private Rectangle m_render_scale_rectangle;
        private Scene m_scene;

        private const int m_desired_width = 1125;
        private const int m_desired_height = 2436;
        private const float m_desired_aspect_ratio = m_desired_width / (float)m_desired_height;

        public Bagel() {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            m_graphics.PreferredBackBufferWidth = m_desired_width / 2;
            m_graphics.PreferredBackBufferHeight = m_desired_height / 2;
            m_graphics.ApplyChanges();
            m_scene = new GameplayScene(Content);
            m_render_target = new RenderTarget2D(
                GraphicsDevice,
                m_desired_width,
                m_desired_height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents
            );
            m_render_scale_rectangle = GetScaleRectangle();
            base.Initialize();
        }

        private Rectangle GetScaleRectangle() {
            var variance = 0.5f;
            var actual_aspect_ratio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            Rectangle scale_rectangle;
            if (actual_aspect_ratio <= m_desired_aspect_ratio) {
                var present_height = (int)(Window.ClientBounds.Width / m_desired_aspect_ratio + variance);
                var bar_height = (Window.ClientBounds.Height - present_height) / 2;
                scale_rectangle = new Rectangle(0, bar_height, Window.ClientBounds.Width, present_height);
            } else {
                var present_width = (int)(Window.ClientBounds.Height * m_desired_aspect_ratio + variance);
                var bar_width = (Window.ClientBounds.Width - present_width) / 2;
                scale_rectangle = new Rectangle(bar_width, 0, present_width, Window.ClientBounds.Height);
            }
            return scale_rectangle;
        }

        protected override void LoadContent() {
            m_sprite_batch = new SpriteBatch(GraphicsDevice);
            m_scene.Start();
        }

        protected override void Update(GameTime game_time) {
            m_scene.Update(game_time);
            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time) {
            GraphicsDevice.SetRenderTarget(m_render_target);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            m_sprite_batch.Begin();
            m_scene.Draw(m_sprite_batch);
            m_sprite_batch.End();
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
            m_sprite_batch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            m_sprite_batch.Draw(m_render_target, m_render_scale_rectangle, Color.White);
            m_sprite_batch.End();
            base.Draw(game_time);
        }

        protected override void UnloadContent() {
            m_scene.Destroy();
            base.UnloadContent();
        }
    }
}