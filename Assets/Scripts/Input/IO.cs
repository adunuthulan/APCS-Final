using UnityEngine;
using System.IO;


//loads game controls
namespace CustomInputManager
{
    public class IO
    {
        private string fileName;
        private bool pretty;
        
        public IO  (string file, bool prettyify)
        {
            fileName = file;
            pretty = prettyify;
        }

        public void Save (InputManager input)
        {
            string json = JsonUtility.ToJson(input, pretty);

            File.WriteAllText(GetPath(), json);
        }

        public InputManager Load ()
        {
            string json = File.ReadAllText(GetPath());
            InputManager input = JsonUtility.FromJson<InputManager>(json);
            
            return input;
        }

        public bool FileExists ()
        {
            return File.Exists(GetPath());
        }

        public string GetPath ()
        {
            string path = Application.dataPath + "/";

#if UNITY_IOS
            path = Application.persistentDataPath + "/";
#elif UNITY_EDITOR
            bool containsAsset = path.Contains("Assets");
            if (!containsAsset)
            {
                path = Application.dataPath + "/Assets/";
            } 
#else
            path = Application.dataPath + "/";
#endif

            return path + fileName + ".json";
        }

    }
}
