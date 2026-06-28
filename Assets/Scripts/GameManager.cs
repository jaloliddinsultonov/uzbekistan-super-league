using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton - only one GameManager exists ever
    public static GameManager Instance;

    [Header("All Clubs in the League")]
    public List<ClubData> allClubs;     // drag all 16 SOs here in Inspector

    // Live season data
    [HideInInspector] public List<ClubStats> leagueTable;
    [HideInInspector] public List<List<Fixture>> allMatchdays;
    [HideInInspector] public int currentMatchday = 0;
    [HideInInspector] public ClubData playerClub;
    [HideInInspector] public MatchResult playerMatchResult;

    // Current matchday results (shown on results screen)
    [HideInInspector] public List<MatchResult> lastMatchdayResults;

    void Awake()
    {
        // Singleton pattern - destroy duplicate if exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);      // survive scene changes!
    }

    // Called when player picks their club on ClubSelect scene
    public void StartSeason(ClubData chosenClub)
    {
        playerClub = chosenClub;

        // Create ClubStats for all 16 clubs
        leagueTable = new List<ClubStats>();
        foreach (ClubData club in allClubs)
        {
            leagueTable.Add(new ClubStats(club));
        }

        // Generate all 30 matchdays
        allMatchdays = FixtureGenerator.Generate(allClubs);

        currentMatchday = 0;

        // Go to league scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("League");
    }

    // Called when player presses Play Next Match
    public void PlayNextMatchday()
    {
        if (currentMatchday >= allMatchdays.Count) return;

        List<Fixture> matchday = allMatchdays[currentMatchday];
        lastMatchdayResults = new List<MatchResult>();

        foreach (Fixture fixture in matchday)
        {
            // Simulate every match
            MatchResult result = MatchSimulator.Simulate(fixture.homeClub, fixture.awayClub);

            // Store goals in fixture
            fixture.homeGoals = result.homeGoals;
            fixture.awayGoals = result.awayGoals;

            // Update league table for both clubs
            UpdateTable(fixture.homeClub, fixture.homeGoals, fixture.awayGoals);
            UpdateTable(fixture.awayClub, fixture.awayGoals, fixture.homeGoals);

            // Store result for results screen
            lastMatchdayResults.Add(result);

            // Store player's match separately 
            if (fixture.homeClub == playerClub || fixture.awayClub == playerClub)
                playerMatchResult = result;
        }

        currentMatchday++;

        // Go to matchday results scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
    }

    // Finds the club in table and records result
    private void UpdateTable(ClubData club, int scored, int conceded)
    {
        ClubStats stats = leagueTable.Find(s => s.club == club);
        if (stats != null)
        {
            stats.RecordResult(scored, conceded);
        }
    }

    // Returns table sorted by points, then Goal Difference(GD), then Goals For(GF)
    public List<ClubStats> GetSortedTable()
    {
        List<ClubStats> sorted = new List<ClubStats>(leagueTable);

        sorted.Sort((a, b) =>
        {
            if (b.Points != a.Points)
                return b.Points.CompareTo(a.Points);
            if (b.GoalDifference != a.GoalDifference)
                return b.GoalDifference.CompareTo(a.GoalDifference);
            return b.goalsFor.CompareTo(a.goalsFor);
        });

        return sorted;
    }

    // Check if season is over
    public bool IsSeasonOver()
    {
        return currentMatchday >= allMatchdays.Count;
    }

    // Get player's fixture from current matchday
    public Fixture GetPlayerFixture()
    {
        if (currentMatchday >= allMatchdays.Count) return null;

        List<Fixture> matchday = allMatchdays[currentMatchday];
        return matchday.Find(f =>
            f.homeClub == playerClub || f.awayClub == playerClub);
    }

    public bool IsChampionDecided()
    {
        List<ClubStats> sorted = GetSortedTable();

        int matchdaysLeft = allMatchdays.Count - currentMatchday;
        int maxPointsGainable = matchdaysLeft * 3;

        // If first place is too far ahead for second place to catch up
        int firstPoints = sorted[0].Points;
        int secondPoints = sorted[1].Points;

        return (firstPoints - secondPoints) > maxPointsGainable;
    }

    public ClubStats GetChampion()
    {
        if (IsChampionDecided())
            return GetSortedTable()[0];
        return null;
    }
}
