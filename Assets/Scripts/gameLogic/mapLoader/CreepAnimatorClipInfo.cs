// using UnityEngine;

// public class CreepAnimatorClipInfo {
//     public Animator m_Animator;
//     public string m_ClipName;
//     public AnimatorClipInfo[] m_CurrentClipInfo;

//     public float m_CurrentClipLength;
//     public GameObject gameObject;

//     public CreepAnimatorClipInfo(GameObject gameObject) {
//         Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- gameObject:" + gameObject);
//         this.gameObject = gameObject;
//         //Get them_Animator, which you attach to the GameObject you intend to animate.
//         m_Animator = gameObject.GetComponent<Animator>();
//         //Fetch the current Animation clip information for the base layer
//         m_CurrentClipInfo = this.m_Animator.GetCurrentAnimatorClipInfo(0);
//         //Access the current length of the clip
//         m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
//         //Access the Animation clip name
//         m_ClipName = m_CurrentClipInfo[0].clip.name;
        
//         Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- Clip Name:" + m_ClipName);
//         Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- Clip Length:" + m_CurrentClipLength);
//         Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- m_CurrentClipInfo:" + m_CurrentClipInfo);
//         Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- m_CurrentClipInfo.Length:" + m_CurrentClipInfo.Length);
//         foreach (AnimatorClipInfo animatorClipInfo in m_CurrentClipInfo) {
//             Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- animatorClipInfo:" + animatorClipInfo);
//             Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- animatorClipInfo.clip:" + animatorClipInfo.clip);
//             Debug.Log("CreepAnimatorClipInfo::CreepAnimatorClipInfo(); -- animatorClipInfo.weight:" + animatorClipInfo.weight);
//         }
//     }
// }