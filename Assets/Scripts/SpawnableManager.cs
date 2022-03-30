using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class SpawnableManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject _spawnablePrefab;

    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    private GameObject _spawnedObject;
    private GameObject labyrinth = null;
    private float scale = 0.1f;

    private void Start()
    {
        _spawnedObject = null;

    }

    private GameObject GenLevel(string path, Vector3 position) {
        TextAsset level_text = (TextAsset)Resources.Load("Levels/" + path);

        var start_cube = Resources.Load("Blocks/start_point");
        var finsh_cube = Resources.Load("Blocks/end_point");
        var walll_cube = Resources.Load("Blocks/wall");

        GameObject maze = new GameObject();
        maze.transform.position = position;
        maze.transform.localScale = new Vector3(scale, scale, scale);

        float px = 0.0f;
        float py = 0.0f;
        float pz = 0.0f;

        foreach (var chr in level_text.text)
        {
            px += 1.0f;
            GameObject cube = null;
            if (chr == 'x') {
                cube = (GameObject)Instantiate(walll_cube, position, Quaternion.identity);
            }
            else if (chr == 'f') {
                cube = (GameObject)Instantiate(finsh_cube, position, Quaternion.identity);
            }
            else if (chr == 's') {
                cube = (GameObject)Instantiate(start_cube, position, Quaternion.identity);
                cube.name = "start_point";
            }
            else if (chr == '\n') {
                pz += 1.0f;
                px  = 0.0f;
            }

            if (cube) {
                cube.transform.SetParent(maze.transform, false);
                cube.transform.localPosition = new Vector3(px - 1.0f, py, pz);
            }
        } 

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.GetComponent<Renderer>().enabled = false;
        BoxCollider floor_collider = maze.AddComponent<BoxCollider>();
        floor.transform.SetParent(maze.transform, false);
        floor.transform.localPosition = new Vector3(0, -0.5f, 0);
        floor.transform.localScale    = new Vector3(100.0f, 0.1f, 100.0f);

        return maze;
    }

    private void startGame(GameObject _lab) {
        GameObject _playerPrefab = (GameObject)Resources.Load("Blocks/homyak");
        GameObject start_point = _lab.transform.Find("start_point").gameObject;
        Vector3 pos = start_point.transform.position;
        pos = new Vector3(
            pos.x,
            pos.y + 1.0f,
            pos.z
        );
        GameObject homyak = Instantiate(_playerPrefab, pos, Quaternion.identity);
        homyak.name = "homyak";
        homyak.transform.localScale = new Vector3(
            0.9f * scale, 
            0.9f * scale, 
            0.9f * scale
        );

    }

    private void Update()
    {
        // No touch events
        if (Input.touchCount == 0)
        {
            return;
        }

        // Save the found touch event
        Touch touch = Input.GetTouch(0);

        if (_raycastManager.Raycast(touch.position, _raycastHits) && !GameObject.Find("homyak"))
        {
            if (labyrinth)
                Destroy(labyrinth);
            // Beginning of the touch, this triggers when the finger first touches the screen
            if (touch.phase == TouchPhase.Began)
            {
                labyrinth = GenLevel("maze2", _raycastHits[0].pose.position);
                startGame(labyrinth);
            }
            if (touch.phase == TouchPhase.Ended)
            {
            }
        }
    }
}

