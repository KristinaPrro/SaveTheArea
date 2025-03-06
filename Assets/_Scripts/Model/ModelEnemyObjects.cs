public class ModelEnemyObjects : ModelObjectBase<IEnemy>
{
	public bool TryGetEnemy(int id, out IEnemy element) => TryGetElementById(id, out element);
	public void AddEnemy(IEnemy element) => AddElement(element);
	public void RemoveEnemy(IEnemy element) => RemoveElement(element);
}