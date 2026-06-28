using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchUI : MonoBehaviour
{
    [Header("Match Info")]
    [SerializeField] TextMeshProUGUI matchdayText;
    [SerializeField] TextMeshProUGUI minuteText;
    [SerializeField] Slider progressBar;

    [Header("Home Team")]
    [SerializeField] Image homeLogo;
    [SerializeField] TextMeshProUGUI homeName;
    [SerializeField] TextMeshProUGUI homeScore;

    [Header("Away Team")]
    [SerializeField] Image awayLogo;
    [SerializeField] TextMeshProUGUI awayName;
    [SerializeField] TextMeshProUGUI awayScore;

    [Header("Goal Feed")]
    [SerializeField] Transform goalFeed;
    [SerializeField] GameObject goalTextPrefab;
    [SerializeField] TMP_FontAsset customFont;

    [Header("Continue Button")]
    [SerializeField] GameObject continueButton;


    // match duration in seconds
    private float matchDuration = 5f;

    void Start()
    {
        // Stop background music during match
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopBackgroundMusic();

        if (GameManager.Instance == null) return;

        SetupMatch();
        StartCoroutine(PlayMatch());
    }

    void SetupMatch()
    {
        MatchResult result = GameManager.Instance.playerMatchResult;

        // Set matchday number
        matchdayText.text = "MATCHWEEK " + GameManager.Instance.currentMatchday;

        // Set club names
        homeName.text = result.homeClub.clubName;
        awayName.text = result.awayClub.clubName;

        // Set club logos
        if (result.homeClub.logo != null)
            homeLogo.sprite = result.homeClub.logo;
        if (result.awayClub.logo != null)
            awayLogo.sprite = result.awayClub.logo;

        // Start score at 0-0
        homeScore.text = "0";
        awayScore.text = "0";

        // Hide continue button at start
        continueButton.SetActive(false);

        // Setup progress bar
        progressBar.minValue = 0;
        progressBar.maxValue = 90;
        progressBar.value = 0;
    }

    IEnumerator PlayMatch()
    {
        MatchResult result = GameManager.Instance.playerMatchResult;

        int currentHomeGoals = 0;
        int currentAwayGoals = 0;
        int homeGoalIndex = 0;
        int awayGoalIndex = 0;

        for (int minute = 1; minute <= 90; minute++)
        {
            // Wait per minute (9 seconds / 90 minutes)
            yield return new WaitForSeconds(matchDuration / 90f);

            // Update minute display
            minuteText.text = minute + "'";
            progressBar.value = minute;

            // Check if home team scored this minute
            if (homeGoalIndex < result.homeGoalMinutes.Count && result.homeGoalMinutes[homeGoalIndex] == minute)
            {
                currentHomeGoals++;
                homeScore.text = currentHomeGoals.ToString();
                homeGoalIndex++;
                ShowGoalMessage(minute, result.homeClub.clubName, true);
            }

            // Check if away team scored this minute
            if (awayGoalIndex < result.awayGoalMinutes.Count && result.awayGoalMinutes[awayGoalIndex] == minute)
            {
                currentAwayGoals++;
                awayScore.text = currentAwayGoals.ToString();
                awayGoalIndex++;
                ShowGoalMessage(minute, result.awayClub.clubName, false);
            }
        }

        // Match finished!
        minuteText.text = "FULL TIME";
        continueButton.SetActive(true);
    }

    void ShowGoalMessage(int minute, string clubName, bool isHome)
    {
        // Play random goal sound!
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGoalSound();

        // Create goal text in field
        GameObject goalObj = new GameObject("GoalText");
        goalObj.transform.SetParent(goalFeed, false);

        TextMeshProUGUI goalText = goalObj.AddComponent<TextMeshProUGUI>();
        goalText.text = ">> " + minute + "' " + clubName + "!";
        goalText.fontSize = 38;
        goalText.alignment = TextAlignmentOptions.Center;

        // Home goals in white, away goals in yellow
        goalText.color = isHome ? Color.white : Color.yellow;

        if (customFont != null)
        {
            goalText.font = customFont;
        }

        // Add layout element so it has proper height
        LayoutElement le = goalObj.AddComponent<LayoutElement>();
        le.preferredHeight = 50;
    }

    public void OnContinuePressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MatchdayResults");
    }
}
