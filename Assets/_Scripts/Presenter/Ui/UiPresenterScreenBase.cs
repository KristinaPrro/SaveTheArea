public abstract class UiPresenterScreenBase<TView>: UiPresenterBase<TView>, IUiPresenter where TView: UiView
{
	public abstract LevelWindowType WindowType { get; }
	
	protected UiPresenterScreenBase(TView view, ModelLevelUi sceneUiModel) : base(view, sceneUiModel)
	{
	}
}