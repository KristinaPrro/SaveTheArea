using Zenject;

public class ModelEnemyObjects : ModelObjectBase<IEnemy>, ITickable
{
	public void Tick()
	{
		foreach (var enemy in Presenters)
			enemy.Tick();
	}
}
