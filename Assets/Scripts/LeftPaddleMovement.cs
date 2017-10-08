using UnityEngine;

public class LeftPaddleMovement : MonoBehaviour {
    // The paddle as a rigid body.
    private Rigidbody2D rigidBody;

    /**
     * Public variables in order to be set on the inspector
     */
    public float speed = 0.0f;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// FixedUpdate is called once per frame and is to be used with physics.
	void FixedUpdate () {
        float verticalMovement = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(0, verticalMovement);
        rigidBody.velocity = movement * speed;
        rigidBody.position = new Vector2(rigidBody.position.x, Mathf.Clamp(rigidBody.position.y, -4.40f, 4.40f));
	}
}
