using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;

namespace SpaceTaxi_1.SpaceStates {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private AnimationContainer explosion;

        private int explosionLength = 500;
        private List<Image> explosionStrides;
        private float height = 0.025f;
        private Player player;
        private List<Player> playerList = new List<Player>();
        public ReadFile readFile;
        private float width = 0.04347826086956521739130434782609f;


        public GameRunning() {
            player = new Player();
            player.SetPosition(0.45f, 0.6f);
            player.SetExtent(0.112f, 0.0576f);
            playerList.Add(player);

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosion = new AnimationContainer(8);

            InitializeGameState();
        }

        private EntityContainer<Entity> Images { get; set; }
        private EntityContainer<Entity> Platforms { get; set; }

        public void GameLoop() { }

        public void InitializeGameState() { }

        public void UpdateGameLogic() {
            player.Movement();
            Collision();
            player.Entity.Shape.Move();
            player.Gravity();
            NextLevel();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            foreach (var play in playerList) {
                play.RenderPlayer();
            }

            Platforms.RenderEntities();
            Images.RenderEntities();
            explosion.RenderAnimations();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            case "KEY_P":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors
                    (GameEventType.GameStateEvent, this,
                        "CHANGE_STATE", "GAME_PAUSED", ""));
                break;
            }
        }

        public void PaintBoard(string level) {
            readFile = new ReadFile();
            readFile.Read(level);

            Images = new EntityContainer<Entity>();
            Platforms = new EntityContainer<Entity>();
            CreateBoard();
        }

        private void CreateBoard() {
            for (var i = 0; i < readFile.BoardList.Count; i++) {
                var currentString = readFile.BoardList[i];
                for (var j = 0; j < currentString.Length; j++) {
                    if (readFile.Dict.ContainsKey(currentString[j]) &&
                        !readFile.Platforms.Contains(currentString[j])) {
                        Images.AddStationaryEntity(new Entity(
                            new StationaryShape(new Vec2F(j * height, i * width),
                                new Vec2F(height, width)),
                            new Image(Path.Combine("Assets", "Images",
                                readFile.Dict[currentString[j]]))));
                    }

                    if (readFile.Platforms.Contains(currentString[j])) {
                        Platforms.AddStationaryEntity(new Entity(
                            new StationaryShape(new Vec2F(j * height, i * width),
                                new Vec2F(height, width)),
                            new Image(Path.Combine("Assets", "Images",
                                readFile.Dict[currentString[j]]))));
                    }
                }
            }
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        public static GameRunning NewGetInstance() {
            return GameRunning.instance = new GameRunning();
        }

        private void AddExplosion(Vec2F pos, Vec2F extend) {
            explosion.AddAnimation(
                new StationaryShape(pos, extend), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }

        private void Collision() {
            foreach (Entity entity in Images) {
                switch (CollisionDetection.Aabb((DynamicShape)
                    player.Entity.Shape, entity.Shape).Collision) {
                case true:
                    AddExplosion(player.Entity.Shape.Position,
                        player.Entity.Shape.Extent);
                    player.Entity.DeleteEntity();
                    break;
                }

                var newPlayerList = new List<Player>();
                foreach (var play in playerList) {
                    if (!play.Entity.IsDeleted()) {
                        newPlayerList.Add(play);
                    }

                    if (play.Entity.IsDeleted()) {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_OVER", ""));
                    }
                }

                playerList = newPlayerList;
            }

            foreach (Entity entity in Platforms) {
                switch (CollisionDetection.Aabb((DynamicShape)
                    player.Entity.Shape, entity.Shape).Collision) {
                case true:
                    if (player.Entity.Shape.AsDynamicShape().Direction.Y < -0.0015f) {
                        AddExplosion(player.Entity.Shape.Position,
                            player.Entity.Shape.Extent);
                        player.Entity.DeleteEntity();
                    } else {
                        player.Entity.Shape.AsDynamicShape().Direction.Y = 0;
                        player.Entity.Shape.AsDynamicShape().Direction.X = 0;
                    }

                    break;
                }

                var newPlayerList = new List<Player>();
                foreach (var play in playerList) {
                    if (!play.Entity.IsDeleted()) {
                        newPlayerList.Add(play);
                    }

                    if (play.Entity.IsDeleted()) {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_OVER", ""));
                    }
                }

                playerList = newPlayerList;
            }
        }


        private void NextLevel() {
            if (player.Entity.Shape.Position.Y >= 0.9f) {
                PaintBoard("the-beach.txt");
                player.SetPosition(0.3f, 0.5f);
                player.Entity.Shape.AsDynamicShape().Direction.Y = 0f;
            }
        }
    }
}