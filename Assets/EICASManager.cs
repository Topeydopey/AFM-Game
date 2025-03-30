using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EICASManager : MonoBehaviour
{
    public Transform alarmLogParent; // Assign the parent container for alarms (a vertical layout group)
    public GameObject alarmPrefab; // Assign a TMP UI prefab for alarms
    public Button masterCautionButton;
    public AudioSource warningSound;

    private List<GameObject> activeAlarms = new List<GameObject>();
    private bool isFlashing = false;
    private bool alarmActive = false;

    void Start()
    {
        masterCautionButton.gameObject.SetActive(false); // Hide button at start
        StartCoroutine(TriggerAlarms()); // Simulate multiple alarms
    }

    IEnumerator TriggerAlarms()
    {
        yield return new WaitForSeconds(2);
        AddAlarm("ENGINE 1 FIRE");

        yield return new WaitForSeconds(5);
        AddAlarm("HYDRAULIC PRESSURE LOW");

        yield return new WaitForSeconds(3);
        AddAlarm("ELEC BUS SHORT");
    }

    public void AddAlarm(string message)
    {
        GameObject newAlarm = Instantiate(alarmPrefab, alarmLogParent);
        TextMeshProUGUI alarmText = newAlarm.GetComponent<TextMeshProUGUI>();
        alarmText.text = $"<color=red>{message}</color>";

        activeAlarms.Add(newAlarm);
        alarmActive = true;
        masterCautionButton.gameObject.SetActive(true);

        // Start looping sound
        if (warningSound != null && !warningSound.isPlaying)
        {
            warningSound.loop = true;
            warningSound.Play();
        }

        if (!isFlashing)
        {
            StartCoroutine(FlashMasterCaution());
        }
    }

    IEnumerator FlashMasterCaution()
    {
        isFlashing = true;
        while (alarmActive)
        {
            masterCautionButton.gameObject.SetActive(!masterCautionButton.gameObject.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
        masterCautionButton.gameObject.SetActive(true);
        isFlashing = false;
    }

    public void ResetAlarms()
    {
        foreach (GameObject alarm in activeAlarms)
        {
            Destroy(alarm);
        }
        activeAlarms.Clear();

        alarmActive = false;
        masterCautionButton.gameObject.SetActive(false);

        if (warningSound != null && warningSound.isPlaying)
        {
            warningSound.Stop();
        }
    }
}
