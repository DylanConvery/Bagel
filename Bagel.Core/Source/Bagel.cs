using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bagel
{
    public class Bagel : Game
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_sprite_batch;
        private EntityManager m_entity_manager;
        private Scene scene;
        public Bagel()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            m_entity_manager = new EntityManager();
            scene = new Scene(m_entity_manager);
        }

        protected override void LoadContent()
        {
            m_sprite_batch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Initialize()
        {
            scene.Initialize();
            base.Initialize();
            //EntityHandle bagel = scene.CreateEntity();
            //TransformComponent transform = new TransformComponent();
            //bagel.AddComponent<TransformComponent>(transform);
        }

        protected override void Update(GameTime game_time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            scene.Update(game_time);
            base.Update(game_time);
        }

        protected override void Draw(GameTime game_time)
        {
            GraphicsDevice.Clear(Color.Black);
            scene.Draw();
            base.Draw(game_time);
        }
    }
}