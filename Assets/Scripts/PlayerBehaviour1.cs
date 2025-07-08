using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour1 : MonoBehaviour
{
    int currentScore = 0;
    int health = 50;
    bool canInteract = false;
    CoinBehaviour currentCoin = null;
    DoorBehaviour currentDoor = null;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float fireStrength = 100f;
    [SerializeField] float interactionDistance = 5f;

    void Start()
    {
        Debug.Log("Health: " + health);
    }

    void Update()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (hitInfo.collider.CompareTag("Collectible"))
            {
                if (currentCoin != null)
                    currentCoin.Unhighlight();

                canInteract = true;
                currentCoin = hitInfo.collider.GetComponent<CoinBehaviour>();
                currentCoin.Highlight();
            }
        }
        else if (currentCoin != null)
        {
            currentCoin.Unhighlight();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.GetComponent<DoorBehaviour>();
        }
        else if (other.CompareTag("Collectible"))
        {
            canInteract = true;
            currentCoin = other.GetComponent<CoinBehaviour>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentCoin != null && other.gameObject == currentCoin.gameObject)
        {
            canInteract = false;
            currentCoin = null;
        }
        else if (currentDoor != null && other.gameObject == currentDoor.gameObject)
        {
            canInteract = false;
            currentDoor = null;
        }
    }

    public void OnInteract()
    {
        if (canInteract)
        {
            if (currentDoor != null)
            {
                Debug.Log("Interacting with door");
                currentDoor.Interact();
            }
            else if (currentCoin != null)
            {
                Debug.Log("Interacting with coin");
                currentCoin.Collect(this);
            }
        }
    }

    public void ModifyScore(int amt)
    {
        currentScore += amt;
        Debug.Log("Score: " + currentScore);
    }

    void OnFire()
    {
        Debug.Log("fire");
        GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Vector3 fireForce = spawnPoint.forward * fireStrength;
        newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            ++currentScore;
            Debug.Log("Score: " + currentScore);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Healing") && health < 100)
        {
            ++health;
            Debug.Log("Health: " + health);
        }
        if (collision.gameObject.CompareTag("Hazard") && health > 0)
        {
            --health;
            Debug.Log("Health: " + health);
            if (health <= 0)
            {
                Debug.Log("Player is Dead");
            }
        }
    }
}