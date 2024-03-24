using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    static public CameraMovement instance;

    public GameObject target;
    public float moveSpeed;
    
    public BoxCollider2D bound;

    
    private Vector3 targetPosition;

    private Vector3 minBound;
    private Vector3 maxBound;

    private float halfWidth;
    private float halfHeight;

    private Camera theCamera;

    IEnumerator setSize;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    public bool isZooming;
    public bool isManual;
    void Awake()
    {
    }

    void Start()
    {
        theCamera = GetComponent<Camera>();
        //setSize = SetSizeCrt;
        //SetSize(10f);
        //SetNewCamera();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

    }
    public void SetNewCamera(float size){
        
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = size;
        halfWidth = halfHeight * Screen.width / Screen.height;

    }

    void Update()
    {
        if(target.gameObject != null)
        {
            if(!isManual){
                targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);

                float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth); //clamp는 (10, 0, 100)이라고되면 이중에서 중간값을 리턴하게해주는 거.
                float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

                this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);

            }
            else{
                
                targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, moveSpeed * 0.2f);
                float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth); //clamp는 (10, 0, 100)이라고되면 이중에서 중간값을 리턴하게해주는 거.
                float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

                this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
            }

        }


    }

    public void SetZoom(float size){
        
        SetNewCamera(size);
        theCamera.orthographicSize = size;
    }
    public void AdjustZoom(float size, float speed = 0.03f){
        isZooming = true;
        SetNewCamera(size);
        StopAllCoroutines();
        if(theCamera.orthographicSize < size){
            StartCoroutine(ZoomOut(size,speed));
        }
        else{
            StartCoroutine(ZoomIn(size,speed));
        }
    }
    public IEnumerator ZoomOut(float size, float speed){
        while(theCamera.orthographicSize < size){
            theCamera.orthographicSize += speed;
            yield return waitTime;
        }
        isZooming = false;
    }
    public IEnumerator ZoomIn(float size, float speed){
        while(theCamera.orthographicSize > size){
            theCamera.orthographicSize -= speed;
            yield return waitTime;
        }
        isZooming = false;
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

//        Debug.Log(minBound+","+ maxBound);
    }
    // public void ZoomOut(){
    //     StartCoroutine(ZoomOutCoroutine());
    // }

    // IEnumerator ZoomOutCoroutine(){
    //     theCamera.siz
    // }
}
