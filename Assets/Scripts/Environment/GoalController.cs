using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject fracturedGoal;
    public Material mat;

    private GameObject winningGround;

    SlowmotionController slowmotionController;

    // Start is called before the first frame update
    void Start()
    {
        winningGround = GameObject.FindGameObjectWithTag("WinningGround");
        slowmotionController = FindObjectOfType<SlowmotionController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RoundPlayer"))
        {
            RoundPlayerController.goalReached = true;

            GetDestroyed();
        }
    }

    void GetDestroyed()
    {
        GameObject fracturedGoalObj = Instantiate(fracturedGoal,
            new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z), Quaternion.identity) as GameObject;
        fracturedGoalObj.transform.localScale = new Vector3(1.993f, 3.700222f, 4.951498f);

        GameObject[] childs = new GameObject[50];

        if (fracturedGoalObj.transform.childCount > 0)
        {
            for (int i = 0; i < fracturedGoalObj.transform.childCount; i++)
            {
                childs[i] = fracturedGoalObj.transform.GetChild(i).gameObject;

                childs[i].AddComponent(typeof(Rigidbody));
                childs[i].GetComponent<Rigidbody>().mass = 15f;
                childs[i].GetComponent<Rigidbody>().velocity = new Vector3(1f, 5f, 1f);

                childs[i].AddComponent(typeof(MeshCollider));
                childs[i].GetComponent<MeshCollider>().convex = true;

                childs[i].GetComponent<MeshRenderer>().material = mat;

                Physics.IgnoreCollision(FindObjectOfType<RoundPlayerController>().GetComponent<SphereCollider>(), childs[i].GetComponent<MeshCollider>());
                Physics.IgnoreCollision(winningGround.GetComponent<MeshCollider>(), childs[i].GetComponent<MeshCollider>());
            }
        }

        if (childs.Length > 0)
        {
            foreach (var child in childs)
            {
                if (child != null)
                {
                    child.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0f, 200f), ForceMode.Impulse);
                }
            }
        }

        Destroy(this.gameObject);

        slowmotionController.slowdownLength = 10f;
        slowmotionController.DoSlowMotion(0.05f);
    }
}
