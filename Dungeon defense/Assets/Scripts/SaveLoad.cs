using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameData
{
    public List<int> PlayerStars = new List<int>();

    public GameData(List<int> playerStars)
    {
        PlayerStars = playerStars;
    }
}


public class SaveLoad : MonoBehaviour
{
    private List<int> playerStars;

    public void SaveFile()
    {
        playerStars = GameManager.Instance.playerStars;
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        GameData data = new GameData(playerStars);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        Debug.Log("Saved successfully hopefully");
        file.Close();
    }

    public List<int> LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();
        playerStars = data.PlayerStars;
        if (GameManager.Instance)
            GameManager.Instance.playerStars = playerStars;
        return playerStars;
    }
}
