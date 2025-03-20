public class ModelUiScreenChangeInit
{
	private readonly ModelUiScreenChange _modelLevelUi;
	private readonly IUiPresenter[] _presenters;

	public ModelUiScreenChangeInit(ModelUiScreenChange modelLevelUi, IUiPresenter[] presenters)
	{
		_modelLevelUi = modelLevelUi;
		_presenters = presenters;

		_modelLevelUi.Init(_presenters);
	}
}