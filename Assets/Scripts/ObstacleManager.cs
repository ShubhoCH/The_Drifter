using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Transform playerTransform;
    //prefabsArray:
    public GameObject[] platformPrefab;
    public GameObject[] obstaclePrefab;
    public GameObject[] pointsPrefab;
    public GameObject[] orbsPrefab;
    public GameObject[] portalPrefab;
    List<GameObject> activePlatforms;
    List<GameObject> activeObstacles;
    List<GameObject> activePoints;
    //Variables:
    float platformLength = 100f;
    float spawnZPlatform = 0.0f;
    int minNoOfPlatforms= 8;
    float safeZone = 160f;
    int platformIndex;
    float markedDistance;
    bool spaceJustChanged = false;
    float spaceChangePosition = 2000f;
    float spawnZ = 0.0f;
    int indexToBeUsedForPoints;
    // Points:
    float distanceBtwnPoints = 5f;
    float angleDiffBtwnPoints = 20f;
    int noOfPoints = 8;
    //Obstacle:
    bool doneOnce = false;
    float distanceBtwnConsObj = 22.5f;
    int noOfObjectsPerSpawn = 1;
    float spawnObjectsAfter = 150f;
    int minNoOfObjects = 5;
    //Difficulty:
    float levelUpDistance = 0f;
    bool inFlight = false;
    public Score ui;
    bool orbSpawnOnce;
    public MouseMovement_RB mm;
    void Start()
    {
        orbSpawnOnce = false;
        platformIndex = Random.Range(0,platformPrefab.Length);
        indexToBeUsedForPoints = platformIndex;
        activePlatforms = new List<GameObject>();
        for(int i=0;i<minNoOfPlatforms;i++)
        {
            SpawnPlatform();
        }
        activeObstacles = new List<GameObject>();
        activePoints = new List<GameObject>();
        for(int i = 0; i< minNoOfObjects; i++)
        {
            SpawnPoints();
            SpawnObjects();
            spawnZ += distanceBtwnConsObj;  
        }
    }
    void Update()
    {
        if(playerTransform.position.z > levelUpDistance && noOfObjectsPerSpawn != 3)
        {
            Difficulty();
            levelUpDistance += 500f;
        }
        if(spaceJustChanged == true && spawnZ > markedDistance)
        {
            indexToBeUsedForPoints = platformIndex;
        }
        //Platform:
        if(activePlatforms[0].transform.position.z + safeZone < playerTransform.position.z)
        {
            if(playerTransform.position.z > spaceChangePosition){
                spawnPortal();
                spaceChangePosition += 2000f;
                SpawnPlatform();
            }
            else{
                SpawnPlatform();
            }
            Destroy(activePlatforms[0]);
            activePlatforms.RemoveAt(0);
        }
        //Points & Obstacle:
        if(playerTransform.position.z > spawnObjectsAfter)
        {
            SpawnPoints();
            for(int i=0; i < noOfObjectsPerSpawn; i++)
            {
                SpawnObjects();
                spawnZ += distanceBtwnConsObj;
            } 
            doneOnce = false; 
            if(noOfObjectsPerSpawn != 1)
            {
                spawnZ -= distanceBtwnConsObj;
            }  
            spawnObjectsAfter += 100f;
        }
        //DeleteObstacleObjects:
        if(activeObstacles[0].transform.position.z + 150f < playerTransform.position.z)
        {
            Destroy(activeObstacles[0]);
            activeObstacles.RemoveAt(0);
        }
        //DeletePointsObject:
        for(int i = 0 ; i < noOfPoints-1 ; i++)
        {
            if(activePoints[i].transform.position.z + 40f < playerTransform.position.z )
            {
                Destroy(activePoints[i]);
                activePoints.RemoveAt(i);
            }
        }
        if(inFlight == true){
            for(int i = 0 ; i < noOfPoints-1 ; i++)
            {
                if(activePoints[i].transform.position.z - 30f < playerTransform.position.z )
                {
                    Destroy(activePoints[i]);
                    activePoints.RemoveAt(i);
                    ui.ScoreUpdater();
                }
            }
        }
    }
    void SpawnPlatform()
    {
        GameObject gmObj;
        gmObj = Instantiate(platformPrefab[platformIndex] as GameObject);
        gmObj.transform.SetParent(transform);
        gmObj.transform.position = Vector3.forward * spawnZPlatform;
        spawnZPlatform += platformLength;
        activePlatforms.Add(gmObj);
    }
    void spawnPortal(){
        GameObject gmObj;
        gmObj = Instantiate(portalPrefab[Random.Range(0,portalPrefab.Length)] as GameObject);
        gmObj.transform.SetParent(transform);
        gmObj.transform.position = Vector3.forward * (spawnZPlatform - 50f);
        ChangeSpace();
        activePlatforms.Add(gmObj);
    }
    void ChangeSpace()
    {
        int x = platformIndex;
        do{
            platformIndex = Random.Range(0,platformPrefab.Length);
        }while(platformIndex == x);
        markedDistance = spawnZPlatform - 50f;
        spaceJustChanged = true;
    }
    void SpawnPoints()
    {
        spawnZ += 10f;
        GameObject gmObj;
        float curLocation = 0;
        float prevLocation = Random.Range(0,19) * 10f;
        int daviationBy = Random.Range(-1,2);
        for(int i=0; i < noOfPoints; i++)
        {
            int x = Random.Range(0,81);
            if(x==5 && spaceChangePosition-playerTransform.position.z < 1900f && orbSpawnOnce == false)
            {
                orbSpawnOnce = true;
                curLocation = prevLocation + (daviationBy * angleDiffBtwnPoints);
                gmObj = Instantiate(orbsPrefab[Random.Range(0,orbsPrefab.Length)] as GameObject);
                gmObj.transform.SetParent(transform);
              //gmObj.transform.position = new Vector3(0,-9.5f,1) + new Vector3(0,0,spawnZ);
                gmObj.transform.position = new Vector3(0, -3f, 1) + new Vector3(0, 0, spawnZ);
                gmObj.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f), curLocation);
                prevLocation = curLocation;
                spawnZ += distanceBtwnPoints;
                activePoints.Add(gmObj);
            }
            else if(x==25)
            {
                curLocation = prevLocation + (daviationBy * angleDiffBtwnPoints);
                gmObj = Instantiate(pointsPrefab[pointsPrefab.Length -1] as GameObject);
                gmObj.transform.SetParent(transform);
                gmObj.transform.position = new Vector3(0,-9.5f,1) + new Vector3(0,0,spawnZ);
                gmObj.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f), curLocation);
                prevLocation = curLocation;
                spawnZ += distanceBtwnPoints;
                activePoints.Add(gmObj);
            }
            else{
                curLocation = prevLocation + (daviationBy * angleDiffBtwnPoints);
                gmObj = Instantiate(pointsPrefab[indexToBeUsedForPoints] as GameObject);
                gmObj.transform.SetParent(transform);
                gmObj.transform.position = new Vector3(0,-9.5f,1) + new Vector3(0,0,spawnZ);
                gmObj.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f), curLocation);
                prevLocation = curLocation;
                spawnZ += distanceBtwnPoints;
                activePoints.Add(gmObj);
            }
        }
        orbSpawnOnce = false;
        spawnZ -= distanceBtwnPoints;
        spawnZ += 10f;
    }
    void SpawnObjects(int prefabIndex = -1)
    {
        GameObject gmObj;
        if(noOfObjectsPerSpawn == 1)
        {
            spawnZ += distanceBtwnConsObj;
        }
        int index = Random.Range(0,obstaclePrefab.Length);
        if(doneOnce == true && index == 2)
        {
            do{
                index = Random.Range(0,obstaclePrefab.Length);
            }while(index == 2);
        }
        if(index == 2)
            doneOnce = true;
        if(index == 3)
        {
            gmObj = Instantiate(obstaclePrefab[index] as GameObject);
            gmObj.transform.SetParent(transform);
            gmObj.transform.position = new Vector3(0,-9.5f,1) + new Vector3(0,0,spawnZ);
            gmObj.transform.RotateAround(new Vector3(0,0,0), new Vector3(0,0,1f),-Random.Range(0,18)*10f);
            activeObstacles.Add(gmObj);
        }
        else{
            gmObj = Instantiate(obstaclePrefab[index] as GameObject);
            gmObj.transform.SetParent(transform);
            gmObj.transform.position = Vector3.forward * (spawnZ);
            gmObj.transform.Rotate(new Vector3(0,0,Random.Range(0,18)*10f), Space.World);
            activeObstacles.Add(gmObj);
        }
    }
    void Difficulty()
    {
        noOfObjectsPerSpawn += 1;
        noOfPoints += 2;
        distanceBtwnPoints = 35f / (noOfPoints-1);
        distanceBtwnConsObj = 45f / (noOfObjectsPerSpawn-1);
    }
    public void ClearObstacleWhenEnteringNewSpace()
    {
        for(int i=0;i<activeObstacles.Count;i++)
        {
            if(activeObstacles[i].transform.position.z < playerTransform.position.z + 110f)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i); 
                i--;
            }
        }
    }
    public void ClearPointsWhenEnteringNewSpace()
    {
        for(int i=0;i<activePoints.Count;i++)
        {
            if(activePoints[i].transform.position.z < playerTransform.position.z + 50f)
            {
                Destroy(activePoints[i]);
                activePoints.RemoveAt(i);  
                i--;
            }
        }
    }
    public void ClearPointsWhenReviving()
    {
        for(int i=0;i<activePoints.Count;i++)
        {
            if(activePoints[i].transform.position.z < mm.R() + 10f)
            {
                Destroy(activePoints[i]);
                activePoints.RemoveAt(i);  
                i--;
            }
        }
    }
    public void ClearObstacleWhenReviving()
    {
        for(int i=0;i<activeObstacles.Count;i++)
        {
            if(activeObstacles[i].transform.position.z < mm.R() + 100f)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i); 
                i--;
            }
        }
    }
    public void CollectPoints(){
        inFlight = true;
    }
    public void StopCollectingPoints(){
        inFlight = false;
    }
}