using Components;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using Physics_World = tainicom.Aether.Physics2D.Dynamics.World;

namespace Systems
{
    public class GameSystem : ISystem<float>
    {
        private DefaultEcs.World world;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;

        private float spawn_timer = 5;
        private static Random random;

        private Entity player;
        private Entity ground;
        private Entity foreground;
        private Entity middle_ground;
        private Entity background;

        private Texture2D[] topping_textures = new Texture2D[2];

        private Vector2[] spawn_positions = new Vector2[] {
            new Vector2(329,239),
            new Vector2(842,457),
            new Vector2(1052,428),
            new Vector2(1145,361),
            new Vector2(1235,241),
            new Vector2(1413,589),
            new Vector2(600,638),
            new Vector2(351,490),
            new Vector2(253,425),
            new Vector2(8,455),
            new Vector2(69,552),
            new Vector2(283,611)
        };

        public GameSystem(DefaultEcs.World world, ContentManager content, GraphicsDevice graphics)
        {
            this.world = world;
            this.content = content;
            this.graphicsDevice = graphics;
            random = new Random();
            Initialize();
            CreateFixures();
        }

        public void Initialize()
        {
            #region city
            ground = world.CreateEntity();
            ground.Set<Physics>();

            #region background
            background = world.CreateEntity();
            background.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/sky"), 0, Color.White, 3));
            background.Set<Physics>();
            #endregion

            #region middle_ground
            middle_ground = world.CreateEntity();
            middle_ground.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/buildings"), 0.1f, new Color(180, 180, 180), 3));
            middle_ground.Set<Physics>();
            #endregion

            #region foreground
            foreground = world.CreateEntity();
            foreground.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/foreground"), 0.2f, Color.White, 3));
            foreground.Set<Physics>();
            #endregion
            #endregion

            #region player
            player = world.CreateEntity();
            player.Set<PlayerInput>();
            player.Set(new PlayerMovement
            {
                speed = 10000000.0f
            });

            player.Set(new Sprite(content.Load<Texture2D>("Assets/Spritesheets/bagel_idle_animation"), 0.3f, 2));
            player.Set(new Animation
            {
                frame_count = 4,
                frame_speed = 0.2f,
                source_rectangle = new Rectangle(0, 0, 32, 32),
                offset = 32,
                current_frame = 0
            });
            player.Set<Physics>();

            topping_textures[0] = content.Load<Texture2D>("Assets/Sprites/bacon");
            topping_textures[1] = content.Load<Texture2D>("Assets/Sprites/fried_egg");
            #endregion
        }

        public void CreateFixures()
        {
            ref var physics_world = ref world.Get<Physics_World>();

            //defines world edges
            physics_world.CreateEdge(new Vector2(0, 0), new Vector2(graphicsDevice.Viewport.Width, 0));
            physics_world.CreateEdge(new Vector2(graphicsDevice.Viewport.Width, 0), new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            physics_world.CreateEdge(new Vector2(0, 0), new Vector2(0, graphicsDevice.Viewport.Height));
            physics_world.CreateEdge(new Vector2(0, graphicsDevice.Viewport.Height), new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));

            ref var ground_physics = ref ground.Get<Physics>();
            ground_physics.body = physics_world.CreateRectangle(graphicsDevice.Viewport.Width, 20, 1f, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height - 20));

            ref var foreground_physics = ref foreground.Get<Physics>();
            foreground_physics.body = physics_world.CreateBody();
            foreground_physics.body.Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            foreground_physics.body.Enabled = false;

            ref var middle_ground_physics = ref middle_ground.Get<Physics>();
            middle_ground_physics.body = physics_world.CreateBody();
            middle_ground_physics.body.Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            middle_ground_physics.body.Enabled = false;

            ref var background_physics = ref background.Get<Physics>();
            background_physics.body = physics_world.CreateBody();
            background_physics.body.Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            background_physics.body.Enabled = false;

            ref var player_physics = ref player.Get<Physics>();
            ref var player_sprite = ref player.Get<Sprite>();
            player_physics.body = physics_world.CreateRectangle(
                32f,
                38f,
                1f,
                new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height - 20 - player_sprite.texture.Height),
                0f,
                BodyType.Dynamic
            );
            player_physics.body.LinearDamping = 8f;
            player_physics.body.FixedRotation = true;
            player_physics.body.Tag = player;
        }

        public bool IsEnabled { get; set; } = true;

        public void Dispose()
        {
        }

        private Texture2D GetRandomTopping()
        {
            return topping_textures[random.Next(topping_textures.Length)];
        }

        public void Update(float deltaTime)
        {
            if ((spawn_timer -= deltaTime) < 0f)
            {
                spawn_timer = 3f;
                
                Vector2 random_position = spawn_positions[random.Next(0, spawn_positions.Length)];

                Entity fire = world.CreateEntity();
                fire.Set(new Sprite(content.Load<Texture2D>("Assets/Spritesheets/fire_animation"), 0.3f, 3));
                fire.Set(new Animation
                {
                    frame_count = 4,
                    frame_speed = 0.2f,
                    source_rectangle = new Rectangle(0, 0, 8, 8),
                    offset = 8,
                    current_frame = 0
                });
                fire.Set<Physics>(); 
                ref var fire_physics = ref fire.Get<Physics>();
                fire_physics.body = world.Get<Physics_World>().CreateBody();
                fire_physics.body.Position = random_position;
                fire_physics.body.Enabled = false;

                Entity topping = world.CreateEntity();
                topping.Set<topping>();
                topping.Set(new Sprite(GetRandomTopping(), 0.3f));
                topping.Set<Physics>();

                ref var topping_sprite = ref topping.Get<Sprite>();
                ref var topping_body = ref topping.Get<Physics>().body;

                topping_body = world.Get<Physics_World>().CreateRectangle(
                    32f * topping_sprite.scale.X,
                    38f * topping_sprite.scale.Y,
                    0.1f,
                    random_position,
                    0f,
                    BodyType.Dynamic
                );
                topping_body.FixedRotation = true;
                topping_body.Tag = topping;
            }
        }
    }
}
