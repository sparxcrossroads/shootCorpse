using UnityEngine;
using System.Collections;

public class FPSPlayer : MonoBehaviour {

    //camer and player
    private CharacterController m_ch;
    public Transform m_transform;
    private float m_movespeed = 3.0f;
    public int m_life = 5;
    private float m_gravity = 2.0f;
    private Transform m_camTransform;
    private Vector3 m_camRot;
    private float m_camHight = 2.0f;

    //m16
    private Transform m_muzzlepoint;    // fire postion
    public LayerMask m_layer;           // 碰撞层
    public Transform m_fx;              // 射击目标时的特效
    public AudioClip m_audio;           // 射击声音
    private float m_shootTimer = 0;     //射击间隔


	// Use this for initialization
	void Start () {

        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();

        m_camTransform = Camera.main.transform;
        Vector3 pos = m_transform.position;
        pos.y += m_camHight;

        m_camTransform.position = pos;
        m_camTransform.rotation = m_transform.rotation;

        m_camRot = m_camTransform.eulerAngles;

        m_muzzlepoint = m_camTransform.FindChild("M16/weapon/muzzlepoint").transform;

        Screen.lockCursor = true;

	}
	
	// Update is called once per frame
    void Update()
    {
        if(m_life!=0)
        Screen.lockCursor = true;

        if (m_life <= 0) return;

        float rv = Input.GetAxis("Mouse Y"); // up -> down  
        float rh = Input.GetAxis("Mouse X"); // left -> right 

        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        // 使主角的面向方向与摄像机一致
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0; camrot.z = 0;
        m_transform.eulerAngles = camrot;

        float xm = 0, ym = 0, zm = 0;

        // gravity
        ym -= m_gravity * Time.deltaTime * 10;

        // forward or back
        if (Input.GetKey(KeyCode.W))
            zm += m_movespeed * Time.deltaTime * 3;
        else if (Input.GetKey(KeyCode.S))
            zm -= m_movespeed * Time.deltaTime * 3;
        // left or right
        if (Input.GetKey(KeyCode.A))
            xm -= m_movespeed * Time.deltaTime * 3;
        else if (Input.GetKey(KeyCode.D))
            xm += m_movespeed * Time.deltaTime * 3;
        // move
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));

        // set play.transform as camer
        Vector3 pos = m_transform.position;
        pos.y += m_camHight;
        m_camTransform.position = pos;

        m_shootTimer -= Time.deltaTime; //shoot gap

        // mouse 0 fire
        if (Input.GetMouseButton(0) && m_shootTimer <= 0)
        {
            m_shootTimer = 0.1f;
            this.audio.PlayOneShot(m_audio);
            GUI_Manager.Instance.SetBall(1);
            RaycastHit info;
            bool hit = Physics.Raycast(
                m_muzzlepoint.position,                             // 发射点
                m_camTransform.TransformDirection(Vector3.forward), //方向
                out info,                                           //发生碰撞时保持光线投射信息
                100,                                                //射线长度
                m_layer);                                           //碰撞目标层

            //print(hit);
            if (hit)
            {
                if (info.transform.tag.CompareTo("Zombie") == 0)
                {
                    Zombie zombie = info.transform.GetComponent<Zombie>();
                    //print("hit-> :ssssssss");
                    zombie.OnDamage(1);
                }
                //print(info.point.ToString());
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "pfsPlayer.png", true);
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;

        GUI_Manager.Instance.SetLife(m_life);
        if (m_life <= 0) Screen.lockCursor = false;
    }
}
