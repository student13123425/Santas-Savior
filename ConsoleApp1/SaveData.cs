using System;
using System.IO;

namespace ConsoleApp1
{
    public class SaveData
    {
        public int level_id;
        private string save_path = "./save.data";

        public SaveData()
        {
            Load();
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(save_path, level_id.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to save game data: " + e.Message);
            }
        }

        public int Load()
        {
            if (File.Exists(save_path))
            {
                try
                {
                    string content = File.ReadAllText(save_path);
                    if (int.TryParse(content, out int loaded_id))
                    {
                        level_id = loaded_id;
                    }
                    else
                    {
                        level_id = 0;
                        Save();
                    }
                }
                catch (Exception)
                {
                    level_id = 0;
                }
            }
            else
            {
                level_id = 0;
                Save();
            }
            return level_id;
        }

        public void SetLevelID(int id)
        {
            level_id = id;
            Save();
        }
    }
}