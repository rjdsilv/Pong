using UnityEngine;
using Pong;

/**
 * Class      : PaddleMovement
 * Description: This class will have the  paddle movement programed in order to make both paddles moving on the screen.
 * Author     : Rodrigo Januario da Silva
 * Version    : 1.0
 */
public class PaddleMovement : MonoBehaviour {
    // The paddle as a rigid body.
    private Rigidbody2D rigidBody2D;

    /**
     * Public variables in order to be set on the inspector
     */
    public float speed = 0.0f;
    public Boundary boundary;
    public string inputAxisName;

    // Use this for initialization
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called once per frame and is to be used with physics.
    void FixedUpdate()
    {
        float verticalMovement = Input.GetAxis(inputAxisName);
        Vector2 movement = new Vector2(0, verticalMovement);
        rigidBody2D.velocity = movement * speed;
        rigidBody2D.position = new Vector2(rigidBody2D.position.x, Mathf.Clamp(rigidBody2D.position.y, boundary.yMin, boundary.yMax));
    }
}
