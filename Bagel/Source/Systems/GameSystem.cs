using Bagel;
using Messages;
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
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Systems
{
    public class GameSystem : ISystem<float>
    {
        private DefaultEcs.World world;

        private int score;
        private float spawn_timer = 3;

        private static Random random = new Random();
        private Queue<Entity> fires = new Queue<Entity>();

        private List<Body> bodies_to_disable = new List<Body>();

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

        public GameSystem(DefaultEcs.World world)
        {
            this.world = world;
            this.world.Subscribe<CatchToppingMessage>(On);
            LoadLevel();
        }

        public void Update(float deltaTime)
        {
            if ((spawn_timer -= deltaTime) < 0f)
            {
                spawn_timer = 3f;

                Vector2 random_position = spawn_positions[random.Next(0, spawn_positions.Length)];

                fires.Enqueue(CreateFire(random_position));
                CreateTopping(random_position);
            }

            if (fires.Count > 4)
            {
                fires.Dequeue().Dispose();
            }

            foreach (var body in bodies_to_disable)
            {
                body.Enabled = false;
            }

            if (bodies_to_disable.Count > 3)
            {
                ref var physics_world = ref world.Get<Physics_World>();
                foreach (var body in bodies_to_disable)
                {
                    physics_world.Remove(body);
                }
                bodies_to_disable.Clear();
            }
        }

        private void On(in CatchToppingMessage _)
        {
            ++score;
            Debug.WriteLine("Score: " + score);
        }

        private void On(in GroundHitMessage _)
        {
            ++score;
            Debug.WriteLine("Score: " + score);
        }


        public bool IsEnabled { get; set; } = true;

        public void Dispose() { }


        public void CreatePlayer()
        {
            ref var content = ref world.Get<ContentManager>();
            ref var graphicsDevice = ref world.Get<GraphicsDevice>();

            #region player
            Entity player = world.CreateEntity();
            player.Set<Player>();
            player.Set<PlayerInput>();
            player.Set(new PlayerMovement
            {
                speed = 10000000.0f
            });

            player.Set(new Sprite(content.Load<Texture2D>("Assets/Spritesheets/bagel_left_run_animation"), 0.3f, 2));
            player.Set(new Animation
            {
                frame_count = 4,
                frame_speed = 0.2f,
                source_rectangle = new Rectangle(0, 0, 32, 32),
                offset = 32,
                current_frame = 0
            });
            player.Set<Physics>();

            ref var player_physics = ref player.Get<Physics>();
            ref var player_sprite = ref player.Get<Sprite>();
            player_physics.body = world.Get<Physics_World>().CreateRectangle(
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
            player_physics.body.OnCollision += PlayerCollisionHandler;
            #endregion
        }

        //shouldn't be here
        private bool PlayerCollisionHandler(Fixture sender, Fixture other, Contact contact)
        {
            if (sender.Body.Tag == null && other.Body.Tag == null) return true;

            if (other.Body.Tag != null && ((Entity)other.Body.Tag).Has<Topping>())
            {
                if (!bodies_to_disable.Contains(other.Body))
                {
                    world.Publish<CatchToppingMessage>(default);
                    bodies_to_disable.Add(other.Body);
                    ((Entity)other.Body.Tag).Dispose();
                }
                return false;
            }

            return true;
        }

        private bool ToppingCollisionHandler(Fixture sender, Fixture other, Contact contact)
        {
            if (sender.Body.Tag == null && other.Body.Tag == null) return true;

            if (other.Body.Tag != null && ((Entity)other.Body.Tag).Has<Ground>())
            {
                if (!bodies_to_disable.Contains(other.Body))
                {
                    world.Publish<GroundHitMessage>(default);
                    bodies_to_disable.Add(sender.Body);
                    ((Entity)sender.Body.Tag).Dispose();
                }
                return false;
            }

            return true;
        }

        public void CreateTopping(Vector2 position)
        {
            ref var content = ref world.Get<ContentManager>();

            Texture2D[] topping_textures = new Texture2D[2];
            topping_textures[0] = content.Load<Texture2D>("Assets/Sprites/bacon");
            topping_textures[1] = content.Load<Texture2D>("Assets/Sprites/fried_egg");

            Entity topping = world.CreateEntity();
            topping.Set<Topping>();
            topping.Set(new Sprite(topping_textures[random.Next(topping_textures.Length)], 0.3f));
            topping.Set<Physics>();

            ref var topping_sprite = ref topping.Get<Sprite>();
            ref var topping_body = ref topping.Get<Physics>().body;

            topping_body = world.Get<Physics_World>().CreateRectangle(
                32f * topping_sprite.scale.X,
                38f * topping_sprite.scale.Y,
                0.1f,
                position,
                0f,
                BodyType.Dynamic
            );
            topping_body.FixedRotation = true;
            topping_body.Tag = topping;
            topping_body.OnCollision += ToppingCollisionHandler;
            world.Publish<NewToppingMessage>(default);
        }

        public Entity CreateFire(Vector2 position)
        {
            ref var content = ref world.Get<ContentManager>();

            Entity fire = world.CreateEntity();
            //better way would be to share the same texture rather than load it for each fire
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
            fire_physics.body.Position = position;
            fire_physics.body.Enabled = false;
            world.Publish<NewFireMessage>(default);
            return fire;
        }

        public void CreateCity()
        {
            ref var physics_world = ref world.Get<Physics_World>();
            ref var content = ref world.Get<ContentManager>();
            ref var graphicsDevice = ref world.Get<GraphicsDevice>();

            #region city
            Entity ground = world.CreateEntity();
            ground.Set<Physics>();
            ground.Set<Ground>();

            #region background
            Entity background = world.CreateEntity();
            background.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/sky"), 0, Color.White, 3));
            background.Set<Physics>();
            #endregion

            #region middle_ground
            Entity middle_ground = world.CreateEntity();
            middle_ground.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/buildings"), 0.1f, new Color(180, 180, 180), 3));
            middle_ground.Set<Physics>();
            #endregion

            #region foreground
            Entity foreground = world.CreateEntity();
            foreground.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/foreground"), 0.2f, Color.White, 3));
            foreground.Set<Physics>();
            #endregion

            //defines world edges
            physics_world.CreateEdge(new Vector2(0, 0), new Vector2(graphicsDevice.Viewport.Width, 0));
            physics_world.CreateEdge(new Vector2(graphicsDevice.Viewport.Width, 0), new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            physics_world.CreateEdge(new Vector2(0, 0), new Vector2(0, graphicsDevice.Viewport.Height));
            physics_world.CreateEdge(new Vector2(0, graphicsDevice.Viewport.Height), new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));

            ref var ground_physics = ref ground.Get<Physics>();
            ground_physics.body = physics_world.CreateRectangle(graphicsDevice.Viewport.Width, 20, 1f, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height - 20));
            ground_physics.body.Tag = ground;

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

            #endregion
        }

        public void LoadLevel()
        {
            CreatePlayer();
            CreateCity();
        }
    }
}
