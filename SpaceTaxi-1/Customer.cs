using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace SpaceTaxi_1 {
    public class Customer : Entity {
        public DynamicShape shape;        
        public Vec2F maxRight;
        public Vec2F maxLeft;
        public float spawnTime;
        public string name;
        public string platform;
        public int score;
        public float timeLimit;
        
        public Orientation CustomerOrientation;
        
        public Customer(Shape shape, Image image) : base(shape, image) {
            CustomerOrientation = Orientation.Right;
            maxRight = new Vec2F();
            maxLeft = new Vec2F();
            spawnTime = new float();           
            Entity = new Entity(shape, image);
            name = "";
            platform = "";
            score = 0;
            timeLimit = new float();
        }
        
        public Entity Entity { get; }

        public void Move() {
            if (CustomerOrientation == Orientation.Right) {
                if (Shape.Position.X + Shape.AsDynamicShape().Direction.X >= maxRight.X) {
                    CustomerOrientation = Orientation.Left;                        
                }
                Entity.Image = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
                Shape.MoveX(0.001f);                                                            
            }

            if (CustomerOrientation == Orientation.Left) {
                if (Shape.Position.X + Shape.AsDynamicShape().Direction.X <= maxLeft.X) {
                    CustomerOrientation = Orientation.Right;
                }
                Entity.Image = new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
                Shape.MoveX(-0.001f);                                                              
            }            
        }

        public void Render() {
            if (spawnTime <= StaticTimer.GetElapsedSeconds()) {
                Entity.RenderEntity();
            }
        }
    }
}