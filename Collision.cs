using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

namespace SpaceTaxi_1 {
    public class Collision {
        public AnimationContainer explosion;
        private List<Image> explosionStrides;
        public List<Player> playerList;        
        private int explosionLength = 500;
        

        public Collision() {
            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosion = new AnimationContainer(8);
            playerList = new List<Player>();                     
        }
        
        
        private void AddExplosion(Vec2F pos, Vec2F extend) {
            explosion.AddAnimation(
                new StationaryShape(pos, extend), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }

        public void WallCollision(EntityContainer<Entity> walls, Player player) {
            foreach (Entity entity in walls) {
                switch (CollisionDetection.Aabb((DynamicShape)
                    player.Entity.Shape, entity.Shape).Collision) {
                case true:
                    AddExplosion(player.Entity.Shape.Position,
                        player.Entity.Shape.Extent);
                    player.Entity.DeleteEntity();
                    break;
                }
                var newPlayerList = new List<Player>();
                foreach (var _player in playerList) {
                    if (!_player.Entity.IsDeleted()) {
                        newPlayerList.Add(_player);
                    }

                    if (_player.Entity.IsDeleted()) {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_OVER", ""));
                    }
                }

                playerList = newPlayerList;
            }
        }

        public void PlatformCollision(List<Platform> platforms, Player player) {
            foreach (Platform platform in platforms) {
                switch (CollisionDetection.Aabb((DynamicShape)
                    player.Entity.Shape, platform.Entity.Shape).Collision) {
                case true:
                    if (player.Entity.Shape.AsDynamicShape().Direction.Y < -0.0015f) {
                        AddExplosion(player.Entity.Shape.Position,
                            player.Entity.Shape.Extent);
                        player.Entity.DeleteEntity();
                    } else {
                        player.Entity.Shape.AsDynamicShape().Direction.Y = 0;
                        player.Entity.Shape.AsDynamicShape().Direction.X = 0;
                                                
                        if (player.currentCustomer != null && player.currentCustomer.platform == platform.platform.ToString()) {
                            Console.WriteLine("You delivered " + player.currentCustomer.name + " at platform " + platform.platform);                            
                            player.score += player.currentCustomer.score;
                            player.DropOff();
                        }
                    }
                    break;
                }
                var newPlayerList = new List<Player>();
                foreach (var _player in playerList) {
                    if (!_player.Entity.IsDeleted()) {
                        newPlayerList.Add(_player);
                    }
                    if (_player.Entity.IsDeleted()) {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_OVER", ""));
                    }
                }
                playerList = newPlayerList;
            }
        }

        public List<Customer> CustomerCollision(List<Customer> customers, Player player) {
            var newCustomerList = new List<Customer>();
            foreach (Customer _customer in customers) {
                switch (CollisionDetection.Aabb((DynamicShape)
                    player.Entity.Shape, _customer.Shape).Collision) {
                case true:
                    if (!player.boolPassenger) {                        
                        player.PickUp(_customer);
                        _customer.Entity.DeleteEntity();                        
                    }
                    break;
                }                
                if (!_customer.Entity.IsDeleted()) {
                    newCustomerList.Add(_customer);                    
                }
                customers = newCustomerList;                
            }            
            return customers;
        }        
    }
}