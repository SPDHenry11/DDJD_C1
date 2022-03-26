using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadStats : MonoBehaviour
{
    public static void Save()
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
            }
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            Stats stats = new Stats(TimeCounter.time);
            bf.Serialize(stream, stats);
            stream.Close();
        }
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
