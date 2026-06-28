using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SeasonEndUI : MonoBehaviour
{
    [Header("Champion")]
    [SerializeField] Image championLogo;
    [SerializeField] TextMeshProUGUI championName;

    [Header("Your Result")]
    [SerializeField] Image yourLogo;
    [SerializeField] TextMeshProUGUI yourClubName;
    [SerializeField] TextMeshProUGUI yourPosition;
    [SerializeField] TextMeshProUGUI yourPoints;
    [SerializeField] TextMeshProUGUI yourStats;

    void Start()
    {
        if (GameManager.Instance == null) return;

        List<ClubStats> sortedTable = GameManager.Instance.GetSortedTable();

        SetupChampion(sortedTable);
        SetupYourResult(sortedTable);
    }

    void SetupChampion(List<ClubStats> sortedTable)
    {
        // First place is the champion
        ClubStats champion = sortedTable[0];

        if (champion.club.logo != null)
            championLogo.sprite = champion.club.logo;
        championName.text = champion.club.clubName;
    }

    void SetupYourResult(List<ClubStats> sortedTable)
    {
        ClubData playerClub = GameManager.Instance.playerClub;

        // Find player's position in table
        int position = 0;
        ClubStats playerStats = null;

        for (int i = 0; i<sortedTable.Count; i++)
        {
            if (sortedTable[i].club == playerClub)
            {
                position = i + 1;
                playerStats = sortedTable[i];
                break;
            }
        }

        if (playerStats == null) return;

        // Set logo
        if (playerClub.logo != null)
            yourLogo.sprite = playerClub.logo;

        // Set club name
        yourClubName.text = playerClub.clubName;

        // Set position with suffix
        yourPosition.text = GetPositionText(position);

        // Change color based on position
        if (position == 1)
            yourPosition.color = new Color(1f, 0.84f, 0f, 1f);  // gold
        else if (position <= 3)
            yourPosition.color = new Color(0.4f, 0.8f, 1f, 1f);  // blue
        else if (position >= 14)
            yourPosition.color = new Color(1f, 0.3f, 0.3f, 1f);  // red
        else
            yourPosition.color = new Color(0.4f, 1f, 0.4f, 1f);  // green

        // Set points
        yourPoints.text = playerStats.Points + " POINTS";

        // Set stats
        yourStats.text =
            "W: " + playerStats.wins +
            "  D:" + playerStats.draws +
            "  L:" + playerStats.losses +
            "  GF:" + playerStats.goalsFor +
            "  GA:" + playerStats.goalsAgainst + 
            "  GD:" + (playerStats.GoalDifference >= 0 ? "+" : "") +
            playerStats.GoalDifference;
    }

    string GetPositionText(int position)
    {
        switch (position)
        {
            case 1: return "1ST PLACE";
            case 2: return "2ND PlACE";
            case 3: return "3RD PlACE";
            default: return position + "TH PlACE";
        }

    }

    public void OnPlayAgainPressed()
    {
        // Destroy GameManager so fresh season starts
        Destroy(GameManager.Instance.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        // Music restarts automatically since AudioManager 
        // calls PlayBackgroundMusic() and MainMenu loads fresh
    }

}
