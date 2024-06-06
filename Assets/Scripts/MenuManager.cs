using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public async Awaitable RealmSelectionMenu()
    {
        await Awaitable.FromAsyncOperation(SceneManager.LoadSceneAsync("RealmSelection", LoadSceneMode.Single));
    }

    public async Awaitable CharacterSelectionMenu()
    {
        await Awaitable.FromAsyncOperation(SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Single));
    }

    public async Awaitable CharacterCreationMenu()
    {
        await Awaitable.FromAsyncOperation(SceneManager.LoadSceneAsync("CharacterCreation", LoadSceneMode.Single));
    }
}
