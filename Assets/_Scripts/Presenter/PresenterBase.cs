using UniRx;

public abstract class PresenterBase<TView> : Presenter where TView : View
{
	protected readonly BoolReactiveProperty IsShownProperty = new();

	public TView View { get; }

	public bool IsShown => IsShownProperty.Value;

	protected PresenterBase(TView view)
	{
		View = view;
	}
}