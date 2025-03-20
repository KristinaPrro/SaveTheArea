public abstract class UiPresenterBase<TView>: Presenter where TView: UiView
{
	protected readonly ModelUiScreenChange SceneUiModel;

	public TView View { get;}

	protected UiPresenterBase(TView view, ModelUiScreenChange sceneUiModel)
	{
		View = view;
		SceneUiModel = sceneUiModel;
	}

	public virtual void Show()
	{
		View.Show();
	}

	public virtual void Hide()
	{
		View.Hide();
	}
}