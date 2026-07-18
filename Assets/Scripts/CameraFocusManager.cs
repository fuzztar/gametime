using System.Collections;
using UnityEngine;

public class CameraFocusManager : MonoBehaviour
{
    public static CameraFocusManager Instance;


    [Header("References")]
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Camera playerCamera;


    private Quaternion originalBodyRotation;
    private Quaternion originalCameraRotation;


    private bool focusing = false;



    private void Awake()
    {
        Instance = this;
    }




    public IEnumerator FocusOnTarget(
        Transform target,
        float speed,
        float holdTime
    )
    {
        if (target == null)
            yield break;



        focusing = true;



        if (mouseLook != null)
        {
            mouseLook.canLook = false;
        }



        // Save original view
        originalBodyRotation = playerBody.rotation;
        originalCameraRotation = playerCamera.transform.localRotation;



        float timer = 0f;
        float maxFocusTime = 2f;



        while (timer < maxFocusTime)
        {
            Vector3 direction =
                target.position -
                playerCamera.transform.position;


            Quaternion lookRotation =
                Quaternion.LookRotation(direction);



            Quaternion bodyRotation =
                Quaternion.Euler(
                    0,
                    lookRotation.eulerAngles.y,
                    0
                );


            Quaternion cameraRotation =
                Quaternion.Euler(
                    lookRotation.eulerAngles.x,
                    0,
                    0
                );



            playerBody.rotation =
                Quaternion.Slerp(
                    playerBody.rotation,
                    bodyRotation,
                    Time.deltaTime * speed
                );



            playerCamera.transform.localRotation =
                Quaternion.Slerp(
                    playerCamera.transform.localRotation,
                    cameraRotation,
                    Time.deltaTime * speed
                );



            float angle =
                Quaternion.Angle(
                    playerCamera.transform.rotation,
                    lookRotation
                );



            if (angle < 2f)
            {
                break;
            }



            timer += Time.deltaTime;

            yield return null;
        }



        yield return new WaitForSeconds(holdTime);
    }





    public IEnumerator ReturnControl(float speed)
    {
        while (true)
        {
            playerBody.rotation =
                Quaternion.Slerp(
                    playerBody.rotation,
                    originalBodyRotation,
                    Time.deltaTime * speed
                );



            playerCamera.transform.localRotation =
                Quaternion.Slerp(
                    playerCamera.transform.localRotation,
                    originalCameraRotation,
                    Time.deltaTime * speed
                );



            bool bodyDone =
                Quaternion.Angle(
                    playerBody.rotation,
                    originalBodyRotation
                ) < 1f;



            bool camDone =
                Quaternion.Angle(
                    playerCamera.transform.localRotation,
                    originalCameraRotation
                ) < 1f;



            if (bodyDone && camDone)
                break;



            yield return null;
        }



        if (mouseLook != null)
        {
            mouseLook.SyncRotation();
            mouseLook.canLook = true;
        }



        focusing = false;
    }




    public bool IsFocusing()
    {
        return focusing;
    }
}