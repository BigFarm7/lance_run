
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Collections;
using UnityEngine.InputSystem;

public class CarManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject myCar;
    public GameObject Roads;
    public GameObject mask3D;
    public GameObject _obj;
    public GameObject hologram;
    public GameObject joystic;
    public Vector3 directionAwayFromCamera,forwardVec;
    public Transform myCarPos;

    public Transform Cam_Offset,MainCam;
    public float relocateDistance = 1.0f;

    ARRaycastManager arManager;
    GameObject placedObject;
    ARPlane _ARPlane;
    NativeArray<Vector2> boundary;
    Vector3 worldPoint, _objPos, newPos;
    public Quaternion instantRot;
    

    List<Vector3> boundaryList;

    public bool isLock = false, isLine = false, CoolTime;


    public TextMeshProUGUI informText;// confirmText;
    void Start()
    {
        // 인디케이터를 비활성화한다.
        indicator.SetActive(false);

        // AR Raycast Manager 컴포넌트를 가져온다.
        arManager = GetComponent<ARRaycastManager>();

        boundaryList = new List<Vector3>();

       
        
    }

    void Update()
    {
        // 바닥 감지 및 이미지 출력 함수
        DetectGround();
        

        if (EventSystem.current.currentSelectedGameObject)
        {
            return;
        }

        // 인디케이터가 활성화된 상태에서 입력을 처리한다.
        if (indicator.activeInHierarchy)
        {
            hologram.SetActive(true);

            Quaternion rota = instantRot * Quaternion.Euler(2, 90, 0);
            Vector3 direction = instantRot * Quaternion.Euler(0, 90, 0) *Vector3.forward;
            newPos = _objPos + direction * 0.1f;

            hologram.transform.position = newPos;
            hologram.transform.rotation = rota;


          
            informText.text = "오브젝트 설치할 곳을 지정하세요.";
            if (Input.GetMouseButtonDown(0) && isLock == false) // 마우스 왼쪽 버튼 클릭

            //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // 모바일 터치
            {
                // 자동차 모델링을 생성한다.
                if (placedObject == null)
                {
                    GameObject gameObjectS = GameObject.FindGameObjectWithTag("Scooter");
                    GameObject[] gameObjectR = GameObject.FindGameObjectsWithTag("Road");
                    GameObject[] gameObjectM = GameObject.FindGameObjectsWithTag("Mask");
                    Destroy(gameObjectS);

                    for(int index = 0;index < gameObjectR.Length;index++)
                    {
                        Destroy(gameObjectR[index]);
                    }
                     
                    for (int index = 0; index < gameObjectM.Length; index++)
                    {
                        Destroy(gameObjectM[index]);
                    }

                    MainCam = GameObject.Find("Main Camera").transform;

                    instantRot = instantRot * Quaternion.Euler(0, 90, 0);

                    forwardVec = instantRot * Vector3.forward;

                    Instantiate(myCar, newPos - new Vector3(0, 0.01f, 0), instantRot);
                    Instantiate(Roads, newPos - new Vector3(0, 0.01f, 0), instantRot);
                    Instantiate(mask3D, newPos - new Vector3(0, 0.01f, 0), instantRot);

                    // confirmText.text = "이 곳에 지정합니까?";

                    isLock = true;
                    indicator.SetActive(false);
                    informText.gameObject.SetActive(false);
                    
                    hologram.SetActive(false);

                    joystic.gameObject.SetActive(true);
                }
                else
                {
                    // 위치를 이동할 조건 확인
                    if (Vector3.Distance(placedObject.transform.position, indicator.transform.position) > relocateDistance)
                    {
                        placedObject.transform.position = indicator.transform.position;
                        placedObject.transform.rotation = indicator.transform.rotation;
                    }
                }
            }
        }
    }


    void DetectGround()
    {/*
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        if (arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            indicator.SetActive(true);
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation = hitInfos[0].pose.rotation;
            indicator.transform.position += indicator.transform.up * 0.01f;
        }
        else
        {
            indicator.SetActive(false);
        }
        
        */




        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        // 레이에 부딪힌 대상의 정보를 저장할 리스트 변수를 만든다.
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        // 레이를 발사한다. (레이어 필터링을 하지 않음)
        if (arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            // 레이캐스트가 성공했다면, 첫 번째 히트 정보 확인
            foreach (ARRaycastHit hitInfo in hitInfos)
            {
                // 히트된 오브젝트의 레이어 확인 (레이어 이름 또는 인덱스를 통해)
                if (hitInfo.trackable.gameObject.layer == LayerMask.NameToLayer("IgnoreLayer"))
                {
                    // 만약 'IgnoreLayer'에 속하면, 그 객체를 무시
                    continue;
                }

                if (isLock)
                    return;
                // 표식 오브젝트를 활성화한다.
                indicator.SetActive(true);

                // 표식 오브젝트의 위치 & 회전 값을 레이의 위치(화면 정 중앙)에 위치시킨다.
                indicator.transform.position = hitInfo.pose.position;
                indicator.transform.rotation = hitInfo.pose.rotation;


                _ARPlane = hitInfo.trackable.gameObject.GetComponent<ARPlane>();
                if (_ARPlane != null && !CoolTime)
                {
                    CoolTime = true;

                    boundaryList.Clear();
                    // ARPlane의 boundary를 가져옴

                    NativeArray<Vector2> boundary = _ARPlane.boundary;
                 
                    // 꼭짓점을 월드 좌표로 변환하여 출력
                    for (int i = 0; i < boundary.Length; i++)
                    {
                        
                        Vector2 localPoint = boundary[i];
                        worldPoint = _ARPlane.transform.TransformPoint(new Vector3(localPoint.x, 0, localPoint.y));
                       // Debug.Log("Vertex " + i + ": " + worldPoint);

                        boundaryList.Add(worldPoint);


                    }
                    StartCoroutine(RunCool());

                }

                if(IsPositionOnLine(indicator.transform.position))
                {
                    
                    isLine = true;
                }
                else
                {
                    isLine = false;
                }

                return; 
            }
        }
        else
        {
           
            informText.text = "주위를 둘러 공간을 확보하세요.";
            indicator.SetActive(false);
        }
    }

    private bool IsPositionOnLine(Vector3 position, float tolerance = 0.1f)
    {

     


        for (int i = 0; i < boundaryList.Count; i++)
        {

            Vector3 currentVertex = boundaryList[i];
            Vector3 nextVertex = boundaryList[(i + 1) % boundaryList.Count];


            if (IsPointNearLine(position, currentVertex, nextVertex, tolerance))
            {
                SetObjectRotation(currentVertex, nextVertex);
                _objPos = GetClosestPointOnLine(indicator.transform.position, currentVertex, nextVertex);
                return true;
            }
        }

        return false;
    }

    private bool IsPointNearLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd, float tolerance)
    {

        Vector3 lineDirection = lineEnd - lineStart;


        Vector3 toPoint = point - lineStart;
        float lineLength = lineDirection.magnitude;
        lineDirection.Normalize();


        float projection = Vector3.Dot(toPoint, lineDirection);
        if (projection < 0 || projection > lineLength)
        {
            return false; 
        }


        Vector3 closestPoint = lineStart + lineDirection * projection;
        float distance = Vector3.Distance(point, closestPoint);

        return distance <= tolerance;
    }

    private void SetObjectRotation(Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;


        instantRot = Quaternion.LookRotation(lineDirection);
    }

    private Vector3 GetClosestPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {

        Vector3 lineDirection = lineEnd - lineStart;


        Vector3 pointToStart = point - lineStart;


        float projection = Vector3.Dot(pointToStart, lineDirection) / lineDirection.sqrMagnitude;


        if (projection < 0)
        {
            return lineStart;
        }
        else if (projection > 1)
        {
            return lineEnd;  
        }
        else
        {
            return lineStart + projection * lineDirection;
        }
    }

    IEnumerator RunCool()
    {
        yield return new WaitForSeconds(0.5f);

        CoolTime = false;
    }
    /*
    private void OnDrawGizmos()
    {
        if (isLock)
            return;
        Gizmos.color = Color.yellow;

        for (int i = 0; i < boundaryList.Count; i++)
        {
            // 현재 꼭짓점과 다음 꼭짓점
            Vector3 currentVertex = boundaryList[i];
            Vector3 nextVertex = boundaryList[(i + 1) % boundaryList.Count];


            Gizmos.DrawLine(currentVertex, nextVertex);
        }
    }
    */
}
 
