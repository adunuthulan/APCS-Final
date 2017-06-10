using UnityEngine;
using System.Collections.Generic;

namespace CustomInputManager
{
    public class CustomInput : MonoBehaviour
    {
        [SerializeField]
        private static string saveFile;

        private static IO io;
        public static InputManager input;

        // used for GetAxis
        private static List<Axis> axisList = new List<Axis>();
        
        
        void Awake ()
        {
            // If save doesn't have a name, set it to controls
            if (saveFile == null)
                SetSaveName("controls");

            io = new IO(saveFile, true);
            input = new InputManager();
        }

        void Update ()
        {
            // Update OnAxis Values
            // If we are checking for more than one axis
            if (axisList.Count != 0)
            {
                foreach (Axis a in axisList)
                {
                    if (OnKey(a.positive.name))
                    {
                        a.value = 1f;
                    }
                    else if (OnKey(a.negative.name))
                    {
                        a.value = -1f;
                    }
                    else
                    {
                        a.value = 0f;
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the given key is currently pressed
        /// </summary>
        public static bool OnKey (string name)
        {
            // Check if key exists
            if (!input.KeyExists(name))
            {
                Debug.LogError("Key doesn't exist!");
                return false;
            }

            // Test for primary key
            if (Input.GetKey(input.GetKey(name).primaryKey))
            {
                return true;
            }


            // If we have a secondary key
            if (input.GetKey(name).secondaryKey != null)
            {
                // Test for secondary key
                if (Input.GetKey((KeyCode)input.GetKey(name).secondaryKey))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true the first frame that a key is released
        /// </summary>
        public static bool OnKeyUp (string name)
        {
            // Check if key exists
            if (!input.KeyExists(name))
            {
                Debug.LogError("Key doesn't exist!");
                return false;
            }

            // Test for primary key
            if (Input.GetKeyUp(input.GetKey(name).primaryKey))
            {
                return true;
            }

            // If we have a secondary key
            if (input.GetKey(name).secondaryKey != null)
            {
                // Test for secondary key
                if (Input.GetKeyUp((KeyCode)input.GetKey(name).secondaryKey))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true the first frame that a key is pressed
        /// </summary>
        public static bool OnKeyDown (string name)
        {
            // Check if key exists
            if (!input.KeyExists(name))
            {
                Debug.LogError("Key doesn't exist!");
                return false;
            }

            // Test for primary key
            if (Input.GetKeyDown(input.GetKey(name).primaryKey))
            {
                return true;
            }

            // If we have a secondary key
            if (input.GetKey(name).secondaryKey != null)
            {
                // Test for secondary key
                if (Input.GetKeyDown((KeyCode)input.GetKey(name).secondaryKey))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns -1, 0, 1 based on the key being pressed (0 = unpressed) using the axis name
        /// </summary>
        public static float OnAxis (string name)
        {
            if (name == "MouseWheel")
            {
                return Input.GetAxis("Mouse ScrollWheel");
            }

            Axis axis = input.GetAxis(name);

            // Check if they are already assigned
            if (!axisList.Contains(axis))
            {
                axisList.Add(axis);
            }

            return axisList[axisList.IndexOf(axis)].value;
        }

        /// <summary>
        /// Save the current input configs to a file, bool is whether or not to overwrite if already exists
        /// </summary>
        public static void SaveConfig (bool overwrite)
        {
            // If the file already exists but we aren't overwriting then don't do it
            if (io.FileExists() && overwrite == false)
                return;

            io.Save(input);
        }

        /// <summary>
        /// Load (and import) the input configs from the default file
        /// </summary>
        public static void LoadConfig ()
        {
            input = io.Load();
        }

        /// <summary>
        /// Set the name of the file to be used (don't include .json)
        /// </summary>
        public static void SetSaveName (string name)
        {
            saveFile = name;
        }

    }
}
