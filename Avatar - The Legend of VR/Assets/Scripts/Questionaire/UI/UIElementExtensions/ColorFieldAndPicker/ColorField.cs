using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
	public class ColorField : BaseField<Color>
	{
		[UnityEngine.Scripting.Preserve]
		public new class UxmlFactory : UxmlFactory<ColorField, UxmlTraits> { }

		[UnityEngine.Scripting.Preserve]
		public new class UxmlTraits : BaseFieldTraits<Color, UxmlColorAttributeDescription>
		{
			private readonly UxmlStringAttributeDescription resetLabel = new UxmlStringAttributeDescription { name = "reset-label", defaultValue = null };

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				var item = ve as ColorField;
				item.resetLabel = resetLabel.GetValueFromBag(bag, cc);
				item.UpdateResetButton();
			}
		}

		public event System.Action ResetButtonPressed;
		public ColorPopup ColorPopup { get; set; }

		private string resetLabel = null;
		private ColorFieldInput colorFieldInput;
		private Button resetButton;

		private const string stylesResource = "GameUI/Styles/ColorFieldStyleSheet";
		private const string ussInputContainer = "color-field-input-container";
		private const string ussFieldName = "color-field";
		private const string ussFieldLabel = "color-field__label";
		private const string ussFieldResetButton = "color-field__reset-button";

		// ------------------------------------------------------------------------------------------------------------

		private VisualElement _colorFieldInputContainer;

		public ColorField()
			: this(null, null)
		{ }

		public ColorField(string label, string resetLabel = null)
			: this(label, resetLabel, new VisualElement())
		{ }

		private ColorField(string label, string resetLabel, VisualElement colorFieldInputInputContainer) : base(label, colorFieldInputInputContainer)
		{
			_colorFieldInputContainer = colorFieldInputInputContainer;
			_colorFieldInputContainer.AddToClassList(ussInputContainer);
			
			this.colorFieldInput = new ColorFieldInput();
			this.resetLabel = resetLabel;
			_colorFieldInputContainer.Add(colorFieldInput);

			styleSheets.Add(Resources.Load<StyleSheet>(stylesResource));
			AddToClassList(ussFieldName);

			labelElement.AddToClassList(ussFieldLabel);

			colorFieldInput.RegisterCallback<ClickEvent>(OnClickEvent);

			UpdateResetButton();

			RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
		}

		public override void SetValueWithoutNotify(Color newValue)
		{
			base.SetValueWithoutNotify(newValue);
			colorFieldInput.SetColor(newValue);
		}

		private void UpdateResetButton()
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
					resetButton.AddToClassList(ussFieldResetButton);
					resetButton.clicked += OnResetButton;
					_colorFieldInputContainer.Add(resetButton);
				}
				resetButton.text = resetLabel;
			}
		}

		private void OnGeometryChangedEvent(GeometryChangedEvent ev)
		{
			UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
			colorFieldInput.SetColor(value);
		}

		private void OnClickEvent(ClickEvent ev)
		{
			ColorPopup?.Show(value, c => value = c);
		}

		public void OnResetButton()
		{
			ResetButtonPressed?.Invoke();
		}

		// ============================================================================================================

		private class ColorFieldInput : VisualElement
		{
			public VisualElement rgbField;
			public VisualElement alphaField;
			public string resetButtonText = null;
			public Button resetButton;
			
			private const string ussFieldInput = "color-field__input";
			private const string ussFieldInputRGB = "color-field__input-rgb";
			private const string ussFieldInputAlpha = "color-field__input-alpha";
			private const string ussFieldInputAlphaContainer = "color-field__input-alpha-container";

			public ColorFieldInput()
			{
				AddToClassList(ussFieldInput);

				rgbField = new VisualElement();
				rgbField.AddToClassList(ussFieldInputRGB);
				Add(rgbField);

				var alphaFieldContainer = new VisualElement();
				alphaFieldContainer.AddToClassList(ussFieldInputAlphaContainer);
				// Add(alphaFieldContainer);

				alphaField = new VisualElement();
				alphaField.AddToClassList(ussFieldInputAlpha);
				alphaFieldContainer.Add(alphaField);
			}

			public void SetColor(Color color)
			{
				rgbField.style.backgroundColor = new Color(color.r, color.g, color.b, 1f);
				alphaField.style.width = alphaField.parent.resolvedStyle.width * color.a;
			}
		}

		// ============================================================================================================
	}
}