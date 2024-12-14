public class UiPresenterStatusScreen : UiPresenterScreenBase<UiViewStatusScreen>
{
	public override WindowViewType ViewType => WindowViewType.StatusScreen;

	public UiPresenterStatusScreen(UiViewStatusScreen view, SceneUiModel sceneUiModel) : base(view, sceneUiModel)
	{
	}
}