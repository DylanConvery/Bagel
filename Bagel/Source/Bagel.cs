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
using Physics_World = tainicom.Aether.Physics2D.Dynamics.World;
using Microsoft.Xna.Framework.Content;
using Messages;

#if DEBUG
using tainicom.Aether.Physics2D.Diagnostics;
#endif

namespace Bagel
{
    public class Bagel : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static int SCALE = 3;
        //TODO: Fix scaling 
        private int WINDOW_HEIGHT = 256 * SCALE;
        private int WINDOW_WIDTH = 512 * SCALE;

        private DefaultEcs.World world;

        private ISystem<float> updateSystem;
        private ISystem<float> renderSystem;

#if DEBUG
        private DebugView debug;
#endif

        public Bagel()
        {
            #region MonoGame
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            #endregion
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region world
            world = new DefaultEcs.World(1000);
            world.Set(new Physics_World(new Vector2(0, 100)));
            world.Set(Content);
            world.Set(graphics.GraphicsDevice);
            world.Subscribe<GroundHitMessage>(On);
            #endregion

#if DEBUG
            #region aether
            ref var physics_world = ref world.Get<tainicom.Aether.Physics2D.Dynamics.World>();
            debug = new DebugView(physics_world);
            debug.LoadContent(graphics.GraphicsDevice, Content);
            debug.DefaultShapeColor = Color.White;
            #endregion
#endif

            #region systems
            updateSystem = new SequentialSystem<float>(
                new WorldPhysicsSystem(world),
                new GameSystem(world),
                new PlayerInputSystem(world),
                new PlayerMovementSystem(world)
            );

            renderSystem = new SequentialSystem<float>(
                new AnimationSystem(world),
                new DrawSystem(world, spriteBatch),
                new LabelSystem(world, spriteBatch)
            );
            #endregion

            base.LoadContent();
        }

        private void On(in GroundHitMessage _)
        {
            Initialize();
        }

        protected override void Update(GameTime game_time)
        {
            updateSystem.Update((float)game_time.ElapsedGameTime.TotalSeconds);
            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time)
        {
            GraphicsDevice.Clear(Color.Black);
            renderSystem.Update((float)game_time.ElapsedGameTime.TotalSeconds);

#if DEBUG
            Matrix i = Matrix.CreateOrthographicOffCenter(0, WINDOW_WIDTH, WINDOW_HEIGHT, 0, -1, 1);
            Matrix view = Matrix.CreateScale(1);
            debug.RenderDebugData(ref i, ref view);
#endif

            base.Draw(game_time);
        }
    }
}
