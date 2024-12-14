using UnityEngine;

[CreateAssetMenu(fileName = "GameUiSettings", menuName = "GameUiSettings", order = 1)]
public class GameUiSettings : ScriptableObject
{
	[field: SerializeField]
	public UiViewEnemyDamageElement PresenterEnemyDamageElement { get; private set; }

	[field: Header("Screen:") ]
	[field: SerializeField]
	public UiViewStatusScreen PresenterStatusScreen { get; private set; }
	[field: SerializeField]
	public UiViewResultScreen PresenterResultScreen { get; private set; }
}