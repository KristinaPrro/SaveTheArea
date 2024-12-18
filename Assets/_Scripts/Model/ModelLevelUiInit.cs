public class ModelLevelUiInit
{
	private readonly ModelLevelUi _modelLevelUi;
	private readonly IUiPresenter[] _presenters;

	public ModelLevelUiInit(ModelLevelUi modelLevelUi, IUiPresenter[] presenters)
	{
		_modelLevelUi = modelLevelUi;
		_presenters = presenters;

		_modelLevelUi.Init(_presenters);
	}
}