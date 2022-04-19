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
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Diagnostics;

namespace Bagel
{
    public class Bagel : Game
    {
        private GraphicsDeviceManager graphicsDevice;
        private SpriteBatch spriteBatch;
        private int GAME_HEIGHT = 600;
        private int GAME_WIDTH = 800;

        private DefaultEcs.World world;

        private ISystem<float> updateSystem;
        private ISystem<float> renderSystem;

        private Entity player;
        private Entity building;

        private DebugView debug;

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

            #region world
            world = new DefaultEcs.World();

            world.Set(new tainicom.Aether.Physics2D.Dynamics.World());
            ref var physics_world = ref world.Get<tainicom.Aether.Physics2D.Dynamics.World>();

            debug = new DebugView(physics_world);
            debug.LoadContent(graphicsDevice.GraphicsDevice, Content);
            debug.DefaultShapeColor = Color.White;
            
            physics_world.Gravity = new Vector2(0, 100);

            var top = 0;
            var bottom = GAME_HEIGHT;
            var left = 0;
            var right = GAME_WIDTH;

            physics_world.CreateEdge(new Vector2(top, left), new Vector2(right, top));
            physics_world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom));
            physics_world.CreateEdge(new Vector2(top, left), new Vector2(left, bottom));
            physics_world.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom));
            #endregion

            #region player
            player = world.CreateEntity();

            player.Set<PlayerInput>();

            player.Set(new PlayerMovement
            {
                speed = 10000000.0f
            });

            player.Set(new Sprite
            {
                texture = Content.Load<Texture2D>("box"),
                layerIndex = 0.0f,
                color = Color.White,
                scale = new Vector2(3),
                source = new Rectangle(0, 0, 32, 32)
            });
            ref var player_sprite = ref player.Get<Sprite>();
            player_sprite.origin = new Vector2(player_sprite.texture.Width/2f, player_sprite.texture.Height/ 2f);

            player.Set<Physics>();
            ref var player_transform = ref player.Get<Physics>();
            //stupid way of doing this.
            player_transform.body = physics_world.CreateRectangle(32f * player_sprite.scale.X, 38f * player_sprite.scale.Y, 0.1f, new Vector2(40,540), 0f, BodyType.Dynamic);
            
            player_transform.body.LinearDamping = 8f;
            player_transform.body.FixedRotation = true;
            player_transform.body.AngularDamping = 8f;
            player_transform.body.Tag = player;
            #endregion

            #region building
            building = world.CreateEntity();

            building.Set(new Sprite
            {
                texture = Content.Load<Texture2D>("building"),
                layerIndex = 1.0f,
                color = Color.White,
                scale = new Vector2(1f),
                source = new Rectangle(0, 0, 800, 445)
            });

            building.Set(new Physics
            {
                body = world.Get<tainicom.Aether.Physics2D.Dynamics.World>().CreateRectangle(1f, 1f, 1f, Vector2.Zero, 0f, BodyType.Static)
            });

            ref var building_transform = ref building.Get<Physics>();
            building_transform.body.Enabled = false;
            #endregion

            #region systems
            updateSystem = new SequentialSystem<float>(
                new WorldPhysicsSystem(world),
                new GameSystem(world, Content),
                new PlayerInputSystem(world),
                new PlayerMovementSystem(world)
            );

            renderSystem = new SequentialSystem<float>(
                new DrawSystem(GraphicsDevice, spriteBatch, world),
                new AnimationSystem(world)
            );
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

            Matrix i = Matrix.CreateOrthographicOffCenter(0, 800, 600, 0, -1, 1);  //what I use on a 1920x1080 screen
            Matrix view = Matrix.CreateScale(1);

            debug.RenderDebugData(ref i, ref view);

        }
    }
}