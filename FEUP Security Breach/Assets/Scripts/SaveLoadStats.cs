using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Save and Load system for reading and writing the highscore
/// </summary>
public class SaveLoadStats : MonoBehaviour
{
    public static bool Save()
    {
        string path = Application.persistentDataPath + "/Stats";
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(path))
        {
            float highscore = Load();
            if (highscore > TimeCounter.time)
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                bf.Serialize(stream, new Stats(TimeCounter.time));
                stream.Close();
                return true;
            }
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            Stats stats = new Stats(TimeCounter.time);
            bf.Serialize(stream, stats);
            stream.Close();
            return true;
        }
        return false;   
    }

    public static float Load()
    {
        string path = Application.persistentDataPath + "/Stats";
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            Stats stats = bf.Deserialize(stream) as Stats;
            stream.Close();
            return stats.bestTime;
        }
        return -1;
    }
}
