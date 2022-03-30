using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finish_script : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        Destroy(collision.gameObject);
        Vector3 new_pos = new Vector3(
            transform.position.x,
            transform.position.y + 0.5f,
            transform.position.z
        );

        var cube = Resources.Load("Blocks/win_cube");
        Instantiate(cube, new_pos, Quaternion.identity);
    }
}
