using JetBrains.Annotations;
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
    public float holdTime = 0.2f;
    public LayerMask uiMask; // For overlapping the needle and target
    public bool shakeOffInProgress = false;

    [Header("Left Stick UI")]
    public RectTransform leftBackground;
    public RectTransform leftCircleTargetEasy;
    public RectTransform leftCircleTargetMedium;
    public RectTransform leftCircleTargetHard;
    public RectTransform leftPlayer;

    [Header("Right Stick UI")]
    public RectTransform rightBackground;
    public RectTransform rightCircleTargetEasy;
    public RectTransform rightCircleTargetMedium;
    public RectTransform rightCircleTargetHard;
    public RectTransform rightPlayer;

    // Inner workings
    private float[] easyAngleRange = new float[4] { -90f, 0f, 90f, 180f };
    private float[] mediumAngleRange = new float[8] { -135f, -90f, -45f, 0f, 45f, 90f, 135f, 180f };
    private float[] hardAngleRange = new float[12] { -150f, -120f, -90f, -60f, -30f, 0f, 30f, 60f, 90f, 120f, 150f, 180f };

    private float leftTargetAngle;
    private float rightTargetAngle;
    private float timer;

    private TopDownRigidbodyController player;
    private PlayerInput playerInput;

    private ShakeOffDifficulty currentDifficulty;
    private int qteSteps = 1;
    private int currentStep = 0;

    private Animator anim;

    public void StartShakeOff(TopDownRigidbodyController playerController, ShakeOffDifficulty diff)
    {
        leftBackground.gameObject.SetActive(true);
        rightBackground.gameObject.SetActive(true);
        shakeOffInProgress = true;

        anim = GetComponent<Animator>();
        anim.SetTrigger("Start");

        player = playerController;
        playerInput = player.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput missing!");
            return;
        }

        // Startup
        currentDifficulty = diff;
        currentStep = 0;

        leftCircleTargetEasy.GetComponent<Image>().color = Color.white;
        rightCircleTargetEasy.GetComponent<Image>().color = Color.white;
        leftCircleTargetMedium.GetComponent<Image>().color = Color.white;
        rightCircleTargetMedium.GetComponent<Image>().color = Color.white;
        leftCircleTargetHard.GetComponent<Image>().color = Color.white;
        rightCircleTargetHard.GetComponent<Image>().color = Color.white;

        // Toggle different size of targets based on difficulty
        if (currentDifficulty == ShakeOffDifficulty.Easy)
        {
            qteSteps = 1;
            leftCircleTargetEasy.gameObject.SetActive(true);
            rightCircleTargetEasy.gameObject.SetActive(true);
            leftCircleTargetMedium.gameObject.SetActive(false);
            rightCircleTargetMedium.gameObject.SetActive(false);
            leftCircleTargetHard.gameObject.SetActive(false);
            rightCircleTargetHard.gameObject.SetActive(false);
        }
        else if (currentDifficulty == ShakeOffDifficulty.Medium)
        {
            qteSteps = 2;
            leftCircleTargetEasy.gameObject.SetActive(false);
            rightCircleTargetEasy.gameObject.SetActive(false);
            leftCircleTargetMedium.gameObject.SetActive(true);
            rightCircleTargetMedium.gameObject.SetActive(true);
            leftCircleTargetHard.gameObject.SetActive(false);
            rightCircleTargetHard.gameObject.SetActive(false);
        }
        else if (currentDifficulty == ShakeOffDifficulty.Hard)
        {
            qteSteps = 3;
            leftCircleTargetEasy.gameObject.SetActive(false);
            rightCircleTargetEasy.gameObject.SetActive(false);
            leftCircleTargetMedium.gameObject.SetActive(false);
            rightCircleTargetMedium.gameObject.SetActive(false);
            leftCircleTargetHard.gameObject.SetActive(true);
            rightCircleTargetHard.gameObject.SetActive(true);
        }

        SetTargetAngles();

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

    private void SetTargetAngles()
    {
        // 0 means the target is up, 90 means target is left, 180 means down, 270 means right
        if (currentDifficulty == ShakeOffDifficulty.Easy) // Consider consolidating later
        {
            // Pick between cardinal directions
            leftTargetAngle = easyAngleRange[Random.Range(0, 4)];
            rightTargetAngle = easyAngleRange[Random.Range(0, 4)];

            leftCircleTargetEasy.eulerAngles = new Vector3(0, 0, leftTargetAngle);
            rightCircleTargetEasy.eulerAngles = new Vector3(0, 0, rightTargetAngle);
        }
        else if (currentDifficulty == ShakeOffDifficulty.Medium)
        {
            // Pick between intercardinal directions
            leftTargetAngle = mediumAngleRange[Random.Range(0, 8)];
            rightTargetAngle = mediumAngleRange[Random.Range(0, 8)];

            leftCircleTargetMedium.eulerAngles = new Vector3(0, 0, leftTargetAngle);
            rightCircleTargetMedium.eulerAngles = new Vector3(0, 0, rightTargetAngle);
        }
        else if (currentDifficulty == ShakeOffDifficulty.Hard)
        {
            // Pick between intercardinal directions
            leftTargetAngle = hardAngleRange[Random.Range(0, 8)];
            rightTargetAngle = hardAngleRange[Random.Range(0, 8)];

            leftCircleTargetHard.eulerAngles = new Vector3(0, 0, leftTargetAngle);
            rightCircleTargetHard.eulerAngles = new Vector3(0, 0, rightTargetAngle);
        }
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

        // Old alignment code
        //bool leftAligned = Mathf.Abs(Mathf.DeltaAngle(leftPlayerAngle, leftTargetAngle)) <= angleTolerance;
        //bool rightAligned = Mathf.Abs(Mathf.DeltaAngle(rightPlayerAngle, rightTargetAngle)) <= angleTolerance;
        */

        if (currentDifficulty == ShakeOffDifficulty.Easy)
        {
            EasyShakeOff();
        }
        else if (currentDifficulty == ShakeOffDifficulty.Medium)
        {
            MediumShakeOff();
        }
        else if (currentDifficulty == ShakeOffDifficulty.Hard)
        {
            HardShakeOff();
        }
    }

    private void EasyShakeOff()
    {
        bool leftAligned = false;
        bool rightAligned = false;

        // Check left
        if (Physics2D.OverlapCircle(leftPlayer.position, 8f) != null)
        {
            if (Physics2D.OverlapCircle(leftPlayer.position, 8f).gameObject == leftCircleTargetEasy.gameObject) // On target
            {
                leftAligned = true;
                leftCircleTargetEasy.GetComponent<Image>().color = Color.darkGreen;
            }
            else // Hitting something else (shouldn't happen)
            {
                leftAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
                leftCircleTargetEasy.GetComponent<Image>().color = Color.white;
            }
        }
        else if (Physics2D.OverlapCircle(leftPlayer.position, 8f) == null) // Off target
        {
            leftAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
            leftCircleTargetEasy.GetComponent<Image>().color = Color.white;
        }

        // Check right
        if (Physics2D.OverlapCircle(rightPlayer.position, 8f) != null)
        {
            if (Physics2D.OverlapCircle(rightPlayer.position, 8f).gameObject == rightCircleTargetEasy.gameObject) // On target
            {
                rightAligned = true;
                rightCircleTargetEasy.GetComponent<Image>().color = Color.darkGreen;
            }
            else // Hitting something else (shouldn't happen)
            {
                rightAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
                rightCircleTargetEasy.GetComponent<Image>().color = Color.white;
            }
        }
        else if (Physics2D.OverlapCircle(rightPlayer.position, 8f) == null) // Off target
        {
            rightAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
            rightCircleTargetEasy.GetComponent<Image>().color = Color.white;
        }

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
            // Some kind of feedback

            // Change to progress the stun QTE
            currentStep += 1;
            if (currentStep >= qteSteps)
            {
                // Call end animation
                anim.SetTrigger("End");
                timer = 1f; // Stops the trigger from being spammed
            }
            else
            {
                anim.SetTrigger("Bounce");
                timer = holdTime;
                SetTargetAngles();
            }
        }
    }

    private void MediumShakeOff()
    {
        bool leftAligned = false;
        bool rightAligned = false;

        // Check left
        if (Physics2D.OverlapCircle(leftPlayer.position, 8f) != null)
        {
            if (Physics2D.OverlapCircle(leftPlayer.position, 8f).gameObject == leftCircleTargetMedium.gameObject) // On target
            {
                leftAligned = true;
                leftCircleTargetMedium.GetComponent<Image>().color = Color.darkGreen;
            }
            else // Hitting something else (shouldn't happen)
            {
                leftAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
                leftCircleTargetMedium.GetComponent<Image>().color = Color.white;
            }
        }
        else if (Physics2D.OverlapCircle(leftPlayer.position, 8f) == null) // Off target
        {
            leftAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
            leftCircleTargetMedium.GetComponent<Image>().color = Color.white;
        }

        // Check right
        if (Physics2D.OverlapCircle(rightPlayer.position, 8f) != null)
        {
            if (Physics2D.OverlapCircle(rightPlayer.position, 8f).gameObject == rightCircleTargetMedium.gameObject) // On target
            {
                rightAligned = true;
                rightCircleTargetMedium.GetComponent<Image>().color = Color.darkGreen;
            }
            else // Hitting something else (shouldn't happen)
            {
                rightAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
                rightCircleTargetMedium.GetComponent<Image>().color = Color.white;
            }
        }
        else if (Physics2D.OverlapCircle(rightPlayer.position, 8f) == null) // Off target
        {
            rightAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
            rightCircleTargetMedium.GetComponent<Image>().color = Color.white;
        }

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
            // Some kind of feedback

            // Change to progress the stun QTE
            currentStep += 1;
            if (currentStep >= qteSteps)
            {
                // Call end animation
                anim.SetTrigger("End");
                timer = 1f; // Stops the trigger from being spammed
            }
            else
            {
                anim.SetTrigger("Bounce");
                timer = holdTime;
                SetTargetAngles();
            }
        }
    }

    private void HardShakeOff()
    {
        bool leftAligned = false;
        bool rightAligned = false;

        // Check left
        if (Physics2D.OverlapCircle(leftPlayer.position, 8f) != null)
        {
            if (Physics2D.OverlapCircle(leftPlayer.position, 8f).gameObject == leftCircleTargetHard.gameObject) // On target
            {
                leftAligned = true;
                leftCircleTargetHard.GetComponent<Image>().color = Color.darkGreen;
            }
            else // Hitting something else (shouldn't happen)
            {
                leftAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
                leftCircleTargetHard.GetComponent<Image>().color = Color.white;
            }
        }
        else if (Physics2D.OverlapCircle(leftPlayer.position, 8f) == null) // Off target
        {
            leftAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
            leftCircleTargetHard.GetComponent<Image>().color = Color.white;
        }

        // Check right
        if (Physics2D.OverlapCircle(rightPlayer.position, 8f) != null)
        {
            if (Physics2D.OverlapCircle(rightPlayer.position, 8f).gameObject == rightCircleTargetHard.gameObject) // On target
            {
                rightAligned = true;
                rightCircleTargetHard.GetComponent<Image>().color = Color.darkGreen;
            }
            else // Hitting something else (shouldn't happen)
            {
                rightAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
                rightCircleTargetHard.GetComponent<Image>().color = Color.white;
            }
        }
        else if (Physics2D.OverlapCircle(rightPlayer.position, 8f) == null) // Off target
        {
            rightAligned = false; // Likely unnecessary but I'm keeping the block in case I want to use it later
            rightCircleTargetHard.GetComponent<Image>().color = Color.white;
        }

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
            // Some kind of feedback

            // Change to progress the stun QTE
            currentStep += 1;
            if (currentStep >= qteSteps)
            {
                // Call end animation
                anim.SetTrigger("End");
                timer = 1f; // Stops the trigger from being spammed
            }
            else
            {
                anim.SetTrigger("Bounce");
                timer = holdTime;
                SetTargetAngles();
            }
        }
    }

    public void EndShakeOff()
    {
        player.SetStun(false);
        leftBackground.gameObject.SetActive(false);
        rightBackground.gameObject.SetActive(false);
        shakeOffInProgress = false;
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
