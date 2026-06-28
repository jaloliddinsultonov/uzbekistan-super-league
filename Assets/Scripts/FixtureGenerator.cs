using System.Collections.Generic;
using UnityEngine;

public class Fixture
{
    public ClubData homeClub;
    public ClubData awayClub;
    public int homeGoals = -1;  // -1 means not played yet
    public int awayGoals = -1;

    public Fixture(ClubData home, ClubData away)
    {
        homeClub = home;
        awayClub = away;
    }

    public bool IsPlayed => homeGoals != -1;
}

public class FixtureGenerator
{
    public static List<List<Fixture>> Generate(List<ClubData> clubs)
    {
        // Shuffle clubs list before generating!
        List<ClubData> shuffled = new List<ClubData>(clubs);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            ClubData temp = shuffled[i];
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        // Use shuffled list 
        List<List<Fixture>> allMatchdays = new List<List<Fixture>>();
        List<ClubData> rotation = new List<ClubData>(shuffled);     // <- shuffled!

        int totalRounds = clubs.Count - 1;  // 15 rounds first half

        // FIRST HALF — home fixtures
        for (int round = 0; round < totalRounds; round++)
        {
            List<Fixture> matchday = new List<Fixture>();

            for (int i = 0; i < clubs.Count / 2; i++)
            {
                ClubData home = rotation[i];
                ClubData away = rotation[clubs.Count - 1 - i];
                matchday.Add(new Fixture(home, away));
            }

            allMatchdays.Add(matchday);

            // Rotate clubs (keep first one fixed)
            ClubData last = rotation[rotation.Count - 1];
            rotation.RemoveAt(rotation.Count - 1);
            rotation.Insert(1, last);
        }

        // SECOND HALF — swap home and away
        int firstHalfCount = allMatchdays.Count;
        for (int i = 0; i < firstHalfCount; i++)
        {
            List<Fixture> matchday = new List<Fixture>();

            foreach (Fixture f in allMatchdays[i])
            {
                // swap home and away
                matchday.Add(new Fixture(f.awayClub, f.homeClub));
            }

            allMatchdays.Add(matchday);
        }

        BalanceHomeAway(allMatchdays, clubs);

        return allMatchdays;
    }

    private static void BalanceHomeAway(List<List<Fixture>> allMatchdays, List<ClubData> clubs)
    {
        // Track consecutive home/away count for each club
        Dictionary<ClubData, int> consecutiveHome = new Dictionary<ClubData, int>();
        Dictionary<ClubData, int> consecutiveAway = new Dictionary<ClubData, int>();

        foreach (ClubData club in clubs)
        {
            consecutiveHome[club] = 0;
            consecutiveAway[club] = 0;
        }

        foreach (List<Fixture> matchday in allMatchdays)
        {
            foreach (Fixture fixture in matchday)
            {
                // Check if home club has had too many home games
                if (consecutiveHome[fixture.homeClub] >= 2)
                {
                    // Find another fixture in this matchday to swap with
                    Fixture swapTarget = matchday.Find(f =>
                        f != fixture && consecutiveAway[f.homeClub] >= 2);

                    if (swapTarget != null)
                    {
                        // Swap home and away for this fixture
                        ClubData tempHome = fixture.homeClub;
                        ClubData tempAway = fixture.awayClub;
                        fixture.homeClub = fixture.awayClub;
                        fixture.awayClub = tempHome;
                    }
                }
            }

            // Update consecutive counts after swaps
            foreach (Fixture fixture in matchday)
            {
                // Reset away count, increment home count for home club
                consecutiveHome[fixture.homeClub]++;
                consecutiveAway[fixture.homeClub] = 0;

                // Reset home count, increment away count for away club
                consecutiveAway[fixture.awayClub]++;
                consecutiveHome[fixture.awayClub] = 0;
            }
        }
    }
}