using System;
using System.Collections.Generic;
using System.Text;

namespace EntityComponentSystem
{
    public abstract class IComponent
    {
        protected uint entity_id;
    }
}
