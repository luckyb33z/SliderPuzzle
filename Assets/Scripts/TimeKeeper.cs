using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeKeeper : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeText;

    private int seconds = 0;

    void Start()
    {
        StartCoroutine("TickTimePlayed");
    }

    public void Reset()
    {
        seconds = 0;
        SetTime(0);
        StopCoroutine("TickTimePlayed");
        StartCoroutine("TickTimePlayed");
    }

    public void StopTimer()
    {
        StopCoroutine("TickTimePlayed");
    }

    private IEnumerator TickTimePlayed()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            SetTime(++seconds);
        }
    }

    private void SetTime(int seconds)
    {
        int displayMinutes = seconds / 60;
        int displaySeconds = seconds % 60;

        string format = "D2";    // decimal, 2 leading zeroes

        timeText.SetText($"Time: {displayMinutes.ToString(format)}:{displaySeconds.ToString(format)}");
    }
}
