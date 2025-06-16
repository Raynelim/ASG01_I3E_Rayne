using TMPro;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public float rayDistance = 3f;

    [Header("UI")]
    public TextMeshProUGUI interactionText;

    Camera cam;
    RaycastHit hitInfo;

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        interactionText.text = "";
    }

    void Update()
    {
        if (CheckForInteractable())
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            // Show appropriate text
            if (hitObject.CompareTag("Door"))
            {
                interactionText.text = "Press E to use door";
            }

            else if (hitObject.CompareTag("Power"))
            {
                Player player = GetComponent<Player>();

                if (!player.hasCollectedWrench)
                    interactionText.text = "Need wrench to fix Power";
                else
                    interactionText.text = "Press E to fix Power";
            }

            // Handle interaction on key press
            if (Input.GetKeyDown(KeyCode.E))
            {
                HandleInteraction(hitObject);
            }
        }
        else
        {
            interactionText.text = "";
        }
    }

    bool CheckForInteractable()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        return Physics.Raycast(ray, out hitInfo, rayDistance) &&
               (hitInfo.collider.CompareTag("Door") || hitInfo.collider.CompareTag("Power"));
    }

    void HandleInteraction(GameObject hitObject)
    {
        if (hitObject.CompareTag("Door"))
        {
            Door door = hitObject.GetComponent<Door>();
            if (door != null)
            {
                door.DoorInteract();
            }
        }

        if (hitObject.CompareTag("Power"))
        {
            Player player = GetComponent<Player>();

            if(!player.hasCollectedWrench)      // if player doesnt have wrench
            {
                AudioManager.Instance.PlaySound("Shutdown");
            }

            else if(player.hasCollectedWrench)     // if player has wrench
            {
                AudioManager.Instance.PlaySound("Electricity");
                player.statusText.text = "You Won.";
            }
        }
    }

    void OnDrawGizmos()
    {
        if (cam == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * rayDistance);
    }
}
