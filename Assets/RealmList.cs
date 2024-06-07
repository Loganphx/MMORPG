using System;
using DefaultNamespace;
using Microsoft.Extensions.DependencyInjection;
using Networking.PackageParser.PackageImplementations;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class RealmList : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private ClientManager _clientManager;
    private ClientConnectionDispatcher _clientConnectionDispatcher;
    
    void Awake()
    {
        _clientManager = GameObject.Find("ClientManager").GetComponent<ClientManager>();
        _clientConnectionDispatcher = GameContext.ServiceProvider.GetRequiredService<ClientConnectionDispatcher>();
    }

    private async void Start()
    {
        int realmIndex = 1;
        
        await RequestRealmList();
    }

    private async Awaitable RequestRealmList()
    {
        await Awaitable.BackgroundThreadAsync();
        _clientConnectionDispatcher.SendPackage(new RealmListRequestPackage()
        {
            Region = "US"
        });

        var realmListResponsePackage = await _clientConnectionDispatcher.WaitForPackage<RealmListResponsePackage>();

        Debug.Log($"Received {realmListResponsePackage.Realms.Count} Realms");
        await Awaitable.MainThreadAsync();

        int i = 0;
        foreach (Transform realmOptionTransform in transform)
        {
            if (i >= realmListResponsePackage.Realms.Count)
            {
                realmOptionTransform.gameObject.SetActive(false);
                continue;
            }
            var realmInfo = realmListResponsePackage.Realms[i];
            
            var realmButton = realmOptionTransform.GetComponent<Button>();
            var realmName = realmOptionTransform.GetChild(0).GetComponent<TMP_Text>();
            var tempIndex = realmInfo.RealmId;
            realmButton.onClick.AddListener(() => _clientManager.Select_Realm(tempIndex));
            realmName.SetText(realmInfo.RealmName); 
            i++;
        }
    }
}
