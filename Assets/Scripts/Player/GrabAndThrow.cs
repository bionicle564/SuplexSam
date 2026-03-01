using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Grab and throw component that allows picking up and throwing rigidbody objects.
/// Uses overlap box for detection based on layermask.
/// Follows the InputSystem integration pattern using PlayerInput component.
/// </summary>
public class GrabAndThrow : MonoBehaviour
{
    [Header("Input References")]
    [Tooltip("Reference to the grab input action.")]
    [SerializeField] private InputActionReference grabActionReference;

    [Tooltip("Reference to the throw input action.")]
    [SerializeField] private InputActionReference throwActionReference;

    [Tooltip("Reference to the throw down action.")]
    [SerializeField] private InputActionReference throwDownActionReference;

    [Header("Detection Settings")]
    [Tooltip("Layer mask for grabbable objects.")]
    [SerializeField] private LayerMask grabbableLayer = 1 << 6; // Default to layer 6

    [Tooltip("Size of the overlap box for detecting grabbable objects.")]
    [SerializeField] private Vector3 detectionBoxSize = new Vector3(1f, 1f, 1f);

    [Tooltip("Distance from the player center to check for grabbable objects.")]
    [SerializeField] private float detectionDistance = 2f;

    [Header("Holding Settings")]
    [Tooltip("Transform to hold the grabbed object in front of the player.")]
    [SerializeField] private Transform holdPoint;

    [Tooltip("Offset from the hold point where the object is positioned.")]
    [SerializeField] private Vector3 holdOffset = new Vector3(0f, 0f, 1.5f);

    [Tooltip("Smoothness of moving the object to the hold point.")]
    [SerializeField] private float holdSmoothness = 10f;

    [Header("Throw Settings")]
    [Tooltip("Force multiplier for throwing the object.")]
    [SerializeField] private float throwForce = 20f;

    [Tooltip("Upward force when throwing (for arc).")]
    [SerializeField] private float throwUpwardForce = 5f;

    [Header("Debug")]
    [Tooltip("Show debug information in the console.")]
    [SerializeField] private bool debugMode = false;
    
	[Header("Visuals")]
	[Tooltip("SpriteRenderer that indicates holding state.")]
	[SerializeField] private SpriteRenderer holdingIndicator;
	
	[Header("Throw Safety")]
	[Tooltip("Disable Throwable Colider to pass Player")]
	[SerializeField] private float colliderDisableTime = 0.1f;
	
	[Header("HEAVY Tag")]
	[Tooltip("Makes Objects interact with stage elements ONLY when thrown")]
	[SerializeField] private string heavyTag = "HEAVY";

    // Components
    private PlayerInput playerInput;

    // Input actions
    private InputAction grabAction;
    private InputAction throwAction;
    private InputAction throwDownAction;

    // Grab state
    private Rigidbody grabbedObject;
    private bool isHoldingObject = false;

    private void Awake()
    {
        // Get PlayerInput from the same GameObject
        playerInput = GetComponent<PlayerInput>();
        
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on this GameObject!", this);
        }

        // Create hold point if not assigned
        if (holdPoint == null)
        {
            holdPoint = new GameObject("HoldPoint").transform;
            holdPoint.SetParent(transform);
            holdPoint.localPosition = holdOffset;
        }
    }
	
	    private void OnEnable()
	    {
        if (playerInput == null) return;

        // Get the grab and throw actions using the ID from InputActionReference
        // This is the CRITICAL pattern from the InputSystem integration guide
        grabAction = playerInput.actions.FindAction(grabActionReference.action.id);
        throwAction = playerInput.actions.FindAction(throwActionReference.action.id);
        throwDownAction = playerInput.actions.FindAction(throwDownActionReference.action.id);

        if (grabAction == null)
        {
            Debug.LogError($"Grab action not found! Action ID: {grabActionReference.action.id}", this);
        }
        else
        {
            // Subscribe to grab events
            grabAction.performed += OnGrabPerformed;
        }

        if (throwAction == null)
        {
            Debug.LogError($"Throw action not found! Action ID: {throwActionReference.action.id}", this);
        }
        else
        {
            // Subscribe to throw events
            throwAction.performed += OnThrowPerformed;
        }

        if (throwDownAction != null)
        {
            throwDownAction.performed += OnThrowDownPerformed;
        }

        if (debugMode)
        {
            Debug.Log($"GrabAndThrow: Actions found and subscribed. Grab ID: {grabActionReference.action.id}, Throw ID: {throwActionReference.action.id}");
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        if (grabAction != null)
        {
            grabAction.performed -= OnGrabPerformed;
        }
        if (throwAction != null)
        {
            throwAction.performed -= OnThrowPerformed;
        }
        if(throwDownAction != null)
        {
            throwDownAction.performed -= OnThrowDownPerformed;
        }

        // Release object if holding when disabled
        if (isHoldingObject)
        {
            ReleaseObject();
        }
    }

	private void UpdateHoldingIndicator()
	{
	    if (holdingIndicator != null)
	    {
	        holdingIndicator.enabled = isHoldingObject;
	    }
	}

    private void FixedUpdate()
    {
    
    
        // Move held object to hold position
        if (isHoldingObject && grabbedObject != null)
        {
            MoveHeldObject();
        }
    }

    /// <summary>
    /// Handles grab input - picks up or drops objects.
    /// </summary>
    private void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (isHoldingObject)
        {
            // Drop the object
            DropObject();
        }
        else
        {
            // Try to grab an object
            TryGrabObject();
        }
    }

    /// <summary>
    /// Handles throw input - throws the held object.
    /// </summary>
    private void OnThrowPerformed(InputAction.CallbackContext context)
    {
        if (isHoldingObject)
        {
            ThrowObject();
        }
    }

    /// <summary>
    /// Handles throw down input - throws the held object down.
    /// </summary>
    private void OnThrowDownPerformed(InputAction.CallbackContext context)
    {
        if (isHoldingObject)
        {
            ThrowObjectDown();
        }
    }

    /// <summary>
    /// Attempts to grab the nearest grabbable object.
    /// </summary>
    private void TryGrabObject()
    {
        // Calculate detection position in front of the player
        Vector3 detectionPosition = transform.position + transform.forward * detectionDistance;

        // Use overlap box to detect grabbable objects
        Collider[] hitColliders = Physics.OverlapBox(
            detectionPosition,
            detectionBoxSize / 2f,
            transform.rotation,
            grabbableLayer
        );

        if (debugMode)
        {
            Debug.Log($"OverlapBox detected {hitColliders.Length} objects");
        }

        // Find the nearest grabbable object
        Rigidbody nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb != grabbedObject)
            {
                float distance = Vector3.Distance(transform.position, rb.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = rb;
                }
            }
        }

        //Debug.Log($"{hitColliders.Length}");

        if (nearestObject != null)
        {
            GrabObject(nearestObject);
            //Debug.Log("Grabbing object");
        }
        else if (debugMode)
        {
            Debug.Log("No grabbable object found");
        }
    }

    /// <summary>
    /// Grabs the specified object.
    /// </summary>
    private void GrabObject(Rigidbody objectToGrab)
    {
        if (objectToGrab == null) return;

        grabbedObject = objectToGrab;
        isHoldingObject = true;
        UpdateHoldingIndicator();

        HoldActions actions = grabbedObject.GetComponent<HoldActions>();
        if(actions != null)
        {
            actions.Grab();
        }

        if (grabbedObject.GetComponent<GrabbableEnemy>() != null)
        {
            grabbedObject.GetComponent<GrabbableEnemy>().OnGrabbed();
        }
        if (grabbedObject.GetComponent<EnemyAttack>() != null)
        {
            grabbedObject.GetComponent<EnemyAttack>().OnGrabbed();
        }

        // Disable gravity while holding
        grabbedObject.useGravity = false;

        // Set object to kinematic to prevent physics interference
        grabbedObject.isKinematic = true;

        // Clear velocity
        //grabbedObject.linearVelocity = Vector3.zero;
        //grabbedObject.angularVelocity = Vector3.zero;
        // ^ The above does not work on kinematic objects, nor is it necessary, luckily

        if (debugMode)
        {
            Debug.Log($"Grabbed object: {grabbedObject.name}");
        }
    }

    /// <summary>
    /// Moves the held object to the hold position smoothly.
    /// </summary>
    private void MoveHeldObject()
    {
        // Calculate target position
        Vector3 targetPosition = holdPoint.position;

        // Smoothly move object to target position
        grabbedObject.transform.position = Vector3.Lerp(
            grabbedObject.transform.position,
            targetPosition,
            holdSmoothness * Time.fixedDeltaTime
        );

        // Also rotate object to match player rotation
        grabbedObject.transform.rotation = Quaternion.Slerp(
            grabbedObject.transform.rotation,
            transform.rotation,
            holdSmoothness * Time.fixedDeltaTime
        );
    }

    /// <summary>
    /// Drops the held object without throwing it.
    /// </summary>
    private void DropObject()
    {
        if (grabbedObject == null) return;

        // Re-enable physics
        grabbedObject.useGravity = true;
        grabbedObject.isKinematic = false;
        
        GrabbableEnemy enemy = grabbedObject.GetComponent<GrabbableEnemy>();
	    if (enemy != null)
	    {
	        enemy.OnDropped(this);
	    }

        if (grabbedObject.GetComponent<EnemyAttack>() != null)
        {
            grabbedObject.GetComponent<EnemyAttack>().OnReleased();
        }

        // Clear velocity so it doesn't fly away
        grabbedObject.linearVelocity = Vector3.zero;
        grabbedObject.angularVelocity = Vector3.zero;

        if (debugMode)
        {
            Debug.Log($"Dropped object: {grabbedObject.name}");
        }

        HoldActions actions = grabbedObject.GetComponent<HoldActions>();
        if (actions != null)
        {
            actions.LetGo();
        }

        grabbedObject = null;
        isHoldingObject = false;
        UpdateHoldingIndicator();
        
    }

    /// <summary>
    /// Releases the held object (used when component is disabled).
    /// </summary>
    private void ReleaseObject()
    {
        if (grabbedObject == null) return;

        if (grabbedObject.GetComponent<EnemyAttack>() != null)
        {
            grabbedObject.GetComponent<EnemyAttack>().OnReleased();
        }

        // Re-enable physics
        grabbedObject.useGravity = true;
        grabbedObject.isKinematic = false;

        // Clear velocity
        grabbedObject.linearVelocity = Vector3.zero;
        grabbedObject.angularVelocity = Vector3.zero;

        HoldActions actions = grabbedObject.GetComponent<HoldActions>();
        if (actions != null)
        {
            actions.LetGo();
        }

        grabbedObject = null;
        isHoldingObject = false;
        UpdateHoldingIndicator();
    }

    /// <summary>
    /// Throws the held object with force.
    /// </summary>
    private void ThrowObject()
    {
        if (grabbedObject == null) return;
        
        Collider col = grabbedObject.GetComponent<Collider>();

        // Re-enable physics
        grabbedObject.useGravity = true;
        grabbedObject.isKinematic = false;
        
        GrabbableEnemy enemy = grabbedObject.GetComponent<GrabbableEnemy>();
		if (enemy != null)
		{
		    enemy.OnThrown(this);
		}

        if (grabbedObject.GetComponent<EnemyAttack>() != null)
        {
            grabbedObject.GetComponent<EnemyAttack>().OnReleased();
        }

        grabbedObject.tag = heavyTag;
        StartCoroutine(ResetTagAfterTime(grabbedObject, 1f));
        
        if (col != null)
	    {
	        col.enabled = false;
	        StartCoroutine(ReenableCollider(col));
	    }

        // Calculate throw direction (player's forward direction)
        Vector3 throwDirection = -transform.forward;

        // Apply throw force
        Vector3 throwVelocity = throwDirection * throwForce + Vector3.up * throwUpwardForce;
        grabbedObject.linearVelocity = throwVelocity;

        // Add some random rotation for more natural throw
        grabbedObject.angularVelocity = Random.insideUnitSphere * 5f;

        if (debugMode)
        {
            Debug.Log($"Threw object: {grabbedObject.name} with velocity: {throwVelocity}");
        }

        HoldActions actions = grabbedObject.GetComponent<HoldActions>();
        if (actions != null)
        {
            actions.LetGo();
        }


        grabbedObject = null;
        isHoldingObject = false;
        UpdateHoldingIndicator();
    }

    /// <summary>
    /// Throws the held object with force.
    /// </summary>
    private void ThrowObjectDown()
    {
        if (grabbedObject == null) return;

        Collider col = grabbedObject.GetComponent<Collider>();

        // Re-enable physics
        grabbedObject.useGravity = true;
        grabbedObject.isKinematic = false;

        GrabbableEnemy enemy = grabbedObject.GetComponent<GrabbableEnemy>();
        if (enemy != null)
        {
            enemy.OnThrown(this);
        }

        grabbedObject.tag = heavyTag;
        StartCoroutine(ResetTagAfterTime(grabbedObject, 1f));

        if (col != null)
        {
            col.enabled = false;
            StartCoroutine(ReenableColliderDown(col));
        }

        // Calculate throw direction (player's forward direction)
        Vector3 throwDirection = -transform.up;

        // Apply throw force
        Vector3 throwVelocity = throwDirection * throwForce + Vector3.up * throwUpwardForce;
        Debug.Log($"{throwVelocity}");
        grabbedObject.linearVelocity = throwVelocity;

        GetComponent<Rigidbody>().AddForce(-throwVelocity* (grabbedObject.mass), ForceMode.Impulse);

        // Add some random rotation for more natural throw
        grabbedObject.angularVelocity = Random.insideUnitSphere * 5f;

        if (debugMode)
        {
            Debug.Log($"Threw object: {grabbedObject.name} with velocity: {throwVelocity}");
        }

        HoldActions actions = grabbedObject.GetComponent<HoldActions>();
        if (actions != null)
        {
            actions.LetGo();
        }


        grabbedObject = null;
        isHoldingObject = false;
        UpdateHoldingIndicator();
    }


    private System.Collections.IEnumerator ReenableCollider(Collider col)
	{
	    yield return new WaitForSeconds(colliderDisableTime);
	    col.enabled = true;
	}

    private System.Collections.IEnumerator ReenableColliderDown(Collider col)
    {
        yield return new WaitForSeconds(0.01f);
        col.enabled = true;
    }

    private System.Collections.IEnumerator ResetTagAfterTime(Rigidbody rb,float time)
	{
	    yield return new WaitForSeconds(time);
	    if (rb != null)
	        rb.tag = "Untagged";
	}

    #region Public Methods

    /// <summary>
    /// Gets whether an object is currently being held.
    /// </summary>
    /// <returns>True if holding an object, false otherwise.</returns>
    public bool IsHoldingObject()
    {
        return isHoldingObject;
    }

    /// <summary>
    /// Gets the currently held object.
    /// </summary>
    /// <returns>The held Rigidbody, or null if not holding anything.</returns>
    public Rigidbody GetHeldObject()
    {
        return grabbedObject;
    }

    /// <summary>
    /// Forces the player to drop the held object.
    /// </summary>
    public void ForceDrop()
    {
        if (isHoldingObject)
        {
            DropObject();
        }
    }

    /// <summary>
    /// Forces the player to throw the held object.
    /// </summary>
    public void ForceThrow()
    {
        if (isHoldingObject)
        {
            ThrowObject();
        }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        // Draw detection box
        Vector3 detectionPosition = transform.position + transform.forward * detectionDistance;
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(detectionPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, detectionBoxSize);
        Gizmos.matrix = Matrix4x4.identity;

        // Draw hold point
        if (holdPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(holdPoint.position, 0.2f);
            Gizmos.DrawLine(transform.position, holdPoint.position);
        }

        // Draw throw direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
    }

    #endregion
}
