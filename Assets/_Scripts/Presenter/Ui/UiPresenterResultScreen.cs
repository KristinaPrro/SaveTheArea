public class UiPresenterResultScreen : UiPresenterScreenBase<UiViewResultScreen>
{
	public override WindowViewType ViewType => WindowViewType.ResultScreen;	
	
	public UiPresenterResultScreen(UiViewResultScreen view, SceneUiModel sceneUiModel) : base(view, sceneUiModel)
	{

	}
}