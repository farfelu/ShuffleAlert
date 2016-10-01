using System.ComponentModel;

namespace ShuffleAlert
{
    class ControlContainer : IContainer
    {
        public ComponentCollection Components { get; set; }

        public ControlContainer()
        {
            Components = new ComponentCollection(new IComponent[] { });
        }

        public void Add(IComponent component)
        {
        }

        public void Add(IComponent component, string name)
        {
        }

        public void Dispose()
        {
        }

        public void Remove(IComponent component)
        {
        }
    }
}