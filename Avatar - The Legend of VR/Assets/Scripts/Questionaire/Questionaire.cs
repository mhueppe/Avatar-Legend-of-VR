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
    private DropdownField _sex;

    private List<Button> _skinColorPickButtons;
    private int _lastSelectedSkinColorIndex = -1;
    [SerializeField] private Color unselectedColorBorderColor = new(27,27,27);
    [SerializeField] private Color selectedColorBorderColor = Color.green;

    private Game.UI.ColorField _hairColor;
    
    private DropdownField _hairLength;

    private DropdownField _hairType;

    private DropdownField _bodyType;

    private Game.UI.ColorField _eyeColor;

    private DropdownField _clothingStyle;

    private Game.UI.ColorField _favouriteColor;

    private RadioButtonGroup _likesGlasses;
    
    private RadioButtonGroup _likesBraces;
    
    private RadioButtonGroup _likesPiercings;
    
    private RadioButtonGroup _likesTattoos;
    
    private RadioButtonGroup _likesJewelry;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;
        
        #region Questions
        
        _age = root.Q<TextField>("age");
        _age.RegisterValueChangedCallback(OnlyAllowInts);

        _sex = root.Q<DropdownField>("sex");
        _sex.choices = GetChoicesFromEnum<Sex>();

        _skinColorPickButtons = root.Query<Button>(className:"color-pick-button").ToList();
        _skinColorPickButtons.ForEach(button => button.RegisterCallback(new EventCallback<ClickEvent>(SkinColorPicked)));
        
        // make color popup that can be reused for all color properties.
        var colorPopup = root.Q<ColorPopup>("color-popup");

        _hairColor = root.Q<Game.UI.ColorField>("hair-color");
        _hairColor.ColorPopup = colorPopup;
        _hairColor.RegisterOnResetButton(() => _hairColor.value = new Color(80, 60, 50));
        _hairColor.OnResetButton();
        
        _hairLength = root.Q<DropdownField>("hair-length");
        _hairLength.choices = GetChoicesFromEnum<HairLength>();

        _hairType = root.Q<DropdownField>("hair-type");
        _hairType.choices = GetChoicesFromEnum<HairType>();
        
        _bodyType = root.Q<DropdownField>("body-type");
        _bodyType.choices = GetChoicesFromEnum<BodyType>();

        _eyeColor = root.Q<Game.UI.ColorField>("eye-color");
        _eyeColor.ColorPopup = colorPopup;
        _eyeColor.RegisterOnResetButton(() => _eyeColor.value = new Color(0.2311321f, 0.2311321f, 1f));
        _eyeColor.OnResetButton();
        
        _clothingStyle = root.Q<DropdownField>("clothing-style");
        _clothingStyle.choices = GetChoicesFromEnum<ClothingStyle>();

        _favouriteColor = root.Q<Game.UI.ColorField>("favourite-color");
        _favouriteColor.ColorPopup = colorPopup;
        _favouriteColor.RegisterOnResetButton(() => _favouriteColor.value = Color.blue);
        _favouriteColor.OnResetButton();

        _likesGlasses = root.Q<RadioButtonGroup>("likes-glasses");
        _likesGlasses.choices = GetChoicesFromEnum<Opinion>();
        
        _likesBraces = root.Q<RadioButtonGroup>("likes-braces");
        _likesBraces.choices = GetChoicesFromEnum<Opinion>();
        
        _likesPiercings = root.Q<RadioButtonGroup>("likes-piercings");
        _likesPiercings.choices = GetChoicesFromEnum<Opinion>();
        
        _likesTattoos = root.Q<RadioButtonGroup>("likes-tattoos");
        _likesTattoos.choices = GetChoicesFromEnum<Opinion>();
        
        _likesJewelry = root.Q<RadioButtonGroup>("likes-jewelry");
        _likesJewelry.choices = GetChoicesFromEnum<Opinion>();

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
                Sex = GetEnumResponse<Sex>(_sex.index),
                SkinColor = _skinColorPickButtons[_lastSelectedSkinColorIndex].resolvedStyle.backgroundColor,
                HairColor = _hairColor.value,
                HairLength = GetEnumResponse<HairLength>(_hairLength.index),
                HairType = GetEnumResponse<HairType>(_hairType.index),
                BodyType = GetEnumResponse<BodyType>(_bodyType.index),
                EyeColor = _eyeColor.value,
                ClothingStyle = GetEnumResponse<ClothingStyle>(_clothingStyle.index),
                FavouriteColor = _favouriteColor.value,
                LikesGlasses = GetEnumResponse<Opinion>(_likesGlasses.value),
                LikesBraces = GetEnumResponse<Opinion>(_likesBraces.value),
                LikesPiercings = GetEnumResponse<Opinion>(_likesPiercings.value),
                LikesTattoos = GetEnumResponse<Opinion>(_likesTattoos.value),
                LikesJewelry = GetEnumResponse<Opinion>(_likesJewelry.value)
            };
        }
        
    }

    private bool EverythingAnswered()
    {
        return WasChanged(_age) &&
               WasChanged(_sex) &&
               _lastSelectedSkinColorIndex != -1 &&
               // don't care for hair color
               WasChanged(_hairLength) &&
               WasChanged(_hairType) &&
               WasChanged(_bodyType) &&
               WasChanged(_eyeColor) &&
               WasChanged(_clothingStyle) &&
               // don't care for favourite color
               WasChanged(_likesGlasses) &&
               WasChanged(_likesBraces) &&
               WasChanged(_likesPiercings) &&
               WasChanged(_likesTattoos) &&
               WasChanged(_likesJewelry);

    }

    private bool WasChanged(Game.UI.ColorField color) => color.value != Color.black; 
    private bool WasChanged(TextField field) => !string.IsNullOrEmpty(field.value);
    private bool WasChanged(DropdownField field) => field.index != -1;
    private bool WasChanged(RadioButtonGroup group) => group.value != -1;

    #endregion
    
    #region Event Functions
    
    private void SkinColorPicked(ClickEvent clickEvent)
    {
        // if not left mouse button return.
        if (clickEvent.button != 0) return;
        var button = clickEvent.target as Button;
        
        // Reset all buttons border color.
        _skinColorPickButtons.ForEach(b => SetBorderColor(b, unselectedColorBorderColor));
        
        // Color the border of the selected one.
        SetBorderColor(button, selectedColorBorderColor);
        
        _lastSelectedSkinColorIndex = _skinColorPickButtons.FindIndex(b => b.style.borderTopColor == selectedColorBorderColor);
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
