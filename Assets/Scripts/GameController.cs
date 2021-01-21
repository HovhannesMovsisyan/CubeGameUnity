using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0,1,0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;
    public GameObject cubeToCreate, allCubes, vfx;
    public GameObject[] canvasStartPage;
    private Rigidbody allCubesRb;
    private float camMoveToYPosition, camMoveSpeed=2f;
    private bool IsLose, firstCube;
    private Transform mainCam;
    public Color[] bgColors;
    private Color toCameraColor;

    private readonly List<Vector3> allCubsPositions = new List<Vector3> 
    { 
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
    };

    private Coroutine showCubePlace;
    private int prevCountMaxHorizontal;

    private void Start()
    {
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToYPosition = 5.9f + nowCube.y - 1f;

        allCubesRb = allCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());
    }

    private void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && allCubes != null && cubeToPlace!=null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif
            if (!firstCube)
            {
                firstCube = true;
                foreach (GameObject item in canvasStartPage)
                {
                    Destroy(item);
                }
            }
            GameObject newCube= Instantiate(cubeToCreate, cubeToPlace.position, Quaternion.identity) as GameObject;
           
            newCube.transform.SetParent(allCubes.transform);
            nowCube.SetVector(cubeToPlace.position);
            allCubsPositions.Add(nowCube.GetVector());

            GameObject newVfx= Instantiate(vfx, newCube.transform.position, Quaternion.identity) as GameObject;
            Destroy(newVfx, 1.5f);

            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;
            
            SpawnPositions();
            MoveCameraChangeBg();
        }

        if (!IsLose && allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(showCubePlace);
        }

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition, new Vector3(mainCam.localPosition.x, camMoveToYPosition,
                                                        mainCam.localPosition.z), camMoveSpeed * Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(nowCube.x+1, nowCube.y, nowCube.z)) && nowCube.x+1!=cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y+1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y+1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z+1)) && nowCube.z + 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z+1));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z-1)) && nowCube.z - 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z-1));

        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0)
            IsLose = true;
        else
            cubeToPlace.position = positions[0];
    }

    private bool IsPositionEmpty(Vector3 targetpos)
    {
        if (targetpos.y == 0)
            return false;
        foreach(Vector3 pos in allCubsPositions)
        {
            if (pos.x == targetpos.x && pos.y == targetpos.y && pos.z == targetpos.z)
                return false;
        }
        return true;
    }

    private void MoveCameraChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHor;
        foreach (Vector3 item in allCubsPositions)
        {
            if (Mathf.Abs(Convert.ToInt32(item.x)) > maxX)
                maxX = Convert.ToInt32(item.x);
            if (Convert.ToInt32(item.y) > maxY)
                maxY = Convert.ToInt32(item.y);
            if (Mathf.Abs(Convert.ToInt32(item.z)) > maxZ)
                maxZ = Convert.ToInt32(item.z);
        }
        camMoveToYPosition = 5.9f + nowCube.y - 1f;
        maxHor = maxX > maxZ ? maxX : maxZ;
        if (maxHor % 3 == 0 && prevCountMaxHorizontal!=maxHor)
        {
            mainCam.localPosition -= new Vector3(0, 0, 3f);
            prevCountMaxHorizontal = maxHor;
        }

        if (maxY >= 7)
            toCameraColor = bgColors[2];
        else if (maxY >= 5)
            toCameraColor = bgColors[1];
        else if (maxY >= 2)
            toCameraColor = bgColors[0];
    }
}
 struct CubePos
{
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}