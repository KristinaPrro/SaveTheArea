using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartSettings", menuName = "StartSettings", order = 1)]
public class StartSettings : ScriptableObject
{
	[field: SerializeField]
	public List<PoolItemData> PoolItemDatas { get; private set; } = new();

	public PoolItemData GetPoolItem(PoolItemType itemType) => PoolItemDatas.Find(item => item.PoolItemType == itemType);

	private void OnEnable()
	{
		if(PoolItemDatas != default)
			PoolItemDatas.ForEach(d => d.SetName());
	}
}