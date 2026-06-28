using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class MatchdayResultsUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Transform content;
    [SerializeField] GameObject resultsRowPrefab;

    void Start()
    {
        if (GameManager.Instance == null) return;

        titleText.text = "MATCHWEEK " + GameManager.Instance.currentMatchday + " RESULTS";

        PopulateResults();
    }

    void PopulateResults()
    {
        List<MatchResult> results = GameManager.Instance.lastMatchdayResults;

        foreach (MatchResult result in results)
        {
            GameObject rowObj = Instantiate(resultsRowPrefab, content);

            // Fill in data
            rowObj.transform.Find("HomeName").GetComponent<TextMeshProUGUI>().text = result.homeClub.clubName;
            rowObj.transform.Find("AwayName").GetComponent<TextMeshProUGUI>().text = result.awayClub.clubName;
            rowObj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = result.homeGoals + " - " + result.awayGoals;

            // Set logos
            Image homeLogo = rowObj.transform.Find("HomeLogo").GetComponent<Image>();
            Image awayLogo = rowObj.transform.Find("AwayLogo").GetComponent<Image>();

            if (result.homeClub.logo != null)
                homeLogo.sprite = result.homeClub.logo;
            if (result.awayClub.logo != null)
                awayLogo.sprite = result.awayClub.logo;

            // Highlight player's match
            bool isPlayerMatch = result.homeClub == GameManager.Instance.playerClub ||
                                 result.awayClub == GameManager.Instance.playerClub;

            if (isPlayerMatch)
            {
                rowObj.transform.Find("Background").GetComponent<Image>().color = new Color(1f, 0.84f, 0f, 0.2f); // gold
            }
            //else
            //{
            //    rowObj.transform.Find("Background").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f); // whitish
            //}

        }

    }

    public void OnSeeTablePressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("League");
    }
}
