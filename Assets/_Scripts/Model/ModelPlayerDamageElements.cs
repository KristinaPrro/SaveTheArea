using Zenject;

public class ModelPlayerDamageElements : ModelObjectBase<IDamageElement>, ITickable
{
	public void Tick()
	{
		foreach (var enemy in Presenters)
			enemy.Tick();
	}
}