using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Game.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

[RequireComponent(typeof(UIDocument))]
public class Questionaire : MonoBehaviour
{
    
    private UIDocument _document;

    private Button _continueButton;
    
    #region Question Fields

    private TextField _age;

    private List<Button> _skinColorPickButtons;
    private int _lastSelectedSkinColorIndex = -1;
    
    private List<Button> _hairColorPickButtons;
    private int _lastSelectedHairColorIndex = -1;
    
    private List<Button> _eyeColorPickButtons;
    private int _lastSelectedEyeColorIndex = -1;
    
    [SerializeField] private Color unselectedColorBorderColor = new(27,27,27);
    [SerializeField] private Color selectedColorBorderColor = Color.green;

    private ColorField _hairColor;
    
    private DropdownField _hairLength;

    private ColorField _eyeColor;

    private DropdownField _clothingStyle;

    private ColorField _favouriteColor;

    private RadioButtonGroup _likesGlasses;
    
    private RadioButtonGroup _likesHats;
    

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;
        
        #region Questions
        
        _age = root.Q<TextField>("age");
        _age.RegisterValueChangedCallback(OnlyAllowInts);
        
        _skinColorPickButtons = root.Query<Button>(className:"skin-color-button").ToList();
        _skinColorPickButtons.ForEach(button => button.RegisterCallback(new EventCallback<ClickEvent>(evt => SkinColorPicked(evt, ref _skinColorPickButtons, ref _lastSelectedSkinColorIndex))));
        
        _hairColorPickButtons = root.Query<Button>(className:"eye-color-button").ToList();
        _hairColorPickButtons.ForEach(button => button.RegisterCallback(new EventCallback<ClickEvent>(evt => SkinColorPicked(evt, ref _hairColorPickButtons, ref _lastSelectedHairColorIndex))));
        
        _eyeColorPickButtons = root.Query<Button>(className:"hair-color-button").ToList();
        _eyeColorPickButtons.ForEach(button => button.RegisterCallback(new EventCallback<ClickEvent>(evt => SkinColorPicked(evt, ref _eyeColorPickButtons, ref _lastSelectedEyeColorIndex))));
        
        // make color popup that can be reused for all color properties.
        var colorPopup = root.Q<ColorPopup>("color-popup");
        
        _hairLength = root.Q<DropdownField>("hair-length");
        _hairLength.choices = GetChoicesFromEnum<HairLength>();

        _clothingStyle = root.Q<DropdownField>("clothing-style");
        _clothingStyle.choices = GetChoicesFromEnum<ClothingStyle>();

        _favouriteColor = root.Q<Game.UI.ColorField>("favourite-color");
        _favouriteColor.ColorPopup = colorPopup;
        _favouriteColor.ResetButtonPressed += () => _favouriteColor.value = Color.blue;
        _favouriteColor.OnResetButton();

        _likesGlasses = root.Q<RadioButtonGroup>("likes-glasses");
        _likesGlasses.choices = GetChoicesFromEnum<Opinion>();
        
        _likesHats = root.Q<RadioButtonGroup>("likes-hats");
        _likesHats.choices = GetChoicesFromEnum<Opinion>();
        
        #endregion

        _continueButton = root.Q<Button>("continue");
        _continueButton.clickable.clicked += OnContinueClicked;

    }
    
    #region Continue Button Handling

    private void OnContinueClicked()
    {
        if (EverythingAnswered())
        {
            var passMeSomewhere = new ParticipantPreferences
            {
                Age = Convert.ToInt32(_age.value),
                SkinColor = _skinColorPickButtons[_lastSelectedSkinColorIndex].resolvedStyle.backgroundColor,
                HairColor = _hairColorPickButtons[_lastSelectedHairColorIndex].resolvedStyle.backgroundColor,
                HairLength = GetEnumResponse<HairLength>(_hairLength.index),
                EyeColor = _eyeColorPickButtons[_lastSelectedEyeColorIndex].resolvedStyle.backgroundColor,
                ClothingStyle = GetEnumResponse<ClothingStyle>(_clothingStyle.index),
                FavouriteColor = _favouriteColor.value,
                LikesGlasses = GetEnumResponse<Opinion>(_likesGlasses.value)
            };
        }
        
    }

    private bool EverythingAnswered()
    {
        return WasChanged(_age) &&
               _lastSelectedSkinColorIndex != -1 &&
               // don't care for hair color
               WasChanged(_hairLength) &&
               WasChanged(_eyeColor) &&
               WasChanged(_clothingStyle) &&
               // don't care for favourite color
               WasChanged(_likesGlasses);

    }

    private bool WasChanged(ColorField color) => color.value != Color.black; 
    private bool WasChanged(TextField field) => !string.IsNullOrEmpty(field.value);
    private bool WasChanged(DropdownField field) => field.index != -1;
    private bool WasChanged(RadioButtonGroup group) => group.value != -1;

    #endregion
    
    #region Event Functions
    
    private void SkinColorPicked(ClickEvent clickEvent, ref List<Button> choices, ref int lastIndex)
    {
        // if not left mouse button return.
        if (clickEvent.button != 0) return;
        var button = clickEvent.target as Button;
        
        // Reset all buttons border color.
        choices.ForEach(b => SetBorderColor(b, unselectedColorBorderColor));
        
        // Color the border of the selected one.
        SetBorderColor(button, selectedColorBorderColor);
        
        lastIndex = choices.FindIndex(b => b.style.borderTopColor == selectedColorBorderColor);
    }
    
    private void OnlyAllowInts(ChangeEvent<string> evt)
    {
        if (Regex.IsMatch(evt.newValue, @"[^0-9]"))
        {
            _age.value = evt.previousValue;
        }
    }
    
    #endregion

    #region Helper
    
    private void SetBorderColor(VisualElement button, Color color)
    {
        button.style.borderBottomColor = color;
        button.style.borderTopColor = color;
        button.style.borderRightColor = color;
        button.style.borderLeftColor = color;
    }
    
    /// <summary>
    /// Returns the all enum names as a list of strings plus a dummy string "-" as it's first element.
    /// </summary>
    /// <typeparam name="T">The enum type from which the names will be generated.</typeparam>
    /// <returns>List of all enum names plus "-" at index 0 of the returned list.</returns>
    private List<string> GetChoicesFromEnum<T>() where T : Enum
    {
        var list = new List<string> { "-" };
        list.AddRange(Enum.GetNames(typeof(T)));
        return list;
    }

    private readonly Random _rnd = new();

    /// <summary>
    /// Returns the associated enum value T with index <see cref="selectedIndex"/> - 1.
    /// !! This function is intended to be used in combination with <see cref="GetChoicesFromEnum{T}"/> which inserts a dummy element at index 0. !!
    /// If index is 0 a random enum value will be returned, else the <see cref="selectedIndex"/> - 1.
    /// </summary>
    /// <param name="selectedIndex">The index of the enum value + 1.</param>
    /// <typeparam name="T">The enum to be indexed</typeparam>
    /// <returns>Enum value form enum <see cref="T"/> at index <see cref="selectedIndex"/> - 1</returns>
    private T GetEnumResponse<T>(int selectedIndex) where T : Enum
    {
        var enumNames = Enum.GetNames(typeof(T));

        // we inserted a dummy element at idx=0 - if that was chosen we want a random pick or we subtract one to get the corresponding index of the enum.
        // (all indices got shifted by +1 because of the dummy object.
        selectedIndex = selectedIndex == 0? _rnd.Next(0, enumNames.Length) : selectedIndex - 1;
    
        return Enum.Parse<T>(enumNames[selectedIndex]);
    }
    
    #endregion
}
