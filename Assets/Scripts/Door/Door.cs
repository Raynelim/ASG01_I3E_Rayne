using UnityEngine;

public class Door : MonoBehaviour
{
    public float doorSpeed = 1.5f;

    public bool isOpen = false;

    Quaternion openRotation;
    Quaternion closeRotation;

    private void Start()
    {
        closeRotation = Quaternion.Euler(-90f, -90f, 0f);
        openRotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    void Update()
    {
        // test

        if(Input.GetKeyUp(KeyCode.O))
        {
            DoorInteract();
        }

        Quaternion smoothRotation = isOpen ? openRotation : closeRotation;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, smoothRotation, Time.deltaTime * doorSpeed);
    }

    public void DoorInteract()
    {
        isOpen = !isOpen;
        AudioManager.Instance.PlaySound("Door");
    }
}
