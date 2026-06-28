using UnityEngine;
using System.Collections.Generic;

public class MatchResult
{
    public ClubData homeClub;
    public ClubData awayClub;
    public int homeGoals;
    public int awayGoals;

    public List<int> homeGoalMinutes = new List<int>();
    public List<int> awayGoalMinutes = new List<int>();

    public MatchResult(ClubData home, ClubData away, int hGoals, int aGoals)
    {
        homeClub = home;
        awayClub = away;
        homeGoals = hGoals;
        awayGoals = aGoals;
    }
}

public class MatchSimulator
{
    //private const float HOME_ADVANTAGE = 0.5f;
    //private const float GOAL_FREQUENCY = 0.4f;

    public static MatchResult Simulate(ClubData home, ClubData away)
    {
        // Step 1 - Roll stats once at kickoff
        int homeAttack = home.GetAttack();
        int homeDefense = home.GetDefense();
        int homeMorale = home.GetMorale();

        int awayAttack = away.GetAttack();
        int awayDefense = away.GetDefense();
        int awayMorale = away.GetMorale();

        // New formula - defense matters much more now
        float homeChance = Mathf.Clamp(
            (homeAttack * 0.4f - awayDefense * 0.5f + homeMorale * 0.1f) / 2500f + 0.008f, 0.003f, 0.032f);

        float awayChance = Mathf.Clamp(
            (awayAttack * 0.4f - homeDefense * 0.5f + awayMorale * 0.1f) / 2500f + 0.005f, 0.003f, 0.028f);

        MatchResult result = new MatchResult(home, away, 0, 0);

        //// Step 2 - Calculate score chance per minute
        //float homeChance = (homeAttack - awayDefense + homeMorale) / 3f / 150f + HOME_ADVANTAGE / 100f;
        //float awayChance = (awayAttack - homeDefense + awayMorale) / 3f / 150f;

        //// Clamp so chance never goes negative or above realistic limit
        //homeChance = Mathf.Clamp(homeChance, 0.002f, 0.03f);
        //awayChance = Mathf.Clamp(awayChance, 0.002f, 0.03f);

        //MatchResult result = new MatchResult(home, away, 0, 0);

        

        for (int minute = 1; minute <= 90; minute++)
        {
            //// First check if this minute is even "active"
            //if (Random.value > GOAL_FREQUENCY) continue;   // skip 60% of minutes

            if (Random.value < homeChance)
            {
                result.homeGoals++;
                result.homeGoalMinutes.Add(minute);
            }
            if (Random.value < awayChance)
            { 
                result.awayGoals++;
                result.awayGoalMinutes.Add(minute);
            }
        }

        return result;
    }
}