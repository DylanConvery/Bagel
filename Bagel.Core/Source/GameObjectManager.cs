using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace Bagel
{
    public class GameObjectManager
    {
        public GameObjectManager(Scene scene)
        {
            m_game_objects = new List<GameObject>();
            this.m_scene = scene;
        }

        public GameObject Add(string name)
        {
            GameObject game_object = new GameObject(name);
            m_game_objects.Add(game_object);
            return game_object;
        }

        public bool Remove(string name)
        {
            return m_game_objects.Remove(Get(name));
        }

        public bool Remove(int id)
        {
            return m_game_objects.Remove(Get(id));
        }

        public bool Remove(GameObject game_object) => m_game_objects.Remove(game_object);

        public void Clear()
        {
            foreach (GameObject game_object in m_game_objects)
            {
                game_object.RemoveAllComponents();
            }
            m_game_objects.Clear();
        }

        public GameObject Get(string name)
        {
            foreach (GameObject game_object in m_game_objects)
            {
                if (game_object.name == name)
                {
                    return game_object;
                }
            }
            return null;
        }

        public GameObject Get(int id)
        {
            foreach (GameObject game_object in m_game_objects)
            {
                if (game_object.id == id)
                {
                    return game_object;
                }
            }
            return null;
        }

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

        private Scene m_scene;
        public int m_count => m_game_objects.Count;
        private List<GameObject> m_game_objects;
    }
}