using System.Collections;
using UnityEngine;

/**
 * Class      : BallMovement
 * Description: This class will have the ball movement programed in order to make fly arround the screen.
 * Author     : Rodrigo Januario da Silva
 * Version    : 1.0
 */
public class Ball : MonoBehaviour {
    // Constant declaration.
    private const string LEFT_PADDLE_NAME = "LeftPaddle";
    private const string RIGHT_PADDLE_NAME = "RightPaddle";

    // Attributes declararion.
    private Rigidbody2D ballRigidBody;
    private bool canReset = false;
    private bool isWaiting = false;

    // Public variables in order to be set on the inspector
    public float speed = 0.0f;
    public float afterScoreWait = 0.0f;

    // Use this for initialization
    void Start ()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
        ballRigidBody.velocity = Vector2.right * speed;
    }

    // Used to get information after a collision happens.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (CollidedWithPaddle(collision.gameObject.name))
        {
            ballRigidBody.velocity = CalculateVelocity(collision) + collision.rigidbody.velocity;
        }
    }

    // Like update method that will be refreshed once per frame, but to be used with physics.
    void FixedUpdate()
    {
        if(HasAnyPaddleScored())
        {
            // Keep waiting for some milliseconds
            if(!canReset)
            {
                if (!isWaiting)
                {
                    StartCoroutine(WaitResetAfterScore());
                }
            }
            // Waited for the defined time. Let's reset the ball's position.
            else
            {
                ResetBall(GetBallDirectionAfterScore());
            }
        }
    }

    /**
     * Method     : WaitResetAfterScore
     * Return     : An asynchronous delayed time to be waited after a score happens.
     * Description: This method will wait for the specied number of seconds before freein the ball reset.
     */
    IEnumerator WaitResetAfterScore()
    {
        // Let's wait some milliseconds before throwing the ball again.
        isWaiting = true;
        yield return new WaitForSeconds(afterScoreWait);
        canReset = true;
        isWaiting = false;
    }

    /**
     * Method     : ResetBall
     * Param      : <b>ballDirection</b> : The direction (left or right) in which the ball will be thrown.
     * Return     : The direction the ball will start.
     * Description: This method will reset the ball to (0, 0) and throw it in the correct direction after a score.
     */
    void ResetBall(Vector2 ballDirection)
    {
        ballRigidBody.position = new Vector2(0, 0);
        ballRigidBody.velocity = ballDirection * speed;
        canReset = false;
    }

    /**
     * Method     : HasLefPaddletScored
     * Return     : True if the left paddle has scored. False otherwise.
     * Description: This method will check score condition for the left paddle and return accordingly.
     */
    bool HasLefPaddletScored()
    {
        return ballRigidBody.position.x > rightPaddleScript.GetPaddleRightBoundary();
    }

    /**
     * Method     : HasRightPaddleScored
     * Return     : True if the right paddle has scored. False otherwise.
     * Description: This method will check score condition for the right paddle and return accordingly.
     */
    bool HasRightPaddleScored()
    {
        return ballRigidBody.position.x < leftPaddleScript.GetPaddleLeftBoundary();
    }

    /**
     * Method     : HasAnyPaddleScored
     * Return     : True if either the right paddle or the left paddle has scored. False otherwise.
     * Description: This method will check score condition for either the right paddle or the left paddle and return accordingly.
     */
    bool HasAnyPaddleScored()
    {
        return HasLefPaddletScored() || HasRightPaddleScored();
    }

    /**
     * Method     : GetBallDirectionAfterScore
     * Return     : The correct ball direction after a score.
     * Description: This method will check which paddle has scored and return the ball direction accordingly.
     */
    Vector2 GetBallDirectionAfterScore()
    {
        if(HasLefPaddletScored())
        {
            return Vector2.right;
        }

        return Vector2.left;
    }

    /**
     * Method     : CalculateReflectionAngle
     * Param      : <b>ballPosition</b>  : The ball position on the screen.
     * Param      : <b>paddlePosition</b>: The paddle position on the screen.
     * Param      : <b>paddleHeight</b>  : The height of the paddle.
     * Return     : The ball's reflection angle.
     * Description: Calculates the angle the ball should reflect after colliding with a racket. More on the edges the ball hits,
     *              greater the reflection angle will be. We subtract the ball position from the paddle position in order to have
     *              a relative position beteween the ball and the paddle
     * 
     * Thanks to www.nobututs.com
     */
    float CalculateReflectionAngle(Vector2 ballPosition, Vector2 paddlePosition, float paddleHeight)
    {
        return (ballPosition.y - paddlePosition.y) / paddleHeight;
    }

    /**
     * Method     : IsLeftPaddle
     * Param      : <b>gameObjectName</b>: The name of the game object to be checked.
     * Return     : True if it's the left paddle. False otherwise.
     * Description: This method will check the game object name in order to know whether it's the left paddle or not.
     */
    bool IsLeftPaddle(string gameObjectName)
    {
        return LEFT_PADDLE_NAME == gameObjectName;
    }

    /**
     * Method     : IsRightPaddle
     * Param      : <b>gameObjectName</b>: The name of the game object to be checked.
     * Return     : True if it's the right paddle. False otherwise.
     * Description: This method will check the game object name in order to know whether it's the right paddle or not.
     */
    bool IsRightPaddle(string gameObjectName)
    {
        return RIGHT_PADDLE_NAME == gameObjectName;
    }

    /**
     * Method     : CollidedWithPaddle
     * Param      : <b>gameObjectName</b>: The name of the game object to be checked.
     * Return     : True if it's the collision was with any of the paddles. False otherwise.
     * Description: This method will check the game object name in order to know whether it's any of the paddles or not.
     */
    bool CollidedWithPaddle(string gameObjectName)
    {
        return IsLeftPaddle(gameObjectName) || IsRightPaddle(gameObjectName);
    }

    /**
     * Method     : CalculateVelocity
     * Param      : <b>collision</b>   : The ball's collision information.
     * Return     : The ball's vectorial speed.
     * Description: This method will check what paddle was hit, the paddle's hit position and calculate the ball's vectorial velocity.
     */
    Vector2 CalculateVelocity(Collision2D collision)
    {
        // X axis direction always for left or right only.
        float xDirection = IsLeftPaddle(collision.gameObject.name) ? 1.0f : -1.0f;
        float reflctionAngle = CalculateReflectionAngle(transform.position, collision.transform.position, collision.collider.bounds.size.y);
        Vector2 direction = new Vector2(xDirection, reflctionAngle);

        return direction * speed;
    }
}
