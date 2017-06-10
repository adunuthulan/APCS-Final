using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInputManager;

public class DimensionManager : MonoBehaviour {

	public static DimensionManager instance;

	public enum Dimension { ThreeD, TwoD };

	public Dimension currentDimension;

	public delegate void ChangeDimension(Dimension newDimension);

	// Event listener, calls methods when the dimension changes
	public ChangeDimension OnChangeDimension;

	private void Awake () {
		if (instance != null) 
			Debug.LogError("There are more than one DimensionManagers in the scene!");
		else 
			instance = this;
	}

	private void Start () {
		currentDimension = Config.START_DIMENSION;

		if (OnChangeDimension != null)
			OnChangeDimension.Invoke(currentDimension);
	}

	private void Update () {
		// When the player presses Swap Key swap
		if (CustomInput.OnKeyDown("Swap"))
			SwapDimension();
	}

	public void SwapDimension () {
		if (currentDimension == Dimension.ThreeD)
			currentDimension = Dimension.TwoD;
		else
			currentDimension = Dimension.ThreeD;

		if (OnChangeDimension != null)
			OnChangeDimension.Invoke(currentDimension);
	}

	public void SetDimension (Dimension dm) {
		currentDimension = dm;

		if (OnChangeDimension != null)
			OnChangeDimension.Invoke(currentDimension);
	}
		
}
