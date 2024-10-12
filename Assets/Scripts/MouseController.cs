using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MouseController : MonoBehaviour
{
    [SerializeField] GameObject cameraGo;
    [SerializeField] private Image aimImage;
    [SerializeField] private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    private Vector3 mousePosHorizontal;
    private Vector3 mousePosVertical;

    // Start is called before the first frame update
    void Start()
    {
        //set image on center
        this.hotSpot.x = cursorTexture.width / 2;
        this.hotSpot.y = cursorTexture.height / 2;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Confined;

        this.ChangeMouseMode();
        this.MouseLook();
        this.MouseVertical();
        //  Debug.Log(this.cameraGo.transform.rotation.eulerAngles);
        var layerMask = 3 << LayerMask.NameToLayer("Tower");
        //  Vector3 rayPos = new Vector3(this.cameraGo.)
        Transform rayTrans = this.aimImage.transform;
        Ray ray = new Ray(rayTrans.position, cameraGo.transform.forward);
        //  Ray ray = new Ray(this.aimImage.rectTransform.anchoredPosition, this.cameraGo.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0.5f);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 10.0f, layerMask))
        {
            Debug.Log(hit.collider.gameObject);
        }
    }


    private void ChangeMouseMode()
    {

        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    private void MouseLook()
    {

        Vector3 mousePos = GetMouseScreenPosition();
        this.aimImage.rectTransform.anchoredPosition = new Vector3(0, mousePos.y * 0.4f - 80, 0);
        // if (this.mousePosHorizontal != GetMouseScreenPosition())
        {


            if (this.mousePosHorizontal.x > GetMouseScreenPosition().x)
            {//마우스를 왼쪽으로 움직이는 경우
                this.mousePosHorizontal = GetMouseScreenPosition();
                this.transform.Rotate(Vector3.up * -150f * Time.deltaTime);//좌우는 몸이 통째로 돌아간다.
                                                                           // this.cameraGo.transform.LookAt(this.transform.position);
                                                                           //  this.cameraGo.transform.Rotate(Vector3.up * -150f * Time.deltaTime);
            }
            else if (this.mousePosHorizontal.x < GetMouseScreenPosition().x)
            {//마우스를 오른쪽으로 움직이는 경우
                this.mousePosHorizontal = GetMouseScreenPosition();
                this.transform.Rotate(Vector3.up * 150f * Time.deltaTime);//좌우는 몸이 통째로 돌아간다.
                                                                          //  this.cameraGo.transform.LookAt(this.transform.position);
                                                                          //  this.cameraGo.transform.Rotate(Vector3.up * 150f * Time.deltaTime);
            }
            else
            {
                this.mousePosHorizontal = GetMouseScreenPosition();
            }


        }




    }
    private void MouseVertical()
    {
        float vertical = this.mousePosVertical.y - GetMouseScreenPosition().y;
        //Debug.Log(vertical);

        if (vertical != 0)
        {

            // Debug.Log(GetMouseScreenPosition().y);
            if (vertical <= 0)
            {
                float rotationValue = 0;
                //  Debug.LogFormat("Up: {0} ", GetMouseScreenPosition().y - this.mousePosVertical.y);
                this.mousePosVertical = GetMouseScreenPosition();
                rotationValue = -this.mousePosVertical.magnitude * 0.04f * Time.deltaTime;
                if (345 < this.cameraGo.transform.eulerAngles.x && this.cameraGo.transform.eulerAngles.x < 360)
                {
                    this.cameraGo.transform.Rotate(Vector3.right * rotationValue);
                }
                else if (this.cameraGo.transform.eulerAngles.x < 20f)
                {
                    this.cameraGo.transform.Rotate(Vector3.right * rotationValue);
                }
                else
                {
                    //   Debug.Log("maximum");
                }
                //   this.cameraGo.transform.rotation = Quaternion.Euler(Vector3.right*rotationValue*10f);
            }
            else if (vertical > 0)
            {
                //Debug.LogFormat("Down: {0} ", this.mousePosVertical.y - GetMouseScreenPosition().y);
                float rotationValue = 0;
                this.mousePosVertical = GetMouseScreenPosition();
                //rotationValue = this.mousePosVertical.y * 0.07f * Time.deltaTime;
                rotationValue = this.mousePosVertical.magnitude * 0.04f * Time.deltaTime;
                //if (this.cameraGo.transform.rotation.x < 0.04)
                if (this.cameraGo.transform.eulerAngles.x < 360f && this.cameraGo.transform.eulerAngles.x > 50)
                {
                    this.cameraGo.transform.Rotate(Vector3.right * rotationValue);
                }
                //  this.cameraGo.transform.rotation = Quaternion.Euler(Vector3.right * rotationValue * -10f);

            }
        }
    }

    private Vector3 GetMouseScreenPosition()
    {

        return Mouse.current.position.ReadValue();//mouse position
    }
}