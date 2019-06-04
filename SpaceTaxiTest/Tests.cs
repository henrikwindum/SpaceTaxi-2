using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using NUnit.Framework;
using SpaceTaxi_1;
using SpaceTaxi_1.Taxi;

namespace SpaceTaxiTest {
    [TestFixture]
    public class Tests {
        [SetUp]
        public void SetUp() {
            Window.CreateOpenGLContext();
            
            player = new Player();
            customer = new Customer(new Shape(), new Image(Path.Combine("Assets", "Images", 
                "CustomerStandRight.png")));
            customer2 = new Customer(new Shape(), new Image(Path.Combine("Assets", "Images", 
                "CustomerStandRight.png")));

            
            paintBoard = new PaintBoard();
            
        }

        private ReadFile readFile;
        private PaintBoard paintBoard;
        private Player player;
        private Customer customer;
        private Customer customer2;
        
        /// <summary>
        /// Tests if the player´s bool "BoolPassenger" is set to true upon a pick up of a customer
        /// </summary> 
        [Test]
        public void PickUpTest() {
            player.BoolPassenger = false;
            
            player.PickUp(customer);
            
            Assert.That(player.BoolPassenger);
            Assert.That(player.CurrentCustomer == customer);
            Assert.That(StaticTimer.GetElapsedSeconds() < 0.01f);
        }
        
        /// <summary>
        /// Checks if the taxi can pick up a customer while already having a customer.
        /// </summary>
        [Test]
        public void NotPickUpTest() {
            player.BoolPassenger = true;
            player.CurrentCustomer = customer;
            
            player.PickUp(customer2);
            
            Assert.That(player.BoolPassenger);
            Assert.That(player.CurrentCustomer == customer);
        }
        
        /// <summary>
        /// Tests if the player´s bool "BoolPassenger" is set to false upon dropping off a customer    
        /// </summary>
        [Test]
        public void DropOffTest() {
            player.BoolPassenger = true;
            
            player.DropOff();
            
            Assert.That(player.BoolPassenger == false);
            Assert.That(player.CurrentCustomer == null);
            Assert.That(StaticTimer.GetElapsedSeconds() < 0.01f);
        }

        /// <summary>
        ///    Checks if the customers of short-n-sweet.txt gets the right attributes. 
        /// </summary>
        [Test]
        public void CustomerTestShortNSweet() {
            paintBoard.PaintTheBoard("short-n-sweet.txt");
            foreach (Customer customer in paintBoard.Customers) {
                Assert.That("Alice" == customer.Name);    
                Assert.That(10 == customer.SpawnTime);
                Assert.That("^J" == customer.Platform); 
                Assert.That(10 == customer.TimeLimit);
                Assert.That(100 == customer.Score);
            }
        }
        
        /// <summary>
        ///    Checks if the customers of the-beach.txt gets the right attributes. 
        /// </summary>
        [Test]
        public void CustomerTestTheBeach() {
            paintBoard.PaintTheBoard("the-beach.txt");
            for (int i = 0; i < paintBoard.Customers.Count; i++) {
                Assert.That("Carol" == paintBoard.Customers[0].Name);
                Assert.That(30 == paintBoard.Customers[0].SpawnTime);
                Assert.That("^" == paintBoard.Customers[0].Platform); 
                Assert.That(10 == paintBoard.Customers[0].TimeLimit);
                Assert.That(100 == paintBoard.Customers[0].Score);
                
                Assert.That("Bob" == paintBoard.Customers[1].Name);
                Assert.That(10 == paintBoard.Customers[1].SpawnTime);
                Assert.That("r" == paintBoard.Customers[1].Platform); 
                Assert.That(10 == paintBoard.Customers[1].TimeLimit);
                Assert.That(100 == paintBoard.Customers[1].Score);
            }
        }
    }
}