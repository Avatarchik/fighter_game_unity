using UnityEngine;
using System.Collections;

public class RightCollider : MonoBehaviour {

    public bool collision = false;

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.transform.parent != null) {
            if (coll.gameObject.transform.parent.name == "Environment")
                collision = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll) {
        if (coll.gameObject.transform.parent != null) {
            if (coll.gameObject.transform.parent.name == "Environment")
                collision = false;
        }
    }

    void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.transform.parent != null) {
            if (coll.gameObject.transform.parent.name == "Environment")
                collision = true;
        }
    }
}
