using UnityEngine;

namespace CustomInputManager {

    public class DefaultInputs : MonoBehaviour {
        public void Start()
        {

            // Menus
            CustomInput.input.AddKey(new Key("Pause", KeyCode.Escape, KeyCode.P));

            // Movements
            CustomInput.input.AddKey(new Key("Forward", KeyCode.W, KeyCode.UpArrow));
            CustomInput.input.AddKey(new Key("Back", KeyCode.S, KeyCode.DownArrow));
            CustomInput.input.AddKey(new Key("Left", KeyCode.A, KeyCode.LeftArrow));
            CustomInput.input.AddKey(new Key("Right", KeyCode.D, KeyCode.RightArrow));

            CustomInput.input.AddKey(new Key("Run", KeyCode.LeftShift, KeyCode.R));

            CustomInput.input.AddKey(new Key("Jump", KeyCode.Space));

            CustomInput.input.AddAxis(new Axis("Horizontal", "Right", "Left"));
            CustomInput.input.AddAxis(new Axis("Vertical", "Forward", "Back"));

            // Swapping Dimensions
            CustomInput.input.AddKey(new Key("Swap", KeyCode.Q));

            // Restarting the Level
            CustomInput.input.AddKey(new Key("Restart", KeyCode.R));



            CustomInput.SaveConfig(true);
        }
    }
}
