using UnityEngine;

public class GameOverController : MonoBehaviour {
    private GUIText winText;

	// Use this for initialization
	void Start ()
    {
        winText = GetComponent<GUIText>();
        winText.text = string.Format(winText.text, PlayerPrefs.GetInt("winner"));
        winText.transform.SetPositionAndRotation(new Vector3(0.5f, 0.5f), new Quaternion());
	}
}
