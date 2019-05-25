using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SaveOptionsData(OptionsController optionsController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options.taco";
        FileStream stream = new FileStream(path, FileMode.Create);

        OptionData data = new OptionData(optionsController);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static OptionData LoadOptionData()
    {
        string path = Application.persistentDataPath + "/options.taco";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OptionData data = formatter.Deserialize(stream) as OptionData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
