using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
    Transform m_transform;      // zombie
    FPSPlayer m_player;         // plaer
    NavMeshAgent m_agent;       // navi
    private float m_movespeed = 0.5f;

    Animator m_ani;
    float m_rotSpeed;
    float m_timer = 0; // flash nav time
    int m_life = 15;

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSPlayer>();
        m_agent = this.GetComponent<NavMeshAgent>();
        m_agent.SetDestination(m_player.transform.position);

        m_ani = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_player.m_life <= 0) return;

        // 获取当前动画状态。AnimatorStateInfo：保存全部动作-动画类型（走、攻、立、死）
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);

        //idle
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0)) {
            m_ani.SetBool("idle", false);
            
            // idle few seconds
            m_timer -= Time.deltaTime;

            if (m_timer > 0) return;

            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 0.3f)
            {
                m_ani.SetBool("attack", true);
            }
            else {
                m_timer = 0;
                m_agent.SetDestination(m_player.transform.position);

                m_ani.SetBool("run", true);
            }
        }

        // run
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);

            // every sceonds reposition play
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.transform.position);
                m_timer = 1;
            }

            float speed = m_movespeed * Time.deltaTime;
            m_agent.Move(m_transform.TransformDirection(new Vector3(0, 0, speed)));

            // distance zombie -> player <=0.3f attack player
            if(Vector3.Distance(m_transform.position,m_player.m_transform.position)<=3.0f)
            {
                //stop pathfind
                m_agent.ResetPath();
                m_ani.SetBool("atteck", true);
            }
        }
        // attck
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.attack") && !m_ani.IsInTransition(0))
        { 
            // face to player
            Vector3 oldangle = m_transform.eulerAngles;

            m_transform.LookAt(m_player.m_transform);
            float target = m_transform.eulerAngles.y;

            float speed = m_rotSpeed * Time.deltaTime;

            float angle = Mathf.MoveTowardsAngle(oldangle.y, target, speed);
            m_transform.eulerAngles = new Vector3(0, angle, 0);

            // open attack
            m_ani.SetBool("attck", false);

            // finish attck animation ->idle
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);
                // reset clock
                m_timer = 1;
                //reset player heath
                m_player.OnDamage(1);
            }
        }
        // death
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.death") && !m_ani.IsInTransition(0))
        {
            if (stateInfo.normalizedTime >= 1.0f) 
            {
                GUI_Manager.Instance.SetScore(10);
                Destroy(this.gameObject);
            }
        }
	}

    //change zombie heath  (ball ray  
    public void OnDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
            m_ani.SetBool("death", true);
    }
}
