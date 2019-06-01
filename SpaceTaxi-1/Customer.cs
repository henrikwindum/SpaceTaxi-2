using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi_1.Taxi;

namespace SpaceTaxi_1 {
    public class Customer : Entity {
        public Orientation CustomerOrientation;
        public Vec2F MaxLeft;
        public Vec2F MaxRight;
        public string Name;
        public string Platform;
        public int Score;
        public float SpawnTime;
        public float TimeLimit;

        public Customer(Shape shape, Image image) : base(shape, image) {
            CustomerOrientation = Orientation.Right;
            MaxRight = new Vec2F();
            MaxLeft = new Vec2F();
            SpawnTime = new float();
            Entity = new Entity(shape, image);
            Name = "";
            Platform = "";
            Score = 0;
            TimeLimit = new float();
        }

        public Entity Entity { get; }

        public void Move() {
            if (CustomerOrientation == Orientation.Right) {
                if (Shape.Position.X + Shape.AsDynamicShape().Direction.X >= MaxRight.X) {
                    CustomerOrientation = Orientation.Left;
                }

                Entity.Image =
                    new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
                Shape.MoveX(0.001f);
            }

            if (CustomerOrientation == Orientation.Left) {
                if (Shape.Position.X + Shape.AsDynamicShape().Direction.X <= MaxLeft.X) {
                    CustomerOrientation = Orientation.Right;
                }

                Entity.Image = new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
                Shape.MoveX(-0.001f);
            }
        }

        public void Render() {
            if (SpawnTime <= StaticTimer.GetElapsedSeconds()) {
                Entity.RenderEntity();
            }
        }

        public void Instantiate(Dictionary<char, string[]> dict, char key) {
            Name = dict[key][0];
            SpawnTime = float.Parse(dict[key][1]);
            Platform = dict[key][3];
            TimeLimit = float.Parse(dict[key][4]);
            Score = int.Parse(dict[key][5]);
        }
    }
}