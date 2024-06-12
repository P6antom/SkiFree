using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    private static List<float> topScores = new List<float>();
    private static int maxScores = 10;

    public static void AddScore(float score)
    {
        topScores.Add(score);
        topScores.Sort();

        if (topScores.Count > maxScores)
        {
            topScores.RemoveAt(maxScores);
        }
    }

    public static List<float> GetTopScores()
    {
        return new List<float>(topScores);
    }

    public static void SaveScores()
    {
        for (int i = 0; i < topScores.Count; i++)
        {
            PlayerPrefs.SetFloat("TopScore" + i, topScores[i]);
        }

        PlayerPrefs.SetInt("ScoreCount", topScores.Count);
        PlayerPrefs.Save();
    }

    public static void LoadScores()
    {
        topScores.Clear();
        int count = PlayerPrefs.GetInt("ScoreCount", 0);
        
        for (int i = 0; i < count; i++)
        {
            float score = PlayerPrefs.GetFloat("TopScore" + i);
            topScores.Add(score);
        }
    }
}
