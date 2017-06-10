using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables Shadows for 2D Camera
/// </summary>
public class TwoDRender : MonoBehaviour
{

	private float storedShadowDistance;

	private void OnPreRender ()
	{
		storedShadowDistance = QualitySettings.shadowDistance;
		QualitySettings.shadowDistance = 0;
	}

	private void OnPostRender () {
		QualitySettings.shadowDistance = storedShadowDistance;
	}

}
