using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi_1.Taxi;
using Image = DIKUArcade.Graphics.Image;

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
        
        /// <summary>
        /// Checks for the orientation and position of the customer, sets the current image of
        /// the customer and moves the customer.
        /// </summary>
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

                Entity.Image = new Image(Path.Combine("Assets", "Images", 
                    "CustomerStandLeft.png"));
                Shape.MoveX(-0.001f);
            }
        }
        
        /// <summary>
        /// If SpawnTime of the customer is less-than or equal to the elapsed seconds, then
        /// the customer is rendered.
        /// </summary>
        public void Render() {
            if (SpawnTime <= StaticTimer.GetElapsedSeconds()) {
                Entity.RenderEntity();
            }
        }

        /// <summary>
        /// Sets Name, SpawnTime, Platform, TimeLimit and Score of the customer.
        /// </summary>
        /// <param name="dict">A dictionary with customer-information.</param>
        /// <param name="key">A key in the dictionary.</param>
        public void Instantiate(Dictionary<char, string[]> dict, char key) {
            Name = dict[key][0];
            SpawnTime = float.Parse(dict[key][1]);
            Platform = dict[key][3];
            TimeLimit = float.Parse(dict[key][4]);
            Score = int.Parse(dict[key][5]);
        }
    }
}