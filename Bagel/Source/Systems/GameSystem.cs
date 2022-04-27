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
        private bool spawn_switch = true;

        private static Random random = new Random();
        private Vector2 random_position;

        private Entity scoreboard_lower;
        private Entity scoreboard_upper;

        private Queue<Entity> fires = new Queue<Entity>();

        private List<Body> bodies_to_disable = new List<Body>();

        //TODO: put in text file and load in
        private Vector2[] spawn_positions = new Vector2[] {
            new Vector2(0,467),
            new Vector2(24,467),
            new Vector2(49,467),
            new Vector2(73,467),
            new Vector2(0,515),
            new Vector2(24,515),
            new Vector2(49,515),
            new Vector2(73,515),
            new Vector2(0,563),
            new Vector2(24,563),
            new Vector2(49,563),
            new Vector2(73,563),
            new Vector2(0,611),
            new Vector2(24,611),
            new Vector2(49,611),
            new Vector2(73,611),
            new Vector2(312,254),
            new Vector2(391,254),
            new Vector2(474,254),
            new Vector2(312,314),
            new Vector2(391,314),
            new Vector2(474,314),
            new Vector2(312,377),
            new Vector2(391,377),
            new Vector2(474,377),
            new Vector2(312,437),
            new Vector2(391,437),
            new Vector2(474,437),
            new Vector2(312,254),
            new Vector2(391,254),
            new Vector2(474,254),
            new Vector2(312,500),
            new Vector2(391,500),
            new Vector2(474,500),
            new Vector2(312,563),
            new Vector2(391,563),
            new Vector2(474,563),
            new Vector2(312,623),
            new Vector2(391,623),
            new Vector2(474,623),
            new Vector2(108,443),
            new Vector2(144,443),
            new Vector2(240,443),
            new Vector2(276,443),
            new Vector2(108,503),
            new Vector2(144,503),
            new Vector2(240,503),
            new Vector2(276,503),
            new Vector2(108,563),
            new Vector2(144,563),
            new Vector2(240,563),
            new Vector2(276,563),
            new Vector2(798,467),
            new Vector2(822,467),
            new Vector2(847,467),
            new Vector2(871,467),
            new Vector2(798,515),
            new Vector2(822,515),
            new Vector2(847,515),
            new Vector2(871,515),
            new Vector2(798,563),
            new Vector2(822,563),
            new Vector2(847,563),
            new Vector2(871,563),
            new Vector2(798,611),
            new Vector2(822,611),
            new Vector2(847,611),
            new Vector2(871,611),
            new Vector2(909,443),
            new Vector2(945,443),
            new Vector2(1041,443),
            new Vector2(1077,443),
            new Vector2(909,503),
            new Vector2(945,503),
            new Vector2(1041,503),
            new Vector2(1077,503),
            new Vector2(909,563),
            new Vector2(945,563),
            new Vector2(1041,563),
            new Vector2(1077,563),
            new Vector2(1113,254),
            new Vector2(1192,254),
            new Vector2(1271,254),
            new Vector2(1113,314),
            new Vector2(1192,314),
            new Vector2(1271,314),
            new Vector2(1113,377),
            new Vector2(1192,377),
            new Vector2(1271,377),
            new Vector2(1113,437),
            new Vector2(1192,437),
            new Vector2(1271,437),
            new Vector2(1113,254),
            new Vector2(1192,254),
            new Vector2(1271,254),
            new Vector2(1113,500),
            new Vector2(1192,500),
            new Vector2(1271,500),
            new Vector2(1113,563),
            new Vector2(1192,563),
            new Vector2(1271,563),
            new Vector2(1113,623),
            new Vector2(1192,623),
            new Vector2(1271,623),
            new Vector2(525,551),
            new Vector2(693,551),
            new Vector2(573,599),
            new Vector2(738,599),
            new Vector2(1326,551),
            new Vector2(1494,551),
            new Vector2(1374,599),
        };

        public GameSystem(DefaultEcs.World world)
        {
            this.world = world;
            this.world.Subscribe<CatchToppingMessage>(On);
            initialize();
        }

        public void initialize()
        {
            random_position = spawn_positions[random.Next(0, spawn_positions.Length)];
            LoadLevel();
        }

        public void Update(float deltaTime)
        {
            if ((spawn_timer -= deltaTime) < 0f)
            {
                spawn_timer = random.Next(1,3);
                if (!spawn_switch) {
                    CreateTopping(random_position);
                    random_position = spawn_positions[random.Next(0, spawn_positions.Length)];
                }
                else
                {
                    fires.Enqueue(CreateFire(random_position));
                }
                spawn_switch = !spawn_switch;
            }

            if (fires.Count > 10)
            {
                fires.Dequeue().Dispose();
            }
            //TODO: bad and inefficient 
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
            scoreboard_lower.Get<Label>().text = score.ToString();
            scoreboard_upper.Get<Label>().text = score.ToString();
        }

        public bool IsEnabled { get; set; } = true;

        public void Dispose() { }

        //TODO: shouldn't be here, move to new class
        public void LoadLevel()
        {
            CreateScoreboard();
            CreatePlayer();
            CreateCity();
        }

        //TODO: Not great having to draw two Labels for the one thing
        public void CreateScoreboard()
        {
            ref var content = ref world.Get<ContentManager>();
            ref var graphicsDevice = ref world.Get<GraphicsDevice>();

            scoreboard_lower = world.CreateEntity();
            scoreboard_lower.Set(
                new Label(
                    content.Load<SpriteFont>("Assets/Fonts/8-bit Arcade Out"),
                    score.ToString(),
                    new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 3),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    1f,
                    0f
                )
            );
            ref var label = ref scoreboard_lower.Get<Label>();
            label.position -= label.font.MeasureString(label.text) * label.scale / 2;

            scoreboard_upper = world.CreateEntity();
            scoreboard_upper.Set(
                new Label(
                    content.Load<SpriteFont>("Assets/Fonts/8-bit Arcade In"),
                    score.ToString(),
                    new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 3),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    0f
                )
            );
            label = ref scoreboard_upper.Get<Label>();
            label.position -= label.font.MeasureString(label.text) * label.scale / 2;
        }

        //TODO: shouldn't be here, move to new class
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
                speed = 6000000.0f
            });

            player.Set(new Sprite(content.Load<Texture2D>("Assets/Spritesheets/bagel_sprite_atlas"), 0.3f, 2));
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
                new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height - 20 - 30),
                0f,
                BodyType.Dynamic
            );
            player_physics.body.LinearDamping = 8f;
            player_physics.body.FixedRotation = true;
            player_physics.body.Tag = player;
            player_physics.body.OnCollision += PlayerCollisionHandler;
            #endregion
        }

        //TODO: shouldn't be here, move to new class
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

        //TODO: shouldn't be here, move to new class
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

        //TODO: shouldn't be here, move to new class
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
            position = new Vector2(position.X + 11, position.Y - 11);

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
            topping_body.Mass = 1;
            topping_body.ApplyLinearImpulse(new Vector2(random.Next(-50, 50), -100));
            world.Publish<NewToppingMessage>(default);
        }

        //TODO: shouldn't be here, move to new class
        public Entity CreateFire(Vector2 position)
        {
            ref var content = ref world.Get<ContentManager>();

            Entity fire = world.CreateEntity();
            //better way would be to share the same texture rather than load it for each fire
            fire.Set(new Animation
            {
                frame_count = 4,
                frame_speed = 0.2f,
                source_rectangle = new Rectangle(0, 0, 8, 8),
                offset = 8,
                current_frame = 0
            });
            fire.Set(new Sprite(content.Load<Texture2D>("Assets/Spritesheets/fire_animation"), 0.2f, 3));
            fire.Set<Physics>();
            ref var fire_physics = ref fire.Get<Physics>();
            fire_physics.body = world.Get<Physics_World>().CreateBody();
            //TODO:bad, setting offset because of origin
            position = new Vector2(position.X + 11, position.Y - 11);
            fire_physics.body.Position = position;
            fire_physics.body.Enabled = false;
            world.Publish<NewFireMessage>(default);
            return fire;
        }

        //TODO: shouldn't be here, move to new class
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
    }
}
