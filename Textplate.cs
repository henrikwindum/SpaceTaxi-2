using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace SpaceTaxi_1 {
    public class Textplate {
        private Text scoreText;
        private Text timerText;
        private Text namePlatformText;
        private Entity textPlate;

        public Textplate() {
            textPlate = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 0.05f)), 
                new DIKUArcade.Graphics.Image(Path.Combine("Assets", "Images", "white-square.png")));
            
            scoreText = new Text("", 
                new Vec2F(0.1f, -0.165f), new Vec2F(0.2f, 0.2f));   
            scoreText.SetColor(Color.Red);
            
            timerText = new Text("",
                new Vec2F(0.75f, -0.165f), new Vec2F(0.2f, 0.2f));
            timerText.SetColor(Color.Red); 
            
            namePlatformText = new Text("",
                new Vec2F(0.25f, -0.165f), new Vec2F(0.2f, 0.2f));
            namePlatformText.SetColor(Color.Red);
        }

        public void Render(Player player) {
            textPlate.RenderEntity();
            scoreText.SetText("Score: " + player.score);
            timerText.SetText("Timer: " + StaticTimer.GetElapsedSeconds());
            if (player.currentCustomer != null) {
                namePlatformText.SetText(player.currentCustomer.name + " | " + player.currentCustomer.platform);    
            } else {
                namePlatformText.SetText("");
            }
            scoreText.RenderText();
            timerText.RenderText();
            namePlatformText.RenderText();
        }
    }
}