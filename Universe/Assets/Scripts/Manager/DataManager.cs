using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public UserData userData;
    private string _path = Application.persistentDataPath +"/.json";
    public void SaveGame()
    {
         string jsonStr = JsonUtility .ToJson(userData);
    }
    // Start is called before the first frame update


    void Start()
    {
        TextAsset JSONfile = Resources.Load<TextAsset>(_path);
        UserData userData = JsonUtility.FromJson<UserData>("UserData");
    }
}


[SerializeField]
public class UserData
{
    
}