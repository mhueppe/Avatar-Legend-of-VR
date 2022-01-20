using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
	public class ColorField : BaseField<Color>
	{
		#region UXML
		
		[UnityEngine.Scripting.Preserve]
		public new class UxmlFactory : UxmlFactory<ColorField, UxmlTraits> { }

		[UnityEngine.Scripting.Preserve]
		public new class UxmlTraits : BaseFieldTraits<Color, UxmlColorAttributeDescription>
		{
			private readonly UxmlStringAttributeDescription _resetLabel = new UxmlStringAttributeDescription { name = "reset-label", defaultValue = null };

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				var item = ve as ColorField;
				item!._colorFieldInput.resetLabel = _resetLabel.GetValueFromBag(bag, cc);
				item._colorFieldInput.UpdateResetButton();
			}
		}
		
		#endregion
		
		public ColorPopup ColorPopup { get; set; }
		
		private readonly ColorFieldInput _colorFieldInput;

		private const string StylesResource = "GameUI/Styles/ColorFieldStyleSheet";
		private const string USSFieldName = "color-field";
		private const string USSFieldLabel = "color-field__label";
		private const string USSFieldResetButton = "color-field__reset-button";
		

		// ------------------------------------------------------------------------------------------------------------

		public ColorField()
			: this(null, null, new ColorFieldInput(null))
		{ }

		public ColorField(string label, string resetLabel = null)
			: this(label, resetLabel, new ColorFieldInput(resetLabel))
		{ }

		private ColorField(string label, string resetLabel, ColorFieldInput colorFieldInput)
			: base(label, colorFieldInput)
		{
			this._colorFieldInput = colorFieldInput;

			styleSheets.Add(Resources.Load<StyleSheet>(StylesResource));
			AddToClassList(USSFieldName);
			
			labelElement.AddToClassList(USSFieldLabel);

			colorFieldInput.RegisterCallback<ClickEvent>(OnClickEvent);

			RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
		}

		public override void SetValueWithoutNotify(Color newValue)
		{
			base.SetValueWithoutNotify(newValue);
			_colorFieldInput.SetColor(newValue);
		}

		

		private void OnGeometryChangedEvent(GeometryChangedEvent ev)
		{
			UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
			_colorFieldInput.SetColor(value);
		}

		private void OnClickEvent(ClickEvent ev)
		{
			ColorPopup?.Show(value, c => value = c);
		}
		
		public void OnResetButton()
		{
			_colorFieldInput.OnResetButton();
		}

		public void RegisterOnResetButton(Action a) => _colorFieldInput.ResetButtonPressed += a;
		

		// ============================================================================================================

		private class ColorFieldInput : VisualElement
		{
			public event Action ResetButtonPressed;

			public VisualElement rgbField;
			public Button resetButton;
			public string resetLabel;

			private const string USSFieldInput = "color-field__input";
			private const string USSFieldInputRGB = "color-field__input-rgb";

			public ColorFieldInput(string resetLabel)
			{
				this.resetLabel = resetLabel;
				
				AddToClassList(USSFieldInput);

				rgbField = new VisualElement();
				rgbField.AddToClassList(USSFieldInputRGB);
				Add(rgbField);
				UpdateResetButton();
			}

			public void SetColor(Color color)
			{
				rgbField.style.backgroundColor = new Color(color.r, color.g, color.b, 1f);
			}
			
			public void UpdateResetButton()
			{
				if (resetLabel == null)
				{
					if (resetButton != null)
					{
						Remove(resetButton);
						resetButton = null;
					}
				}
				else
				{
					if (resetButton == null)
					{
						resetButton = new Button();
						resetButton.AddToClassList(USSFieldResetButton);
						resetButton.clicked += OnResetButton;
						Add(resetButton);
					}
					resetButton.text = resetLabel;
				}
			}
			
			public void OnResetButton()
			{
				ResetButtonPressed?.Invoke();
			}
		}

	}
}