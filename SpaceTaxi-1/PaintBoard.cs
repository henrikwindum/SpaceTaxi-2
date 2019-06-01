using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class PaintBoard {
        private Customer customer;
        public List<Customer> Customers;
        private float height = 0.025f;

        private Platform platform;

        public EntityContainer<Platform> PlatformList;
        private ReadFile readFile;
        public EntityContainer<Entity> Walls;
        private float width = 0.04347826086956521739130434782609f;

        public void PaintTheBoard(string level) {
            readFile = new ReadFile();
            readFile.Read(level);
            Walls = new EntityContainer<Entity>();
            Customers = new List<Customer>();
            PlatformList = new EntityContainer<Platform>();
            CreateBoard();
        }

        private void CreateBoard() {
            for (var i = 0; i < readFile.BoardList.Count; i++) {
                var currentPlatform = ' ';
                var currentString = readFile.BoardList[i];
                for (var j = 0; j < currentString.Length; j++) {
                    // handles platforms
                    if (readFile.Platforms.Contains(currentString[j])) {
                        platform = new Platform(new StationaryShape(
                                new Vec2F(j * height, i * width),
                                new Vec2F(height, width)),
                            new Image(Path.Combine("Assets", "Images",
                                readFile.BoardDict[currentString[j]])));
                        platform.AsciiPlatform = currentString[j];
                        platform.Name.SetText(platform.AsciiPlatform.ToString());
                        PlatformList.AddStationaryEntity(platform);

                        // handles customers
                        if (readFile.CustomerDict.ContainsKey(currentString[j]) &&
                            currentPlatform == currentString[j]) {
                            customer.MaxRight = new Vec2F(j * height, i * width + 1.75f * height);
                        } else if (readFile.CustomerDict.ContainsKey(currentString[j])) {
                            customer = new Customer(
                                new DynamicShape(new Vec2F(j * height, i * width + 1.75f * height),
                                    new Vec2F(0.05f, 0.05f)),
                                new Image(
                                    Path.Combine("Assets", "Images", "CustomerStandRight.png")));

                            customer.MaxRight = new Vec2F(j * height, i * width + 1.75f * height);
                            customer.MaxLeft = new Vec2F(j * height, i * width + 1.75f * height);
                            customer.Instantiate(readFile.CustomerDict, currentString[j]);
                            Customers.Add(customer);
                            currentPlatform = currentString[j];
                        }
                    }
                    // handles walls
                    else if (readFile.BoardDict.ContainsKey(currentString[j])) {
                        Walls.AddStationaryEntity(new Entity(
                            new StationaryShape(new Vec2F(j * height, i * width),
                                new Vec2F(height, width)),
                            new Image(Path.Combine("Assets", "Images",
                                readFile.BoardDict[currentString[j]]))));
                    }
                }
            }
        }
    }
}