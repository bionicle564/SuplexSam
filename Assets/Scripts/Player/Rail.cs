using UnityEngine;
using System.Collections;

/// <summary>
/// Heavy objects can pass through these walls
/// </summary>
public class HeavyPassThroughTimed : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float ignoreDuration = 0.25f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("HEAVY"))
            return;

        Collider other = collision.collider;
        Collider mine = GetComponent<Collider>();

        Physics.IgnoreCollision(mine, other, true);
        StartCoroutine(ReEnableCollision(mine, other));
    }

    private IEnumerator ReEnableCollision(Collider mine, Collider other)
    {
        yield return new WaitForSeconds(ignoreDuration);

        if (mine != null && other != null)
            Physics.IgnoreCollision(mine, other, false);
    }
}