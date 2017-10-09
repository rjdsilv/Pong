using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Class      : GameController
 * Description: This class will control all the game logic including ball reset position and the
 *              game win condition.
 * Author     : Rodrigo Januario da Silva
 * Version    : 1.0
 */
public class GameController : MonoBehaviour {
    // Attributes declaration.
    private PaddleController leftPaddleController;
    private PaddleController rightPaddleController;
    private BallController ballController;
    private GUIText leftScore;
    private GUIText rightScore;

    // Control attributes declaration.
    private bool canReset = false;
    private bool isWaiting = false;
    private int leftPaddleScore = 0;
    private int rightPaddleScore = 0;

    // public attributes to apear on the script properties.
    public int winScore = 0;
    public float afterScoreWait = 0.0f;

    // Use this for initialization
    void Start () {
        // Paddle scripts.
        leftPaddleController = GameObject.Find(PaddleController.LEFT_PADDLE_NAME).GetComponent<PaddleController>();
        rightPaddleController = GameObject.Find(PaddleController.RIGHT_PADDLE_NAME).GetComponent<PaddleController>();

        // Ball script.
        ballController = GameObject.Find(BallController.BALL_NAME).GetComponent<BallController>();

        // Score texts.
        leftScore = GameObject.Find("LeftScore").GetComponent<GUIText>();
        rightScore = GameObject.Find("RightScore").GetComponent<GUIText>();

        // Let's set up the score position for each paddle.
        leftScore.transform.SetPositionAndRotation(new Vector3(0.25f, 0.8f, 1.0f), new Quaternion());
        rightScore.transform.SetPositionAndRotation(new Vector3(0.75f, 0.8f, 1.0f), new Quaternion());

        // Resets the player preferences.
        PlayerPrefs.DeleteAll();
    }

    // Like update method that will be refreshed once per frame, but to be used with physics.
    void FixedUpdate()
    {
        if (HasAnyPaddleScored())
        {
            if (!IsGameOver())
            {
                // Keep waiting for some milliseconds
                if (!canReset)
                {
                    if (!isWaiting)
                    {
                        IncreaseProperPaddleScore();
                        StartCoroutine(WaitResetAfterScore());
                    }
                }
                // Waited for the defined time. Let's reset the ball's position.
                else
                {
                    SetBallResetDirectionAfterScore();
                    ballController.ResetBall();
                    canReset = false;
                }
            }
            else
            {
                GameOver();
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
     * Method     : SetBallResetDirectionAfterScore
     * Return     : Sets, after a score, the direction the ball will restart.
     * Description: This method will check which paddle has scored in order to set the ball restart direction properly..
     */
    void SetBallResetDirectionAfterScore()
    {
        if(HasLeftPaddletScored())
        {
            ballController.SetBallStartDirection(Vector2.right);
        }
        else
        {
            ballController.SetBallStartDirection(Vector2.left);
        }
    }

    /**
     * Method     : HasLefPaddletScored
     * Return     : True if the left paddle has scored. False otherwise.
     * Description: This method will check score condition for the left paddle and return accordingly.
     */
    bool HasLeftPaddletScored()
    {
        if (ballController.GetBallPositionX() >= rightPaddleController.GetPaddleRightBoundary())
        {
            return true;
        }
        return false;

    }

    /**
     * Method     : HasRightPaddleScored
     * Return     : True if the right paddle has scored. False otherwise.
     * Description: This method will check score condition for the right paddle and return accordingly.
     */
    bool HasRightPaddleScored()
    {
        if (ballController.GetBallPositionX() <= leftPaddleController.GetPaddleLeftBoundary())
        {
            return true;
        }
        return false;
    }

    /**
     * Method     : HasAnyPaddleScored
     * Return     : True if either the right paddle or the left paddle has scored. False otherwise.
     * Description: This method will check score condition for either the right paddle or the left paddle and return accordingly.
     */
    bool HasAnyPaddleScored()
    {
        return HasLeftPaddletScored() || HasRightPaddleScored();
    }

    /**
     * Method     : IsGameOver
     * Return     : True if either the left or the right paddle has achieved the win condition. False otherwise
     * Description: This method will check if any of the paddles has achieved the win condition and return accordingly.
     */
    bool IsGameOver()
    {
        return LeftPaddleWon() || RightPaddleWon();
    }

    /**
     * Method     : LeftPaddleWon
     * Return     : True if the left paddle has achieved the win condition. False otherwise
     * Description: This method will check if the left paddle has achieved the win condition and return accordingly.
     */
    bool LeftPaddleWon()
    {
        return leftPaddleScore == winScore;
    }

    /**
     * Method     : RightPaddleWon
     * Return     : True if the right paddle has achieved the win condition. False otherwise
     * Description: This method will check if the right paddle has achieved the win condition and return accordingly.
     */
    bool RightPaddleWon()
    {
        return rightPaddleScore == winScore;
    }

    /**
     * Method     : IncreaseProperPaddleScore
     * Description: This method will check if any of the paddles has scored ahd increase its score accordingly.
     */
    void IncreaseProperPaddleScore()
    {
        if(HasLeftPaddletScored())
        {
            leftPaddleScore++;
            leftScore.text = leftPaddleScore.ToString();
        }
        else if(HasRightPaddleScored())
        {
            rightPaddleScore++;
            rightScore.text = rightPaddleScore.ToString();
        }
    }

    /**
     * Method     : GameOver
     * Description: This method will store the winner player and store the next scene.
     */
    void GameOver()
    {
        // If the key exists, remove it first.
        if (PlayerPrefs.HasKey("winner"))
        {
            PlayerPrefs.DeleteKey("winner");
        }

        // Determines the winner to be stored.
        if (LeftPaddleWon())
        {
            PlayerPrefs.SetInt("winner", 1);
        }
        else
        {
            PlayerPrefs.SetInt("winner", 2);
        }

        SceneManager.LoadScene("GameOver");
    }
}
