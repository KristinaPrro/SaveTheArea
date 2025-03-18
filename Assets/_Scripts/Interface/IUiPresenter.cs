public interface IUiPresenter
{
	public LevelWindowType WindowType { get; }
	public void Show();
	public void Hide();
}