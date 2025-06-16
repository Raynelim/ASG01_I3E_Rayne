using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    int deathCounter;
    int maximumKeys = 4;
    public int keyCounter;
    Vector3 respawnPositon = new Vector3(-12f, 3.3f, -28.57f);
    public bool hasCollectedWrench;

    [Header("HUD")]
    public TextMeshProUGUI keysCollectedText;
    public TextMeshProUGUI statusText;

    [Header("Parkour Course")]
    public GameObject parkourArea;

    bool hasShownParkourMessage = false;
    bool hasShownWrenchMessage = false;

    void Update()
    {
        CheckParkourUnlock();
    }

    void CheckParkourUnlock()
    {
        if (keyCounter >= maximumKeys && !hasShownParkourMessage)
        {
            parkourArea.SetActive(true);
            ShowStatusMessage("You unlocked the final room\nFind your way up", 4f);
            hasShownParkourMessage = true;
        }
    }

    public void IncrementKeyCount()
    {
        keyCounter++;
        keysCollectedText.text = $"Keys Collected: {keyCounter}/{maximumKeys}";
    }

    void Die()
    {
        transform.position = respawnPositon;
        deathCounter++;
        ShowStatusMessage("You Died.", 2f);
        AudioManager.Instance.PlaySound("Die");
    }

    void ShowStatusMessage(string message, float duration = 3f)
    {
        statusText.text = message;
        CancelInvoke(nameof(ClearStatusText)); // Cancel any previous invoke
        Invoke(nameof(ClearStatusText), duration);
    }

    void ClearStatusText()
    {
        statusText.text = string.Empty;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            Die();
        }
        else if (other.CompareTag("Wrench") && !hasCollectedWrench)
        {
            hasCollectedWrench = true;
            AudioManager.Instance.PlaySound("Pickup");
            Destroy(other.gameObject);

            if (!hasShownWrenchMessage)
            {
                ShowStatusMessage("Wrench collected.\nGo to the power source and fix it!", 4f);
                hasShownWrenchMessage = true;
            }
        }
    }
}
