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
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private Physics_World physicsWorld;

        private int score;
        private float spawn_timer = 3;
        private bool spawn_switch = true;

        private static Random random = new Random();
        private Vector2 random_position;

        private Entity scoreboard_lower;
        private Entity scoreboard_upper;

        private Queue<Entity> fires = new Queue<Entity>();
        Texture2D[] topping_textures = new Texture2D[2];
        private Texture2D buildings_texture;
        private Texture2D fire_texture;

        private List<Entity> entities_to_remove = new List<Entity>();

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

            content = world.Get<ContentManager>();
            graphicsDevice = world.Get<GraphicsDevice>();
            physicsWorld = world.Get<Physics_World>();

            random_position = spawn_positions[random.Next(0, spawn_positions.Length)];
            topping_textures[0] = content.Load<Texture2D>("Assets/Sprites/bacon");
            topping_textures[1] = content.Load<Texture2D>("Assets/Sprites/fried_egg");
            buildings_texture = content.Load<Texture2D>("Assets/Sprites/buildings");
            fire_texture = content.Load<Texture2D>("Assets/Spritesheets/fire_animation");

            LoadLevel();
        }

        public void Update(float deltaTime)
        {
            if ((spawn_timer -= deltaTime) < 0f)
            {
                if (!spawn_switch)
                {
                    CreateTopping(random_position);
                    random_position = spawn_positions[random.Next(0, spawn_positions.Length)];
                }
                else
                {
                    fires.Enqueue(CreateFire(random_position));
                    if (fires.Count > 8)
                    {
                        entities_to_remove.Add(fires.Dequeue());
                    }
                }

                spawn_timer = random.Next(1, 3);
                spawn_switch = !spawn_switch;
            }

            foreach (var entity in entities_to_remove)
            {
                entity.Get<Physics>().body.Enabled = false;
                entity.Disable();
            }

            if (entities_to_remove.Count > 20)
            {
                foreach (var entity in entities_to_remove)
                {
                    physicsWorld.Remove(entity.Get<Physics>().body);
                    entity.Dispose();
                }
                entities_to_remove.Clear();
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
            #region player
            Entity player = world.CreateEntity();
            player.Set<Player>();
            player.Set<PlayerDirection>();
            player.Set(new PlayerSpeed
            {
                speed = 1400.0f
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
            player_physics.body = physicsWorld.CreateRectangle(
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
                if (!entities_to_remove.Contains((Entity)other.Body.Tag))
                {
                    world.Publish<CatchToppingMessage>(default);
                    entities_to_remove.Add((Entity)other.Body.Tag);
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
                if (!entities_to_remove.Contains((Entity)other.Body.Tag))
                {
                    world.Publish<GroundHitMessage>(default);
                    entities_to_remove.Add((Entity)sender.Body.Tag);
                }
                return false;
            }
            return true;
        }

        //TODO: shouldn't be here, move to new class
        public void CreateTopping(Vector2 position)
        {
            Entity topping = world.CreateEntity();
            topping.Set<Topping>();
            topping.Set(new Sprite(topping_textures[random.Next(topping_textures.Length)], 0.3f));
            topping.Set<Physics>();

            ref var topping_sprite = ref topping.Get<Sprite>();
            ref var topping_body = ref topping.Get<Physics>().body;
            position = new Vector2(position.X + 11, position.Y - 11);

            topping_body = physicsWorld.CreateRectangle(
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
            Entity fire = world.CreateEntity();
            fire.Set(new Animation
            {
                frame_count = 4,
                frame_speed = 0.2f,
                source_rectangle = new Rectangle(0, 0, 8, 8),
                offset = 8,
                current_frame = 0
            });
            fire.Set(new Sprite(fire_texture, 0.2f, 3));
            fire.Set<Physics>();
            ref var fire_physics = ref fire.Get<Physics>();
            fire_physics.body = physicsWorld.CreateBody();
            //TODO:bad, setting offset because of origin
            position = new Vector2(position.X + 11, position.Y - 11);
            fire_physics.body.Position = position;
            fire_physics.body.Enabled = false;
            world.Publish<NewFireMessage>(default);
            return fire;
        }

        //TODO: shouldn't be here, move to new class nor should it be this long
        public void CreateCity()
        {
            #region city
            #region background
            Entity background = world.CreateEntity();
            background.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/sky"), 0, Color.White, 3));
            background.Set<Physics>();

            Entity background_building_right = world.CreateEntity();
            background_building_right.Set(new Sprite(buildings_texture, 0f, new Color(100, 100, 100), 3));
            background_building_right.Set<Physics>();

            ref var background_physics = ref background.Get<Physics>();
            background_physics.body = physicsWorld.CreateBody();
            background_physics.body.Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            background_physics.body.Enabled = false;

            ref var background_building_right_phyiscs = ref background_building_right.Get<Physics>();
            background_building_right_phyiscs.body = physicsWorld.CreateBody();
            background_building_right_phyiscs.body.Position = new Vector2(graphicsDevice.Viewport.Width / 2 + 200, graphicsDevice.Viewport.Height / 2 + 50);
            background_building_right_phyiscs.body.Enabled = false;
            #endregion

            #region middle_ground
            Entity middle_ground_building_left = world.CreateEntity();
            middle_ground_building_left.Set(new Sprite(buildings_texture, 0.1f, new Color(180, 180, 180), 3));
            middle_ground_building_left.Set<Physics>();

            Entity middle_ground_building_middle = world.CreateEntity();
            middle_ground_building_middle.Set(new Sprite(buildings_texture, 0.1f, new Color(180, 180, 180), 3));
            middle_ground_building_middle.Set<Physics>();

            Entity middle_ground_building_right = world.CreateEntity();
            middle_ground_building_right.Set(new Sprite(buildings_texture, 0.1f, new Color(180, 180, 180), 3));
            middle_ground_building_right.Set<Physics>();

            ref var middle_ground_building_left_phyiscs = ref middle_ground_building_left.Get<Physics>();
            middle_ground_building_left_phyiscs.body = physicsWorld.CreateBody();
            middle_ground_building_left_phyiscs.body.Position = new Vector2(graphicsDevice.Viewport.Width / 5, graphicsDevice.Viewport.Height / 2);
            middle_ground_building_left_phyiscs.body.Enabled = false;

            ref var middle_ground_building_middle_phyiscs = ref middle_ground_building_middle.Get<Physics>();
            middle_ground_building_middle_phyiscs.body = physicsWorld.CreateBody();
            middle_ground_building_middle_phyiscs.body.Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            middle_ground_building_middle_phyiscs.body.Enabled = false;

            ref var middle_ground_building_right_physics = ref middle_ground_building_right.Get<Physics>();
            middle_ground_building_right_physics.body = physicsWorld.CreateBody();
            middle_ground_building_right_physics.body.Position = new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height / 2);
            middle_ground_building_right_physics.body.Enabled = false;
            #endregion

            #region foreground
            Entity foreground = world.CreateEntity();
            foreground.Set(new Sprite(content.Load<Texture2D>("Assets/Sprites/foreground"), 0.2f, Color.White, 3));
            foreground.Set<Physics>();

            Entity ground = world.CreateEntity();
            ground.Set<Physics>();
            ground.Set<Ground>();

            ref var foreground_physics = ref foreground.Get<Physics>();
            foreground_physics.body = physicsWorld.CreateBody();
            foreground_physics.body.Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            foreground_physics.body.Enabled = false;

            ref var ground_physics = ref ground.Get<Physics>();
            ground_physics.body = physicsWorld.CreateRectangle(graphicsDevice.Viewport.Width, 20, 1f, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height - 20));
            ground_physics.body.Tag = ground;

            //defines world edges
            physicsWorld.CreateEdge(new Vector2(0, 0), new Vector2(graphicsDevice.Viewport.Width, 0));
            physicsWorld.CreateEdge(new Vector2(graphicsDevice.Viewport.Width, 0), new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            physicsWorld.CreateEdge(new Vector2(0, 0), new Vector2(0, graphicsDevice.Viewport.Height));
            physicsWorld.CreateEdge(new Vector2(0, graphicsDevice.Viewport.Height), new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            #endregion
            #endregion
        }
    }
}
