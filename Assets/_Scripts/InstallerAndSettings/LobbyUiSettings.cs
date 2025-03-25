using UnityEngine;

[CreateAssetMenu(fileName = "LobbyUiSettings", menuName = "ScriptableObject_Settings/Lobby/LobbyUiSettings", order = 1)]
public class LobbyUiSettings : ScriptableObject
{
	[field: Header("Screen:")]
	[field: SerializeField]
	public UiViewLobbyMain ViewLobbyMain { get; private set; }
}