using UnityEngine;

public class ClubStats
{
    public ClubData club;

    public int played;
    public int wins;
    public int draws;
    public int losses;
    public int goalsFor;
    public int goalsAgainst;

    public int GoalDifference => goalsFor - goalsAgainst;
    public int Points => (wins * 3) + draws;

    // Constructor - called when creating a new ClubStats
    public ClubStats(ClubData clubData)
    {
        club = clubData;
        played = 0;
        wins = 0;
        draws = 0;
        losses = 0;
        goalsFor = 0;
        goalsAgainst = 0;
    }

    // Called after every match to update stats
    public void RecordResult(int scored, int conceded)
    {
        played++;
        goalsFor += scored;
        goalsAgainst += conceded;

        if (scored > conceded)
        {
            wins++;
        }
        else if (scored == conceded)
        {
            draws++;
        }
        else
        {
            losses++;
        }
    }
}

