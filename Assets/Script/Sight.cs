using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour {

    public Texture2D s;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos() {
        Transform m_camTransform = Camera.main.transform;

        Transform m_muzzlepoint = m_camTransform.FindChild("M16/weapon/muzzlepoint").transform;
        //Gizmos.DrawIcon(m_muzzlepoint.transform.position*(2), s.name, true); wwww
        //GUI.Label(new )

    }
}
