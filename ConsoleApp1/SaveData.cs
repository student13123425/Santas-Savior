using System;
using System.IO;

namespace ConsoleApp1
{
    public class SaveData
    {
        public int level_id;
        public bool is_debug;
        private string save_path = "./save.data";

        public SaveData()
        {
            Load();
        }

        public void Save()
        {
            try
            {
                string data = $"{level_id},{is_debug}";
                File.WriteAllText(save_path, data);
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
                    string[] parts = content.Split(',');

                    if (parts.Length >= 1 && int.TryParse(parts[0], out int loaded_id))
                    {
                        level_id = loaded_id;
                    }
                    else
                    {
                        level_id = 0;
                    }

                    if (parts.Length >= 2 && bool.TryParse(parts[1], out bool loaded_debug))
                    {
                        is_debug = loaded_debug;
                    }
                    else
                    {
                        is_debug = true; 
                    }
                }
                catch (Exception)
                {
                    level_id = 0;
                    is_debug = true;
                }
            }
            else
            {
                level_id = 0;
                is_debug = true;
                Save();
            }
            return level_id;
        }

        public void SetLevelID(int id)
        {
            level_id = id;
            Save();
        }

        public void SetDebug(bool debug)
        {
            is_debug = debug;
            Save();
        }
    }
}