using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour {

	private RoomTemplates templates;

	/// <summary>
	/// 방 추가하면서 맵매니저에 방 추가.
	/// </summary>
	void Start(){
		templates = MapManager.Instance.RoomTemplates;
		MapManager.Instance.rooms.Add(this.gameObject);
	}
}
