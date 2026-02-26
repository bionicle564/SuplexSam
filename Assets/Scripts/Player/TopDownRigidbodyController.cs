/// Sorry for the AI code its what Tom gave us, goodluck learning C# or just make a new one ¯\_(ツ)_/¯

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Top-down rigidbody controller that uses AddForce with impulse force mode for movement.
/// Follows the InputSystem integration pattern using PlayerInput component.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TopDownRigidbodyController : MonoBehaviour
{
    [Header("Input References")]
    [Tooltip("Reference to the movement input action.")]
    [SerializeField] private InputActionReference moveActionReference;

    [Header("Movement Settings")]
    [Tooltip("Force multiplier for movement acceleration.")]
    [SerializeField] private float moveForce = 50f;

    [Tooltip("Maximum velocity the player can reach.")]
    [SerializeField] private float maxSpeed = 10f;

    [Tooltip("Drag applied to slow down when no input is provided.")]
    [SerializeField] private float dragCoefficient = 5f;

    [Tooltip("Speed at which the player rotates to face movement direction.")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Debug")]
    [Tooltip("Show debug information in the console.")]
    [SerializeField] private bool debugMode = false;
    
    [Header("Stun")]
    [SerializeField] private InputActionReference stunActionReference;
    [SerializeField] private float spinSpeed = 720f;
    
    public ShakeOff shakeOff;

    private InputAction stunAction;
    private bool stunned = false;

    // Components
    private Rigidbody rb;
    private PlayerInput playerInput;

    // Input actions
    private InputAction moveAction;

    // Input state
    private Vector2 moveInput;

    private void Awake()
    {
        // Get required components
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found!", this);
            return;
        }

        // Get PlayerInput from the same GameObject
        playerInput = GetComponent<PlayerInput>();
        
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on this GameObject!", this);
        }

        // Configure Rigidbody for top-down movement
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnEnable()
    {
        if (playerInput == null) return;

        // Get the move action using the ID from InputActionReference
        // This is the CRITICAL pattern from the InputSystem integration guide
        moveAction = playerInput.actions.FindAction(moveActionReference.action.id);

        if (moveAction == null)
        {
            Debug.LogError($"Move action not found! Action ID: {moveActionReference.action.id}", this);
            return;
        }

        // Subscribe to input events
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;

        if (debugMode)
        {
            Debug.Log($"TopDownRigidbodyController: Move action found and subscribed. Action ID: {moveActionReference.action.id}");
        }
        
        stunAction = playerInput.actions.FindAction(stunActionReference.action.id);

		if (stunAction != null)
		{
		    stunAction.performed += OnStunPressed;
		}
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        if (moveAction != null)
        {
            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMoveCanceled;
        }
        
        if (stunAction != null)
        {
        	stunAction.performed -= OnStunPressed;
        }

    }
    
    //Changes stun state
    public void ToggleStun()
	{
	    stunned = !stunned;
	}
	
	public void SetStun(bool value)
	{
	    stunned = value;
	}
	
	public bool IsStunned()
	{
	    return stunned;
	}
	    
    private void OnStunPressed(InputAction.CallbackContext context)
	{
        //this is the code that runs once when the player gets stunned
	    ToggleStun();
	}

    private void Update()
    {
    
        //this happans once per frame when stunned
	    if (stunned)
		{
		    // Spin in place
		    transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
		
		    // Kill movement input
		    moveInput = Vector2.zero;
		    return;
		}
    
        // Read continuous input values
        //don't worry about this
        if (moveAction != null && moveAction.enabled)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
    
    if (stunned)
	{
	    rb.linearVelocity = Vector3.zero; //halt the player when stunned
	    return;
	}
    
        if (rb == null) return;

        //rb.AddForce(Vector3.down*10, ForceMode.Force);

        ApplyMovement(); //move first
        ApplyRotation(); //turn second
        ApplyDrag(); //slow down (ridged body does this anyway?)(AI wackness)
        ClampVelocity(); //don't go past max speed
        rb.AddForce(Vector3.down * 10, ForceMode.Force);
    }

    /// <summary>
    /// Applies force to the rigidbody based on input using impulse force mode.
    /// </summary>
    private void ApplyMovement()
    {
        if (moveInput == Vector2.zero) return;

        // Convert 2D input to 3D world space (X and Z axes for top-down)
        Vector3 movementDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        movementDirection = Quaternion.AngleAxis(-45f, new Vector3(0, 1, 0)) * movementDirection;
        

        // Normalize to prevent faster diagonal movement
        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }

        // Apply force using impulse mode for responsive movement
        // ForceMode.Impulse applies an instantaneous force
        Vector3 force = movementDirection * moveForce * Time.fixedDeltaTime;
        rb.AddForce(force, ForceMode.Impulse);

        if (debugMode && moveInput != Vector2.zero)
        {
            Debug.Log($"Applying force: {force} | Input: {moveInput}");
        }
    }

    /// <summary>
    /// Smoothly rotates the player to face the movement direction.
    /// </summary>
    private void ApplyRotation()
    {
        if (moveInput == Vector2.zero) return;

        // Convert 2D input to 3D world space (X and Z axes for top-down)
        Vector3 movementDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        movementDirection = Quaternion.AngleAxis(-45f, new Vector3(0, 1, 0)) * movementDirection;

        // Normalize to ensure consistent rotation speed
        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }

        // Calculate target rotation based on movement direction
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );

        if (debugMode)
        {
            Debug.Log($"Rotating towards: {movementDirection} | Current rotation: {transform.rotation.eulerAngles}");
        }
    }

    /// <summary>
    /// Applies drag to slow down the player when no input is provided.
    /// </summary>
    private void ApplyDrag()
    {
        // Make it so that this only applies when grounded
        if (moveInput == Vector2.zero)
        {
            // Apply drag to slow down
            rb.linearVelocity *= (1f - dragCoefficient * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Clamps the velocity to the maximum speed.
    /// Does not clamp Y, so gravety is uncapped
    /// </summary>
    private void ClampVelocity()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }
    }

    #region Input Event Handlers

    /// <summary>
    /// Called when movement input is performed (continuous).
    /// </summary>
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Input is read in Update() for continuous actions
        // This event is mainly for tracking input state changes
        if (debugMode)
        {
            Debug.Log("Move input performed");
        }
    }

    /// <summary>
    /// Called when movement input is canceled (no input).
    /// </summary>
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        
        if (debugMode)
        {
            Debug.Log("Move input canceled");
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Applies an external impulse force to the player (e.g., from knockback).
    /// </summary>
    /// <param name="force">The force vector to apply.</param>
    public void ApplyExternalForce(Vector3 force)
    {
        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
            
            if (debugMode)
            {
                Debug.Log($"External force applied: {force}");
            }
        }
    }

    /// <summary>
    /// Gets the current velocity of the player.
    /// </summary>
    /// <returns>The current velocity vector.</returns>
    public Vector3 GetVelocity()
    {
        return rb != null ? rb.linearVelocity : Vector3.zero;
    }

    /// <summary>
    /// Gets the current speed of the player (horizontal only).
    /// </summary>
    /// <returns>The current speed.</returns>
    public float GetSpeed()
    {
        if (rb == null) return 0f;
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        return horizontalVelocity.magnitude;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        // Draw movement direction in editor
        if (moveInput != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
            Gizmos.DrawLine(transform.position, transform.position + direction.normalized * 2f);
        }

        // Draw forward direction (current rotation)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);

        // Draw velocity vector
        if (Application.isPlaying && rb != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + rb.linearVelocity);
        }
    }

    #endregion
}