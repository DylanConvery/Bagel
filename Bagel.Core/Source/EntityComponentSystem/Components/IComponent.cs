using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    public interface IComponent{ }

    public struct ComponentFamily
    {
        public static int counter = 0;
    }

    public class Component<T> : IComponent
    {
        public static int GetComponentFamily<T>()
        {
            return family;
        }

        public static int family = ComponentFamily.counter++;
    }
}
