using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace SpaceTaxi_1 {
    public class Platform : Entity {

        public char platform;
        public Platform(Shape shape, IBaseImage image) : base(shape, image) {
            platform = ' ';
            Entity = new Entity(shape, image);
        }
        
        public Entity Entity { get; }
    }
}