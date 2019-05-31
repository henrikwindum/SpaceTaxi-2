using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class PaintBoard {
        private ReadFile readFile;
        private Customer customer;
        public EntityContainer<Entity> Walls;        
        public List<Customer> CustomerList;
        private float height = 0.025f;
        private float width = 0.04347826086956521739130434782609f;

        private Platform platform;
        public List<Platform> PlatformList;
        
        public void PaintTheBoard(string level) {
            readFile = new ReadFile();
            readFile.Read(level);
            Walls = new EntityContainer<Entity>();            
            CustomerList = new List<Customer>();            
            PlatformList = new List<Platform>();
            CreateBoard();
        }

        private void CreateBoard() {            
            for (int i = 0; i < readFile.BoardList.Count; i++) {
                var currentPlatform = ' ';
                var currentString = readFile.BoardList[i];
                for (int j = 0; j < currentString.Length; j++) {
                    // handles platforms
                    if (readFile.Platforms.Contains(currentString[j])) {                                                
                        platform = new Platform(new StationaryShape(
                                new Vec2F(j * height, i * width),
                                new Vec2F(height, width)),
                            new Image(Path.Combine("Assets", "Images",
                                readFile.BoardDict[currentString[j]])));                        
                        platform.platform = currentString[j];
                        platform.name.SetText(platform.platform.ToString());
                        PlatformList.Add(platform);
                        
                        // handles customers
                        if (readFile.CustomerDict.ContainsKey(currentString[j]) && 
                            currentPlatform == currentString[j]) {
                            customer.maxRight = new Vec2F(j*height, i*width+1.75f*height);                            
                        } else if (readFile.CustomerDict.ContainsKey(currentString[j])) {
                            customer = new Customer(
                                new DynamicShape(new Vec2F(j*height, i*width+1.75f*height), 
                                    new Vec2F(0.05f, 0.05f)),
                                new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png")));
                                
                            customer.maxRight = new Vec2F(j * height, i * width + 1.75f * height);
                            customer.maxLeft = new Vec2F(j * height, i * width + 1.75f * height);                            
                            customer.Instantiate(readFile.CustomerDict, currentString[j]);                                                        
                            CustomerList.Add(customer);
                            currentPlatform = currentString[j];
                        }
                    } 
                    // handles walls
                    else if (readFile.BoardDict.ContainsKey(currentString[j])) {
                        Walls.AddStationaryEntity(new Entity(
                            new StationaryShape(new Vec2F(j*height, i*width), 
                                new Vec2F(height, width)), 
                            new Image(Path.Combine("Assets", "Images",
                                readFile.BoardDict[currentString[j]]))));
                    }
                }
            }
        }                
    }
}