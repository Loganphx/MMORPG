using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using Microsoft.Extensions.DependencyInjection;
using Networking;
using Networking.PackageParser;
using Networking.PackageParser.PackageImplementations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class  ClientManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] public TMP_InputField passwordInput;
    [SerializeField] public TMP_Text errorText;

    // private readonly IPackageParser _packageParser;
    private readonly NetworkConnection _clientConnection;
    private readonly ClientConnectionDispatcher _clientConnectionDispatcher;
    private readonly MenuManager _menuManager;

    public static string RealmIdText { get; set; }
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public ClientManager()
    {
        _clientConnection = GameContext.ServiceProvider.GetRequiredService<NetworkConnection>();
        _clientConnectionDispatcher = GameContext.ServiceProvider.GetRequiredService<ClientConnectionDispatcher>();
    }

    public async void Login()
    {
        await ExecuteLogin();
    }
    public async Awaitable ExecuteLogin()
    {
        Debug.Log("Build Connection");
        var t = GetComponentInChildren<MenuManager>(true);

        string username = usernameInput.text;
        string password = passwordInput.text;
        
        await Awaitable.BackgroundThreadAsync();
        
        await _clientConnection.Connect("127.0.0.1", 3456);
        
        _clientConnectionDispatcher.Start();
        
        _clientConnectionDispatcher.SendPackage(new LoginRequestPackage
        {
            Username = username,
            Password = password
        });
        
        var loginResponseData = await _clientConnectionDispatcher.WaitForPackage<LoginResponsePackage>();

        if (!loginResponseData.IsValid)
        {
            
            await Awaitable.MainThreadAsync();
            errorText.text = "Error - Failed to Login";
        }
        else
        {
            await Awaitable.MainThreadAsync();
            errorText.text = "Successfully Logged in";
            await Awaitable.WaitForSecondsAsync(5);
            await t.RealmSelectionMenu();
        }
    }

    public async void Select_Realm(int realmId)
    {
        await ExecuteRealmTransfer(realmId);
    }

    private async Awaitable ExecuteRealmTransfer(int realmId)
    {
        // Debug.Log("Write Realm Packages");
        // _packageParser.ParsePackageToStream();
        
        await Awaitable.BackgroundThreadAsync();

        _clientConnectionDispatcher.SendPackage(new RealmRequestPackage()
        {
            RealmId = realmId,
        });

        var packageData = await _clientConnectionDispatcher.WaitForPackage<RealmResponsePackage>();
        Debug.Log($"Receive Realm Response Package - {packageData.RealmId}, {packageData.CharacterCount} Characters");

        RealmIdText = packageData.RealmId.ToString();

        var charExists = false;

        await Awaitable.MainThreadAsync();
        
        var t = GetComponentInChildren<MenuManager>(true);

        await Awaitable.WaitForSecondsAsync(2);
        if (packageData.CharacterCount > 0) await t.CharacterSelectionMenu();
        else await t.CharacterCreationMenu();
    }

    public async Awaitable SaveCharacter(string characterName, string umaRecipeData)
    {
        await Awaitable.BackgroundThreadAsync();

        _clientConnectionDispatcher.SendPackage(new CharacterCreationRequestPackage()
        {
            CharacterName = characterName,
            UmaRecipe = umaRecipeData,
        });
    }
}
