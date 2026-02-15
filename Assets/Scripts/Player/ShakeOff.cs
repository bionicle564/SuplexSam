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
    [Header("Settings")]
    public float circleRadius = 100f;     
    public float playerSpeed = 180f;      
    public float holdTime = 0.5f;
    public float angleTolerance = 15f;

    [Header("Left Stick UI")]
    public RectTransform leftBackground;
    public RectTransform leftTarget;
    public RectTransform leftPlayer;

    [Header("Right Stick UI")]
    public RectTransform rightBackground;
    public RectTransform rightTarget;
    public RectTransform rightPlayer;

    private float leftTargetAngle;
    private float rightTargetAngle;
    private float leftPlayerAngle;
    private float rightPlayerAngle;
    private float timer;

    private TopDownRigidbodyController player;
    private PlayerInput playerInput;

    public void StartShakeOff(TopDownRigidbodyController playerController)
    {
        player = playerController;
        playerInput = player.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput missing!");
            return;
        }

        // Look into changing this
        leftTargetAngle = Random.Range(0f, 360f);
        rightTargetAngle = Random.Range(0f, 360f);

        leftPlayerAngle = 0f;
        rightPlayerAngle = 0f;

        //UpdateCirclePosition(leftPlayer, leftBackground, leftPlayerAngle);
        //UpdateCirclePosition(leftTarget, leftBackground, leftTargetAngle);
        //UpdateCirclePosition(rightPlayer, rightBackground, rightPlayerAngle);
        //UpdateCirclePosition(rightTarget, rightBackground, rightTargetAngle);

        timer = holdTime;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (player == null || !player.IsStunned()) return;

        Vector2 leftInput = playerInput.actions["MoveLeftStick"].ReadValue<Vector2>();
        Vector2 rightInput = playerInput.actions["MoveRightStick"].ReadValue<Vector2>();

        // New code
        leftInput *= 100f;
        leftPlayer.position = new Vector2(leftInput.x, leftInput.y);
        rightPlayer.position = new Vector2(rightInput.x, rightInput.y);

        // Old left/right code
        //leftPlayerAngle += leftInput.magnitude * playerSpeed * Time.deltaTime * Mathf.Sign(Vector2.SignedAngle(Vector2.up, leftInput));
        //rightPlayerAngle += rightInput.magnitude * playerSpeed * Time.deltaTime * Mathf.Sign(Vector2.SignedAngle(Vector2.up, rightInput));

        //leftPlayerAngle = Mathf.Repeat(leftPlayerAngle, 360f);
        //rightPlayerAngle = Mathf.Repeat(rightPlayerAngle, 360f);

        //UpdateCirclePosition(leftPlayer, leftBackground, leftPlayerAngle);
        //UpdateCirclePosition(rightPlayer, rightBackground, rightPlayerAngle);
        // Old left/right code ^

        // Look into changing the as well
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

    /*
    private void UpdateCirclePosition(RectTransform circle, RectTransform background, float angle)
    {
        if (circle == null || background == null) return;
        Vector2 pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * circleRadius;
        circle.anchoredPosition = pos;
    }
    */

    private void UpdateCirclePosition(RectTransform circle, float angle)
    {
        float tempLength = 100f;
        Vector2 pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * circleRadius;
    }
}