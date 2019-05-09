﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private readonly DynamicShape shape;
        private Orientation taxiOrientation;

        private List<Image> taxiStrides;
        private Boolean MoveUp = false;
        private Boolean MoveLeft = false;
        private Boolean MoveRight = false;
        
        private Vec2F stopMovement = new Vec2F(0.0f, 0.0f);
        private Vec2F moveUp = new Vec2F(0.0f,0.00005f);
        private Vec2F moveLeft = new Vec2F(0.00005f,0.0f);
        private Vec2F moveRight = new Vec2F(-0.00005f,0.0f);

        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Txi_Thrust_None.png"));
            taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Txi_Thrust_None_Right.png"));
            
            taxiStrides = new List<Image>();
            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            
            SpaceBus.GetBus().Subscribe(GameEventType.PlayerEvent,this);
        }

        public Entity Entity { get; }

        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        public void SetExtent(float width, float height) {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        public void RenderPlayer() {
            //TODO: Next version needs animation. Skipped for clarity.
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;
            Entity.RenderEntity();
            ActivateThrusters();
            foreach (var stride in taxiStrides) {
                stride.Render(shape);
            }
        }

        public void Gravity() {
            shape.Direction.Y -= 0.00002f;
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "BOOSTER_UPWARDS":
                    MovementUp(shape.Direction);
                    break;
                case "BOOSTER_TO_LEFT":
                    taxiOrientation = Orientation.Left;
                    MovementRight(shape.Direction);
                    break;
                case "BOOSTER_TO_RIGHT":
                    taxiOrientation = Orientation.Right;
                    MovementLeft(shape.Direction);
                    break;
                case "STOP_ACCELERATE_LEFT":
                case "STOP_ACCELERATE_RIGHT":
                case "STOP_ACCELERATE_UP":
                    StopMovement(shape.Direction);
                    break;
                }   
            }
        }

        private void ActivateThrusters() {
            if (MoveUp) {
                if (taxiOrientation == Orientation.Left) {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom_Right.png"));
                } else {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom.png"));
                }
            }
            if (MoveLeft) {
                if (MoveUp) {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom_Back_Right.png"));
                } else {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Back_Right.png"));
                }
            }
            if (MoveRight) {
                if (MoveUp) {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Bottom_Back.png"));
                } else {
                    taxiStrides = ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Txi_Thrust_Back.png"));
                }
            }
            if (MoveUp == false && MoveRight == false && MoveLeft == false) {
                taxiStrides.Clear();
            }
        }

        private void MovementUp(Vec2F vec) {
            MoveUp = true;
        }
        
        private void MovementLeft(Vec2F vec) {
            MoveLeft = true;
        }

        private void MovementRight(Vec2F vec) {
            MoveRight = true;
        }

        private void StopMovement(Vec2F vec) {
            MoveUp = false;
            MoveRight = false;
            MoveLeft = false;
        }

        
        
        public void Movement() {
            if (MoveUp) {
                shape.Direction += moveUp;
            }
            if (MoveLeft) {
                shape.Direction += moveLeft;
            }
            if (MoveRight) {
                shape.Direction += moveRight;
            }
        }
    }
}