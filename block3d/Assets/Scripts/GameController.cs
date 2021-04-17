using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform CubeToPlace;
    public Text scoreTxt;
    public GameObject[] canvasStartPage;
    public Color[] bgColors;
    public GameObject AllCubes, vfx;
    public GameObject[] cubesToCreate;

    private CubePosition nowCube = new CubePosition(0, 1, 0);
    private float camMoveToTPosition, camMoveSpeed = 2f;
    private Rigidbody AllCubesRb;
    private Color toCameraColor;
    private int prevCountMaxHorizontal;
    private bool IsLose, firstCube;
    private Coroutine showCubePlace;
    private Transform mainCam;
    private List<GameObject> possibleCubesToCreate = new List<GameObject>();

    private List<Vector3> allCubesPositions = new List<Vector3>
    {
        new Vector3 (0,0,0),
        new Vector3 (1,0,0),
        new Vector3 (-1,0,0),
        new Vector3 (0,1,0),
        new Vector3 (0,0,1),
        new Vector3 (0,0,-1),
        new Vector3 (1,0,1),
        new Vector3 (-1,0,-1),
        new Vector3 (-1,0,1),
        new Vector3 (1,0,-1),
    };

    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < 5)
        {
            possibleCubesToCreate.Add(cubesToCreate[0]);
        }
        else if (PlayerPrefs.GetInt("score") < 10)
            AddPossibleCubes(2);
        else if (PlayerPrefs.GetInt("score") < 20)
            AddPossibleCubes(3);
        else if (PlayerPrefs.GetInt("score") < 30)
            AddPossibleCubes(4);
        else if (PlayerPrefs.GetInt("score") < 50)
            AddPossibleCubes(5);
        else if (PlayerPrefs.GetInt("score") < 80)
            AddPossibleCubes(6);
        else if (PlayerPrefs.GetInt("score") < 100)
            AddPossibleCubes(7);
        else if (PlayerPrefs.GetInt("score") < 500)
            AddPossibleCubes(8);
        else if (PlayerPrefs.GetInt("score") < 550)
            AddPossibleCubes(9);
        else 
            AddPossibleCubes(10);

        scoreTxt.text = "<size=40><color=#E06156>best:</color></size> " + PlayerPrefs.GetInt("score") + "\n<size=24>now:</size> 0";
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToTPosition = 5.9f + nowCube.y - 1f;
        AllCubesRb = AllCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());

    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && CubeToPlace != null && AllCubes != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
        if (Input.GetTouch(0).phase != TouchFace.Began)	    
        return;	    
#endif


            if (!firstCube)
            {
                firstCube = true;
                foreach (GameObject obj in canvasStartPage)
                {
                    Destroy(obj);
                }
            }

            GameObject createCube = null;
            if (possibleCubesToCreate.Count == 1)
            {
                createCube = possibleCubesToCreate[0];
            }
            else
                createCube = possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)];


            GameObject NewCube = Instantiate(createCube,
                CubeToPlace.position,
                Quaternion.identity) as GameObject;

            NewCube.transform.SetParent(AllCubes.transform);
            nowCube.SetVector(CubeToPlace.position);
            allCubesPositions.Add(nowCube.GetVector());

            Instantiate(vfx, CubeToPlace.position, Quaternion.identity);

            AllCubesRb.isKinematic = true;
            AllCubesRb.isKinematic = false;

            SpawnPositions();
            MoveCameraChangeBg();
        }

        if (!IsLose && AllCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(CubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(showCubePlace);
        }
        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, camMoveToTPosition, mainCam.localPosition.z), camMoveSpeed*Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
        }

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
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) 
            && nowCube.x + 1 != CubeToPlace.position.x)       
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));      
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z))
            && nowCube.x - 1 != CubeToPlace.position.x)        
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));        
        if (IsPositionEmpty(new Vector3(nowCube.x , nowCube.y +1, nowCube.z))
            && nowCube.y + 1 != CubeToPlace.position.y)        
            positions.Add(new Vector3(nowCube.x , nowCube.y+1, nowCube.z));        
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z))
            && nowCube.y - 1 != CubeToPlace.position.y)       
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));       
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1))
            && nowCube.z + 1 != CubeToPlace.position.z)       
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z+ 1));        
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z-1))
            && nowCube.z - 1 != CubeToPlace.position.z)       
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z-1));

        if (positions.Count > 1)
        {
            CubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        }
        else if (positions.Count == 0)
        {
            IsLose = true;
        }
        else
        {
            CubeToPlace.position = positions[0];
        }
    }

    private bool IsPositionEmpty(Vector3 targetPosition)
    {
        if (targetPosition.y == 0)
        {
            return false;
        }

        foreach(Vector3 position in allCubesPositions)
        {
            if (position.x == targetPosition.x && position.y == targetPosition.y && position.z == targetPosition.z)
            {
                return false;
            }
        }
        return true;
    }

    public void AddPossibleCubes (int till)
    {
        for (int i = 0; i < till; i++)
            possibleCubesToCreate.Add(cubesToCreate[i]);
    }

    private void MoveCameraChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHor;
        foreach (Vector3 pos in allCubesPositions)
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Convert.ToInt32(pos.x);
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Convert.ToInt32(pos.z);
        }

        maxY--;
        if(PlayerPrefs.GetInt("score") < maxY )     
            PlayerPrefs.SetInt("score", maxY );

        scoreTxt.text = "<size=40><color=#E06156>best:</color></size> " + PlayerPrefs.GetInt("score") + "\n<size=24>now:</size> " + maxY;

        camMoveToTPosition = 5.9f + nowCube.y - 1f;

        maxHor = maxX > maxZ ? maxX : maxZ;
        if (maxHor % 3 == 0 && prevCountMaxHorizontal != maxHor)
        {
            mainCam.localPosition += new Vector3(0, 0, -2.5f);
            prevCountMaxHorizontal = maxHor;
        }

        if (maxY >= 7)      
            toCameraColor = bgColors[2];
        else if (maxY >=5)
            toCameraColor = bgColors[1];
        else if (maxY >= 2)
            toCameraColor = bgColors[0];

    }
}


struct CubePosition
{
    public int x, y, z;

    public CubePosition (int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 position)
    {
        x = Convert.ToInt32(position.x);
        y = Convert.ToInt32(position.y);
        z = Convert.ToInt32(position.z);
    }
}
