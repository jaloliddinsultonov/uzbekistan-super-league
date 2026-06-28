using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeagueUI : MonoBehaviour
{
    [Header("Table")]
    [SerializeField] Transform tableContent;
    [SerializeField] GameObject clubRowPrefab;

    [Header("Next Match Panel")]
    [SerializeField] GameObject nextMatchPanel;
    [SerializeField] Image homeLogo;
    [SerializeField] TextMeshProUGUI homeName;
    [SerializeField] Image awayLogo;
    [SerializeField] TextMeshProUGUI awayName;

    [Header("Play Button")]
    [SerializeField] Button playButton;

    [Header("ChampionBanner")]
    [SerializeField] GameObject championBanner;
    [SerializeField] TextMeshProUGUI championBannerText;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found! Please run from MainMenu scene.");
            return;
        }
        RefreshTable();
        ShowNextMatch();
        CheckEarlyChampion();

        playButton.onClick.AddListener(OnPlayPressed);
    }

    void RefreshTable()
    {
        // Clear old rows first
        foreach (Transform child in tableContent)
        {
            Destroy(child.gameObject);
        }

        // Get sorted table from GameManager
        List<ClubStats> sortedTable = GameManager.Instance.GetSortedTable();

        // Spawn one row per club
        for (int i = 0; i < sortedTable.Count; i++)
        {
            ClubStats stats = sortedTable[i];

            GameObject rowObj = Instantiate(clubRowPrefab, tableContent);

            // Fill in all the data
            rowObj.transform.Find("Position").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            rowObj.transform.Find("Logo").GetComponent<Image>().sprite = stats.club.logo;
            rowObj.transform.Find("ClubName").GetComponent<TextMeshProUGUI>().text = stats.club.clubName;
            rowObj.transform.Find("Played").GetComponent<TextMeshProUGUI>().text = stats.played.ToString();
            rowObj.transform.Find("Wins").GetComponent<TextMeshProUGUI>().text = stats.wins.ToString();
            rowObj.transform.Find("Draws").GetComponent<TextMeshProUGUI>().text = stats.draws.ToString();
            rowObj.transform.Find("Losses").GetComponent<TextMeshProUGUI>().text = stats.losses.ToString();
            rowObj.transform.Find("GF").GetComponent<TextMeshProUGUI>().text = stats.goalsFor.ToString();
            rowObj.transform.Find("GA").GetComponent<TextMeshProUGUI>().text = stats.goalsAgainst.ToString();
            rowObj.transform.Find("GD").GetComponent<TextMeshProUGUI>().text = stats.GoalDifference.ToString();
            rowObj.transform.Find("Points").GetComponent<TextMeshProUGUI>().text = stats.Points.ToString();

            // ALternating row colors
            Image bgImage = rowObj.transform.Find("Background").GetComponent<Image>();
            if (i % 2 == 0)
                bgImage.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);   // dark'
            else
                bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);      // slightly lighter

            // Player highlight overrides alternating color
            if (stats.club == GameManager.Instance.playerClub)
            {
                bgImage.color = new Color(0f, 0.5f, 0.1f, 0.9f); // dark green
            }
        }
    }

    void ShowNextMatch()
    {
        // Check if season is over
        if (GameManager.Instance.IsSeasonOver())
        {
            // Hide next match panel completely!
            nextMatchPanel.SetActive(false);

            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "SEASON COMPLETE";
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() =>
                UnityEngine.SceneManagement.SceneManager.LoadScene("SeasonEnd"));
            return;
        }

        Fixture nextFixture = GameManager.Instance.GetPlayerFixture();
        
        if (nextFixture != null)
        {
            homeName.text = nextFixture.homeClub.clubName;
            awayName.text = nextFixture.awayClub.clubName;

            if (nextFixture.homeClub.logo != null)
                homeLogo.sprite = nextFixture.homeClub.logo;
            if (nextFixture.awayClub.logo != null)
                awayLogo.sprite = nextFixture.awayClub.logo;
        }
    }

    void CheckEarlyChampion()
    {
        if (GameManager.Instance.IsChampionDecided())
        {
            ClubStats champion = GameManager.Instance.GetChampion();
            // Show a banner at top of screen
            championBanner.SetActive(true);
            championBannerText.text = champion.club.clubName + " are Champions!";
        }
    }

    void OnPlayPressed()
    {
        GameManager.Instance.PlayNextMatchday();
    }
}
