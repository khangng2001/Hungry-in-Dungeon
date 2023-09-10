using Realms.Sync;
using Realms.Sync.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistence : MonoBehaviour
{
    MongoClient.Collection<GameData> collection;

    private string pid;

    private CloudDataHandler dataHandler;
    private List<IDataPersistence> dataPersistencesObjects;

    public static DataPersistence instance { get; private set; }

    //----------------------------------------------------------------------------------------------------------------------------

    private async void Awake()
    {
        try
        {
            //User user = GameObject.FindObjectOfType<ConnectMongoDb>().GetComponent<ConnectMongoDb>().user;
            //var mongoDbClient = user.GetMongoClient("mongodb-atlas");
            //var database = mongoDbClient.GetDatabase("HungryInDungeon");
            //collection = database.GetCollection<GameData>("Player");

            //pid = await FindPlayerPid(user.Id);
            //Debug.Log("pid: " + pid);
            //this.dataHandler = new CloudDataHandler(collection);

            //dataPersistencesObjects = FindAllDataPersistenceObjects();
            //LoadGame();

            if (instance != null)
            {
                Debug.LogError("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            } 
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        catch (AppException ex)
        {
            Debug.LogException(ex);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }

    public async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called");
        User user = GameObject.FindObjectOfType<ConnectMongoDb>().GetComponent<ConnectMongoDb>().user;
        var mongoDbClient = user.GetMongoClient("mongodb-atlas");
        var database = mongoDbClient.GetDatabase("HungryInDungeon");
        collection = database.GetCollection<GameData>("Player");

        pid = await FindPlayerPid(user.Id);
        Debug.Log("pid: " + pid);

        this.dataHandler = new CloudDataHandler(collection);

        dataPersistencesObjects = FindAllDataPersistenceObjects();
        //LoadGame();
    }

    public void OnSceneUnLoaded(Scene scene)
    {
        Debug.Log("OnSceneUnLoaded Called");
        SaveGame();
    }

    private async Task<string> FindPlayerPid(string findPid)
    {
        try
        {
            GameData myAccount = await collection.FindOneAsync(new {pid = findPid});
            return myAccount.Pid;
        }
        catch (AppException ex)
        {
            Debug.LogException(ex);
            return null;  
        }
    }

    public async void NewGame()
    {
        GameData myAccount = await collection.FindOneAsync(new { pid = pid });
        // Create new data for game
        myAccount = new GameData();
    }

    public async void LoadGame()
    {
        GameData myAccount = await collection.FindOneAsync(new { pid = pid });

        // Load any saved data from a player in mongodb
        Debug.Log("pid error: " + myAccount.Pid);
        myAccount = await dataHandler.Load(myAccount.Pid);
        
        // Push the load data to all other script that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.LoadData(myAccount);
        }
    }

    public async void SaveGame()
    {
        GameData myAccount = await collection.FindOneAsync(new { pid = pid });

        // Pass the data other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.SaveData(ref myAccount);
        }

        // Save that data to a player in mongodb  
        dataHandler.Save(myAccount, myAccount.Pid);
        Debug.Log("Save success");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
