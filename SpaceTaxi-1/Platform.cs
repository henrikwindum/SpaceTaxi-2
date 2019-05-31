using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Platform : Entity {

        public char platform;
        public Text name;
        
        public Platform(Shape shape, IBaseImage image) : base(shape, image) {
            platform = ' ';
            Entity = new Entity(shape, image);
            name = new Text(platform.ToString(), 
                new Vec2F(Entity.Shape.Position.X, Entity.Shape.Position.Y - 0.16f), 
                new Vec2F(0.2f, 0.2f));
        }
        
        public Entity Entity { get; }
    }
}