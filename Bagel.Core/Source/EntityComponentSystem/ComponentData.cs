using System.Collections.Generic;

namespace EntityComponentSystem
{
    public class ComponentData
    {
        public int size = 0;
        public List<IComponent> data = new List<IComponent>(1024);
    }
}
