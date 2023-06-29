namespace Amres.WebPortal.Shared.Components.Inputs;
public class BaseInputField : OwningComponentBase
{
	[CascadingParameter]
	public PropertyInfo? PropertyInfo { get; set; }

	[Parameter, EditorRequired]
	public string? PropertyName { get; set; }

	[Parameter, EditorRequired]
	public object? PropertyObject { get; set; }

	public string? Label => GetLabel();

	[Parameter]
	public KeyValuePair<string, object>? UpdateField { get; set; }

	[Parameter]
	public int Index { get; set; }

	[Parameter]
	public string? Class { get; set; }

	[Parameter]
	public string? Style { get; set; }

	[Parameter]
	public Adornment Adornment { get; set; } = Adornment.End;

	[Parameter]
	public string? AdornmentIcon { get; set; }

	[Parameter]
	public InputMode InputMode { get; set; }

	[Parameter]
	public InputType InputType { get; set; }

	[Parameter]
	public string? Format { get; set; }

	[Parameter]
	public CultureInfo? Culture { get; set; }

	[Parameter]
	public bool Clearable { get; set; }

	[Parameter]
	public bool ReadOnly { get; set; }

	[Parameter]
	public bool Disabled { get; set; }

	[Parameter]
	public string? Placeholder { get; set; }

	[Inject]
	protected LoanService? LoanSvc { get; set; }

	[Parameter]
	public bool UseDescriptionAsLabel { get; set; }

	[Parameter]
	public bool HideLabel { get; set; }

	[Parameter]
	/// Comma delimited list of group names
	public string? GroupName { get; set; }

	[Parameter]
	public bool FireFirstRenderChangeEvent { get; set; }

	[Parameter]
	public bool Required { get; set; }

	[Parameter]
	public string? RequiredMessage { get; set; } = "Required";

	[Parameter]
	public IMask? Mask { get; set; }

	[Parameter]
	// Used for Enum Select values to be converted to string and bound back to a string property instead of an enum property.
	public bool ConvertValueToString { get; set; }

	[Parameter]
	public int DebounceInterval { get; set; }

	[Parameter]
	public int MaxLength { get; set; } = 20;

	private string GetLabel()
	{
		return HideLabel
			? string.Empty
			: UseDescriptionAsLabel
			? PropertyInfo?.GetCustomAttribute<DisplayAttribute>()?.GetDescription() ?? PropertyInfo!.Name
			: PropertyInfo?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? PropertyInfo!.Name;
	}
}
