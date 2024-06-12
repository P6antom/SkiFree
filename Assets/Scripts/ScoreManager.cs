using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    private static Dictionary<string, List<float>> sceneTopScores = new Dictionary<string, List<float>>();
    private static int maxScores = 10;

    public static void AddScore(string sceneName, float score)
    {
        if (!sceneTopScores.ContainsKey(sceneName))
        {
            sceneTopScores[sceneName] = new List<float>();
        }

        List<float> topScores = sceneTopScores[sceneName];
        topScores.Add(score);
        topScores.Sort();

        if (topScores.Count > maxScores)
        {
            topScores.RemoveAt(maxScores);
        }
    }

    public static List<float> GetTopScores(string sceneName)
    {
        if (sceneTopScores.ContainsKey(sceneName))
        {
            return new List<float>(sceneTopScores[sceneName]);
        }
        return new List<float>();
    }

    public static void SaveScores()
    {
        foreach (var sceneScores in sceneTopScores)
        {
            string sceneName = sceneScores.Key;
            List<float> topScores = sceneScores.Value;

            for (int i = 0; i < topScores.Count; i++)
            {
                PlayerPrefs.SetFloat(sceneName + "_TopScore" + i, topScores[i]);
            }

            PlayerPrefs.SetInt(sceneName + "_ScoreCount", topScores.Count);
        }
        PlayerPrefs.Save();
    }

    public static void LoadScores()
    {
        sceneTopScores.Clear();

        foreach (string sceneName in new string[] { "Level 1 (Snow)", "Level 2 (Sand)" }) // Add all scene names here
        {
            int count = PlayerPrefs.GetInt(sceneName + "_ScoreCount", 0);
            List<float> topScores = new List<float>();

            for (int i = 0; i < count; i++)
            {
                float score = PlayerPrefs.GetFloat(sceneName + "_TopScore" + i);
                topScores.Add(score);
            }

            sceneTopScores[sceneName] = topScores;
        }
    }
}