using System;
using System.Collections.Generic;
using DefaultNamespace;
using Microsoft.Extensions.DependencyInjection;
using Networking.PackageParser.PackageImplementations;
using TMPro;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class CharacterCreator : MonoBehaviour
{
    private DynamicCharacterAvatar _characterAvatar;
    private Dictionary<string, DnaSetter> _dna;

    [SerializeField] 
    private List<string> _maleHairModels = new List<string>();
    
    private int _currentMaleHair;
    
    [SerializeField] 
    private List<string> _femaleHairModels = new List<string>();
    
    private int _currentFemaleHair;
    
    [SerializeField] private Slider _heightSlider, _weightSlider, _muscleSlider;
    [SerializeField] private TMP_InputField _characterNameInput;
    [SerializeField] private TMP_Text _promptText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private MenuManager _clientManager;
    private ClientConnectionDispatcher _clientConnectionDispatcher;
    
    void Awake()
    {
        _clientManager = GameObject.Find("ClientManager").GetComponent<MenuManager>();
        _clientConnectionDispatcher = GameContext.ServiceProvider.GetRequiredService<ClientConnectionDispatcher>();
  
        _characterAvatar = GetComponent<DynamicCharacterAvatar>();
        _characterAvatar.CharacterCreated.AddListener(OnCharacterCreate);
        _characterAvatar.CharacterUpdated.AddListener(OnCharacterCreate);
        
        _heightSlider.onValueChanged.AddListener(OnHeightChanged);
        _weightSlider.onValueChanged.AddListener(OnWeightChanged);
        _muscleSlider.onValueChanged.AddListener(OnMuscleChanged);
    }

    private async void Start()
    {
        await Awaitable.FixedUpdateAsync(destroyCancellationToken);

        _characterAvatar.BuildCharacter();
        _dna = _characterAvatar.GetDNA();
        
        _heightSlider.value = _dna["height"].Value;
        _weightSlider.value = _dna["lowerWeight"].Value;
        _muscleSlider.value = _dna["lowerMuscle"].Value;
    }

    private void OnDisable()
    {
        _characterAvatar.CharacterCreated.RemoveListener(OnCharacterCreate);
        _characterAvatar.CharacterUpdated.RemoveListener(OnCharacterUpdate);
        _heightSlider.onValueChanged.RemoveListener(OnHeightChanged);
        _weightSlider.onValueChanged.RemoveListener(OnWeightChanged);
        _muscleSlider.onValueChanged.RemoveListener(OnMuscleChanged);    
    }

    private void OnHeightChanged(float value)
    {
        Debug.Log($"OnHeightChanged: {value}");
        _dna["height"].Set(value);
        _characterAvatar.BuildCharacter();
    }

    private void OnWeightChanged(float value)
    {
        _dna["lowerWeight"].Set(value);
        _dna["upperWeight"].Set(value);
        _characterAvatar.BuildCharacter();
    }
    private void OnMuscleChanged(float value)
    {
        _dna["lowerMuscle"].Set(value);
        _dna["upperMuscle"].Set(value);
        _characterAvatar.BuildCharacter();
    }

    public void ChangeHair(bool hair)
    {
        Debug.Log($"ChangeHair {_characterAvatar.activeRace.name}");

        // Debug.Log(string.Join(",", _characterAvatar.AvailableRecipes["Hair"]));
        if (_characterAvatar.activeRace.name == "HumanMale")
        {
            if (hair)
            {
                _currentMaleHair++;
            }
            else
            {
                _currentMaleHair--;
            }

            _currentMaleHair = Mathf.Clamp(_currentMaleHair, 0, _maleHairModels.Count - 1);
            if (_maleHairModels[_currentMaleHair] == "None")
            {
                _characterAvatar.ClearSlot("Hair");
            }
            else
            {
                _characterAvatar.SetSlot("Hair", _maleHairModels[_currentMaleHair]);
            }
        }
        else if (_characterAvatar.activeRace.name == "HumanFemale")
        {
            if (hair)
            {
                _currentFemaleHair++;
            }
            else
            {
                _currentFemaleHair--;
            }

            _currentFemaleHair = Mathf.Clamp(_currentFemaleHair, 0, _femaleHairModels.Count - 1);
            if (_femaleHairModels[_currentFemaleHair] == "None")
            {
                _characterAvatar.ClearSlot("Hair");
            }
            else
            {
                _characterAvatar.SetSlot("Hair", _femaleHairModels[_currentFemaleHair]);
            }
        }
        
        _characterAvatar.BuildCharacter();
    }

    public void ChangedGender(bool gender)
    {
        if (gender && _characterAvatar.activeRace.name != "HumanMale")
        {
            _characterAvatar.ChangeRace("HumanMale");
        }
        else if (!gender && _characterAvatar.activeRace.name != "HumanFemale")
        {
            _characterAvatar.ChangeRace("HumanFemale");
        }
    }

    public void SwitchSkinColor(Color skinColor)
    {
        _characterAvatar.SetColor("Skin", skinColor);
        _characterAvatar.UpdateColors(true);
    }
    private void OnCharacterCreate(UMAData data)
    {
        _dna = _characterAvatar.GetDNA();
    }

    private void OnCharacterUpdate(UMAData data)
    {
        _dna = _characterAvatar.GetDNA();
    }

    public async void CreateCharacter()
    {
        await ExecuteCharacterCreation(_characterNameInput.text, _characterAvatar.GetCurrentRecipe());
    }

    private async Awaitable ExecuteCharacterCreation(string characterName, string umaRecipeData)
    {
        Debug.Log($"ExecuteCharacterCreation - {characterName}, {umaRecipeData}");
        await Awaitable.BackgroundThreadAsync();

        _clientConnectionDispatcher.SendPackage(new CharacterCreationRequestPackage()
        {
            CharacterName = characterName,
            UmaRecipe = umaRecipeData,
        });

        var response = await _clientConnectionDispatcher.WaitForPackage<CharacterCreationResponsePackage>();

        if (response.IsValid)
        {
            Debug.Log("Successfully created character.");
            await Awaitable.MainThreadAsync();
            _promptText.SetText("<color=green>Successfully created character.</color>");
            await _clientManager.CharacterSelectionMenu();
            // Proceed to character selection.
            return;
        }
        else
        {
            Debug.Log($"ERROR: {response.ErrorMessage}");
            await Awaitable.MainThreadAsync();
            _promptText.SetText("<color=red>ERROR: " + response.ErrorMessage +"</color>");
        }

            
    }

}
