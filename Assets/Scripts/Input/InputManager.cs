using System.Collections.Generic;
using UnityEngine;
using System;

namespace CustomInputManager
{
    public class InputManager
    {
        [SerializeField]
        private List<Key> input = new List<Key>();

        [SerializeField]
        private List<Axis> axisList = new List<Axis>();

        /// <summary>
        /// Adds a key to the list
        /// </summary>
        public void AddKey (Key key)
        {
            input.Add(key);
        }

        /// <summary>
        /// Removes any key with the same object
        /// </summary>
        public void RemoveKey (Key key)
        {
            input.Remove(key);
        }

        /// <summary>
        /// Removes any key with the same name
        /// </summary>
        public void RemoveKey (string name)
        {
            if (KeyExists(name))
            {
                input.Remove(GetKey(name));
            }
        }

        /// <summary>
        /// Gets a Key Object with the specific name
        /// </summary>
        public Key GetKey (string name)
        {
            foreach (Key k in input)
            {
                if (k.name == name)
                    return k;
            }

            return null;
        }

        /// <summary>
        /// Returns whether or not the given key exists
        /// </summary>
        public bool KeyExists (string name)
        {
            return (GetKey(name) != null);
        }

        /// <summary>
        /// Adds an axis to the list
        /// </summary>
        public void AddAxis (Axis axis)
        {
            axisList.Add(axis);
        }

        /// <summary>
        /// Removes any axis with the same object
        /// </summary>
        public void RemoveAxis (Axis axis)
        {
            axisList.Remove(axis);
        }

        /// <summary>
        /// Removes any key with the same name
        /// </summary>
        public void RemoveAxis (string name)
        {
            if (AxisExists(name))
            {
                axisList.Remove(GetAxis(name));
            }
        }

        /// <summary>
        /// Gets an Axis Object with the specific name
        /// </summary>
        public Axis GetAxis (string name)
        {
            foreach (Axis k in axisList)
            {
                if (k.name == name)
                    return k;
            }

            return null;
        }

        /// <summary>
        /// Returns whether or not the given axis exists
        /// </summary>
        public bool AxisExists (string name)
        {
            return (GetAxis(name) != null);
        }

        public void Reset ()
        {
            input = new List<Key>();
            axisList = new List<Axis>();
        }
        
        public void Import (Key[] inputArr, Axis[] axisArr, bool reset)
        {
            if (reset)
                Reset();

            for (int i = 0; i < inputArr.Length; i++)
            {
                AddKey(inputArr[i]);
            }

            for (int i = 0; i < axisArr.Length; i++)
            {
                AddAxis(axisArr[i]);
            }
        }


    }
    
    [Serializable]
    public class Key {
        public KeyCode primaryKey;
        public KeyCode? secondaryKey; // Optional
        public string name;

        public Key (string n, KeyCode prim)
        {
            name = n;
            primaryKey = prim;
            secondaryKey = null;
        }

        public Key (string n, KeyCode prim, KeyCode sec)
        {
            name = n;
            primaryKey = prim;
            secondaryKey = sec;
        }

        
        public override string ToString ()
        {
            return "Key " + name + " " + primaryKey + " " + secondaryKey;
        }
    }

    [Serializable]
    public class Axis
    {
        public Key positive;
        public Key negative;
        public string name;

        [NonSerialized] // Don't need to serialize this as it is only used internally
        public float value;


        /// <summary>
        /// Creates an axis object that can be used for the OnAxis method
        /// </summary>
        /// <param name="_name">Name of the axis</param>
        /// <param name="pos">Name of the positive key to use as 1</param>
        /// <param name="neg">Name of the negative key to use as -1</param>
        public Axis (string _name, string pos, string neg)
        {
            // Check if it exists
            if (!CustomInput.input.KeyExists(pos) && !CustomInput.input.KeyExists(neg))
            {
                Debug.LogError("One of the keys in axis " + _name + " does not exist in the CustomInput.input!");
            }

            name = _name;
            positive = CustomInput.input.GetKey(pos);
            negative = CustomInput.input.GetKey(neg);
            value = 0f;
        }

        public bool Equals (Axis item)
        {
            return (positive == item.positive && negative == item.negative);
        }

        public override bool Equals (object obj)
        {
            return Equals(obj as Axis);
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode(); 
        }
    }
}
