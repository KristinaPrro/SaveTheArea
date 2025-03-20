public class ModelLevelUiInit
{
	private readonly ModelUiScreenChange _modelLevelUi;
	private readonly IUiPresenter[] _presenters;

	public ModelLevelUiInit(ModelUiScreenChange modelLevelUi, IUiPresenter[] presenters)
	{
		_modelLevelUi = modelLevelUi;
		_presenters = presenters;

		_modelLevelUi.Init(_presenters);
	}
}