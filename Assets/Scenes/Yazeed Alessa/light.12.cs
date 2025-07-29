using UnityEngine;

public class TorchPickup : MonoBehaviour
{
    public string playerTag = "Player";
    public KeyCode pickupKey = KeyCode.E;
    public GameObject torchInHand;

    private bool canPickup = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canPickup = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canPickup = false;
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(pickupKey))
        {
            torchInHand.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}