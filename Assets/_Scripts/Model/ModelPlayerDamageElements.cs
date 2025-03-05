public class ModelPlayerDamageElements : ModelObjectBase<IDamageElement>
{
	public void AddDamageElement(IDamageElement element) => AddElement(element);
	public void DisposeDamageElement(int id) => DisposeElementById(id);
}