using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines.ExtrusionShapes;
using UnityEngine.UI;
/// <summary>
/// This class handles the stun minigame
/// Lots of math in here, you have been warned
/// </summary>
public class ShakeOff : MonoBehaviour
{
    // Lets us make certain enemies give more challenging/involved minigames. Progression ig
    public enum ShakeOffDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    [Header("Settings")]
    public float circleRadius = 95f;
    public float playerSpeed = 180f; // Might not be necessary
    public float holdTime = 0.5f;
    public float angleTolerance = 15f;

    [Header("Left Stick UI")]
    public RectTransform leftBackground;
    //public RectTransform leftTarget;
    public RectTransform leftCircleTarget;
    public RectTransform leftPlayer;

    [Header("Right Stick UI")]
    public RectTransform rightBackground;
    //public RectTransform rightTarget;
    public RectTransform rightCircleTarget;
    public RectTransform rightPlayer;

    private float[] easyAngleRange = new float[4] { -90f, 0f, 90f, 180f };

    private float leftTargetAngle;
    private float rightTargetAngle;
    private float leftPlayerAngle;
    private float rightPlayerAngle;
    private float timer;

    private TopDownRigidbodyController player;
    private PlayerInput playerInput;

    public void StartShakeOff(TopDownRigidbodyController playerController, int count, ShakeOffDifficulty diff)
    {
        player = playerController;
        playerInput = player.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput missing!");
            return;
        }

        // Set the size of the targets based on difficulty
        if (diff == ShakeOffDifficulty.Easy)
        {
            // Set targets to semicircles
            leftCircleTarget.GetComponent<Image>().fillAmount = 0.5f;
            rightCircleTarget.GetComponent<Image>().fillAmount = 0.5f;
        }
        else
        {
            // Set targets to be smaller (tweak later)
            leftCircleTarget.GetComponent<Image>().fillAmount = 0.25f;
            rightCircleTarget.GetComponent<Image>().fillAmount = 0.25f;
        }

        // 0 means the target is up, 90 means target is left, 180 means down, 270 means right
        if (diff == ShakeOffDifficulty.Easy) // Consider consolidating later
        {
            // Pick between cardinal directions
            leftTargetAngle = easyAngleRange[Random.Range(0, 4)];
            rightTargetAngle = easyAngleRange[Random.Range(0, 4)];

            Debug.Log($"{leftTargetAngle}");
            Debug.Log($"{rightTargetAngle}");

            leftCircleTarget.eulerAngles = new Vector3(0, 0, leftTargetAngle);
            rightCircleTarget.eulerAngles = new Vector3(0, 0, rightTargetAngle);
        }
        // Continue with code for other difficulties, easy to expand on

        // Old angle target code
        /*
        //leftTargetAngle = Random.Range(0f, 360f);
        //rightTargetAngle = Random.Range(0f, 360f);

        //leftPlayerAngle = 0f;
        //rightPlayerAngle = 0f;

        //UpdateCirclePosition(leftPlayer, leftBackground, leftPlayerAngle);
        //UpdateCirclePosition(leftTarget, leftBackground, leftTargetAngle);
        //UpdateCirclePosition(rightPlayer, rightBackground, rightPlayerAngle);
        //UpdateCirclePosition(rightTarget, rightBackground, rightTargetAngle);
        */

        timer = holdTime;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (player == null || !player.IsStunned()) return;

        // Read control stick input
        Vector2 leftInput = playerInput.actions["MoveLeftStick"].ReadValue<Vector2>();
        Vector2 rightInput = playerInput.actions["MoveRightStick"].ReadValue<Vector2>();

        // Display the position of the control sticks
        leftInput *= circleRadius;
        rightInput *= circleRadius;
        leftPlayer.position = new Vector2(leftInput.x + leftBackground.position.x, leftInput.y + leftBackground.position.y);
        rightPlayer.position = new Vector2(rightInput.x + rightBackground.position.x, rightInput.y + rightBackground.position.y);

        // Old left/right turning code
        /*
        //leftPlayerAngle += leftInput.magnitude * playerSpeed * Time.deltaTime * Mathf.Sign(Vector2.SignedAngle(Vector2.up, leftInput));
        //rightPlayerAngle += rightInput.magnitude * playerSpeed * Time.deltaTime * Mathf.Sign(Vector2.SignedAngle(Vector2.up, rightInput));

        //leftPlayerAngle = Mathf.Repeat(leftPlayerAngle, 360f);
        //rightPlayerAngle = Mathf.Repeat(rightPlayerAngle, 360f);

        //UpdateCirclePosition(leftPlayer, leftBackground, leftPlayerAngle);
        //UpdateCirclePosition(rightPlayer, rightBackground, rightPlayerAngle);
        // Old left/right code ^
        */

        // Change the following to be a certain half/quarter of the circles in order to progress
        // Need to make sure that input vector aligns with target angle somehow, excludes things close to center ideally
        // a

        // Use this
        //Vector2 pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * circleRadius;

        bool leftAligned = Mathf.Abs(Mathf.DeltaAngle(leftPlayerAngle, leftTargetAngle)) <= angleTolerance;
        bool rightAligned = Mathf.Abs(Mathf.DeltaAngle(rightPlayerAngle, rightTargetAngle)) <= angleTolerance;

        if (leftAligned && rightAligned)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = holdTime;
        }

        if (timer <= 0f)
        {
            player.SetStun(false);
            gameObject.SetActive(false);
        }
    }

    // Old update circle method
    /*
    private void UpdateCirclePosition(RectTransform circle, RectTransform background, float angle)
    {
        if (circle == null || background == null) return;
        Vector2 pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * circleRadius;
        circle.anchoredPosition = pos;
    }
    */
}
