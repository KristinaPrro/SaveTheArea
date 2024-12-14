using UniRx;

public abstract class UiPresenterBase<TView>: PresenterBase where TView: UiView
{
	protected readonly BoolReactiveProperty IsShownProperty = new();
	protected readonly SceneUiModel SceneUiModel = new ();

	public TView View { get;}

	public bool IsShown => IsShownProperty.Value;

	protected UiPresenterBase(TView view, SceneUiModel sceneUiModel)
	{
		View = view;
		SceneUiModel = sceneUiModel;
	}

	public virtual void Show()
	{
		IsShownProperty.Value = true;
		View.Show();
	}

	public virtual void Hide()
	{
		IsShownProperty.Value = false;
		View.Hide();
	}
}