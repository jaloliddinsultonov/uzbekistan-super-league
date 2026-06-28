using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ClubSelectUI : MonoBehaviour
{
    [SerializeField] GameObject confirmPanel;
    [SerializeField] TextMeshProUGUI confirmText;
    private ClubData pendingClub;

    [Header("UI References")]
    [SerializeField] Transform contentParent;       // The Content inside ScrollView
    [SerializeField] GameObject clubButtonPrefab;       // the prefab we made


    void Start()
    {
        PopulateClubList();
    }

    void PopulateClubList()
    {
        List<ClubData> clubs = GameManager.Instance.allClubs;

        foreach (ClubData club in clubs)
        {
            // Spawn a button for each club
            GameObject buttonObj = Instantiate(clubButtonPrefab, contentParent);

            // Set club name text
            TextMeshProUGUI nameText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            nameText.text = club.clubName;

            // Set club logo
            Image logoImage = buttonObj.transform.Find("ClubLogo").GetComponent<Image>();
            if (club.logo != null)
                logoImage.sprite = club.logo;

            // Set button color to club's primary color
            Image bgImage = buttonObj.transform.Find("Background").GetComponent<Image>();
            bgImage.color = club.primaryColor;

            // Button click -> start season with this club
            Button btn = buttonObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnClubSelected(club));
        }
    }

    void OnClubSelected(ClubData club)
    {
        pendingClub = club;
        confirmText.text = "Are you sure you want to\nchoose " + club.clubName + "?";
        confirmPanel.SetActive(true);
    }

    public void OnConfirmYes()
    {
        confirmPanel.SetActive(false);
        GameManager.Instance.StartSeason(pendingClub);
    }

    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);
        pendingClub = null;
    }


}
