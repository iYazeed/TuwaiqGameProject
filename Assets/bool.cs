using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Animator animator;
    public Transform playerTransform;
    public float interactRange = 2f;
    private bool hasTorch = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !hasTorch)
        {
            Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, interactRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Torch"))
                {
                    PickUpTorch(hitCollider.gameObject);
                    break;
                }
            }
        }
    }

    void PickUpTorch(GameObject torch)
    {
        hasTorch = true;

        // Disable torch or attach it to the player visually
        torch.SetActive(false); // Or torch.transform.SetParent(playerTransform) if you want to keep it visible

        // Switch animation state
        animator.SetBool("HasTorch", true); // If using a bool parameter
        // animator.SetTrigger("Tw"); // If using a trigger instead
    }
}