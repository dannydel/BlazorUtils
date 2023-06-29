public class BaseEnumField : BaseInputField
{
	[Parameter]
	public Action? OnChangeEvent { get; set; }
	[Parameter]
	public Enum? DefaultValue { get; set; }

	public List<DisplayField> Values { get; set; } = new List<DisplayField>();

	private Type? _propertyType;

	private DisplayField? _selectedValue;
	public DisplayField? SelectedValue
	{
		get => _selectedValue;
		set
		{
			UpdateSelectedValue(value);
		}
	}

	protected override void OnInitialized()
	{
		Values.Clear();

		if (PropertyInfo == null)
		{
			PropertyInfo = PropertyObject!.GetType().GetProperty(PropertyName!);
		}

		_propertyType = PropertyInfo!.PropertyType.IsGenericType && PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
			? Nullable.GetUnderlyingType(PropertyInfo.PropertyType) ?? PropertyInfo.PropertyType
			: PropertyInfo.PropertyType;

		int index = 0;
		foreach (object enumValue in Enum.GetValues(_propertyType))
		{
			Values.Add(new()
			{
				Display = _propertyType.GetField(enumValue?.ToString() ?? "")?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? enumValue?.ToString()!,
				Index = index,
				EnumValue = enumValue?.ToString() ?? "",
				Groups = _propertyType.GetField(enumValue?.ToString() ?? "")?.GetCustomAttribute<DisplayAttribute>()?
				.GetGroupName()?.Replace(" ", "").Split(',').ToList() ?? new()
			});

			index++;
		}

		SetGrouping();

		base.OnInitialized();
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			if (FireFirstRenderChangeEvent)
			{
				UpdateSelectedValue(_selectedValue);
			}
		}
	}

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		SetGrouping(true);

		var defaultValueIndex = PropertyInfo!.GetDefaultValueForProperty();

		string? tempValue = PropertyInfo!.GetValue(PropertyObject)?.ToString() ?? string.Empty;

		//-- Value is already set to what is selected.
		if (_selectedValue?.EnumValue == tempValue)
		{
			StateHasChanged();

			return;
		}

		//-- default value from object available to be set, else just do selected.
		if (DefaultValue != null)
		{
			_selectedValue = Values.FirstOrDefault(e => e.EnumValue == DefaultValue.ToString());
			UpdateSelectedValue(_selectedValue);
		}
		else
		{
			_selectedValue = defaultValueIndex != null && tempValue == null
				? Values.FirstOrDefault(e => e.EnumValue == defaultValueIndex.ToString())
				: (Values?.FirstOrDefault(value => value.EnumValue == tempValue));
		}

		GroupName = "";

		StateHasChanged();
	}

	private void UpdateSelectedValue(DisplayField? value)
	{
		if (value != null)
		{
			_selectedValue = value;

			if (PropertyObject != null)
			{
				var enumValue = Enum.Parse(_propertyType!, value.EnumValue);

				if (ConvertValueToString)
				{
					PropertyInfo!.SetValue(PropertyObject, value.EnumValue);
				}
				else
				{
					PropertyInfo!.SetValue(PropertyObject, enumValue);
				}
				
				if (Required)
				{
					_ = LoanSvc!.ValidateLoan();
				}

				OnChangeEvent?.Invoke();
			}
		}
	}

	private void SetGrouping(bool resetValues = false)
	{
		if (resetValues)
		{
			Values = new();

			int index = 0;
			foreach (object enumValue in Enum.GetValues(_propertyType))
			{
				Values.Add(new()
				{
					Display = _propertyType.GetField(enumValue?.ToString() ?? "")?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? enumValue?.ToString()!,
					Index = index,
					EnumValue = enumValue?.ToString() ?? "",
					Groups = _propertyType.GetField(enumValue?.ToString() ?? "")?.GetCustomAttribute<DisplayAttribute>()?
						.GetGroupName()?.Replace(" ", "").Split(',').ToList() ?? new()
				});

				index++;
			}
		}

		//-- string delimited groupname 
		List<string> groupNameValues = GroupName?.Replace(" ", "").Split(',').ToList() ?? new();
		var displayGroupDistinct = Values.SelectMany(x => x.Groups.Intersect(groupNameValues));

		if (!GroupName.IsNullOrEmpty() && displayGroupDistinct.Any())
		{
			List<DisplayField> foundValues = new();

			foreach (DisplayField value in Values)
			{
				var foundValue = value.Groups.SelectMany(x => value.Groups.Intersect(groupNameValues));
				if (foundValue.Any())
				{
					foundValues.Add(value);
				}
			}
			Values = foundValues;
			
		}
	}

	public class DisplayField
	{
		public string Display { get; set; } = "";
		public int Index { get; set; }
		public string EnumValue { get; set; } = "";
		public List<string> Groups { get; set; } = new();
	}
}
