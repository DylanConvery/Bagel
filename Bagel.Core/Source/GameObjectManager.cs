using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace Bagel
{
    public class GameObjectManager
    {

        public GameObject Add(string name)
        {
            GameObject game_object = new GameObject(name);
            m_game_objects.Add(game_object);
            return game_object;
        }

        public void Remove(string name) { }

        public void Remove(int id) { }

        public void Remove(GameObject game_object) { }

        public GameObject Get(string name) { return null; }

        public GameObject Get(int id) { return null; }

        public void Update(GameTime game_time)
        {
            foreach (GameObject game_object in m_game_objects)
            {
                game_object.Update(game_time);
            }
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            foreach (GameObject game_object in m_game_objects)
            {
                game_object.Draw(sprite_batch);
            }
        }

        private int m_count => m_game_objects.Count;
        private Scene m_scene;
        private List<GameObject> m_game_objects;
    }
}