using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using SpaceTaxi_1.Taxi;

namespace SpaceTaxi_1 {
    public class Collision {
        public AnimationContainer Explosion;
        private int explosionLength = 500;
        private List<Image> explosionStrides;
        public List<Player> PlayerList;


        public Collision() {
            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            Explosion = new AnimationContainer(8);
            PlayerList = new List<Player>();
        }


        private void AddExplosion(Vec2F pos, Vec2F extend) {
            Explosion.AddAnimation(
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
                foreach (var tempPlayer in PlayerList) {
                    if (!tempPlayer.Entity.IsDeleted()) {
                        newPlayerList.Add(tempPlayer);
                    }

                    if (tempPlayer.Entity.IsDeleted()) {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_OVER", ""));
                    }
                }

                PlayerList = newPlayerList;
            }
        }

        public void PlatformCollision(EntityContainer<Platform> platforms, Player player) {
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

                        if (player.CurrentCustomer != null && player.CurrentCustomer.Platform ==
                            platform.AsciiPlatform.ToString()) {
                            player.Score += player.CurrentCustomer.Score;
                            player.DropOff();
                        }
                    }

                    break;
                }

                var newPlayerList = new List<Player>();
                foreach (var tempPlayer in PlayerList) {
                    if (!tempPlayer.Entity.IsDeleted()) {
                        newPlayerList.Add(tempPlayer);
                    }

                    if (tempPlayer.Entity.IsDeleted()) {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_OVER", ""));
                    }
                }

                PlayerList = newPlayerList;
            }
        }

        public List<Customer> CustomerCollision(List<Customer> customers, Player player) {
            var newCustomerList = new List<Customer>();
            foreach (var customer in customers) {
                if (customer.Shape.Position.X <
                    player.Entity.Shape.Extent.X + player.Entity.Shape.Position.X &&
                    customer.Shape.Extent.X + customer.Shape.Position.X >
                    player.Entity.Shape.Position.X &&
                    customer.Shape.Position.Y <
                    player.Entity.Shape.Extent.Y + player.Entity.Shape.Position.Y &&
                    customer.Shape.Extent.Y + customer.Shape.Position.Y >
                    player.Entity.Shape.Position.Y &&
                    !player.BoolPassenger) {
                    player.PickUp(customer);
                    customer.Entity.DeleteEntity();
                }

                if (!customer.Entity.IsDeleted()) {
                    newCustomerList.Add(customer);
                }

                customers = newCustomerList;
            }

            return customers;
        }
    }
}