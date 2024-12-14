public abstract class UiPresenterScreenBase<TView>: UiPresenterBase<TView> where TView: UiView
{
	public abstract WindowViewType ViewType { get; }
	
	protected UiPresenterScreenBase(TView view, SceneUiModel sceneUiModel) : base(view, sceneUiModel)
	{
	}
}