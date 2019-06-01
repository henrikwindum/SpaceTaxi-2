using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Platform : Entity {
        public char AsciiPlatform;
        public Text Name;

        public Platform(Shape shape, IBaseImage image) : base(shape, image) {
            AsciiPlatform = ' ';
            Entity = new Entity(shape, image);
            Name = new Text(AsciiPlatform.ToString(),
                new Vec2F(Entity.Shape.Position.X, Entity.Shape.Position.Y - 0.16f),
                new Vec2F(0.2f, 0.2f));
        }

        public Entity Entity { get; }
    }
}