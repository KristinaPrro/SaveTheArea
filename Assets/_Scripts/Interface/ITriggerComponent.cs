public interface ITriggerComponent
{
	public int Id { get;}
	public void SetId(int id);
	public bool IsVisible { get; }
	public void SetVisible(bool isVisible);
}