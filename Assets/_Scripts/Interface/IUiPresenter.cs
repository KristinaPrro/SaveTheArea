public interface IUiPresenter
{
	public WindowType WindowType { get; }
	public void Show();
	public void Hide();
}