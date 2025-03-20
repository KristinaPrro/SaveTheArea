public abstract class UiPresenterScreenBase<TView>: UiPresenterBase<TView>, IUiPresenter where TView: UiView
{
	public abstract WindowType WindowType { get; }
	
	protected UiPresenterScreenBase(TView view, ModelUiScreenChange sceneUiModel) : base(view, sceneUiModel)
	{
	}
}