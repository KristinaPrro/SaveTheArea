using UnityEngine;

[CreateAssetMenu(fileName = "GameUiSettings", menuName = "GameUiSettings", order = 1)]
public class GameUiSettings : ScriptableObject
{
	[field: SerializeField]
	public UiViewEnemyDamageElement ViewEnemyDamageElement { get; private set; }

	[field: Header("Screen:") ]
	[field: SerializeField]
	public UiViewGameStatusScreen ViewStatusScreen { get; private set; }
	[field: SerializeField]
	public UiViewResultScreen ViewResultScreen { get; private set; }
}