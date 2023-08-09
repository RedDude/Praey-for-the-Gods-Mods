using UnityEngine;

namespace FlyPraeyMod
{
  public class DebugCamera : MonoBehaviour
  {
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5f;
    public float maxDistance = 20f;
    public float minDistance = 0.6f;
    public float xSpeed = 200f;
    public float ySpeed = 200f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5f;
    private float xDeg;
    private float yDeg;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private bool _active;
    private float _aperture;
    private float _focalLength;
    private float _focusDistanceScale;
    public Player actor;
    private bool invetoryPressed;

    public void ToggleActive()
    {
      if (this._active)
      {
        this._active = false;
        this.target = null;
        // Object.Destroy((Object) this.target.gameObject);
      }
      else
      {
        // if (!(bool) (Object) this.target)
          // this.target = new GameObject("Cam Target")
          // {
          //   transform =
          //   {
          //     position = (this.transform.position + this.transform.forward * this.distance)
          //   }
          // }.transform;
        this.distance = Vector3.Distance(this.transform.position, this.target.position);
        this.currentDistance = this.distance;
        this.desiredDistance = this.distance;
        this.position = this.transform.position;
        this.rotation = this.transform.rotation;
        this.currentRotation = this.transform.rotation;
        this.desiredRotation = this.transform.rotation;
        this.xDeg = Vector3.Angle(Vector3.right, this.transform.right);
        this.yDeg = Vector3.Angle(Vector3.up, this.transform.up);
        this._active = true;
        
      }
    }

    private void FixedUpdate()
    {
      if (!this._active)
        return;

      actor.invincible = true;
      
      // actor.IgnoreAllInputExceptCamera(true);
      actor.GetComponent<Rigidbody>().useGravity = false;
      // if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
      //   this.desiredDistance -=
      //     (float) ((double) Input.GetAxis("mouse y") * (double) Time.unscaledDeltaTime * (double) this.zoomRate *
      //              0.125) * Mathf.Abs(this.desiredDistance);
      // else if (Input.GetMouseButton(2))
      // {
      //   this.xDeg += (float) ((double) Input.GetAxis("mouse x") * (double) this.xSpeed * 0.0199999995529652);
      //   this.yDeg -= (float) ((double) Input.GetAxis("mouse y") * (double) this.ySpeed * 0.0199999995529652);
      //   this.yDeg = DebugCamera.ClampAngle(this.yDeg, (float) this.yMinLimit, (float) this.yMaxLimit);
      //   this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, this.transform.localEulerAngles.z);
      //   this.currentRotation = this.transform.rotation;
      //   this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation,
      //     Time.unscaledDeltaTime * this.zoomDampening);
      //   this.transform.rotation = this.rotation;
      // }
      // else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))
      //   this.transform.rotation = Quaternion.Euler(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y,
      //     this.transform.localEulerAngles.z +
      //     (float) ((double) Input.GetAxis("mouse x") * (double) this.xSpeed * 0.0199999995529652));
      // else if (Input.GetMouseButton(1))
      // {
      //   this.target.rotation = this.transform.rotation;
      //   this.target.Translate(Vector3.right * -Input.GetAxis("mouse x") * this.panSpeed);
      //   this.target.Translate(this.transform.up * -Input.GetAxis("mouse y") * this.panSpeed, UnityEngine.Space.World);
      // }

      var isLeftShift = Input.GetKey(KeyCode.LeftShift);
      var newPosition = Vector3.zero;
      float speedModifier = 4f;
      if (isLeftShift)
        speedModifier = 20f;

    
      if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.M)))
      {
        // invetoryPressed = true;
        // if (actor.InventoryMode)
          // actor.CloseInventoryMode();
        // else
          actor.OpenInventoryMode(true);
      }
      
      if (Input.GetKey(KeyCode.W))
      {
        // this.target.rotation = this.transform.rotation;
        // this.target.Translate(Vector3.forward * (this.panSpeed * speedModifier), UnityEngine.Space.Self);
        newPosition = Vector3.forward * (this.panSpeed * speedModifier);
      }
      

      if (Input.GetKey(KeyCode.S))
      {
        // this.target.rotation = this.transform.rotation;
        // this.target.Translate(-Vector3.forward * (this.panSpeed * speedModifier), UnityEngine.Space.Self);
        newPosition = -Vector3.forward * (this.panSpeed * speedModifier);
        // target.position += -Vector3.forward * (this.panSpeed * speedModifier);
      }

      if (Input.GetKey(KeyCode.A))
      {
        // this.target.rotation = this.transform.rotation;
        // this.target.Translate(-Vector3.right * (this.panSpeed * speedModifier), UnityEngine.Space.Self);

        newPosition = -Vector3.right * (this.panSpeed * speedModifier);
        // target.position += -Vector3.right * (this.panSpeed * speedModifier);
      }
      
      if (Input.GetKey(KeyCode.D))
      {
        // this.target.rotation = this.transform.rotation;
        // this.target.Translate(Vector3.right * (this.panSpeed * speedModifier), UnityEngine.Space.Self);
        
        newPosition =  Vector3.right * (this.panSpeed * speedModifier);
        // target.position += Vector3.right * (this.panSpeed * speedModifier);
      }

      // if (Input.GetKey(KeyCode.LeftControl))
        // this._camera.fieldOfView += Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.LeftControl))
        {
          if(isLeftShift)
            newPosition = Vector3.down * (this.panSpeed) / 2;
          else
            newPosition = Vector3.up * speedModifier;
        }
         
        
          // target.position = target.position + Vector3.up * (this.panSpeed * speedModifier) * 100;
        // else
        // this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.unscaledDeltaTime * (float) this.zoomRate *
        // Mathf.Abs(this.desiredDistance);
        // this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
        // this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance,
        //   Time.unscaledDeltaTime * this.zoomDampening);
        // this.position = this.target.position -
        //                 (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);

        // this.target.position = this.position;
        if(newPosition.Equals(Vector3.zero))
          return;
        
        newPosition = target.position + Camera.main.transform.TransformDirection(newPosition);
        actor.PlayerController.SetPosition(newPosition, true);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
      if ((double) angle < -360.0)
        angle += 360f;
      if ((double) angle > 360.0)
        angle -= 360f;
      return Mathf.Clamp(angle, min, max);
    }
  }
}