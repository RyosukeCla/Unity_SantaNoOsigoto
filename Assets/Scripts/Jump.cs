using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	public void GoToScene(string scene) {
		Application.LoadLevel (scene);
	}
}
