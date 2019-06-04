using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi_1.Taxi;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1 {
    public class Textplate {
        private Text namePlatformText;
        private Text scoreText;
        private Entity textPlate;
        private Text timerText;

        public Textplate() {
            textPlate = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), 
                    new Vec2F(1.0f, 0.05f)),
                new Image(Path.Combine("Assets", "Images", "white-square.png")));

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
            scoreText.SetText("Score: " + player.Score);
            if (player.CurrentCustomer != null) {
                var temp = player.CurrentCustomer.TimeLimit -
                           float.Parse(StaticTimer.GetElapsedSeconds().ToString());
                timerText.SetText("Timer: " + temp);
                namePlatformText.SetText(player.CurrentCustomer.Name + " | " +
                                         player.CurrentCustomer.Platform);
            } else {
                timerText.SetText("Timer: " + StaticTimer.GetElapsedSeconds());
                namePlatformText.SetText("");
            }

            scoreText.RenderText();
            timerText.RenderText();
            namePlatformText.RenderText();
        }
    }
}