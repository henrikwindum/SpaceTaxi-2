using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SpaceTaxi_1 {
    public class ReadFile {
        public Dictionary<char, string> BoardDict;
        public List<string> BoardList;
        private int counter;
        public Dictionary<char, string[]> CustomerDict;
        private string line;
        public List<char> Platforms;

        /// <summary>
        ///     Adds the level-map as strings to BoardList.
        ///     Adds the char and .png-string to Dict as key and value.
        /// </summary>
        /// <param name="level">Filename of the level</param>
        public void Read(string level) {
            var file = new StreamReader(GetLevelPath(level));
            BoardList = new List<string>();
            Platforms = new List<char>();

            BoardDict = new Dictionary<char, string>();
            CustomerDict = new Dictionary<char, string[]>();

            // Reads and adds the line to lists.            
            while ((line = file.ReadLine()) != null) {
                counter++;
                if (line.EndsWith(".png")) {
                    BoardDict.Add(line[0], line.Substring(3));
                }

                if (line.StartsWith("Platforms:")) {
                    for (var i = 11; i < line.Length; i++) {
                        if (line[i] != ' ' && line[i] != ',') {
                            Platforms.Add(line[i]);
                        }
                    }
                }

                if (line.StartsWith("Customer:")) {
                    line = line.Substring(10);
                    var customerStats = line.Split(' ');
                    CustomerDict.Add(char.Parse(customerStats[2]), customerStats);
                }

                if (counter < 24) {
                    BoardList.Add(line);
                }
            }

            BoardList.Reverse();
            file.Close();
        }

        /// <summary>
        ///     Finds full directory path of the given level.
        /// </summary>
        /// <remarks>This code is borrowed from Texture.cs in DIKUArcade.</remarks>
        /// <param name="fileName">Filename of the level.</param>
        /// <returns>Directory path of the level.</returns>
        /// <exception cref="FileNotFoundException">File does not exist.</exception>
        private string GetLevelPath(string fileName) {
            var dir = new DirectoryInfo(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            while (dir.Name != "bin") {
                dir = dir.Parent;
            }

            dir = dir.Parent;
            var path = Path.Combine(dir.FullName, "Levels", fileName);
            if (!File.Exists(path)) {
                throw new FileNotFoundException($"Error: The file \"{path}\" does not exist.");
            }

            return path;
        }
    }
}