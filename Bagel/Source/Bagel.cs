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
using System.Diagnostics;

namespace Bagel
{
    public class Bagel : Game
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;

        private World world;
        private ISystem<float> updateSystem;
        private ISystem<float> renderSystem;

        private Texture2D playerTexture;
        private Texture2D buildingTexture;
        private Entity player;
        private Entity building;
        private Entity npc;

        public Bagel()
        {
            #region MonoGame
            graphicsDevice = new GraphicsDeviceManager(this);
            graphicsDevice.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            graphicsDevice.PreferredBackBufferWidth = 800;
            graphicsDevice.PreferredBackBufferHeight = 600;
            graphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            graphicsDevice.ApplyChanges();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            #endregion 

            playerTexture = Content.Load<Texture2D>("bagel");
            buildingTexture = Content.Load<Texture2D>("building");

            world = new World();

            updateSystem = new SequentialSystem<float>(
                new PlayerInputSystem(world),
                new PlayerMovementSystem(world)
            );

            renderSystem = new SequentialSystem<float>(
                new DrawSystem(spriteBatch, world)
            );

            #region player
            player = world.CreateEntity();
            player.Set<PlayerInputComponent>();
            player.Set<RigidbodyComponent>();
            player.Set(new PlayerMovementComponent
            {
                speed = 2000.0f
            });
            player.Set(new SpriteComponent
            {
                texture = playerTexture,
                layerIndex = 0.0f,
                color = Color.White
            });
            player.Set(new TransformComponent
            {
                position = new Vector2(50, 400),
                rotation = 0,
                scale = new Vector2(0.15f)
            });
            #endregion

            #region npc
            npc = world.CreateEntity();
            npc.Set<AIComponent>();
            npc.Set(new SpriteComponent
            {
                texture = playerTexture,
                layerIndex = 0.0f,
                color = Color.White
            });
            npc.Set(new TransformComponent
            {
                position = new Vector2(100, 200),
                rotation = 0,
                scale = new Vector2(0.1f)
            });
            #endregion

            #region building
            building = world.CreateEntity();
            building.Set(new SpriteComponent
            {
                texture = buildingTexture,
                layerIndex = 1.0f,
                color = Color.White

            });
            building.Set(new TransformComponent
            {
                position = Vector2.Zero,
                rotation = 0,
                scale = new Vector2(1, 1)
            });

            #endregion
        }

        protected override void Update(GameTime game_time)
        {
            updateSystem.Update((float)game_time.ElapsedGameTime.TotalSeconds);
        }

        protected override void Draw(GameTime game_time)
        {
            GraphicsDevice.Clear(Color.Black);
            renderSystem.Update((float)game_time.ElapsedGameTime.TotalSeconds);
        }
    }
}