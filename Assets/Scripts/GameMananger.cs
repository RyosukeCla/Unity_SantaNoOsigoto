using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMananger : MonoBehaviour {
	public int HP = 5;

	public GameObject player;
	public GameObject house;
	public int house_num;
	public int score;
	public float interval;
	public Text scoreUI;
	public int timeGame;
	private int currentTime;
	public Text TimeText;
	public Text gameEndText;
	public GameObject end;

	public Button button0, button1, button2;
	public Button up, down, right, left, rest;
	public Text[] slot = new Text[3];
	public int slotState;
	public Text HpUI;
	public int HPcure;
	private int maxHp;
	private bool[] isSloting = new bool[3];
	private int[] state = new int[3];
	private Vector3 direction;
	private Vector3 currentPosition;
	private bool actionState;
	private bool isFinished;

	private float timeSlot;
	
	void Start() {
		StartCoroutine (TimeCount());
		currentTime = 0;
		maxHp = HP;
		currentPosition = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		score = 0;
		direction = new Vector3 (0,0,0);
		actionState = false;
		SetIsSlotingAll (false);
		isFinished = false;
		SetHouse ();
	}

	void Update() {
		if (isFinished == true)
			return;
		UpdateText ();
		SlotAct ();
		PlayerAction ();
		this.transform.position = Vector3.Lerp (this.transform.position, currentPosition, 0.2f);
		if (timeGame <= currentTime) {
			currentTime = timeGame;
			if (isFinished == false) {
				StartCoroutine (GameOver());
			}
			isFinished = true;
		}

	}
	
	void OnTriggerEnter(Collider col) {
		//Debug.Log ("hit_");
		if (col.tag == "house") {
			Debug.Log ("hit");
			score += 100;
			Destroy (col.gameObject);
		}
	}

	IEnumerator TimeCount() {
		while (true) {
			if (timeGame <= currentTime) break;
			currentTime ++;
			yield return new WaitForSeconds (1.0f);
		}
	}

	// when game over
	IEnumerator GameOver() {
		end.SetActive (true);
		yield return new WaitForSeconds (10.0f);
		Application.LoadLevel ("title");
	}

	void SetHouse() {
		for (int i = 0; i < house_num; i++) {
			int xpos = Random.Range (-10, 10);
			int zpos = Random.Range (-10, 10);
			Instantiate (house, new Vector3(xpos, 1.0f, zpos), Quaternion.identity);
		}
	}

	// slot no kaiten 
	void SlotAct() {
		timeSlot += Time.deltaTime;
		if (timeSlot > interval) {
			for (int i = 0; i < isSloting.Length; i++) {
				if (isSloting [i] == true) {
					int st = Random.Range (0, slotState);
					string tx = "" + st;
					slot [i].text = tx;
					state [i] = st;
				}
			}
			timeSlot = 0.0f;
		}
	}

	// player no koudou no tokoro
	void PlayerAction() {
		if (actionState == true && isAll (false)) {
			if (isAllSame()) {
				Debug.Log ("Go");
				PlayerActionWithDirection ();
			} else {
				Debug.Log ("zannen");
			}
			HP--;
			actionState = false;
		}

		// when game over
		if (HP == 0) {
			Debug.Log("GameOver");
			if (isFinished == false) {
				StartCoroutine (GameOver());
			}
			isFinished = true;
		}
	}

	void PlayerActionWithDirection() {
		if (direction == new Vector3 (0, 0, 0)) {
			HP += HPcure;
			if (HP > maxHp + 1) HP = maxHp + 1;
		} else {
			currentPosition += direction;
		}
	}

	// update hp text
	public void UpdateText() {
		string tx = "HP" + "\n";
		HpUI.text = tx + HP;
		tx = "Score " + score;
		scoreUI.text = tx;
		tx = "Your Score " + score;
		gameEndText.text = tx;
		int i = timeGame - currentTime;
		tx = "Time " + i;
		TimeText.text = tx;
	}

	public void SetActionState() {
		if (actionState)
			return;
		SetIsSlotingAll (true);
		actionState = true;
	}

	public void SetDirection(string di) {
		if (di == "UP")
			direction = new Vector3 (0, 0, 1.0f);
		else if (di == "DOWN")
			direction = new Vector3 (0, 0, -1.0f);
		else if (di == "RIGHT")
			direction = new Vector3 (1.0f, 0, 0);
		else if (di == "LEFT")
			direction = new Vector3 (-1.0f, 0, 0);
		else if (di == "REST")
			direction = new Vector3 (0, 0, 0);
	}

	public void SetIsSlotingTrue(int num) {
		isSloting [num] = true;
	}

	public void SetIsSlotingFalse(int num) {
		isSloting [num] = false;
	}

	public void ButtonActToIsSloting(int num) {
		if (isSloting [num] == true) {
			isSloting [num] = false;
		}
	}

	public void SetIsSlotingAll(bool tof) {
		for (int i = 0; i < isSloting.Length; i++) {
			isSloting[i] = tof;
		}
	}

	public bool isAll(bool tof) {
		for (int i = 0; i < isSloting.Length; i++) {
			if (isSloting[i] != tof) return false;
		}
		return true;
	}

	public bool isAllSame() {
		int sta = state[0];
		for (int i = 1; i < state.Length; i++) {
			if (state[i] != sta) return false;
		}
		return true;
	}
}
