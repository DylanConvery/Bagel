using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DefaultEcs;
using DefaultEcs.System;
using System.IO;
using Components;
using Systems;
using DefaultEcs.Threading;

namespace Bagel
{
    public class Bagel : Game
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_sprite_batch;

        private World m_world;
        private ISystem<float> m_update_system;
        private ISystem<float> m_render_system;

        private Texture2D m_player_texture;
        private Texture2D m_building_texture;
        private Entity m_player;
        private Entity m_building;
        private Entity m_npc;

        public Bagel()
        {
            #region MonoGame
            m_graphics = new GraphicsDeviceManager(this);
            m_graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            m_graphics.PreferredBackBufferWidth = 800;
            m_graphics.PreferredBackBufferHeight = 600;
            m_graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            m_graphics.ApplyChanges();
            m_sprite_batch = new SpriteBatch(GraphicsDevice);
            #endregion 

            m_player_texture = Content.Load<Texture2D>("bagel");
            m_building_texture = Content.Load<Texture2D>("building");

            m_world = new World();

            m_update_system = new SequentialSystem<float>(
                new PlayerInputSystem(m_world)
            );

            m_render_system = new SequentialSystem<float>(
                new DrawSystem(m_sprite_batch, m_world)
            );

            #region player
            m_player = m_world.CreateEntity();
            m_player.Set(new PlayerComponent
            {
                speed = 4
            });
            m_player.Set(new SpriteComponent
            {
                texture = m_player_texture,
                layer_index = 0.0f,
                color = Color.White
            });
            m_player.Set(new TransformComponent
            {
                position = new Vector2(50,400),
                rotation = 0,
                scale = new Vector2(0.15f)
            });
            #endregion

            #region npc
            m_npc = m_world.CreateEntity();
            m_npc.Set<AIComponent>();
            m_npc.Set(new SpriteComponent
            {
                texture = m_player_texture,
                layer_index = 0.0f,
                color = Color.White
            });
            m_npc.Set(new TransformComponent
            {
                position = new Vector2(100,200),
                rotation = 0,
                scale = new Vector2(0.1f)
            });
            #endregion

            #region building
            m_building = m_world.CreateEntity();
            m_building.Set(new SpriteComponent
            {
                texture = m_building_texture,
                layer_index = 1.0f,
                color = Color.White

            });
            m_building.Set(new TransformComponent
            {
                position = Vector2.Zero,
                rotation = 0,
                scale = new Vector2(1, 1)
            });

            #endregion
        }

        protected override void Update(GameTime game_time)
        {
            m_update_system.Update((float)game_time.ElapsedGameTime.TotalSeconds);
        }

        protected override void Draw(GameTime game_time)
        {
            GraphicsDevice.Clear(Color.Black);
            m_render_system.Update((float)game_time.ElapsedGameTime.TotalSeconds);
        }
    }
}