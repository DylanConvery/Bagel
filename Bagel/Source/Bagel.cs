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
using tainicom.Aether.Physics2D.Dynamics;
using World = DefaultEcs.World;

namespace Bagel
{
    public class Bagel : Game
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;
        private int GAME_HEIGHT = 600;
        private int GAME_WIDTH = 800;

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
            graphicsDevice.PreferredBackBufferWidth = GAME_WIDTH;
            graphicsDevice.PreferredBackBufferHeight = GAME_HEIGHT;
            graphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            graphicsDevice.ApplyChanges();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            #endregion 

            playerTexture = Content.Load<Texture2D>("bagel");
            buildingTexture = Content.Load<Texture2D>("building");

            world = new World();

            world.Set(new tainicom.Aether.Physics2D.Dynamics.World());
            ref var physics_world = ref world.Get<tainicom.Aether.Physics2D.Dynamics.World>();
            physics_world.Gravity = new Vector2(0, 0);

            var top = 0;
            var bottom = GAME_HEIGHT;
            var left = 0;
            var right = GAME_WIDTH;

            physics_world.CreateEdge(new Vector2(top,left), new Vector2(right, top));
            physics_world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom));
            physics_world.CreateEdge(new Vector2(top,left), new Vector2(left, bottom));
            physics_world.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom));

            #region player
            player = world.CreateEntity();

            player.Set<PlayerInputComponent>();

            player.Set(new PlayerMovementComponent
            {
                speed = 2000000.0f
            });

            player.Set(new SpriteComponent
            {
                texture = playerTexture,
                layerIndex = 0.0f,
                color = Color.White,
                origin = Vector2.Zero,
                scale = new Vector2(0.15f)
            });

            player.Set<TransformComponent>();
            ref var player_transform = ref player.Get<TransformComponent>();
            player_transform.body = physics_world.CreateBody(new Vector2(50,400), 0, BodyType.Dynamic);
            player_transform.body.CreateRectangle(8f, 8f, 0f, Vector2.Zero);
            player_transform.body.LinearDamping = 80f;
            player_transform.body.AngularDamping = 80f;

            //player_transform.body.Tag = player;
            #endregion

            #region npc
            npc = world.CreateEntity();

            npc.Set<AIComponent>();

            npc.Set(new SpriteComponent
            {
                texture = playerTexture,
                layerIndex = 0.0f,
                color = Color.White,
                scale = new Vector2(0.1f)

            });

            npc.Set<TransformComponent>();
            ref var npc_transform = ref npc.Get<TransformComponent>();
            npc_transform.body = world.Get<tainicom.Aether.Physics2D.Dynamics.World>().CreateRectangle(1f, 1f, 1f, new Vector2(100, 200), 0f, BodyType.Dynamic);
            //npc_transform.body.Tag = npc;
            #endregion

            #region building
            building = world.CreateEntity();

            building.Set(new SpriteComponent
            {
                texture = buildingTexture,
                layerIndex = 1.0f,
                color = Color.White,
                scale = new Vector2(1f)

            });

            building.Set(new TransformComponent
            {
                body = world.Get<tainicom.Aether.Physics2D.Dynamics.World>().CreateRectangle(1f, 1f, 1f, Vector2.Zero, 0f, BodyType.Static)
            });

            #endregion


            updateSystem = new SequentialSystem<float>(
                new WorldPhysicsSystem(world),
                new PlayerInputSystem(world),
                new PlayerMovementSystem(world)
            );

            renderSystem = new SequentialSystem<float>(
                new DrawSystem(spriteBatch, world)
            );

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